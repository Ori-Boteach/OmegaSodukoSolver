namespace SodukoSolver
{
    public class UI
    {
        public const int SIZE = 9;

        public void StartAndValidation() // recieving the input from the user and validating it
        {
            Console.WriteLine("\nPlease enter the soduko pazzle that you need solved at a string format and press enter:");
            string input = Console.ReadLine();

            // if user input is null -> custom exception raised
            if (input == null)
                throw new NullReferenceException("Null input");

            // if user input's length is different than 81 (size of 9X9 cube) -> custom exception raised
            if (input.Length != 81)
                throw new InvalidInputLengthException("Invalid number of chars in inputted string: " + input.Length + " instead of " + SIZE * SIZE);

            // if char in user input isn't a valid digit -> custom exception raised
            char[] inputChars = input.ToCharArray();
            for (int i = 0; i < inputChars.Length; i++)
            {
                if (inputChars[i] < '0' || inputChars[i] > '9')
                    throw new InvalidCastException("Invalid char in index " + i + " of the inputted puzzle");
            }

            Console.WriteLine("your input is valid");

            ConvertToBoard(input);
        }

        public void ConvertToBoard(string validInput) // converting the puzzle string to a 2D array
        {
            int[,] initialSodukoBoard = new int[SIZE, SIZE];
            
            int index = 0;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    initialSodukoBoard[i, j] = validInput[index] - '0';
                    index++;
                }
            }

            Calculation calculation = new Calculation();
            int[,] solvedSodukoBoard = calculation.SolveSudoku(initialSodukoBoard);
            SodukoResult(solvedSodukoBoard);
        }

        public void SodukoResult(int[,] solvedSodukoBoard)
        {
            if (solvedSodukoBoard[0, 0] == 0) // if the first cell is 0 -> soduko is unsolvable
                PrintBoard(solvedSodukoBoard);
            else
                Console.WriteLine("No solution found");
        }

        // the function that prints the solution of the given Soduko puzzle
        public void PrintBoard(int[,] solvedSodukoBoard) // TODO: write more nice printing
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                    Console.Write(solvedSodukoBoard[i, j] + " ");

                Console.WriteLine();
            }
        }

        public void endMessage() // printing to the screen the the end message
        {
            Console.WriteLine("\n");
            Console.WriteLine("THANK YOU FOR USING MY SODUKO SOLVER! HOPE TO SEE YOU AGAIN SOON :)");
            Console.WriteLine("Made by @Ori_Boteach");
            Console.WriteLine("Press any key to exit your solver");
            Console.ReadKey();
        }
    }
}
