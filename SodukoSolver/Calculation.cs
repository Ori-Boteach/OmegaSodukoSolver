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
        public bool SimpleElimination()
        {
            bool positionedValue = false; // checking if there was a positioning of a value in the board

            for (int row = 0; row < UI.SIZE; row++) // going over the board and looking for cells with only one possible value
            {
                for (int col = 0; col < UI.SIZE; col++)
                {
                    if (UI.initialSodukoBoard[row, col].Value == 0) // only if doesn't have a value yet
                    {
                        // an array to store all the possible values for this current cell
                        int[] possibleValues = new int[UI.SIZE];

                        // if a value is already in row or coumn -> marking it in the possibleValues array
                        for (int i = 0; i < UI.SIZE; i++)
                        {
                            if (UI.initialSodukoBoard[row, i].Value > 0)
                                possibleValues[UI.initialSodukoBoard[row, i].Value - 1] = 1;

                            if (UI.initialSodukoBoard[i, col].Value > 0)
                                possibleValues[UI.initialSodukoBoard[i, col].Value - 1] = 1;
                        }

                        // if a value is already in cube -> marking it in the possibleValues array
                        int cubeRow = row - row % (UI.SIZE / (int)Math.Sqrt(UI.SIZE));
                        int cubeCol = col - col % (UI.SIZE / (int)Math.Sqrt(UI.SIZE));
                        for (int i = cubeRow; i < cubeRow + (UI.SIZE / (int)Math.Sqrt(UI.SIZE)); i++)
                        {
                            for (int j = cubeCol; j < cubeCol + (UI.SIZE / (int)Math.Sqrt(UI.SIZE)); j++)
                            {
                                if (UI.initialSodukoBoard[i, j].Value > 0)
                                    possibleValues[UI.initialSodukoBoard[i, j].Value - 1] = 1;
                            }
                        }

                        // counting the number of possible values for the current cell
                        int count = 0;
                        int value = 0;
                        for (int i = 0; i < UI.SIZE; i++)
                        {
                            if (possibleValues[i] == 0) // -> the value i+1 doesn't conflict with the current value
                            {
                                value = i + 1;
                                count++;
                            }
                        }

                        // if there is only one possible value, filling it into the board
                        if (count == 1)
                        {
                            UI.initialSodukoBoard[row, col].Value = value;
                            positionedValue = true;
                        }
                    }
                }
            }
            return positionedValue;
        }

        private void UpdatePossiblesForCells(int row, int col)
        {
            int value = UI.initialSodukoBoard[row, col].Value;

            // update the candidates of the cells in the same row
            for (int i = 0; i < UI.SIZE; i++)
            {
                if (i != col)
                {
                    UI.initialSodukoBoard[row, i].PossibleValues.Remove(value);
                }
            }

            // update the candidates of the cells in the same column
            for (int i = 0; i < UI.SIZE; i++)
            {
                if (i != row)
                {
                    UI.initialSodukoBoard[i, col].PossibleValues.Remove(value);
                }
            }

            // update the candidates of the cells in the same block
            int blockRowStart = row - row % (int)Math.Sqrt(UI.SIZE);
            int blockColStart = col - col % (int)Math.Sqrt(UI.SIZE);
            
            for (int i = blockRowStart; i < blockRowStart + (int)Math.Sqrt(UI.SIZE); i++)
            {
                for (int j = blockColStart; j < blockColStart + (int)Math.Sqrt(UI.SIZE); j++)
                {
                    if (i != row && j != col)
                    {
                        UI.initialSodukoBoard[i, j].PossibleValues.Remove(value);
                    }
                }
            }
        }

        public bool HiddenSingle()
        {
            bool positionedValue = false; // checking if there was a positioning of a value in the board

            for (int row = 0; row < UI.SIZE; row++) // go over each row
            {
                for (int value = 1; value <= UI.SIZE; value++) // check for each value
                {
                    // count the number of cells in the row that have the value as a candidate
                    int count = 0;
                    int col = 0;
                    for (int i = 0; i < UI.SIZE; i++)
                    {
                        if (UI.initialSodukoBoard[row, i].PossibleValues.Contains(value))
                        {
                            count++;
                            col = i;
                        }
                    }

                    // if there is only one cell that has current value as possible, set it
                    if (count == 1)
                    {
                        UI.initialSodukoBoard[row, col].Value = value;
                        UI.initialSodukoBoard[row, col].PossibleValues.Clear();
                        UpdatePossiblesForCells(row, col);
                        positionedValue = true;
                    }
                }
            }

            for (int col = 0; col < UI.SIZE; col++) // go over each col
            {
                for (int value = 1; value <= UI.SIZE; value++) // check for each value
                {
                    // count the number of cells in the column that have the value as a candidate
                    int count = 0;
                    int row = 0;
                    for (int i = 0; i < UI.SIZE; i++)
                    {
                        if (UI.initialSodukoBoard[i, col].PossibleValues.Contains(value))
                        {
                            count++;
                            row = i;
                        }
                    }

                    // if there is only one cell that has current value as possible, set it
                    if (count == 1)
                    {
                        UI.initialSodukoBoard[row, col].Value = value;
                        UpdatePossiblesForCells(row, col);
                        positionedValue = true;
                    }
                }
            }

            for (int blockRow = 0; blockRow < (int)Math.Sqrt(UI.SIZE); blockRow++) // go over each subgrid
            {
                for (int blockCol = 0; blockCol < (int)Math.Sqrt(UI.SIZE); blockCol++)
                {
                    for (int value = 1; value <= UI.SIZE; value++) // check for each value
                    {
                        // count the number of cells in the block that have the value as a possibility
                        int count = 0;
                        int row = 0;
                        int col = 0;
                        for (int i = blockRow * (int)Math.Sqrt(UI.SIZE); i < blockRow * (int)Math.Sqrt(UI.SIZE) + (int)Math.Sqrt(UI.SIZE); i++)
                        {
                            for (int j = blockCol * (int)Math.Sqrt(UI.SIZE); j < blockCol * (int)Math.Sqrt(UI.SIZE) + (int)Math.Sqrt(UI.SIZE); j++)
                            {
                                if (UI.initialSodukoBoard[i, j].PossibleValues.Contains(value))
                                {
                                    count++;
                                    row = i;
                                    col = j;
                                }
                            }
                        }

                        // if there is only one cell that has current value as possible, set it
                        if (count == 1)
                        {
                            UI.initialSodukoBoard[row, col].Value = value;
                            UpdatePossiblesForCells(row, col);
                            positionedValue = true;
                        }
                    }
                }
            }
            // return true if any changes were made, false otherwise
            return positionedValue;
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
    }
}
