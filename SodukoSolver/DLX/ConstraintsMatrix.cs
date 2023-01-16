namespace SodukoSolver.DLX
{
    public class ConstraintsMatrix
    { 
        public static int[,] ConvertSudokuBoard(int[,] matrix) // converting the given sudoku board to a big 0/1 matrix according to sudoku constraints
        {
            // setting global variables locally to shorten used names
            int SIZE = UI.SIZE;
            int CUBE_SIZE = UI.CUBE_SIZE;

            DancingLinksSolver dancingLinksSolver = new(); // creating an instance of the DancingLinksSolver class

            int[,] ConstraintsMatrix = new int[dancingLinksSolver.numRows, dancingLinksSolver.numCols]; // creating a new matrix with the right size -> (n^3, 4n^2)

            int currentCellCol = 0; // cell constraint -> from the 1st col of the constraints matrix
            int currentCol = SIZE * SIZE * 2; // col constraint -> from the 3rd col of the constraints matrix
            int currentCube = SIZE * SIZE * 3; // cube constraint -> from the 4th col of the constraints matrix

            int rowIndex = 0;

            
            for (int row = 0; row < SIZE; row++) // this loop iterates through each row of the input matrix 
            {
                int currentRow = SIZE * SIZE; // row constraint -> from the 2nd col of the constraints matrix

                for (int col = 0; col < SIZE; col++) // this loop iterates through each column of the input matrix 
                {
                    int value = matrix[row, col];
                    
                    for (int possibleValue = 1; possibleValue <= SIZE; possibleValue++)
                    {
                        if (value == 0 || value == possibleValue) // if the cell is empty or the cell's value is equal to the current possible value
                        {
                            // insert 1's for every constraint that the current cell satisfies
                            ConstraintsMatrix[rowIndex, currentCellCol] = 1;
                            ConstraintsMatrix[rowIndex, currentRow] = 1;
                            ConstraintsMatrix[rowIndex, currentCol + possibleValue - 1] = 1;
                            ConstraintsMatrix[rowIndex, currentCube + SIZE * (row / CUBE_SIZE * CUBE_SIZE + col / CUBE_SIZE) + possibleValue - 1] = 1;
                        }

                        // increase the constraints indicators
                        rowIndex++;
                        currentRow++;
                    }
                    currentCellCol++;
                }
                currentCol += SIZE;
            }
            return ConstraintsMatrix; // return the new constraints matrix
        }
    }
}
