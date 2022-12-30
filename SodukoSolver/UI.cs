using System.Text;
using System.Diagnostics;

namespace SodukoSolver
{
    public class UI
    {
        #pragma warning disable CS8618 // disable -> Non-nullable field must contain a non-null value
        #pragma warning disable CS8600 // disable -> converting null literal or possible null value to non nullable type
        #pragma warning disable CS8604 // disable -> possible null reference argument

        public static int SIZE;
        public static int[,] initialSodukoBoard;

        public void getInputAsFile() // recieving the input from the user as a string from a file by provided file path
        {
            Console.WriteLine("\nEnter the file path:");
            string filePath = Console.ReadLine();
            
            try
            {
                string input = System.IO.File.ReadAllText(filePath);
                Console.WriteLine("\nSOLVING THIS SODUKO:\n"+input);
                ValidationAndStart(input);
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("\nError: File not found -> file path should be like C:\\Users\\user\\Downloads\\sudoku_example.txt");
            }
            catch (System.IO.IOException)
            {
                Console.WriteLine("\nError reading file");
            }
        }
        public void getInputAsString() // recieving the input from the user as a string from the console
        {
            Console.WriteLine("\nPlease enter the soduko pazzle that you need solved at a string format and press enter:");
            string input = Console.ReadLine();
            ValidationAndStart(input);
        }

        public string ValidationAndStart(string input) // validating the input and starting the calculation process
        {
            List<int> possibleSizes = new List<int> {1, 4, 9, 16, 25}; // a list that holds all possible soduko sizes

            SIZE = (int)Math.Sqrt(input.Length);
            initialSodukoBoard = new int[SIZE, SIZE];

            if (!possibleSizes.Contains(SIZE)) // if user input's length is invalid -> custom exception raised
                throw new InvalidInputLengthException("Invalid number of chars in inputted string: " + input.Length);

            // if char in user input isn't a valid digit -> custom exception raised
            char[] inputChars = input.ToCharArray();
            for (int i = 0; i < inputChars.Length; i++)
            {
                if (inputChars[i] < '0' || inputChars[i] > (char)(SIZE + '0'))
                    throw new InvalidInputCharException("Invalid char in index " + i + " of the inputted puzzle");
            }
            
            // timing the solution process -> starting stopwatch, solving and printing solution time
            var timer = new Stopwatch();
            timer.Start();
            
            string result = ConvertToBoard(input);

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("\nTime taken for the solving operation: " + timeTaken.ToString(@"m\:ss\.fff") + " minutes");
            return result;
        }

        public string ConvertToBoard(string validInput) // converting the puzzle string to a 2D array
        {            
            int index = 0;
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    initialSodukoBoard[i, j] = validInput[index] - '0'; // converting chars from their ascii codes to actual int values
                    index++;
                }
            }
            return CallByOrder();
        }

        // a function that checks for initialy invalid board and calls the calculation process by their order and necessity
        public string CallByOrder()
        {
            Calculation calculation = new Calculation();
            for (int i = 0; i < SIZE; i++) // checking for an INITIALY INVALID soduko board
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (initialSodukoBoard[i, j] != 0)
                    {
                        // saving the current num, changing to -1 and checking if can be there. if true -> change back to num and continue, if false -> custom exception raised
                        int temp = initialSodukoBoard[i, j];
                        initialSodukoBoard[i, j] = -1;
                        if (!calculation.CanBePlaced(i, j, temp))
                            throw new InvalidInputPlaceException("***Invalid inputted puzzle: can't place " + temp + " in place [" + (i + 1) + ", " + (j + 1) + "] of the puzzle***");
                        initialSodukoBoard[i, j] = temp;
                    }
                }
            }

            int zeroCounter = 0;
            for (int i = 0; i < SIZE; i++) // counting the number of filled cells
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (initialSodukoBoard[i, j] == 0)
                        zeroCounter++;
                }
            }

            // if more than half of the cells are empty -> calling SimpleElimination before backtracking to optimize solving time
            if (zeroCounter > (SIZE * SIZE / 2)) 
                while (calculation.SimpleElimination() == true) ; // calling SimpleElimination while it stills helps 

            bool answer = calculation.SolveSudoku();
            return SodukoResult(answer); // calling the function that prints the solved string
        }

        // a function that returns the answer to the user, if solvable ->  prints the solved soduko, if not -> prints a message
        public string SodukoResult(bool answer)
        {
            if (!answer) // if the returned value from SolveSudoku is flase -> soduko is UNSOLVABLE
            {
                Console.WriteLine("\n***The soduko is unsolvable***");
                return "***The soduko is unsolvable***";
            }
            else
            {
                Console.WriteLine("\nTHE SOLVED SODUKO PUZZLE IS:");
                return PrintBoard();
            }
        }

        // printing the solution of the given Soduko puzzle at a string format
        public string PrintBoard()
        {
            string solvedSodukoString = ConvertBackToString();
            Console.WriteLine(solvedSodukoString);
            return solvedSodukoString;
        }

        public string ConvertBackToString() // converting a soduko represented as a 2D array to string representation
        {
            // creating a string builder to store the solved puzzle -> appending to it char by char
            StringBuilder solvedSodukoString = new StringBuilder();

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    solvedSodukoString.Append((char)(initialSodukoBoard[i, j] + '0')); // converting back the values to their assigned chars
                }
            }

            // returning the soduko string
            return solvedSodukoString.ToString();
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
