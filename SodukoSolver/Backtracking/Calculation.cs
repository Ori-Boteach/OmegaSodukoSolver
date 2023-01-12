namespace SodukoSolver.Backtracking
{
    class Calculation
    {
        public bool SolveSudoku() // solving the soduko by the backtracing algorithm -> recursively calling itself
        {
            Optimize optimize = new();
            bool appliedOptimization; // a flag to track if an optimization technique was applied

            // keep applying optimization techniques until they can no longer be applied
            do
            {
                appliedOptimization = optimize.SimpleElimination();

                if (!appliedOptimization)
                    appliedOptimization = optimize.HiddenSingle();

                if (!appliedOptimization)
                    appliedOptimization = optimize.NakedPairs();
            }
            while (appliedOptimization);

            // initialize variables to store the position of the cell with minimum possible values
            int row = 0;
            int col = 0;
            int minPossibles = UI.SIZE + 1; // setting the initial value of minPossibles to be SIZE+1

            // update the position of the cell with the minimum number of possible values if necessary
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

            // if all cells are filled, then the puzzle is already solved
            if (minPossibles == UI.SIZE + 1)
                return true;

            // trying filling the cell with the minimum possible values, with values from 1 to the soduko's SIZE
            foreach (int num in UI.initialSodukoBoard[row, col].PossibleValues)
            {
                if (CanBePlaced(row, col, num)) // checking if the current value can be places if this cell
                {
                    UI.initialSodukoBoard[row, col].Value = num; // placing the correct number in the empty cell

                    if (SolveSudoku()) // the function is recursively calling itself now that this position is solved
                        return true;

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
            int startRow = row - row % (UI.SIZE / UI.CUBE_SIZE);
            int startCol = col - col % (UI.SIZE / UI.CUBE_SIZE);

            for (int i = startRow; i < startRow + UI.SIZE / UI.CUBE_SIZE; i++)
            {
                for (int j = startCol; j < startCol + UI.SIZE / UI.CUBE_SIZE; j++)
                {
                    if (UI.initialSodukoBoard[i, j].Value == num)
                        return false;
                }
            }
            return true; // if num passed all tests -> returns true to SolveSudoku
        }
    }
}