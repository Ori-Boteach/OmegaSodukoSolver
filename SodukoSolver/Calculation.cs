namespace SodukoSolver
{
    class Calculation
    {
        static Stack<(int, int)> emptyCells = new Stack<(int, int)>();

        // old method
        public bool SolveSudoku() // solving the soduko by the backtracing algorithm -> recursively calling itself
         {
            // initializing variables to store the position of the first empty cell
            int row = 0;
            int col = 0;
            bool isEmpty = true;

            // searching for the first empty cell
            for (int i = 0; i < UI.SIZE; i++)
            {
                for (int j = 0; j < UI.SIZE; j++)
                {
                    if (UI.initialSodukoBoard[i, j].Value == 0) // if found empty cell -> storing it's information
                    {
                        row = i;
                        col = j;
                        isEmpty = false;
                        break;
                    }
                }
                if (!isEmpty) // if found empty cell -> breaking from the loop
                    break;
            }

            if (isEmpty) // if no empty cells are found, the puzzle is already solved
                return true;

            // trying to fill the empty cell with a number from 1 to soduko's SIZE
            foreach (int num in UI.initialSodukoBoard[row, col].PossibleValues)
            {
                if (CanBePlaced(row, col, num)) // checking if the current value can be places if this cell
                {
                    UI.initialSodukoBoard[row, col].Value = num; // placing the correct number in the empty cell

                    if (SolveSudoku()) // the function is recursively calling itself now that this position is solved
                        return true;
                    else
                        UI.initialSodukoBoard[row, col].Value = 0; // can't position a number in there yet (0 == empty cell)
                }
            }
            return false;
        }

        //public bool populate()
        //{
        //    // populating the emptyCells stack with the coordinates of the empty cells in the board
        //    for (int row = 0; row < UI.SIZE; row++)
        //    {
        //        for (int col = 0; col < UI.SIZE; col++)
        //        {
        //            if (UI.initialSodukoBoard[row, col].Value == 0)
        //            {
        //                emptyCells.Push((row, col));
        //            }
        //        }
        //    }
        //    return SolveSudoku(); // calling SolveSudoku
        //}

        //public bool SolveSudoku()
        //{
        //    int row, col;
        //    (int row, int col) poppedTuple; // create a tuple to store the popped value from the stack

        //    if (emptyCells.TryPop(out poppedTuple)) // poping the next empty cell from the stack
        //    {
        //        row = poppedTuple.Item1;
        //        col = poppedTuple.Item2;

        //        // trying to fill the empty cell with a number from 1 to soduko's SIZE
        //        foreach (int num in UI.initialSodukoBoard[row, col].PossibleValues)
        //        {
        //            if (CanBePlaced(row, col, num))
        //            {
        //                UI.initialSodukoBoard[row, col].Value = num;

        //                // the function is recursively calling itself now that this position is solved
        //                if (SolveSudoku())
        //                    return true;
        //                else
        //                    UI.initialSodukoBoard[row, col].Value = 0; // can't position a number in there yet (0 == empty cell)
        //            }
        //        }

        //        // pushing the cell back onto the stack so it can be tried again later
        //        emptyCells.Push((row, col));
        //        return false;
        //    }

        //    // if the stack is empty, all empty cells have been filled and the puzzle is solved
        //    return true;
        //}

        //applying Simple Elimination algorithm -> placing the correct value in cells that have only one value option
        

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
            int startRow = row - row % (UI.SIZE / (int)Math.Sqrt(UI.SIZE));
            int startCol = col - col % (UI.SIZE / (int)Math.Sqrt(UI.SIZE));

            for (int i = startRow; i < startRow + (UI.SIZE / (int)Math.Sqrt(UI.SIZE)); i++)
            {
                for (int j = startCol; j < startCol + (UI.SIZE / (int)Math.Sqrt(UI.SIZE)); j++)
                {
                    if (UI.initialSodukoBoard[i, j].Value == num)
                        return false;
                }
            }
            return true; // if num passed all tests -> returns true to SolveSudoku
        }
    }
}
