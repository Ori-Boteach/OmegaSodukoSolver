using System;

namespace SodukoSolver
{
    class Calculation
    {
        public const int SIZE = 9;

        public int[,] SolveSudoku(int[,] initialSodukoBoard) // solving the soduko by the backtracing algorithm -> recursively calling itself
        {
            
        }

        // checking if it is safe to place a number in a given cell
        static bool CanBePlaced(int[,] initialSodukoBoard, int row, int col, int num)
        {
            for (int i = 0; i < SIZE; i++)
            {
                if (initialSodukoBoard[row, i] == num) // checking num's column for an already exsiting identical
                    return false;
            }

            for (int i = 0; i < SIZE; i++) // checking num's row for an already exsiting identical
            {
                if (initialSodukoBoard[i, col] == num)
                    return false;
            }

            int startRow = row - row % (SIZE / 3);
            int startCol = col - col % (SIZE / 3);

            for (int i = startRow; i < startRow + (SIZE / 3); i++) // checking num's 3X3 cube for an already exsiting identical
            {
                for (int j = startCol; j < startCol + (SIZE / 3); j++)
                {
                    if (initialSodukoBoard[i, j] == num)
                        return false;
                }
            }

            return true; // if num passed all tests -> returns TRUE to SolveSudoku
        }
    }
}
