namespace IndividuelltProjektBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] usernameArray = {"anders", "elin", "klara", "jan", "göran"};
            string[] passwordArray = { "1234", "1234", "1234", "1234", "1234", };

            
            if (userLogin(usernameArray, passwordArray) == -1)
            {
                Console.WriteLine("fel");
            }
            else
            {
                Console.WriteLine("rätt");
            }
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

    }
}
