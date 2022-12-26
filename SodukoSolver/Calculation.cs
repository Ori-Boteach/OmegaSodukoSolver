namespace SodukoSolver
{
    class Calculation
    {
        public bool SolveSudoku() // solving the soduko by the backtracing algorithm -> recursively calling itself
        {
            // variables to store the position of the first empty cell
            int row = 0;
            int col = 0;
            bool isEmpty = true;

            // searching for the first empty cell
            for (int i = 0; i < UI.SIZE; i++)
            {
                for (int j = 0; j < UI.SIZE; j++)
                {
                    if (UI.initialSodukoBoard[i, j] == 0) // if found empty cell -> storing it's information
                    {
                        row = i;
                        col = j;
                        isEmpty = false;
                        break;
                    }
                }
                if (!isEmpty)
                    break;
            }

            // if no empty cells are found, the puzzle is already solved
            if (isEmpty)
                return true;

            // trying to fill the empty cell with a number from 1 to 9
            for (int num = 1; num <= UI.SIZE; num++)
            {
                if (CanBePlaced(row, col, num))
                {
                    UI.initialSodukoBoard[row, col] = num; // placing the correct number in the empty cell

                    if (SolveSudoku()) // the function is recursively calling itself now that this position is solved
                        return true;
                    else
                        UI.initialSodukoBoard[row, col] = 0; // can't position a number in there yet
                }
            }
            return false;
        }

        // checking if it is safe to place a number in a given cell
        public bool CanBePlaced(int row, int col, int num)
        {
            for (int i = 0; i < UI.SIZE; i++)
            {
                if (UI.initialSodukoBoard[row, i] == num) // checking num's column for an already exsiting identical
                    return false;
            }

            for (int i = 0; i < UI.SIZE; i++) // checking num's row for an already exsiting identical
            {
                if (UI.initialSodukoBoard[i, col] == num)
                    return false;
            }

            int startRow = row - row % (UI.SIZE / 3);
            int startCol = col - col % (UI.SIZE / 3);

            for (int i = startRow; i < startRow + (UI.SIZE / 3); i++) // checking num's 3X3 cube for an already exsiting identical
            {
                for (int j = startCol; j < startCol + (UI.SIZE / 3); j++)
                {
                    if (UI.initialSodukoBoard[i, j] == num)
                        return false;
                }
            }
            return true; // if num passed all tests -> returns TRUE to SolveSudoku
        }
    }
}
