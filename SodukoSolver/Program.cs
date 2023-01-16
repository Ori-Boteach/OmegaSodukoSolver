#pragma warning disable CS8600 // disable warning -> converting null literal or possible null value to non nullable type
#pragma warning disable CS8622 // disable warning -> Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

namespace SodukoSolver
{
    // the main module of the program
    class Program 
    {
        static void Main(string[] args)
        {
            UI ui = new();

            ui.StartMessage();
            
            bool wantsAgain = true;

            // displaying a message to the user (by an event) when a keyboard interrupt is detected
            Console.CancelKeyPress += new ConsoleCancelEventHandler(ui.OnKeyboardInterruptEvent);

            while (wantsAgain == true) // a loop that allows the user to enter multiple sodukos until choosing to exit
            {
                Console.WriteLine("\n\nWould you like to enter the soduko puzzle as a string to the console or as a file?");
                Console.WriteLine("Enter 1 for console, 2 for file or 'q' to quit");
                string choise = Console.ReadLine();
                
                try
                {
                    switch (choise) // the different options the user get enter at the menu and their corresponding actions
                    {
                        // entering the soduko as a string
                        case "1": 
                            ui.GetInputAsString();
                            break;

                        // entering the soduko from a file
                        case "2": 
                            ui.GetInputAsFile();
                            break;

                        // exiting the program
                        case "q": 
                            ui.EndMessage(); // displaying a final message to the user
                            Environment.Exit(0);
                            break;
                            
                        // if the user entered nothing
                        case null: 
                            throw new NullInputException("");

                        // if the user entered an invalid input
                        default:
                            Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                            Console.WriteLine("Invalid choice, please try again.");
                            Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
                            continue;
                    }
                }
                catch (InvalidInputLengthException iile) // catching the thrown exception in case the user entered an invalid length of soduko
                {
                    Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                    Console.WriteLine(iile.Message);
                    Console.WriteLine("valid board length sizes are -> 1 / 4 / 9 / 16 / 25");
                    Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
                }
                // catching the thrown exception in case the user entered  an invalid char for the detected soduko size
                catch (InvalidInputCharException iice)
                {
                    Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                    Console.WriteLine(iice.Message);
                    Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
                }
                // catching the thrown exception in case the user entered an invalid soduko (coliding numbers in the same row / column or cube)
                catch (InvalidInputPlaceException iipe)
                {
                    Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                    Console.WriteLine(iipe.Message);
                    Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
                }
                catch (ArgumentNullException ane) // catching the thrown exception in case the provided file was empty
                {
                    Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                    Console.WriteLine(ane.Message);
                    Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
                }
                catch (InvalidFileTypeException ife) // catching the thrown exception in case the user entered an invalid file type
                {
                    Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                    Console.WriteLine(ife.Message);
                    Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
                }
                catch (NullInputException nie) // catching the thrown exception in case the user entered nothing as a file path
                {
                    Console.ForegroundColor = ConsoleColor.Red; // changing console to red
                    Console.WriteLine(nie.Message);
                    Console.ForegroundColor = ConsoleColor.Gray; // changing console back to gray
                    break;
                }
               
                Console.WriteLine("\nEnter anything to solve another soduko puzzle or 'q' to quit ");
                ConsoleKeyInfo UserInput = Console.ReadKey();
                string answer = UserInput.KeyChar.ToString();

                if (answer == "q") // if the user entered 'q' the program will exit
                    wantsAgain = false;
            }
            ui.EndMessage(); // calling the function to print the message at the end of the solver
        }
    }
}