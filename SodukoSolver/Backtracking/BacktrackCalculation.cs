using System.Diagnostics;
using System.Text;

namespace SodukoSolver.Backtracking
{
    class BacktrackCalculation
    {
        // counting the number of empty cells in the sudoku
        public static int CountEmptyCells()
        {
            int countEmpty = 0; // initializing the counter to 0

            for (int row = 0; row < UI.SIZE; row++)
            {
                for (int col = 0; col < UI.SIZE; col++)
                {
                    // if the cell is empty, increase counter
                    if (UI.initialSodukoMatrix[row, col] == 0)
                        countEmpty++;
                }
            }
            return countEmpty;
        }
        
        // the function that handles all of the backtracking solvig operation
        public string InitiateBacktracking() 
        {                
            // setting up the timer and starting it 
            var timer = new Stopwatch();
            timer.Start();


            // if the soduko is more than 90% empty,apply optimization techniques
            if (CountEmptyCells() > 90*UI.SIZE* UI.SIZE / 100) 
            {
                bool appliedOptimization; // a flag to track if an optimization technique was applied
                
                // keep applying optimization techniques until they can no longer be applied
                do
                {
                    appliedOptimization = Optimize.SimpleElimination(); // applying simple elimination algorithm

                    if (!appliedOptimization)
                        appliedOptimization = Optimize.HiddenSingle(); // applying hidden single algorithm

                    if (!appliedOptimization)
                        appliedOptimization = Optimize.NakedPairs(); // applying naked pairs algorithm
                }
                while (appliedOptimization);

                Optimize.HiddenDoubles(); // calling the HiddenDoubles method to optimize the soduko board even farther if possible
            }


            BacktrackCalculation calculation = new(); // creating an instance of the BacktrackCalculation class
            bool answer = calculation.SolveSudoku(); // solving the sudoku using backtracking algorithm
                                                    
            // stopping the timer and printing the answer
            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("\n\nTime taken for BACKTRACKING solve: " + timeTaken.TotalMilliseconds + " milliseconds");

            return SodukoResult(answer); // calling the function that prints the solved string
        }

        // solving the soduko by the backtracing algorithm -> recursively calling itself
        public bool SolveSudoku() 
        {
            // initializing variables to store the position of the last empty cell
            int row = UI.SIZE - 1;
            int col = UI.SIZE - 1;
            
            bool isEmpty = true;

            // searching for the last empty cell
            for (int i = UI.SIZE - 1; i >= 0; i--)
            {
                for (int j = UI.SIZE - 1; j >= 0; j--)
                {
                    // if found empty cell -> storing it's information, changing the flag and breaking
                    if (UI.initialSodukoBoard[i, j].Value == 0)
                    {
                        row = i;
                        col = j;
                        isEmpty = false;
                        break;
                    }
                }
                if (!isEmpty) // if found empty cell -> also breaking from the outer loop
                    break;
            }

            if (isEmpty) // if no empty cells are found, the puzzle is already solved
                return true;

            // trying to fill the empty cell with a number from 1 to soduko's SIZE
            int possibleValues = 0;
            foreach (int value in UI.initialSodukoBoard[row, col].PossibleValues)
                possibleValues |= (1 << (value - 1)); // -> using bitwise operations OR and SHIFT LEFT for a faster operation

            for (int num = 1; num <= UI.SIZE; num++) // checking for every number between 1 and SIZE
            {
                // checking if the current value can be placed if this cell:
                // again, using bitwise operations AND and SHIFT LEFT for a faster operation 
                if ((possibleValues & (1 << (num - 1))) != 0 && CanBePlaced(row, col, num)) 
                {
                    UI.initialSodukoBoard[row, col].Value = num; // placing the correct number in the empty cell

                    // the function is recursively calling itself now that this position is solved
                    if (SolveSudoku())
                        return true;
                    else
                        UI.initialSodukoBoard[row, col].Value = 0; // can't position a number in there yet (0 == empty cell)
                }
            }
            return false;
        }

        // checking if it is safe to place a number in the given cell
        public static bool CanBePlaced(int row, int col, int num)
        {
            // checking num's column for an already exsiting identical
            for (int i = 0; i < UI.SIZE; i++)
            {
                if (UI.initialSodukoBoard[row, i].Value == num)
                    return false;
            }

            // checking num's row for an already exsiting identical
            for (int i = 0; i < UI.SIZE; i++)
            {
                if (UI.initialSodukoBoard[i, col].Value == num)
                    return false;
            }

            // checking num's cube for an already exsiting identical
            int startRow = row - row % (UI.SIZE / UI.CUBE_SIZE); // finding the starting row of the cube
            int startCol = col - col % (UI.SIZE / UI.CUBE_SIZE); // finding the starting column of the cube

            for (int i = startRow; i < startRow + UI.SIZE / UI.CUBE_SIZE; i++)
            {
                for (int j = startCol; j < startCol + UI.SIZE / UI.CUBE_SIZE; j++)
                {
                    // if found identical in the same cube -> returning false
                    if (UI.initialSodukoBoard[i, j].Value == num) 
                        return false;
                }
            }
            return true; // if num passed all tests -> returns true to SolveSudoku
        }

        // a function that returns the answer, if solvable ->  prints the solved soduko, if not -> prints a message
        public static string SodukoResult(bool answer)
        {
            if (!answer) // if the returned value from SolveSudoku is flase -> soduko is UNSOLVABLE
            {
                Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                Console.WriteLine("\n***The soduko is unsolvable***");
                Console.ResetColor(); // reset console color to default (gray)
                return "***The soduko is unsolvable***"; // also returning the string for later tests
            }
            else
            {
                Console.WriteLine("\nTHE SOLVED SODUKO PUZZLE IS:");
                return PrintBoardAsString();
            }
        }

        // printing the solution of the given Soduko puzzle at a string format
        public static string PrintBoardAsString()
        {
            string solvedSodukoString = ConvertBackToString();
            Console.ForegroundColor = ConsoleColor.Green; // changing console to green
            Console.WriteLine(solvedSodukoString);
            Console.ResetColor(); // reset console color to default (gray)
            return solvedSodukoString; // retruning the converted string to calling method
        }

        public static string ConvertBackToString() // converting a soduko represented as a 2D array to string representation
        {
            // creating a string builder to store the solved puzzle -> appending to it char by char
            StringBuilder solvedSodukoString = new();

            for (int row = 0; row < UI.SIZE; row++)
            {
                for (int col = 0; col < UI.SIZE; col++)
                    // converting back the values to their assigned chars
                    solvedSodukoString.Append((char)(UI.initialSodukoBoard[row, col].Value + '0'));
            }

            // returning the soduko string
            return solvedSodukoString.ToString();
        }
    }
}