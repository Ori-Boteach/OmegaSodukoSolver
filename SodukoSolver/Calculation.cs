﻿namespace SodukoSolver
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

            // trying to fill the empty cell with a number from 1 to Size
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
        
        public bool SimpleElimination()
        {
            bool positionedValue = false; // checking if there was a positioning of a value in the board

            for (int row = 0; row < UI.SIZE; row++) // going over the board and looking for cells with only one possible value
            {
                for (int col = 0; col < UI.SIZE; col++)
                {
                    if (UI.initialSodukoBoard[row, col] == 0) // only if doesnt have a value yet
                    {
                        // an array to store all the possible values for this current cell
                        int[] possibleValues = new int[UI.SIZE];

                        // if a value is already in row or coumn -> marking it in the possibleValues array
                        for (int i = 0; i < UI.SIZE; i++)
                        {
                            if (UI.initialSodukoBoard[row, i] > 0)
                                possibleValues[UI.initialSodukoBoard[row, i] - 1] = 1;
                            
                            if (UI.initialSodukoBoard[i, col] > 0)
                                possibleValues[UI.initialSodukoBoard[i, col] - 1] = 1;
                        }

                        // if a value is already in cube -> marking it in the possibleValues array
                        int cubeRow = row - row % (UI.SIZE / (int)Math.Sqrt(UI.SIZE));
                        int cubeCol = col - col % (UI.SIZE / (int)Math.Sqrt(UI.SIZE));
                        for (int i = cubeRow; i < cubeRow + (UI.SIZE / (int)Math.Sqrt(UI.SIZE)); i++)
                        {
                            for (int j = cubeCol; j < cubeCol + (UI.SIZE / (int)Math.Sqrt(UI.SIZE)); j++)
                            {
                                if (UI.initialSodukoBoard[i, j] > 0)
                                    possibleValues[UI.initialSodukoBoard[i, j] - 1] = 1;
                            }
                        }

                        // counting the number of possible values for the current cell
                        int count = 0;
                        for (int i = 0; i < UI.SIZE; i++)
                        {
                            if (possibleValues[i] == 0) // -> the value i+1 doesn't conflict with the current value
                                count++;
                        }

                        // if there is only one possible value, filling it into the board
                        if (count == 1)
                        {
                            for (int i = 0; i < UI.SIZE; i++)
                            {
                                if (possibleValues[i] == 0)
                                {
                                    UI.initialSodukoBoard[row, col] = i + 1;
                                    positionedValue = true;
                                    break; // entered the only value, exiting the loop
                                }
                            }
                        }
                    }
                }
            }            
            return positionedValue;
        }

        // checking if it is safe to place a number in the given cell
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

            int startRow = row - row % (UI.SIZE / (int)Math.Sqrt(UI.SIZE));
            int startCol = col - col % (UI.SIZE / (int)Math.Sqrt(UI.SIZE));

            for (int i = startRow; i < startRow + (UI.SIZE / (int)Math.Sqrt(UI.SIZE)); i++) // checking num's cube for an already exsiting identical
            {
                for (int j = startCol; j < startCol + (UI.SIZE / (int)Math.Sqrt(UI.SIZE)); j++)
                {
                    if (UI.initialSodukoBoard[i, j] == num)
                        return false;
                }
            }
            return true; // if num passed all tests -> returns TRUE to SolveSudoku
        }
    }
}
