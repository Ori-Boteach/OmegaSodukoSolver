using System.Diagnostics;

namespace SodukoSolver.DLX
{
    #pragma warning disable CS8600 // disable -> converting null literal or possible null value to non nullable type
    #pragma warning disable CS8603 // disable -> Possible null reference return.
    #pragma warning disable CS8618 // disable -> Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class InitiatingDlxClass
    {
        public static int given_SIZE; // the sudoku's size

        public void InitiateDLX(int[,] board)
        {
            given_SIZE = board.GetLength(0); // set the sudoku's size as a globl variable
            int[,] ConstraintsMatrix = DLX.ConstraintsMatrix.ConvertSudokuBoard(board); // convert given sudoku board to big 0/1 matrix according to sudoku constraints

            // create a new instance of the DancingLinksSolver class and convert to correlating node spares matrix
            DancingLinksSolver dLX = new();
            dLX.CreateDancingLinksMatrix(ConstraintsMatrix);

            // setting up the timer and starting it 
            var timer = new Stopwatch();
            timer.Start();

            bool result = dLX.Search(dLX.GetSolution()); // call the Search method for a solution
            if (!result)
                Console.WriteLine("No Solution!");
            else
                dLX.ConvertBackToMatrix();

            // stopping the timer and printing the answer
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("\nTime taken for DLX solving operation: " + timeTaken.ToString(@"m\:ss\.fff") + " minutes");
        }
    }
    
    public class DancingLinksSolver // DancingLinksSolver class that converts constraints matrix to node spares matrix and performs the claculations
    {
        // setting global size variables
        public static readonly int SIZE = InitiatingDlxClass.given_SIZE;
        public static readonly int CUBE_SIZE = (int)Math.Sqrt(SIZE);
        public static readonly int numRows = SIZE * SIZE * SIZE;
        public static readonly int numCols = 4 * SIZE * SIZE;

        private HeaderNode root; // root access to the nodes mesh

        readonly List<Node> solution = new(); // a list of nodes that represents the final solution

        public void CreateDancingLinksMatrix(int[,] problamMatrix)
        {
            root = new HeaderNode(-1); // initialize root's "name" to -1

            HeaderNode[] matrixHeaders = new HeaderNode[numCols];

            for (int i = 0; i < numCols; i++) // initialize column headers 
            {
                matrixHeaders[i] = new HeaderNode(i);

                root.LinkRight(matrixHeaders[i]); // connect between the moved root and the current header
                root = matrixHeaders[i]; // move the root to the next header
            }
            root = root.right.columnHeader; // set root back to it's original place

            HeaderNode currentHeader;
            Node insertedNode, lastInsertedNode;

            for (int row = 0; row < numRows; row++) // go through each row of the constraints matrix
            {
                lastInsertedNode = null; // start a new route of node insertions

                for (int col = 0; col < numCols; col++) // go through each col of the constraints matrix
                {
                    // only if the problem matrix has 1 in the current cell, then, create a node
                    if (problamMatrix[row, col] == 1)
                    {
                        currentHeader = matrixHeaders[col]; // get the current node's header

                        insertedNode = new Node(currentHeader); // create a new node currentHeader as it's header

                        currentHeader.up.LinkDown(insertedNode); // link the current header's node to the inserted node

                        if (lastInsertedNode != null)
                        {
                            lastInsertedNode.LinkRight(insertedNode); // link the last inserted node to the current inserted node
                            lastInsertedNode = lastInsertedNode.right; // move lastInsertedNode to the right
                        }
                        else
                            lastInsertedNode = insertedNode; // lastInsertedNode is now currently inserted node

                        currentHeader.headerNodeCount++; // increase the current node header's node count
                    }
                }
            }
        }

        public List<Node> GetSolution() { return solution; } // a function that return the solution list of nodes

        public bool Search(List<Node> solution) // searching  for a solution through the node mesh
        {
            if (root.right == root) // if there is no column left, then must have found the solution
                return true;

            Node minCol = GetMinCol();  // choose column deterministically -> according to the number of nodes in each column

            Cover(minCol); // cover chosen column -> remove it from the mesh

            for (Node currentRow = minCol.down; currentRow != minCol; currentRow = currentRow.down)
            {
                solution.Add(currentRow);

                for (Node rightNode = currentRow.right; rightNode != currentRow; rightNode = rightNode.right)
                    Cover(rightNode.columnHeader);

                if (Search(GetSolution()))
                    return true; // move to the next level (recursively)

                currentRow = solution[^1]; // currentRow updates -> tried route didn't work

                // if solution in not possible, backtrack (uncover) and remove the selected row ("set") from solution
                solution.RemoveAt(solution.Count - 1);

                minCol = currentRow.columnHeader;

                for (Node leftNode = currentRow.left; leftNode != currentRow; leftNode = leftNode.left)
                    Uncover(leftNode.columnHeader);
            }

            // solution not found, restore minCol to spares matrix andd return false
            Uncover(minCol);
            return false; 
        }

        Node GetMinCol() // traversing through column headers to the right and return the column that has the minimum node count
        {
            HeaderNode tempHeader = (HeaderNode)root.right;
            HeaderNode saveHeader = null;
            
            int nodeCounter = SIZE * SIZE * SIZE; // maximum number of nodes in a column

            while (tempHeader != root) // traversing through each column header
            {
                if (tempHeader.headerNodeCount < nodeCounter) // if found a new min value of nodes in col -> setting it in saveHeader
                {
                    nodeCounter = tempHeader.headerNodeCount;
                    saveHeader = tempHeader;
                }
                tempHeader = (HeaderNode)tempHeader.right;
            }
            
            return saveHeader; // returning the column header with the mininum amount of nodes under it
        }

        public void ConvertBackToMatrix() // converting the solution list back into a matrix and printing the solved sudoku board
        {
            int[,] solvedBoard = new int[SIZE, SIZE];
            
            foreach (Node node in solution) // go over each node in the solution list 
            {
                int minName = node.columnHeader.name;
                Node tempNode = node;

                for (Node temp = node.right; temp != node; temp = temp.right) // go left to right and search fo the minimal column Header
                {
                    int name = temp.columnHeader.name;
                    if (name < minName)
                    {
                        minName = name;
                        tempNode = temp;
                    }
                }
                int columnHeaderPos = tempNode.columnHeader.name;
                int row = columnHeaderPos / SIZE;
                int col = columnHeaderPos % SIZE;
                
                solvedBoard[row, col] = tempNode.right.columnHeader.name % SIZE + 1;
            }

            // printing the board to the console at a matrix form
            Console.WriteLine("The Solution:");
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                    Console.Write(solvedBoard[i, j] + " ");
                Console.WriteLine("");
            }
        }

        private static void Cover(Node targetNode) // covering the given node completely -> unlinking it from the mesh
        {
            //unlinking node's right links 
            targetNode.left.right = targetNode.right;
            targetNode.right.left = targetNode.left;

            Node currentHeader = targetNode.down;
            Node currentNode;

            while (currentHeader != targetNode) // traversing through the mesh and covering the links of the given node
            {
                currentNode = currentHeader.right;

                while (currentNode != currentHeader)
                {
                    // unlink each of the up/down links of the nodes in the same column
                    currentNode.up.down = currentNode.down;
                    currentNode.down.up = currentNode.up;

                    currentNode.columnHeader.headerNodeCount--; // decrease this column header's node count

                    currentNode = currentNode.right;
                }
                currentHeader = currentHeader.down;
            }
        }

        private static void Uncover(Node targetNode) // uncovering the given node completely -> restoring it to the board, connecting back it's links
        {
            // relink node's right links
            targetNode.left.right = targetNode;
            targetNode.right.left = targetNode;

            HeaderNode currentHeader = (HeaderNode)targetNode.up;
            Node currentNode;

            while (currentHeader != targetNode) // traversing through the mesh and uncovering the links of the given node
            {
                currentNode = currentHeader.left;

                while (currentNode != currentHeader)
                {
                    // relink each of the up/down links of the nodes in the same column
                    currentNode.up.down = currentNode;
                    currentNode.down.up = currentNode;

                    currentNode.columnHeader.headerNodeCount++; // increase this column header's node count

                    currentNode = currentNode.left;
                }
                currentHeader = (HeaderNode)currentHeader.up;
            }
        }
    }
}
