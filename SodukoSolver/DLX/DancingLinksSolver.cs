using System.Diagnostics;
using System.Text;

namespace SodukoSolver.DLX
{
    #pragma warning disable CS8600 // disable -> converting null literal or possible null value to non nullable type
    #pragma warning disable CS8603 // disable -> Possible null reference return.
    #pragma warning disable CS8618 // disable -> Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    public class InitiatingDlxClass
    {
        public string InitiateDLX(int[,] board) // the function that handles all of the dlx solvig operation
        {
            int[,] ConstraintsMatrix = DLX.ConstraintsMatrix.ConvertSudokuBoard(board); // convert given sudoku board to big 0/1 matrix according to sudoku constraints

            // create a new instance of the DancingLinksSolver class and convert the constraints matrix to correlating node spares matrix
            DancingLinksSolver dLX = new();
            dLX.CreateDancingLinksMatrix(ConstraintsMatrix);

            // setting up the timer and starting it 
            var timer = new Stopwatch();
            timer.Start();

            string answer;
            bool result = dLX.Search(dLX.GetSolution()); // calling the Search method for a solution
            if (!result) // if a solution was not found
            {
                Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                Console.WriteLine("\n***The soduko is unsolvable***"); // printing an indicative message to the screen
                Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
                answer = "***The soduko is unsolvable***"; // also returning the message to the calling function for testings
            }
            else
                answer = dLX.ConvertBackToMatrix(); // a solution was found, convert the solution to a string, print and return it

            // stopping the timer and printing the answer
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("\n\nTime taken for DLX solve: " + timeTaken.ToString(@"m\:ss\.fff") + " minutes");

            return answer; // return the answer to the calling function
        }
    }
    
    public class DancingLinksSolver // DancingLinksSolver class that converts constraints matrix to node spares matrix and performs the claculations
    {
        // setting global size variables (all depend on the size of the soduko board which was calculated earlier)
        public int SIZE = UI.SIZE; // the size of the soduko board
        public int CUBE_SIZE = (int)Math.Sqrt(UI.SIZE); // the size of the sub cube in the soduko board
        public int numRows = UI.SIZE * UI.SIZE * UI.SIZE; // the number of rows in the constraints and spares matrix
        public int numCols = 4 * UI.SIZE * UI.SIZE; // the number of columns in the constraints and spares matrix

        private HeaderNode root; // root access to the nodes mesh (defined as a HeaderNode)

        readonly List<Node> solution = new(); // a list of nodes that represents the final solution

        public void CreateDancingLinksMatrix(int[,] problamMatrix) // converting the constraints matrix into a node spares matrix 
        {
            root = new HeaderNode(-1); // initializing root's "name" (index) to -1

            HeaderNode[] matrixHeaders = new HeaderNode[numCols];

            for (int i = 0; i < numCols; i++) // initializing column headers 
            {
                matrixHeaders[i] = new HeaderNode(i);

                root.LinkRight(matrixHeaders[i]); // connecting between the moved root and the current header
                root = matrixHeaders[i]; // move the root to the next header
            }
            root = root.right.columnHeader; // set root back to it's original place (to the right of the first header)

            // initializing these variables to be used in the coming loops
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

                        if (lastInsertedNode != null) // if this is not the first node in the route
                        {
                            lastInsertedNode.LinkRight(insertedNode); // link the last inserted node to the current inserted node
                            lastInsertedNode = lastInsertedNode.right; // move lastInsertedNode to the right
                        }
                        else // this is the first node in the route
                            lastInsertedNode = insertedNode; // lastInsertedNode is now currently inserted node

                        currentHeader.headerNodeCount++; // increase the current node header's node count
                    }
                }
            }
        }

        public List<Node> GetSolution() { return solution; } // a function that returns the solution list of nodes

        public bool Search(List<Node> solution) // searching for a solution through the created node mesh
        {
            if (root.right == root) // if there is no column left, then I must have found the solution
                return true;

            Node minCol = GetMinCol();  // choosing column deterministically -> according to the number of nodes in each column (the column with the least nodes is chosen)

            Cover(minCol); // cover chosen column -> remove it from the mesh

            for (Node currentRow = minCol.down; currentRow != minCol; currentRow = currentRow.down) // iterating through each row of the chosen minCol
            {
                solution.Add(currentRow); // add current row to the solution list

                // covering all the columns that are connected to the current row
                for (Node rightNode = currentRow.right; rightNode != currentRow; rightNode = rightNode.right)
                    Cover(rightNode.columnHeader);

                if (Search(GetSolution())) // move to the next level (recursively)
                    return true; // if a soultion was found somewhere down the line, return true

                currentRow = solution[^1]; // currentRow is being updated -> tried route didn't work out

                // if solution in not possible, backtrack (uncover) and remove the selected row from solution
                solution.RemoveAt(solution.Count - 1);

                minCol = currentRow.columnHeader; // updating minCol to the current row's header

                // uncovering all the columns that were covered in the current row
                for (Node leftNode = currentRow.left; leftNode != currentRow; leftNode = leftNode.left)
                    Uncover(leftNode.columnHeader);
            }

            // solution not found, restore minCol to spares matrix andd return false
            Uncover(minCol);
            return false; 
        }

        Node GetMinCol() // traversing through column headers to the right and returning the column that has the minimum node count
        {
            HeaderNode tempHeader = (HeaderNode)root.right; // start from the first header (to the right of the root)
            HeaderNode saveHeader = tempHeader; // save the first header as the one with the minimum node count

            int nodeCounter = SIZE * SIZE * SIZE; // maximum number of nodes in a column

            while (tempHeader != root) // traverse through each column header
            {
                if (tempHeader.headerNodeCount < nodeCounter) // if found a new minimum value of nodes in col -> setting it in saveHeader and updating nodeCounter
                {
                    nodeCounter = tempHeader.headerNodeCount;
                    saveHeader = tempHeader;
                }
                tempHeader = (HeaderNode)tempHeader.right; // move to the next header (left to right)
            }
            
            return saveHeader; // returning the column header with the mininum amount of nodes under it
        }

        public string ConvertBackToMatrix() // converting the solution list back into a matrix and printing the solved sudoku board
        {
            int[,] solvedBoard = new int[SIZE, SIZE]; // initializing the solved board matrix to be the size of the given board

            foreach (Node node in solution) // go over each node in the solution list 
            {
                int minName = node.columnHeader.name; // get the name of the current node's header
                Node tempNode = node; // set a temp node to be the current node

                for (Node temp = node.right; temp != node; temp = temp.right) // go left to right and search fo the minimal column Header
                {
                    int name = temp.columnHeader.name;
                    if (name < minName) // if a new minimal was found, updating
                    {
                        minName = name;
                        tempNode = temp;
                    }
                }

                int columnHeaderPos = tempNode.columnHeader.name; // the position of the column header in the constraints matrix is the name of tempNode's column header
                int row = columnHeaderPos / SIZE; // the row of the current node in the constraints matrix is the column header's position divided by the size of the board
                int col = columnHeaderPos % SIZE; // the column of the current node in the constraints matrix is the column header's position modulo the size of the board

                // the value of the current cell in the board is the name of the node to the right of the current node modulo the size of the board
                solvedBoard[row, col] = tempNode.right.columnHeader.name % SIZE + 1;
            }

            // creating a string builder to store the solved puzzle -> appending to it char by char
            StringBuilder solvedSodukoString = new();

            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    solvedSodukoString.Append((char)(solvedBoard[row, col] + '0')); // converting back the values to their assigned chars
                }
            }

            // printing solved sudoku as string
            Console.WriteLine("\nTHE SOLVED SODUKO PUZZLE IS:");
            Console.ForegroundColor = ConsoleColor.Green; // changing console to green

            // writing the solved sudoku board to the console char by char
            for (int stringIndex=0;stringIndex<solvedSodukoString.Length;stringIndex++)
                Console.Write(solvedSodukoString[stringIndex]);

            Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray

            // also returning the soduko string (for later tests)
            return solvedSodukoString.ToString();
        }

        private static void Cover(Node targetNode) // covering the given node completely -> unlinking it from the mesh
        {
            //unlinking node's right links 
            targetNode.left.right = targetNode.right;
            targetNode.right.left = targetNode.left;

            // defining the nodeBelow and currentNode
            Node nodeBelow = targetNode.down;
            Node currentNode;

            while (nodeBelow != targetNode) // traversing through the mesh and covering the links of the given node
            {
                currentNode = nodeBelow.right;

                while (currentNode != nodeBelow) // until didn't reach the currentNode from the "top"
                {
                    // unlink each of the up/down links of the nodes in the same column
                    currentNode.up.down = currentNode.down;
                    currentNode.down.up = currentNode.up;

                    currentNode.columnHeader.headerNodeCount--; // decrease this column header's node count

                    currentNode = currentNode.right; // move to the node to the right in the same column
                }
                nodeBelow = nodeBelow.down; // go down a row
            }
        }

        private static void Uncover(Node targetNode) // uncovering the given node completely -> restoring it to the board, connecting back it's links
        {
            // relink node's right links
            targetNode.left.right = targetNode;
            targetNode.right.left = targetNode;

            // defining the nodeAbove and currentNode
            Node nodeAbove = targetNode.up;
            Node currentNode;

            while (nodeAbove != targetNode) // traversing through the mesh and uncovering the links of the given node
            {
                currentNode = nodeAbove.left;

                while (currentNode != nodeAbove)  // until didn't reach the currentNode from the "below"
                {
                    // relink each of the up/down links of the nodes in the same column
                    currentNode.up.down = currentNode;
                    currentNode.down.up = currentNode;

                    currentNode.columnHeader.headerNodeCount++; // increase this column header's node count

                    currentNode = currentNode.left; // move to the node to the right in the same column
                }
                nodeAbove = nodeAbove.up; // go up a row
            }
        }
    }
}
