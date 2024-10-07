namespace IndividuelltProjektBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] usernameArray = {"anders", "elin", "klara", "jan", "göran"};
            string[] passwordArray = { "1234", "1234", "1234", "1234", "1234", };

            Console.WriteLine("Välkommen till banken! Ange användarnamn:");
            //Menu();
        }

        /*
        static string[][][] Accounts()
        {
            return ;
        }
        */

        static void Menu()
        {
            string[] menuItems = { "Option 1", "Option 2", "Option 3", "Option 4" };
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
