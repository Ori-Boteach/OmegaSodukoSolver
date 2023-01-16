namespace SodukoSolver.Backtracking
{
    class Optimize
    {
        //applying Simple Elimination algorithm -> placing the correct value in cells that have only one value option
        public static bool SimpleElimination()
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
                        int cubeRow = row - row % (UI.SIZE / UI.CUBE_SIZE); // finding the starting row of the cube
                        int cubeCol = col - col % (UI.SIZE / UI.CUBE_SIZE); // finding the starting column of the cube

                        for (int i = cubeRow; i < cubeRow + UI.SIZE / UI.CUBE_SIZE; i++)
                        {
                            for (int j = cubeCol; j < cubeCol + UI.SIZE / UI.CUBE_SIZE; j++)
                            {
                                if (UI.initialSodukoBoard[i, j].Value > 0) // if cell has a value, mark it in the possibleValues array
                                    possibleValues[UI.initialSodukoBoard[i, j].Value - 1] = 1;
                            }
                        }

                        // counting the number of possible values for the current cell
                        int count = 0;
                        int value = 0;
                        for (int i = 0; i < UI.SIZE; i++)
                        {
                            if (possibleValues[i] == 0) // meaning that the value i+1 doesn't conflict with the current value
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

            // returning a boolean value to indicate if there was a positioning of a value in the board
            return positionedValue; 
        }
        
        // applying Hidden Single algorithm:
        // placing the correct value in cells that have only one possible value, after checking other cells in the same row, column, or block
        public static bool HiddenSingle()
        {
            bool positionedValue = false; // checking if there was a positioning of a value in the board

            // calling helper functions for this algorithm
            positionedValue = OptimizeHelpers.RowHiddenSingles(positionedValue);
            positionedValue = OptimizeHelpers.ColHiddenSingles(positionedValue);
            positionedValue = OptimizeHelpers.CubeHiddenSingles(positionedValue);

            // return true if any changes were made, false otherwise
            return positionedValue;
        }

        // applying Naked Pairs algorithm:
        // eliminating candidates from other cells in the same row, column, or block where two cells have the same two candidates
        public static bool NakedPairs()
        {
            bool appliedOptimization = false; // checking if there was an application of the algorithm

            appliedOptimization = RowsNakedPairs(appliedOptimization);
            appliedOptimization = ColsNakedPairs(appliedOptimization);
            appliedOptimization = CubesNakedPairs(appliedOptimization);

            return appliedOptimization; // return boolean vlaue that indicates if the algorithm was useful
        }

        public static bool RowsNakedPairs(bool appliedOptimization)
        {
            // check for Naked Pairs in rows
            for (int row = 0; row < UI.SIZE; row++) // go over each row
            {
                for (int col = 0; col < UI.SIZE; col++) // go over each col
                {
                    // only if the cell is empty and has two possible values
                    if (UI.initialSodukoBoard[row, col].Value == 0 && UI.initialSodukoBoard[row, col].PossibleValues.Count == 2)
                    {
                        bool hasPair = false; // assuming false
                        int saveOtherPair = row; // saving the other cell in the pair (being initialized as the index of the first pair)

                        for (int i = 0; i < UI.SIZE; i++) // for every col index in the row
                        {
                            // checking if there are any other empty cells in the same row that have the same 2 possible values
                            if (i != row && UI.initialSodukoBoard[i, col].Value == 0 &&
                                UI.initialSodukoBoard[i, col].PossibleValues.SetEquals(UI.initialSodukoBoard[row, col].PossibleValues))
                            {
                                hasPair = true;
                                saveOtherPair = i;
                                break;
                            }
                        }

                        if (hasPair) // found a pair
                        {
                            appliedOptimization = true;

                            // naked pair found, eliminate its values as possibilities for all other cells in the same row
                            for (int i = 0; i < UI.SIZE; i++)
                            {
                                if (i != row && i != saveOtherPair && UI.initialSodukoBoard[i, col].Value == 0) // if empty and not one of the pairs
                                    // removing the possibilities of the pairs from other empty cells
                                    foreach (int value in UI.initialSodukoBoard[row, col].PossibleValues)
                                        appliedOptimization |= UI.initialSodukoBoard[i, col].PossibleValues.Remove(value);
                            }
                        }
                    }
                }
            }
            return appliedOptimization;
        }
        public static bool ColsNakedPairs(bool appliedOptimization)
        {
            // check for Naked Pairs in columns
            for (int col = 0; col < UI.SIZE; col++) // go over each col
            {
                for (int row = 0; row < UI.SIZE; row++) // go over each row
                {
                    // only if the cell is empty and has two possible values
                    if (UI.initialSodukoBoard[row, col].Value == 0 && UI.initialSodukoBoard[row, col].PossibleValues.Count == 2)
                    {
                        int saveOtherPair = col; // saving the other cell in the pair (being initialized as the index of the first pair)
                        bool hasPair = false; // assuming false

                        for (int i = 0; i < UI.SIZE; i++)
                        {
                            // checking if there are any other empty cells in the same col that have the same 2 possible values
                            if (i != col && UI.initialSodukoBoard[row, i].Value == 0 &&
                                UI.initialSodukoBoard[row, i].PossibleValues.SetEquals(UI.initialSodukoBoard[row, col].PossibleValues))
                            {
                                hasPair = true;
                                saveOtherPair = i;
                                break;
                            }
                        }

                        if (hasPair) // found a pair
                        {
                            appliedOptimization = true;

                            // naked pair found, eliminate its values as possibilities for all other cells in the same col
                            for (int i = 0; i < UI.SIZE; i++)
                            {
                                if (i != col && i != saveOtherPair && UI.initialSodukoBoard[row, i].Value == 0) // if empty and not one of the pairs
                                    // removing the possibilities of the pairs from other empty cells
                                    foreach (int value in UI.initialSodukoBoard[row, col].PossibleValues)
                                        appliedOptimization |= UI.initialSodukoBoard[i, col].PossibleValues.Remove(value);
                            }
                        }
                    }
                }
            }
            return appliedOptimization;
        }
        public static bool CubesNakedPairs(bool appliedOptimization)
        {
            // check for Naked Pairs in sub cubes
            for (int subgridRow = 0; subgridRow < UI.CUBE_SIZE; subgridRow++) // for all sub cubes rows
            {
                for (int subgridCol = 0; subgridCol < UI.CUBE_SIZE; subgridCol++) // for all sub cubes cols
                {
                    for (int row = subgridRow * UI.CUBE_SIZE; row < (subgridRow + 1) * UI.CUBE_SIZE; row++) // current sub cube starting row
                    {
                        for (int col = subgridCol * UI.CUBE_SIZE; col < (subgridCol + 1) * UI.CUBE_SIZE; col++) // current sub cube starting col
                        {
                            // only if the cell is empty and has two possible values
                            if (UI.initialSodukoBoard[row, col].Value == 0 && UI.initialSodukoBoard[row, col].PossibleValues.Count == 2)
                            {
                                // Check if there are any other cells in the same subgrid that have the same 2 possible values
                                bool hasPair = false; // assuming false

                                for (int i = subgridRow * UI.CUBE_SIZE; i < (subgridRow + 1) * UI.CUBE_SIZE; i++)
                                {
                                    for (int j = subgridCol * UI.CUBE_SIZE; j < (subgridCol + 1) * UI.CUBE_SIZE; j++)
                                    {
                                        // checking if there are any other empty cells in the same cube that have the same 2 possible values
                                        if (i != row && j != col && UI.initialSodukoBoard[i, j].Value == 0 &&
                                            UI.initialSodukoBoard[i, j].PossibleValues.SetEquals(UI.initialSodukoBoard[row, col].PossibleValues))
                                        {
                                            hasPair = true;
                                            break;
                                        }
                                    }
                                }

                                if (hasPair) // found a pair
                                {
                                    appliedOptimization = true;

                                    // Naked pair found, eliminate its values as possibilities for all other cells in the same subgrid
                                    for (int i = subgridRow * UI.CUBE_SIZE; i < (subgridRow + 1) * UI.CUBE_SIZE; i++)
                                    {
                                        for (int j = subgridCol * UI.CUBE_SIZE; j < (subgridCol + 1) * UI.CUBE_SIZE; j++)
                                        {
                                            if (i != row && j != col && UI.initialSodukoBoard[i, j].Value == 0)  // if empty and not one of the pairs
                                                // removing the possibilities of the pairs from other empty cells
                                                foreach (int value in UI.initialSodukoBoard[row, col].PossibleValues)
                                                    appliedOptimization |= UI.initialSodukoBoard[row, i].PossibleValues.Remove(value);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return appliedOptimization;
        }

        // applying Hidden Doubles algorithm:
        // placing the correct values in cells that have only two possible values,
        // after looking for two cells in the same row, column or block that share the same two candidates,
        // meaning that no other cell in those constraints can contain those values
        public static void HiddenDoubles()
        {
            for (int row = 0; row < UI.SIZE; row++) // go over each row
            {
                for (int col = 0; col < UI.SIZE; col++) // go over each column
                {
                    Cell cell = UI.initialSodukoBoard[row, col]; // create a cell object of the current cell in the board

                    if (cell.Value == 0) // only consider empty cells
                    {
                        // get hashset of the values in the same row, col and cube by the helper functions below
                        HashSet<int> rowValues = OptimizeHelpers.GetRowValues(row);
                        HashSet<int> colValues = OptimizeHelpers.GetColumnValues(col);
                        HashSet<int> cubeValues = OptimizeHelpers.GetCubeValues(row, col);

                        // find the intersection of the possible values for the row, column, and cube and activate helper functions
                        HashSet<int> intersection = new(rowValues);
                        intersection.IntersectWith(colValues);
                        intersection.IntersectWith(cubeValues);

                        // if there are two or more values in the intersection, remove them from the PossibleValues sets
                        // of all other cells in the row, column, and cube
                        if (intersection.Count == 2)
                        {
                            OptimizeHelpers.RemoveValuesFromRow(row, intersection);
                            OptimizeHelpers.RemoveValuesFromColumn(col, intersection);
                            OptimizeHelpers.RemoveValuesFromCube(row, col, intersection);
                        }
                    }
                }
            }
        }
    }
}