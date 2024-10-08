namespace IndividuelltProjektBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {

            string[] usernameArray = {"anders", "elin", "klara", "jan", "göran"};
            string[] passwordArray = { "1234", "1234", "1234", "1234", "1234", };
            string[] menuItems = { "1. Se dina konton och saldo.", "2. överföring mellan konton.", "3. ta ut pengar.", "4. Logga ut." };

            string filePath = "..//..//..//exempel.txt";

            string allLines = File.ReadAllText(filePath);
            string[] lines = allLines.Split('\n');
            //string str = lines[0].Substring(Array.IndexOf(lines,'{'), lines[0].Length);
            //foreach (var c in lines[0]) { Console.Write(c); }
            Console.WriteLine(lines[0].IndexOf('{'));
            string NameFromFile = lines[0].Substring(lines[0].IndexOf("{")+1, lines[0].IndexOf('}')- lines[0].IndexOf('{')-1);
            NameFromFile += ", gruber";
            string[] gruber = NameFromFile.Split(",");
            foreach (var c in gruber)
            {
                Console.Write(c);
            }
                //Console.WriteLine(userAccounts[0,1,1]);
                //File.WriteAllText("..//..//..//exempel.txt", "hej");

                /*bool runProgram = true;
                while (runProgram)
                {
                    int userIndex = userLogin(usernameArray, passwordArray);
                    bool backToLogin = false;
                    while (userIndex != -1)
                    {
                        string[] names = AccountNames(userIndex);
                        double[] amounts = AccountAmounts(userIndex);
                        switch (Menu(menuItems))
                        {
                            case 0:
                                for (int i = 0; i < names.Length; i++)
                                {
                                    Console.WriteLine($"{names[i]} : {amounts[i]:C}");
                                }
                                Console.ReadKey();
                                break;
                            case 1:
                                TransferFunds(names, amounts);
                                Console.ReadKey();
                                break;
                            case 2:
                                WithdrawFunds(names, amounts);
                                Console.ReadKey();
                                break;
                            case 3:
                                userIndex = -1;
                                backToLogin = true;

                                break;
                        }
                    }
                    if (!backToLogin)
                    {
                        runProgram = false;
                    }
                }*/

            }

        static (string[], string[]) LoadLoginDetails()
        {
            string filePath = "..//..//..//exempel.txt";
            
            string allLines = File.ReadAllText(filePath);
            string[] lines = allLines.Split('\n');

            string[] username = lines[0].Split(',');
            string[] password = lines[1].Split(',');

            return (username, password);
        }
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
        static int Menu(string[] menuItems, string? message = "")
        {
            //string[] menuItems = { "1. Se dina konton och saldo.", "2. överföring mellan konton.", "3. ta ut pengar.", "4. Logga ut." };
            int currentSelection = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine(message);
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if(i == currentSelection)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($"> {menuItems[i]}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine($"  {menuItems[i]}");
                    }
                    // writes the menu with the ">" marker at the current option.
                    //Console.WriteLine(i == currentSelection ? $"> {menuItems[i]}" : $"  {menuItems[i]}");
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
            return currentSelection;

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

        static void TransferFunds(string[] names, double[] amounts)
        {
            Console.WriteLine("Välj konto att överföra från");
            int from = Menu(names);
            Console.WriteLine($"Du valde {names[from]}");
            Console.WriteLine("Välj konto att överföra till");
            int to = Menu(names, $"Du valde {names[from]}");
            while (from == to)
            {
                Console.WriteLine("Cannot transfer to the same account!");
                to = Menu(names);
            }
            Console.WriteLine();
            Console.Write("Summa att överföra: ");
            double sum = Convert.ToDouble(Console.ReadLine());
            if (amounts[from] >= sum)
            {
                amounts[from] -= sum;
                amounts[to] += sum;
                Console.Clear();
                Console.WriteLine($"{names[from]} : {amounts[from]:C}");
                Console.WriteLine($"{names[to]} : {amounts[to]:C}");
            }
        }

        static void WithdrawFunds(string[] names, double[] amounts)
        {
            string[] withdrawOptions = { "100", "200", "500", "1000", "Custom" };
            Console.WriteLine("choose account for withdrawel");
            int from = Menu(names);
            Console.Write("Sum to withdraw: ");
            if (Menu(withdrawOptions) != 4)
            {
                double sum = Convert.ToDouble(withdrawOptions[from]);
                if (amounts[from] >= Math.Abs(sum))
                {
                    amounts[from] -= sum;
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("amount is not available");
                }
            }
            else 
            {
                double sum = Convert.ToDouble(Console.ReadLine());
                if (amounts[from] >= Math.Abs(sum))
                {
                    amounts[from] -= sum;
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine("amount is not available");
                }
            }
            
            
        }
    }
}
