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
                try
                {
                    ui.validation();
                }
                catch (NullReferenceException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (InvalidInputLengthException e)
                {
                    Console.WriteLine(e.Message);
                }
                catch (InvalidCastException e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine("\nEnter anything to solVe another soduko puzzle or 'q' to quit ");
                ConsoleKeyInfo UserInput = Console.ReadKey();
                string answer = UserInput.KeyChar.ToString();
                if (answer == "q")
                    wantsAgain = false;
            }
            ui.endMessage();
        }
    }
}