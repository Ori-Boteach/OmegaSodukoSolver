namespace SodukoSolver
{
    #pragma warning disable CS8600 // disable -> converting null literal or possible null value to non nullable type
    #pragma warning disable CS8622 // disable -> Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("WELCOME TO YOUR SODUKO PUZZLE SOLVER! made by @Ori_Boteach");

            bool wantsAgain = true;
            UI ui = new();

            // displaying a message to the user (via an event) when a keyboard interrupt is detected
            Console.CancelKeyPress += new ConsoleCancelEventHandler(ui.OnKeyboardInterruptEvent);

            while (wantsAgain == true) // a loop that allows the user to enter multiple sodukos
            {
                Console.WriteLine("\n\nWould you like to enter the soduko puzzle as a string to the console or as a file?");
                Console.WriteLine("Enter 1 for console, 2 for file or 'q' to quit");
                string choise = Console.ReadLine();
                
                try
                {
                    switch (choise) // the different options the user get enter at the menu and their corresponding actions
                    {
                        case "1":
                            ui.GetInputAsString();
                            break;
                            
                        case "2":
                            ui.GetInputAsFile();
                            break;

                        case null:
                            throw new NullInputException("");

                        case "q":
                            ui.EndMessage();
                            Environment.Exit(0);
                            break;

                        default:
                            Console.WriteLine("Invalid choice, please try again.");
                            continue;
                    }
                }
                catch (InvalidInputLengthException iile)
                {
                    Console.WriteLine(iile.Message);
                }
                catch (InvalidInputCharException iice)
                {
                    Console.WriteLine(iice.Message);
                }
                catch (InvalidInputPlaceException iipe)
                {
                    Console.WriteLine(iipe.Message);
                }
                catch(ArgumentNullException ane)
                {
                    Console.WriteLine(ane.Message);
                }
                catch (NullInputException nie)
                {
                    Console.WriteLine(nie.Message);
                    break;
                }

                Console.WriteLine("\nEnter anything to solve another soduko puzzle or 'q' to quit ");
                ConsoleKeyInfo UserInput = Console.ReadKey();
                string answer = UserInput.KeyChar.ToString();
                
                if (answer == "q")
                    wantsAgain = false;
            }
            ui.EndMessage(); // calling the function to print the message at the end of the solver
        }
    }
}