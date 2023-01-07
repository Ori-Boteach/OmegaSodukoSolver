namespace SodukoSolver.DLX
{
    public class ConstraintsMatrix
    { 
        public static int[,] ConvertSudoku(int[,] matrix) // converting the given sudoku board to a big 0/1 matrix according to sudoku constraints
        {
            int SIZE = DancingLinksSolver.SIZE;
            
            int[,] newMatrix = new int[DancingLinksSolver.numRows, DancingLinksSolver.numCols];

            int currentCellLocationCol = 0;
            int currentCol = SIZE * SIZE * 2;
            int currentCube = SIZE * SIZE * 3;

            int rowIndex = 0;

            for (int rowI = 0; rowI < SIZE; rowI++)
            {
                int currentRow = SIZE * SIZE;
                
                for (int colI = 0; colI < SIZE; colI++)
                {
                    int value = matrix[rowI, colI];
                    for (int possible = 1; possible <= SIZE; possible++)
                    {
                        if (value == 0 || value == possible)
                        {
                            newMatrix[rowIndex, currentCellLocationCol] = 1;
                            newMatrix[rowIndex, currentRow] = 1;
                            newMatrix[rowIndex, currentCol + possible - 1] = 1;
                            newMatrix[rowIndex, currentCube + SIZE * (rowI / DancingLinksSolver.CUBE_SIZE * DancingLinksSolver.CUBE_SIZE + colI / DancingLinksSolver.CUBE_SIZE) + possible - 1] = 1;
                        }
                        rowIndex++;
                        currentRow++;
                    }
                    currentCellLocationCol++;
                }
                currentCol += SIZE;
            }
            return newMatrix;
        }
    }
}
