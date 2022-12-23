using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodukoSolver
{
    public class UI
    {
        public void validation() // add documentaion
        {
            Console.WriteLine("\nPlease enter the soduko pazzle that you need solved at a string format and press enter:");
            string input = Console.ReadLine();

            // if user input is null -> custom exception raised
            if (input == null)
                throw new NullReferenceException("Null input");

            // if user input's length is different than 81 (size of 9X9 cube) -> custom exception raised
            if (input.Length != 81)
                throw new Exception("Invalid number of chars in inputted string: " + input.Length + " instead of 81"); // custom exception!

            // if char in user input isn't a valid digit -> custom exception raised
            char[] inputChars = input.ToCharArray();
            for (int i = 0; i < inputChars.Length; i++)
            {
                if (inputChars[i] < '0' || inputChars[i] > '9')
                    throw new Exception("Invalid char in index " + i + " of the inputted puzzle");  // custom exception!
            }

            Console.WriteLine("All good!!!");

            //Calculation calculation = new Calculation();
            //calculation.Solve();
        }

        public void endMessage() // add documentaion
        {
            Console.WriteLine("\n");
            Console.WriteLine("THANK YOU FOR USING MY SODUKO SOLVER! HOPE TO SEE YOU AGAIN SOON :)");
            Console.WriteLine("Press any key to exit your solver");
            Console.ReadKey();
        }
    }
}
