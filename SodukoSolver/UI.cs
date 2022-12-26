﻿using System.Text;
using System.Diagnostics;

namespace SodukoSolver
{
    public class UI
    {
        public const int SIZE = 9;
        public static int[,] initialSodukoBoard = new int[SIZE, SIZE];

        public void StartAndValidation() // recieving the input from the user and validating it
        {
            Console.WriteLine("\nPlease enter the soduko pazzle that you need solved at a string format and press enter:");
            string input = Console.ReadLine();

            // if user input's length is different than 81 (size of 9X9 cube) -> custom exception raised
            if (input.Length != 81)
                throw new InvalidInputLengthException("Invalid number of chars in inputted string: " + input.Length + " instead of " + SIZE * SIZE);

            // if char in user input isn't a valid digit -> custom exception raised
            char[] inputChars = input.ToCharArray();
            for (int i = 0; i < inputChars.Length; i++)
            {
                if (inputChars[i] < '0' || inputChars[i] > '9')
                    throw new InvalidInputCharException("Invalid char in index " + i + " of the inputted puzzle");
            }

            // timing the solution process
            var timer = new Stopwatch();
            timer.Start();
            
            ConvertToBoard(input);

            timer.Stop();
            TimeSpan timeTaken = timer.Elapsed;
            Console.WriteLine("\nTime taken for the solving operation: " + timeTaken.ToString(@"m\:ss\.fff") + " minutes");
        }

        public void ConvertToBoard(string validInput) // converting the puzzle string to a 2D array
        {            
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
                            throw new InvalidInputPlaceException("Invalid inputted puzzle: can't place " + temp + " in place [" + (i+1) + ", " + (j+1) + "] of the grid");
                        initialSodukoBoard[i, j] = temp;
                    }
                }
            }
            bool answer = calculation.SolveSudoku();
            SodukoResult(answer);
        }

        public void SodukoResult(bool answer)
        {
            if (!answer) // if the returned value from SolveSudoku is flase -> soduko is UNSOLVABLE
                Console.WriteLine("\n***The soduko is unsolvable***");
            else
            {
                Console.WriteLine("\nTHE SOLVED SODUKO PUZZLE IS:");
                PrintBoard();
            }
        }

        // printing the solution of the given Soduko puzzle at a string format
        public void PrintBoard()
        {
            string solvedSodukoString = ConvertBackToString();
            Console.WriteLine(solvedSodukoString);
        }

        public string ConvertBackToString() // converting a soduko represented as a 2D array to string representation
        {
            // creating a string builder to store the solved puzzle -> appending to it char by char
            StringBuilder solvedSodukoString = new StringBuilder();

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    solvedSodukoString.Append(initialSodukoBoard[i, j]);
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
