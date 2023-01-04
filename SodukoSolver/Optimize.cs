namespace SodukoSolver
{
    class Optimize
    {
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
        
        public void HiddenDoubles()
        {
            for (int row = 0; row < UI.SIZE; row++)
            {
                for (int col = 0; col < UI.SIZE; col++)
                {
                    Cell cell = UI.initialSodukoBoard[row, col];
                    if (cell.Value == 0) // only consider empty cells
                    {
                        HashSet<int> rowValues = GetRowValues(row);
                        HashSet<int> colValues = GetColumnValues(col);
                        HashSet<int> cubeValues = GetCubeValues(row, col);

                        // find the intersection of the possible values for the row, column, and cube
                        HashSet<int> intersection = new HashSet<int>(rowValues);
                        intersection.IntersectWith(colValues);
                        intersection.IntersectWith(cubeValues);

                        // if there are two or more values in the intersection, remove them from the PossibleValues sets of all other cells in the row, column, and cube
                        if (intersection.Count == 2)
                        {
                            RemoveValuesFromRow(row, intersection);
                            RemoveValuesFromColumn(col, intersection);
                            RemoveValuesFromCube(row, col, intersection);
                        }
                    }
                }
            }
        }

        private HashSet<int> GetRowValues(int row)
        {
            HashSet<int> values = new HashSet<int>();
            for (int col = 0; col < UI.SIZE; col++)
            {
                Cell cell = UI.initialSodukoBoard[row, col];
                if (cell.Value != 0) // only consider cells with a value
                {
                    values.Add(cell.Value);
                }
            }
            return values;
        }
        private HashSet<int> GetColumnValues(int col)
        {
            HashSet<int> values = new HashSet<int>();
            for (int row = 0; row < UI.SIZE; row++)
            {
                Cell cell = UI.initialSodukoBoard[row, col];
                if (cell.Value != 0) // only consider cells with a value
                {
                    values.Add(cell.Value);
                }
            }
            return values;
        }

        private HashSet<int> GetCubeValues(int row, int col)
        {
            HashSet<int> values = new HashSet<int>();

            // calculate the top left cell of the cube that contains the current cell
            int cubeStartRow = row - (row % (int)Math.Sqrt(UI.SIZE));
            int cubeStartCol = col - (col % (int)Math.Sqrt(UI.SIZE));

            for (int i = 0; i < (int)Math.Sqrt(UI.SIZE); i++)
            {
                for (int j = 0; j < (int)Math.Sqrt(UI.SIZE); j++)
                {
                    Cell cell = UI.initialSodukoBoard[cubeStartRow + i, cubeStartCol + j];
                    if (cell.Value != 0) // only consider cells with a value
                    {
                        values.Add(cell.Value);
                    }
                }
            }
            return values;
        }

        private void RemoveValuesFromRow(int row, HashSet<int> values)
        {
            for (int col = 0; col < UI.SIZE; col++)
            {
                Cell cell = UI.initialSodukoBoard[row, col];
                if (cell.Value == 0) // only consider empty cells
                {
                    cell.PossibleValues.ExceptWith(values);
                }
            }
        }

        private void RemoveValuesFromColumn(int col, HashSet<int> values)
        {
            for (int row = 0; row < UI.SIZE; row++)
            {
                Cell cell = UI.initialSodukoBoard[row, col];
                if (cell.Value == 0) // only consider empty cells
                {
                    cell.PossibleValues.ExceptWith(values);
                }
            }
        }

        private void RemoveValuesFromCube(int row, int col, HashSet<int> values)
        {
            // calculate the top left cell of the cube that contains the current cell
            int cubeStartRow = row - (row % (int)Math.Sqrt(UI.SIZE));
            int cubeStartCol = col - (col % (int)Math.Sqrt(UI.SIZE));

            for (int i = 0; i < (int)Math.Sqrt(UI.SIZE); i++)
            {
                for (int j = 0; j < (int)Math.Sqrt(UI.SIZE); j++)
                {
                    Cell cell = UI.initialSodukoBoard[cubeStartRow + i, cubeStartCol + j];

                    if (cell.Value == 0) // only consider empty cells
                        cell.PossibleValues.ExceptWith(values);
                }
            }
        }
    }
}
