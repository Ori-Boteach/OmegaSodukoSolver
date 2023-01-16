namespace SodukoSolver.Backtracking
{
    public class OptimizeHelpers
    {
        // updating the possible values for the cells in the same row, column, and block of the cell in the given row and column
        private static void UpdatePossiblesForCells(int row, int col)
        {
            int value = UI.initialSodukoBoard[row, col].Value; // get the value that was just positioned in the board

            // update the candidates of the cells in the same row (while staying clear of the cell that was just positioned)
            for (int i = 0; i < UI.SIZE; i++)
            {
                if (i != col)
                    UI.initialSodukoBoard[row, i].PossibleValues.Remove(value);
            }

            // update the candidates of the cells in the same column (while staying clear of the cell that was just positioned)
            for (int i = 0; i < UI.SIZE; i++)
            {
                if (i != row)
                    UI.initialSodukoBoard[i, col].PossibleValues.Remove(value);
            }

            int blockRowStart = row - row % UI.CUBE_SIZE; // finding the row of the first cell in the block
            int blockColStart = col - col % UI.CUBE_SIZE; // finding the column of the first cell in the block

            // update the candidates of the cells in the same block (while staying clear of the cell that was just positioned)
            for (int i = blockRowStart; i < blockRowStart + UI.CUBE_SIZE; i++)
            {
                for (int j = blockColStart; j < blockColStart + UI.CUBE_SIZE; j++)
                {
                    if (i != row && j != col)
                        UI.initialSodukoBoard[i, j].PossibleValues.Remove(value);
                }
            }
        }
        
        public static bool RowHiddenSingles(bool positionedValue)
        {
            // going over each row
            for (int row = 0; row < UI.SIZE; row++)
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
                        UI.initialSodukoBoard[row, col].Value = value; // setting the value in the cell
                        UI.initialSodukoBoard[row, col].PossibleValues.Clear(); // clearing the possible values hashset
                        // updating the possible values for the cells in the same row, column, and block according to the new positioning
                        UpdatePossiblesForCells(row, col);
                        positionedValue = true; // there has been a positioning of a value in the board
                    }
                }
            }
            return positionedValue;
        }
        public static bool ColHiddenSingles(bool positionedValue)
        {
            // going over each column
            for (int col = 0; col < UI.SIZE; col++)
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
                        UI.initialSodukoBoard[row, col].Value = value; // setting the value in the cell
                        UI.initialSodukoBoard[row, col].PossibleValues.Clear(); // clearing the possible values hashset
                        // updating the possible values for the cells in the same row, column, and block according to the new positioning
                        UpdatePossiblesForCells(row, col);
                        positionedValue = true; // there has been a positioning of a value in the board
                    }
                }
            }
            return positionedValue;
        }

        public static bool CubeHiddenSingles(bool positionedValue)
        {
            for (int blockRow = 0; blockRow < UI.CUBE_SIZE; blockRow++) // go over each sub-cube
            {
                for (int blockCol = 0; blockCol < UI.CUBE_SIZE; blockCol++)
                {
                    for (int value = 1; value <= UI.SIZE; value++) // check for each value
                    {
                        int count = 0;
                        int row = 0;
                        int col = 0;

                        // count the number of cells in the block that have the value as a possibility
                        for (int i = blockRow * UI.CUBE_SIZE; i < blockRow * UI.CUBE_SIZE + UI.CUBE_SIZE; i++)
                        {
                            for (int j = blockCol * UI.CUBE_SIZE; j < blockCol * UI.CUBE_SIZE + UI.CUBE_SIZE; j++)
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
                            UI.initialSodukoBoard[row, col].Value = value; // setting the value in the cell
                            UI.initialSodukoBoard[row, col].PossibleValues.Clear(); // clearing the possible values hashset
                            // updating the possible values for the cells in the same row, column, and block according to the new positioning
                            UpdatePossiblesForCells(row, col);
                            positionedValue = true; // there has been a positioning of a value in the board
                        }
                    }
                }
            }
            return positionedValue;
        }

        // helper function for HiddenDoubles to get all the values in a specific row
        public static HashSet<int> GetRowValues(int row)
        {
            HashSet<int> values = new(); // create a new - empty, hashset of values

            // go through all the cells in the given row and if they are not empty, add their values to the created hashset
            for (int col = 0; col < UI.SIZE; col++)
            {
                Cell cell = UI.initialSodukoBoard[row, col];

                if (cell.Value != 0) // only consider cells with a value
                    values.Add(cell.Value);
            }
            return values;
        }

        // helper function for HiddenDoubles to get all the values in a specific column
        public static HashSet<int> GetColumnValues(int col)
        {
            HashSet<int> values = new(); // create a new - empty, hashset of values

            // go through all the cells in the given col and if they are not empty, add their values to the created hashset
            for (int row = 0; row < UI.SIZE; row++)
            {
                Cell cell = UI.initialSodukoBoard[row, col];

                if (cell.Value != 0) // only consider cells with a value
                    values.Add(cell.Value);
            }
            return values;
        }

        // helper function for HiddenDoubles to get all the values in the specific cube that contains the current cell
        public static HashSet<int> GetCubeValues(int row, int col)
        {
            HashSet<int> values = new(); // create a new - empty, hashset of values

            // calculate the top left cell of the cube that contains the current cell
            int cubeStartRow = row - row % UI.CUBE_SIZE;
            int cubeStartCol = col - col % UI.CUBE_SIZE;

            // go through all the cells in the cube (given row and col) and if they are not empty,
            // add their values to the created hashset
            for (int i = 0; i < UI.CUBE_SIZE; i++)
            {
                for (int j = 0; j < UI.CUBE_SIZE; j++)
                {
                    Cell cell = UI.initialSodukoBoard[cubeStartRow + i, cubeStartCol + j];

                    if (cell.Value != 0) // only consider cells with a value
                        values.Add(cell.Value);
                }
            }
            return values;
        }

        //  for HiddenDoubles: removing from empty cells in the given row, value possibilities that are in the given hashset
        public static void RemoveValuesFromRow(int row, HashSet<int> values)
        {
            for (int col = 0; col < UI.SIZE; col++)
            {
                Cell cell = UI.initialSodukoBoard[row, col];

                if (cell.Value == 0) // only consider empty cells
                    cell.PossibleValues.ExceptWith(values);
            }
        }

        //  for HiddenDoubles: removing from empty cells in the given col, value possibilities that are in the given hashset
        public static void RemoveValuesFromColumn(int col, HashSet<int> values)
        {
            for (int row = 0; row < UI.SIZE; row++)
            {
                Cell cell = UI.initialSodukoBoard[row, col];

                if (cell.Value == 0) // only consider empty cells
                    cell.PossibleValues.ExceptWith(values);
            }
        }

        // for HiddenDoubles: removing from empty cells in the cube (given row and col), value possibilities that are in the given hashset
        public static void RemoveValuesFromCube(int row, int col, HashSet<int> values)
        {
            // calculate the top left cell of the cube that contains the current cell
            int cubeStartRow = row - row % UI.CUBE_SIZE;
            int cubeStartCol = col - col % UI.CUBE_SIZE;

            for (int i = 0; i < UI.CUBE_SIZE; i++)
            {
                for (int j = 0; j < UI.CUBE_SIZE; j++)
                {
                    Cell cell = UI.initialSodukoBoard[cubeStartRow + i, cubeStartCol + j];

                    if (cell.Value == 0) // only consider empty cells
                        cell.PossibleValues.ExceptWith(values);
                }
            }
        }
    }
}