namespace SodukoSolver
{
    class Optimize
    {
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
