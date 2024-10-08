namespace IndividuelltProjektBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] usernameArray = {"anders", "elin", "klara", "jan", "göran"};
            string[] passwordArray = { "1234", "1234", "1234", "1234", "1234", };
            
            //Console.WriteLine(userAccounts[0,1,1]);

            /*
            if (userLogin(usernameArray, passwordArray) == -1)
            {
                Console.WriteLine("fel");
            }
            else
            {
                Console.WriteLine("rätt");
            }
            */
            //Menu();
        }

        /*
        static string[][][] Accounts()
        {
            return ;
        }
        21 oktober branchdag
        */
        static int userLogin(string[] usernameArray, string[] passwordArray)
        {
            //The method does not count wrong username as a login attempt. Does not increase count.
            int count = 0;
            Console.WriteLine("Welcome to the bank!");

            Console.WriteLine("Please enter your username: ");
            string username = Console.ReadLine();

            while (count<3)
            {
                
                if (!Array.Exists(usernameArray, element => element == username)) 
                {
                    Console.WriteLine("User does not exist. Try again.");
                    Console.WriteLine("Please enter your username: ");
                    username = Console.ReadLine();
                }
                else if(Array.Exists(usernameArray, element => element == username))
                {
                    Console.WriteLine("Enter your password: ");
                    string password = passwordArray[Array.IndexOf(usernameArray, username)];
                    if (Console.ReadLine() == password)
                    {
                        Console.WriteLine("Login successful");
                        return Array.IndexOf(usernameArray, username);
                    }
                    else
                    {
                        Console.WriteLine("Wrong password! Try again.");
                    }
                    count++;
                }
            }
            Console.WriteLine("Attempts reached limit! Program restarting.");
            return -1;
            
        }
        static void Menu()
        {
            string[] menuItems = { "1. Se dina konton och saldo.", "2. överföring mellan konton.", "3. ta ut pengar.", "4. Logga ut." };
            int currentSelection = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                for (int i = 0; i < menuItems.Length; i++)
                {
                    // writes the menu with the ">" marker at the current option.
                    Console.WriteLine(i == currentSelection ? $"> {menuItems[i]}" : $"{menuItems[i]}");
                }
                //reads the users key
                key = Console.ReadKey(true).Key;
                //Logic for traversing the menu for what happens if you reach the edges. Last and first option.
                if (key == ConsoleKey.UpArrow) currentSelection = (currentSelection == 0) ? menuItems.Length - 1 : currentSelection - 1;
                if (key == ConsoleKey.DownArrow) currentSelection = (currentSelection == menuItems.Length - 1) ? 0 : currentSelection + 1;
                // the loop continues until the user presses enter to chose a selected option.
            } while (key != ConsoleKey.Enter);

            Console.Clear();
            Console.WriteLine($"You selected {menuItems[currentSelection]}");

        }

        static string[] AccountNames(int index)
        {
            if(index < 0 && index > 4)
            {
                return new string[] {"error"} ;
            }
            string[][] accountNames = new string[5][];
            accountNames[0] = new string[] { "Privatkonto", "Sparkonto" };
            accountNames[1] = new string[] { "Privatkonto", "Sparkonto", "Lönekonto" };
            accountNames[2] = new string[] { "Privatkonto", "Sparkonto", "Lönekonto", "Pensionskonto" };
            accountNames[3] = new string[] { "Privatkonto", "Sparkonto", "Lönekonto", "Pensionskonto", "Investeringssparkonto" };
            accountNames[4] = new string[] { "Ungdomskonto", "Sparkonto"};
            return accountNames[index];
        }

        static double[] AccountAmounts(int index)
        {
            if (index < 0 && index > 4)
            {
                return new double[] { 0.0 };
            }
            double[][] accountAmount = new double[5][];
            accountAmount[0] = new double[] { 5000.0, 15000.0 };
            accountAmount[1] = new double[] { 3000.0, 10000.0, 25000.0 };
            accountAmount[2] = new double[] { 4000.0, 8000.0, 20000.0, 50000.0};
            accountAmount[3] = new double[] { 2000.0, 7000.0, 15000.0, 45000.0, 30000.0};
            accountAmount[4] = new double[] { 1000.0, 6000.0};
            return accountAmount[index];
        }
    }
}
