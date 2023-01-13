using System.Drawing;
using SodukoSolver.Backtracking;
using SodukoSolver.DLX;

#pragma warning disable CS8618 // disable warning -> Non-nullable field must contain a non-null value
#pragma warning disable CS8600 // disable warning -> converting null literal or possible null value to non nullable type
#pragma warning disable CS8622 // disable warning -> Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

namespace SodukoSolver
{
    public class UI
    {
        // setting global variables
        public static int SIZE;
        public static int CUBE_SIZE;

        public static bool isFromFile = false;
        public static bool choseDLX = true; // defualt choise is true

        public static Cell[,] initialSodukoBoard;
        public static int[,] initialSodukoMatrix;

        // when the user enters a keyboard interrupt (Ctrl+C) this event will be called to display end message and end the program
        public void OnKeyboardInterruptEvent(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("\nKeyboard interrupt detected... Teminating, please start over");
            e.Cancel = true; // Set the Cancel property to true to prevent the process from being terminated
            Environment.Exit(0); // Terminate the program
        }

        public void GetInputAsFile() // recieving the input from the user as a string from a file by provided file path
        {
            // displaying a message to the user (via an event) when a keyboard interrupt is detected
            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnKeyboardInterruptEvent);

            Console.WriteLine("\nEnter the file path:");
            string filePath = Console.ReadLine();
            
            if (filePath == null) // Check if the input string is null
                throw new NullInputException("*solver terminated due to null input -> keyboard interrupt detected*");

            try
            {
                string input = "";
                try // catching an even where the provided file path is null
                {
                    input = System.IO.File.ReadAllText(filePath);
                }
                catch(ArgumentException)
                {
                    throw new ArgumentNullException();
                }

                isFromFile = true; // setting the isFromFile variable to true to indicate that the input is from a file

                Console.WriteLine("\nSOLVING THIS SUDOKU:\n"+input);
                ValidationAndStart(input);
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("\nError: File not found -> file path should be like C:\\Users\\user\\Downloads\\sudoku_example.txt");
            }
            catch (IOException)
            {
                Console.WriteLine("\nError reading file");
            }
        }
        public void GetInputAsString()
        {
            // displaying a message to the user (via an event) when a keyboard interrupt is detected
            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnKeyboardInterruptEvent);

            Console.WriteLine("\nPlease enter the soduko pazzle that you need solved at a string format and press enter:");
            string input = Console.ReadLine();
            
            if (input == null) // checking if the input string is null
                throw new NullInputException("*solver terminated due to null input -> keyboard interrupt detected*");

            ValidationAndStart(input);
        }

        public string ValidationAndStart(string input) // validating the input and starting the calculation process
        {
            List<int> possibleSizes = new() { 1, 4, 9, 16, 25}; // a list that holds all possible soduko sizes

            SIZE = (int)Math.Sqrt(input.Length);
            CUBE_SIZE = (int)Math.Sqrt(UI.SIZE);
            initialSodukoBoard = new Cell[SIZE, SIZE];
            initialSodukoMatrix = new int[SIZE, SIZE];

            if (!possibleSizes.Contains(SIZE)) // if user input's length is invalid -> custom exception raised
                throw new InvalidInputLengthException("Invalid number of chars in inputted string: " + input.Length);

            // if char in user input isn't a valid digit -> custom exception raised
            char[] inputChars = input.ToCharArray();
            for (int inputIndex = 0; inputIndex < inputChars.Length; inputIndex++)
            {
                if (inputChars[inputIndex] < '0' || inputChars[inputIndex] > (char)(SIZE + '0'))
                    throw new InvalidInputCharException("Invalid char -> '" + inputChars[inputIndex] + "', in index " + inputIndex + " of the inputted puzzle");
            }

            Console.WriteLine("\nVALID INPUT!");
            // giving the user the option to choose between solving algorithms
            Console.WriteLine("\nWould you like to solve your sudoku using backtracking algorithm or dancing links algorihm?");
            Console.WriteLine("Type 'b' for backtracking or anyting else for dancing links");
            string choise = Console.ReadLine();
            if (choise == "b") // checking what the user chose
                choseDLX = false;
            else
                choseDLX = true;

            Console.ForegroundColor = ConsoleColor.Yellow; // changing console to yellow
            PrintBoard(input); // printing the input at a board format
            Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
            Console.WriteLine("\nGOT IT! processing...");
            
            string result = CallByOrder(input);
            
            return result;
        }
        
        // a function that checks for initialy invalid board and calls the calculation process by their order and necessity
        public static string CallByOrder(string input)
        {
            ConvertToBoardAndSolve(input);
            
            for (int row = 0; row < SIZE; row++) // checking for an INITIALY INVALID soduko board
            {
                for (int col = 0; col < SIZE; col++)
                {
                    if (initialSodukoBoard[row, col].Value != 0)
                    {
                        // saving the current num, changing to -1 and checking if can be there. if true -> change back to num and continue, if false -> custom exception raised
                        int temp = initialSodukoBoard[row, col].Value;
                        initialSodukoBoard[row, col].Value = -1;
                        if (!BacktrackCalculation.CanBePlaced(row, col, temp))
                            throw new InvalidInputPlaceException("***Invalid inputted puzzle: can't place " + temp + " in place [" + (row + 1) + ", " + (col + 1) + "] of the puzzle***");
                        initialSodukoBoard[row, col].Value = temp;
                    }
                }
            }

            string result;
            if (choseDLX) // the user chose to solve his sudoku using dlx algorithm
            {
                InitiatingDlxClass callDLX = new();
                result = callDLX.InitiateDLX(initialSodukoMatrix); // returning string result for AAA testing on ValidationAndStart method
            }
            else // the user chose to solve his sudoku using backtracking algorithm
            {
                BacktrackCalculation callBacktrack = new();
                result = callBacktrack.InitiateBacktracking(); // returning string result for AAA testing on ValidationAndStart method
            }
            
            if (isFromFile) // if given sudoku came from a file -> writing it's solution to a sudoku_result text file in addition to printing it to the console 
            {
                File.WriteAllTextAsync("C:\\Users\\user\\Downloads\\sudoku_result.txt", result);
                Console.WriteLine("\n(your solved soduko is also in C:\\Users\\user\\Downloads\\sudoku_result.txt)");
            }

            Console.ForegroundColor = ConsoleColor.Green; // changing console to green
            PrintBoard(result); // printing the solution at a board format
            Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray

            return result;
        }

        public static void ConvertToBoardAndSolve(string validInput) // converting the puzzle string to a 2D array
        {
            int index = 0;
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    initialSodukoBoard[row, col] = new Cell(validInput[index] - '0'); // creating cells and converting chars from their ascii codes to actual int values
                    initialSodukoMatrix[row, col] = validInput[index] - '0';
                    index++;
                }
            }
            UpdatePossibleValuesForEmpty();
        }

        public static void UpdatePossibleValuesForEmpty() // updating the possible values for the empty cells in the board
        {
            for (int row = 0; row < SIZE; row++) // for each row
            {
                for (int col = 0; col < SIZE; col++) // for each col
                {
                    if (initialSodukoBoard[row, col].Value == 0) // if cell is empty
                    {
                        for (int value = 1; value <= SIZE; value++) // check every possible value with CanBePlaced and remove the invalid ones
                        {
                            if (BacktrackCalculation.CanBePlaced(row, col, value) == false)
                                initialSodukoBoard[row, col].PossibleValues.Remove(value);
                        }
                    }
                }
            }
        }
        public static void PrintBoard(string solved) // printing the board to the console
        {
            if (solved == "***The soduko is unsolvable***") // if there is no solution, retrun immediately
                return;

            // converting string solution to matrix solution
            int index = 0;
            int[,] solvedMatrix = new int[SIZE, SIZE];
            for (int row = 0; row < SIZE; row++)
            {
                for (int col = 0; col < SIZE; col++)
                {
                    solvedMatrix[row, col] = solved[index] - '0'; // converting chars from their ascii codes to actual int values
                    index++;
                }
            }

            // printing the solved matrix to the console:
            Console.Write("\n╔");
            for (int i = 0; i < SIZE * 3 + CUBE_SIZE + (CUBE_SIZE - 1); i++)
                Console.Write("═");
            Console.Write("╗\n");

            for (int row = 0; row < SIZE; row++) // loop through the values
            {
                Console.Write("║ ");

                for (int col = 0; col < SIZE; col++)
                {
                    Console.Write(solvedMatrix[row, col].ToString().PadLeft(2) + " "); // each value gets extra space for boards with bigger values

                    if ((col + 1) % CUBE_SIZE == 0 && col != SIZE - 1)
                        Console.Write("║ ");
                }
                Console.Write("║\n");

                if ((row + 1) % CUBE_SIZE == 0 && row != SIZE - 1)
                {
                    Console.Write("╠");
                    for (int i = 0; i < SIZE * 3 + CUBE_SIZE + (CUBE_SIZE - 1); i++)
                        Console.Write("═");
                    Console.Write("╣\n");
                }
            }
            Console.Write("╚");
            for (int i = 0; i < SIZE * 3 + CUBE_SIZE + (CUBE_SIZE - 1); i++)
                Console.Write("═");
            Console.Write("╝");
        }

        public void EndMessage() // printing to the screen the the end message
        {
            Console.WriteLine("\n\nTHANK YOU FOR USING MY SODUKO SOLVER! HOPE TO SEE YOU AGAIN SOON :)");
            Console.WriteLine("Made by @Ori_Boteach");
            Console.WriteLine("Press any key to exit your solver");
        }
    }
}
