namespace SodukoSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO YOUR SODUKO PUZZLE SOLVER! made by @Ori_Boteach");

            bool wantsAgain = true;
            UI ui = new UI();
            while (wantsAgain == true)
            {
                Console.WriteLine("\n\nWould you like to enter the soduko puzzle as a string to the console or as a file?");
                Console.WriteLine("Enter 1 for console, 2 for file or 'q' to quit");
                string choise = Console.ReadLine();
                try
                {
                    switch (choise)
                    {
                        case "1":
                            ui.getInputAsString();
                            break;
                            
                        case "2":
                            ui.getInputAsFile();
                            break;
                            
                        case "q":
                            ui.endMessage();
                            Environment.Exit(0);
                            break;
                            
                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            continue;
                    }
                }
                catch (InvalidInputLengthException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (InvalidInputCharException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (InvalidInputPlaceException e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("\nEnter anything to solve another soduko puzzle or 'q' to quit ");
                ConsoleKeyInfo UserInput = Console.ReadKey();
                string answer = UserInput.KeyChar.ToString();
                if (answer == "q")
                    wantsAgain = false;
            }
            ui.endMessage();
        }
    }
}