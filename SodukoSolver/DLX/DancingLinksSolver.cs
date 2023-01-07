using System.Diagnostics;

namespace SodukoSolver.DLX
{
    public class CallDLX
    {
        public static int given_SIZE;
        
        public void Call(int[,] board)
        {
            var timer = new Stopwatch();
            timer.Start();
            
            given_SIZE = board.GetLength(0);
            int[,] bigMatrix = ConstraintsMatrix.ConvertSudoku(board); // convert given sudoku board to big 0/1 matrix according to sudoku constraints
            
            DancingLinksSolver dLX = new();
            dLX.CreateDancingLinksMatrix(bigMatrix);

            bool result = dLX.Search(dLX.GetSolution()); // call the Search method for a solution
            if (!result)
                Console.WriteLine("No Solution!");

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("\nTime taken for DLX solving operation: " + timeTaken.ToString(@"m\:ss\.fff") + " minutes");
        }
    }
    public class DancingLinksSolver
    {
        public static readonly int SIZE = CallDLX.given_SIZE;
        public static readonly int CUBE_SIZE = (int)Math.Sqrt(SIZE);
        public static readonly int numRows = SIZE * SIZE * SIZE;
        public static readonly int numCols = 4 * SIZE * SIZE;

        HeaderNode root; // root to the nodes mesh

        List<Node> solution = new(); // a list of nodes that represents the final solution

        public void CreateDancingLinksMatrix(int[,] ProbMat)
        {
            root = new HeaderNode(-1); // initialize root's name to -1

            HeaderNode[] headers = new HeaderNode[numCols];

            for (int i = 0; i < numCols; i++) // initialize column headers 
            {
                headers[i] = new HeaderNode(i);

                root.LinkRight(headers[i]); // connect between the moved root and the current header
                root = headers[i]; // move the root to the next header
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
                    if (ProbMat[row, col] == 1)
                    {
                        currentHeader = headers[col]; // get the current node's header

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

        public List<Node> GetSolution()
        {
            return solution;
        }

        public bool Search(List<Node> solution) // searching  for a solution through the node mesh
        {
            if (root.right == root) // if there is no column left, then must have found the solution
            {
                ConvertToMatrix();
                return true;
            }

            Node minCol = GetMinCol();  // choose column deterministically -> according to the number of nodes in each column

            Cover(minCol); // cover chosen column -> remove it from the mesh

            for (Node currentRow = minCol.down; currentRow != minCol; currentRow = currentRow.down)
            {
                solution.Add(currentRow);

                for (Node rightNode = currentRow.right; rightNode != currentRow; rightNode = rightNode.right)
                    Cover(rightNode.columnHeader);

                if (Search(GetSolution()))
                    return true; // move to the level (recursively)

                currentRow = solution[solution.Count - 1];

                // if solution in not possible, backtrack (uncover) and remove the selected row (set) from solution
                solution.RemoveAt(solution.Count - 1);

                minCol = currentRow.columnHeader;

                for (Node leftNode = currentRow.left; leftNode != currentRow; leftNode = leftNode.left)
                    Uncover(leftNode.columnHeader);
            }

            Uncover(minCol);
            return false;
        }

        Node GetMinCol() // traversing through column headers to the right and return the column that has the minimum node count
        {
            HeaderNode tempNode = (HeaderNode)root.right;
            HeaderNode saveHeader = null;
            int nodeCounter = SIZE * SIZE * SIZE; // maximum number of nodes in a column

            while (tempNode != root)
            {
                if (tempNode.headerNodeCount < nodeCounter)
                {
                    nodeCounter = tempNode.headerNodeCount;
                    saveHeader = tempNode;
                }
                tempNode = (HeaderNode)tempNode.right;
            }
            return saveHeader;
        }

        public void ConvertToMatrix() // converting the solution list back into a matrix and printing the solved sudoku board
        {
            int[,] mat = new int[SIZE, SIZE];
            
            foreach (Node n in solution)
            {
                Node tempNode = n;
                int min = tempNode.columnHeader.name;

                for (Node temp = tempNode.right; temp != tempNode; temp = temp.right)
                {
                    int var = temp.columnHeader.name;
                    if (var < min)
                    {
                        min = var;
                        tempNode = temp;
                    }
                }
                int placement = tempNode.columnHeader.name;
                int row = placement / SIZE;
                int col = placement % SIZE;
                
                mat[row, col] = tempNode.right.columnHeader.name % SIZE + 1;
            }

            Console.WriteLine("The Solution:");
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                    Console.Write(mat[i, j] + " ");
                Console.WriteLine("");
            }
        }

        private static void Cover(Node targetNode) // covering the given node completely -> unlinking it from the mesh
        {
            //unlink node's right links 
            targetNode.left.right = targetNode.right;
            targetNode.right.left = targetNode.left;

            Node currentCol = targetNode.down;
            Node currentNode;

            while (currentCol != targetNode)
            {
                currentNode = currentCol.right;

                while (currentNode != currentCol)
                {
                    // unlink each of the up/down links of the nodes in the same column
                    currentNode.up.down = currentNode.down;
                    currentNode.down.up = currentNode.up;

                    currentNode.columnHeader.headerNodeCount--; // decrease this column header's node count

                    currentNode = currentNode.right;
                }
                currentCol = currentCol.down;
            }
        }

        private static void Uncover(Node targetNode) // uncovering the given node completely -> restoring it to the board, connecting back it's links
        {
            // relink node's right links
            targetNode.left.right = targetNode;
            targetNode.right.left = targetNode;

            Node currentCol = targetNode.up;
            Node currentNode;

            while (currentCol != targetNode)
            {
                currentNode = currentCol.left;

                while (currentNode != currentCol)
                {
                    // relink each of the up/down links of the nodes in the same column
                    currentNode.up.down = currentNode;
                    currentNode.down.up = currentNode;

                    currentNode.columnHeader.headerNodeCount++; // increase this column header's node count

                    currentNode = currentNode.left;
                }
                currentCol = currentCol.up;
            }
        }
    }
}
