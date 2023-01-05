namespace SodukoSolver
{
    class Calculation
    {
        //public bool SolveSudoku() // solving the soduko by the backtracing algorithm -> recursively calling itself
        //{
        //    // initializing variables to store the position of the first empty cell
        //    int row = 0;
        //    int col = 0;
        //    bool isEmpty = true;

        //    // searching for the first empty cell
        //    for (int i = 0; i < UI.SIZE; i++)
        //    {
        //        for (int j = 0; j < UI.SIZE; j++)
        //        {
        //            if (UI.initialSodukoBoard[i, j].Value == 0) // if found empty cell -> storing it's information
        //            {
        //                row = i;
        //                col = j;
        //                isEmpty = false;
        //                break;
        //            }
        //        }
        //        if (!isEmpty) // if found empty cell -> breaking from the loop
        //            break;
        //    }

        //    if (isEmpty) // if no empty cells are found, the puzzle is already solved
        //        return true;

        //    // trying to fill the empty cell with a number from 1 to soduko's SIZE
        //    foreach (int num in UI.initialSodukoBoard[row, col].PossibleValues)
        //    {
        //        if (CanBePlaced(row, col, num)) // checking if the current value can be places if this cell
        //        {
        //            UI.initialSodukoBoard[row, col].Value = num; // placing the correct number in the empty cell

        //            if (SolveSudoku()) // the function is recursively calling itself now that this position is solved
        //                return true;
        //            else
        //                UI.initialSodukoBoard[row, col].Value = 0; // can't position a number in there yet (0 == empty cell)
        //        }
        //    }
        //    return false;
        //}

        public bool SolveSudoku()
        {
            Optimize optimize = new Optimize();
            bool appliedOptimization; // Flag to track if an optimization technique was applied

            // Keep applying optimization techniques until they can no longer be applied
            do
            {
                appliedOptimization = optimize.SimpleElimination();
                if (!appliedOptimization)
                {
                    appliedOptimization = optimize.HiddenSingle();
                }
                if (!appliedOptimization)
                {
                    appliedOptimization = optimize.NakedPairs();
                }
            }
            while (appliedOptimization);

            // Initialize variables to store the position of the cell with the minimum number of possible values
            int row = 0;
            int col = 0;
            int minPossibles = UI.SIZE + 1; // Set the initial value of minPossibles to be greater than SIZE

            // Update the position of the cell with the minimum number of possible values if necessary
            for (int i = 0; i < UI.SIZE; i++)
            {
                for (int j = 0; j < UI.SIZE; j++)
                {
                    if (UI.initialSodukoBoard[i, j].Value == 0 && UI.initialSodukoBoard[i, j].PossibleValues.Count < minPossibles)
                    {
                        row = i;
                        col = j;
                        minPossibles = UI.initialSodukoBoard[i, j].PossibleValues.Count;
                    }
                }
            }

            // If all cells are filled, the puzzle is already solved
            if (minPossibles == UI.SIZE + 1)
                return true;

            // Try filling the cell with the minimum number of possible values
            foreach (int num in UI.initialSodukoBoard[row, col].PossibleValues)
            {
                if (IsValidBitwise(row, col, num))
                {
                    UI.initialSodukoBoard[row, col].Value = num;

                    if (SolveSudoku())
                        return true;

                    UI.initialSodukoBoard[row, col].Value = 0;
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
        
        public static bool IsValidBitwise(int row, int col, int num)
        {
            // check row using bitmask
            int rowMask = 0;
            for (int i = 0; i < UI.SIZE; i++)
            {
                rowMask |= 1 << UI.initialSodukoBoard[row, i].Value;
            }
            if ((rowMask & (1 << num)) > 0)
            {
                return false;
            }

            // check col using bitmask
            int colMask = 0;
            for (int i = 0; i < UI.SIZE; i++)
            {
                colMask |= 1 << UI.initialSodukoBoard[i, col].Value;
            }
            if ((colMask & (1 << num)) > 0)
            {
                return false;
            }

            // check sub grid using bitmask
            int gridMask = 0;
            int startRow = row - row % UI.CUBE_SIZE;
            int startCol = col - col % UI.CUBE_SIZE;
            for (int i = 0; i < UI.CUBE_SIZE; i++)
            {
                for (int j = 0; j < UI.CUBE_SIZE; j++)
                {
                    gridMask |= 1 << UI.initialSodukoBoard[startRow + i, startCol + j].Value;
                }
            }
            if ((gridMask & (1 << num)) > 0)
            {
                return false;
            }

            // number is valid
            return true;
        }
    }
}