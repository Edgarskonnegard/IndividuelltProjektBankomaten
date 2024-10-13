using System;
using System.Diagnostics.Metrics;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace IndividuelltProjektBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Reads in the usernames and passwords from the text file.
            (string[] usernameArray, string[] passwordArray) = LoadLoginDetails();
            string[] menuItems = { "1. Se dina konton och saldo.", "2. överföring mellan konton.", "3. ta ut pengar.", "4. Logga ut." };
            //Array of boolean values to record if an account is locked.
            bool[] lockedAccount = {false, false, false, false, false };
            //Runs the program.
            RunProgram(usernameArray, passwordArray, menuItems, lockedAccount);

        }
        static void RunProgram(string[] usernameArray, string[] passwordArray, string[] menuItems, bool[] lockedAccount)
        {

            //Gets the names of the accounts and the amounts into arrays.
            //These need to be set outside to loop so that the changes are recorded and saved.
            string[][] names = new string[5][];
            double[][] amounts = new double[5][] ;
            for (int i = 0; i<5 ; i++)
            {
                names[i] = AccountNames(i);
                amounts[i] = AccountAmounts(i);
            }
            bool runProgram = true;
            while (runProgram)
            {
                string[] welcome = { "Logga in", "Avsluta programmet" };
                //If Menu returns 1 then the user chose to exit the program.
                if (Menu(welcome, "Välkommen till Varebergs Campusbank") == 1)
                {
                    //Goodbye message animation. sleep makes the program pause for a
                    //determined amount of time
                    string goodbye = "Programmet avslutas";
                    foreach (char c in goodbye)
                    {
                        Console.Write(" "+c);
                        Thread.Sleep(200);
                    }
                    Thread.Sleep(500);
                    return;
                }
                //UserLogin manages the logic for logging in and returns the index of the user.
                int userIndex = UserLogin(usernameArray, passwordArray, lockedAccount);
                //If the user failed to login then the method returns the value -2.
                if (userIndex < 0)
                {
                    //Skips the rest of the code and starts the while loop again.
                    continue;
                }
                //This loop is the main menu and runs if the user was able to log in.
                //Exiting this loop returns the user to the start menu.
                while (userIndex != -2)
                {
                    //The switch performs the tasks available in the main menu.
                    //user: usernameArray[userIndex] assigns the optional parameter in the method
                    //in such a way that not all optional parameters must be assigned.
                    switch (Menu(menuItems, "Campusbanken", user: usernameArray[userIndex]))
                    {
                        case 0:
                            //Displayes the accounts and their amounts.
                            Console.WriteLine("Mina konton\n");
                            Console.ForegroundColor = ConsoleColor.Yellow;

                            for (int i = 0; i < names[userIndex].Length; i++)
                            {
                                Console.WriteLine($"{names[userIndex][i]} : {amounts[userIndex][i]:C}");
                            }
                            Console.ResetColor();
                            Console.WriteLine();
                            ReturnToMenu();
                            break;
                        case 1:
                            //Performes trasnfers between the users accounts.
                            TransferFunds(names[userIndex], amounts[userIndex]);
                            break;
                        case 2:
                            //Manages withdrawels.
                            //if the user inputs the wrong code the method returns -2 and
                            //the users account is locked, then returns to the start menu. 
                            userIndex = WithdrawFunds(names[userIndex], amounts[userIndex], passwordArray[userIndex], lockedAccount, userIndex);
                            break;
                        case 3:
                            //User selects option Log out. Return to the start menu.
                            userIndex = -2;
                            break;
                    }
                }
            }
        }
        //Press a key to return to the main menu and displays simple animation while awaiting key. 
        static void ReturnToMenu()
        {
            int count = 1;
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);
                    return;
                }
                Console.Write($"Tryck på valfri tangent för att återgå till huvudmenyn..{new string('.', count % 2)}   ");
                Console.SetCursorPosition(0, Console.CursorTop);
                //Paus for 0.5 seconds.
                Thread.Sleep(500);
                count++;
            }
        }
        //Method to display a message in the color red.
        static void ErrorMessage(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        //Method to write all usernames and passwords into the textfile.
        static void WriteFile(string username, string password)
        {
            string filePath = "..//..//..//exempel.txt";
            File.WriteAllText(filePath, "username {" + username + "}\n password {" + password + "}");
        }
        //Reads the information from the text file. 
        static (string[], string[]) LoadLoginDetails()
        {
            string filePath = "..//..//..//exempel.txt";

            string allLines = File.ReadAllText(filePath);
            //Separate all the lines.
            string[] lines = allLines.Split('\n');
            //Creates a string from the information between the { } characters on each line.
            string usernameString = lines[0].Substring(lines[0].IndexOf("{") + 1, lines[0].IndexOf('}') - lines[0].IndexOf('{') - 1);
            string passwordString = lines[1].Substring(lines[1].IndexOf("{") + 1, lines[1].IndexOf('}') - lines[1].IndexOf('{') - 1);
            //Separate the strings by the , so we get an array where
            //the indices of usernames corresponds to the password.
            string[] username = usernameString.Split(',');
            string[] password = passwordString.Split(',');
            //Returns these arrays.
            return (username, password);
        }
        //Method to read the users input for withdrawel and transfers.
        static double ReadSum()
        {
            string sum = "";
            ConsoleKey key;
            do
            {
                //Reads the key and displays it.
                var keyPressed = Console.ReadKey(true);
                key = keyPressed.Key;
                if (key == ConsoleKey.Backspace && sum.Length > 0)
                {
                    //Removes the last character in the string.
                    sum = sum.Substring(0, sum.Length - 1);
                    //Removes the last added character on the console.
                    Console.Write("\b \b");
                }
                else if (char.IsDigit(keyPressed.KeyChar) || keyPressed.KeyChar==',')
                {
                    //Adds the character to the string and writes it.
                    sum += keyPressed.KeyChar;
                    Console.Write(keyPressed.KeyChar);
                }
                //Logic allowing the user to exit this method if wanted.
                else if (key == ConsoleKey.Escape)
                {
                    return -1;
                }

            } while (key != ConsoleKey.Enter);
            //Skip to the next row for a nicer console display.
            Console.Write("\n");
            return Convert.ToDouble(sum);
        }
        static int ReadPassword(string password)
        {
            string pinCode = "";
            ConsoleKey key;
            do
            {
                //Reads the key but doesn't display it.
                var keyPressed = Console.ReadKey(intercept: true);
                key = keyPressed.Key;
                if(key == ConsoleKey.Backspace && pinCode.Length > 0)
                {
                    //Removes the last character in the string.
                    pinCode = pinCode.Substring(0, pinCode.Length - 1);
                    //Removes the last "*" on the console.
                    Console.Write("\b \b");
                }
                else if (char.IsDigit(keyPressed.KeyChar))
                {
                    //Adds the input to the string and writes it to the console.
                    pinCode += keyPressed.KeyChar;
                    Console.Write("*");
                }
                //Exits the chosen option.
                else if(key == ConsoleKey.Escape)
                {
                    return -1;
                }

            } while (key != ConsoleKey.Enter);
            //Logic to check if the user managed to enter the correct password.
            if(pinCode == password)
            {
                return 1;
            }
            else
            {
                return -2;
            }
        }
        //Method to manage the logic regarding password attempts
        //and locking a users account
        static int checkPassword(string password, int userIndex, bool[] lockedAccount)
        {
            int count = 0;
            while (count < 3)
            {
                Console.WriteLine("Ange pinkod: ");
                //Gets the rownumber that the cursor is currently on.
                var cursorPosition = Console.GetCursorPosition();
                switch (ReadPassword(password))
                {
                    case -2:
                        //If the password was incorrect. Writes an error message.
                        //Moves the cursor back up above the error message so that
                        //the message is displayed below.
                        Console.WriteLine();
                        ErrorMessage($"Fel kod!\n{2 - count} försök kvar innan kontot spärras");
                        Console.SetCursorPosition(0, cursorPosition.Top);
                        Console.WriteLine(new string(' ', Console.WindowWidth)); // Radera rad 3
                        Console.SetCursorPosition(0, cursorPosition.Top - 1);
                        Console.WriteLine(new string(' ', Console.WindowWidth)); // Radera rad 3
                        Console.SetCursorPosition(0, cursorPosition.Top - 1);
                        count++;
                        break;
                    case -1:
                        return -1;
                    case 1:
                        return 1;
                }
            }
            //Three failed attempts result in a locked account. 
            lockedAccount[userIndex] = true;
            Console.Clear();
            ErrorMessage("Fel pinkod angiven 3 gånger! Ditt konto är nu spärrat.");
            Thread.Sleep(2000);

            return -2;
        }
        //Method for inputing the username and checking if it
        //exists in the array of users. 
        static int CheckUsername(string[] usernameArray)
        {
            Console.WriteLine("Ange användarnamn: ");
            var cursorPosition = Console.GetCursorPosition();
            while (true)
            {
                string username = "";
                ConsoleKey key;
                do
                {
                    //Reads the key.
                    var keyPressed = Console.ReadKey(true);
                    key = keyPressed.Key;
                    if (key == ConsoleKey.Backspace && username.Length > 0)
                    {
                        //Removes the last character in the string
                        username = username.Substring(0, username.Length - 1);
                        //Removes the last character on the console
                        Console.Write("\b \b");
                    }
                    else if (char.IsLetter(keyPressed.KeyChar))
                    {
                        //Adds the character to the string and the console.
                        username += keyPressed.KeyChar;
                        Console.Write(keyPressed.KeyChar);
                    }
                    //Option to exit.
                    else if (key == ConsoleKey.Escape)
                    {
                        return -1;
                    }

                } while (key != ConsoleKey.Enter);
                Console.Write("\n");
                //If the username is correct (is in the array) the inex is returned.
                if(Array.Exists(usernameArray, element => element == username))
                {
                    //Removes previous error message from console.
                    Console.SetCursorPosition(0, cursorPosition.Top+1);
                    Console.WriteLine(new string(' ', Console.WindowWidth));
                    Console.SetCursorPosition(0, cursorPosition.Top+1);
                    return Array.IndexOf(usernameArray, username);
                }
                //If the username is incorrect.
                else
                {
                    ErrorMessage("Användarnamnet är fel. Försök igen eller tryck på esc för att avbryta.");
                    //Thread.Sleep(1500);
                    //Backs the cursor up one step and removes the row with the input
                    Console.SetCursorPosition(0, cursorPosition.Top);
                    Console.WriteLine(new string(' ', Console.WindowWidth)); 
                    Console.SetCursorPosition(0, cursorPosition.Top);
                }
            }

        }
        
        static int UserLogin(string[] usernameArray, string[] passwordArray, bool[] lockedAccount)
        {
            //The method does not count wrong username as a login attempt.
            Console.SetCursorPosition(0, 10);
            Console.WriteLine("Tryck på esc för att avbryta.");
            Console.SetCursorPosition(0, 0);
            int userIndex = CheckUsername(usernameArray);
            //User chose to exit login attempt.
            if(userIndex == -1)
            {
                return -1;
            }
            //Check if the username was correct (userIndex >= 0)
            //and that the account isn't locked.
            if ( userIndex >= 0 && !lockedAccount[userIndex])
            {
                //Read in password
                switch (checkPassword(passwordArray[userIndex], userIndex, lockedAccount))
                {
                    case 1:
                        //Password is correct, return index of user.
                        return userIndex;
                    case -1:
                        //Exit attempt
                        return -1;
                    case -2:
                        //3 failed attempts.
                        return -2;
                }
            }
            else if (lockedAccount[userIndex])
            {
                //Displays message if the account is locked.
                Console.Clear();
                Console.WriteLine("Detta kontot är spärrat!");
                Thread.Sleep(2000);
                return -2;
            }
            return -1;

        }
        //Method to display a message as a rubric.
        static void Rubric(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(message);

            Console.ResetColor();
            Console.WriteLine();
        }
        //Method for the menu feature. All options except menuItems are optional.
        static int Menu(string[] menuItems, string? message = "", int? previousSelection = -1, bool? escapeButton = false, string? user = "")
        {
            int currentSelection = 0;
            ConsoleKey key;

            do
            {
                //Displays a message if
                Rubric(user.Length == 0 ? message + "\n": message + " - " + user);
                //Exiting the current choice is possible.
                if (escapeButton == true)
                {
                    Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\nTryck på esc för att gå tillbaka.");
                    Console.SetCursorPosition(0, 3);
                }
                //For loop going through all items in the string array.
                for (int i = 0; i < menuItems.Length; i++)
                {
                    //currentSelection is which option will be highlighted.
                    if (i == currentSelection)
                    {
                        //Highlighting, white background and black font.
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine($">>    {menuItems[i]}");
                        Console.ResetColor();
                    }
                    //If we have multiple choices our previous choice will be displayed with the color red.
                    else if (i == previousSelection)
                    {
                        //Colored red.
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"      {menuItems[i]}");
                        Console.ResetColor();
                    }
                    //All other strings in the array will be displayed normally
                    else
                    {
                        Console.WriteLine($"     {menuItems[i]}");
                    }
                }
                //reads the users key
                key = Console.ReadKey(true).Key;
                //Logic for traversing the menu for what happens if you reach the edges. Last and first option.
                if (key == ConsoleKey.UpArrow) currentSelection = (currentSelection == 0) ? menuItems.Length - 1 : currentSelection - 1;
                if (key == ConsoleKey.DownArrow) currentSelection = (currentSelection == menuItems.Length - 1) ? 0 : currentSelection + 1;
                //Go back. Exiting the method when escape is pressed if the optional parameter is set.
                if(key == ConsoleKey.Escape && escapeButton == true)
                {
                    return -1;
                }
                // the loop continues until the user presses enter to chose a selected option.
            } while (key != ConsoleKey.Enter);

            Console.Clear();
            //Returns the index of the choice in the array.
            return currentSelection;

        }
        //Method that sets up the amount of accounts connected to each user and their names.
        static string[] AccountNames(int index)
        {
            if (index < 0 && index > 4)
            {
                return new string[] { "error" };
            }
            string[][] accountNames = new string[5][];
            accountNames[0] = new string[] { "Privatkonto", "Sparkonto" };
            accountNames[1] = new string[] { "Privatkonto", "Sparkonto", "Lönekonto" };
            accountNames[2] = new string[] { "Privatkonto", "Sparkonto", "Lönekonto", "Pensionskonto" };
            accountNames[3] = new string[] { "Privatkonto", "Sparkonto", "Lönekonto", "Pensionskonto", "Investeringssparkonto" };
            accountNames[4] = new string[] { "Ungdomskonto", "Sparkonto" };
            return accountNames[index];
        }
        //Method that sets up the amount of accounts connected to each user and their value.
        static double[] AccountAmounts(int index)
        {
            if (index < 0 && index > 4)
            {
                return new double[] { 0.0 };
            }
            double[][] accountAmount = new double[5][];
            accountAmount[0] = new double[] { 5000.0, 15000.0 };
            accountAmount[1] = new double[] { 3000.0, 10000.0, 25000.0 };
            accountAmount[2] = new double[] { 4000.0, 8000.0, 20000.0, 50000.0 };
            accountAmount[3] = new double[] { 2000.0, 7000.0, 15000.0, 45000.0, 30000.0 };
            accountAmount[4] = new double[] { 1000.0, 6000.0 };
            return accountAmount[index];
        }
        //Method to transfer money between accounts.
        static void TransferFunds(string[] names, double[] amounts)
        {
            Console.WriteLine("Välj konto att överföra från");
            //Choose from all the users accounts which account to transfer from.
            int from = Menu(names, "Överföring\nVälj konto att överföra från.", escapeButton: true);
            switch (from)
            {
                //If an account is chosen.
                case >= 0:
                    //Choose account to transfer to.
                    int to = Menu(names, $"Överföring\nDu valde {names[from]}, välj konto att överföra till", from, escapeButton: true);
                    switch (to)
                    {
                        //If an account is chosen.
                        case >= 0:
                            //If user chooses the same accounts, continue asking until valid choice is made.
                            while (from == to)
                            {
                                Console.Clear();
                                ErrorMessage("Kan ej överföra till samma konto!");
                                Thread.Sleep(1500);
                                Console.Clear();
                                to = Menu(names, $"Överföring\nDu valde {names[from]}, välj konto att överföra till", from, escapeButton: true);
                            }
                            Console.WriteLine();
                            Console.Write("Summa att överföra: ");
                            //Valid choice is made and user needs to input amount to transfer.
                            double sum = ReadSum();
                            //If the amount is available in the account and the amount is nonzero or nonnegative.
                            if (amounts[from] >= sum && sum > 0)
                            {
                                //Remove the sum from the first account.
                                amounts[from] -= sum;
                                //Add the sum to the second account.
                                amounts[to] += sum;
                                Console.Clear();
                                Console.WriteLine("Transaktion genomförd!\n\n\n\n\n\n\n\n");
                            }
                            //Invalid input.
                            else if (sum <= 0)
                            {
                                Console.Clear();
                                ErrorMessage("Ogiltig summa!\n\n\n\n\n\n\n\n");
                            }
                            //Amount is to large.
                            else
                            {
                                Console.Clear();
                                ErrorMessage("Medges ej!");
                                Console.WriteLine("Summan överstiger tillgängliga medel\n\n\n\n\n\n\n\n");
                            }
                            //Wait for key before returning to menu.
                            ReturnToMenu();
                            break;
                        //Exit the method when picking the second account.
                        case -1:
                            return;

                    }
                    break;
                //Exit the method when picking the first account.
                case -1:
                    return;
            }
        }
        //Method to withdraw amount.
        static int WithdrawFunds(string[] accountNames, double[] account, string password,bool[] lockedAccount, int userIndex)
        {
            string[] withdrawOptions = { "100", "200", "500", "1000", "Ange eget belopp"};
            //Choose account for withdrawel.
            int chosen = Menu(accountNames, "Välj konto för uttag",escapeButton:true);
            //If user decides to exit.
            if (chosen == -1)
            {
                //Exit method and return index of the current user.
                return userIndex;
            }
            //Choose option for withdraw, set amount, custom or exit.
            int option = Menu(withdrawOptions, "Summa att ta ut: ", escapeButton: true);
            //If set amount is selected.
            if (option < 4)
            {
                double sum = Convert.ToDouble(withdrawOptions[chosen]);
                //Check if the sum is valid.
                if (ValidSum(account, chosen, sum))
                {
                    //If the sum is valid then we check the password, if the password is correct
                    //then the amount is removed from the account.
                    return Withdraw(checkPassword(password, userIndex, lockedAccount), account, accountNames, chosen, sum, userIndex);

                }
            }
            //If custom amount is selected.
            else if (option == 4)
            {
                Console.WriteLine("Ange belopp:");
                Console.WriteLine("\n\n\n\n\n\n\n\n\n Tryck på esc för att återgå till menyn.");
                Console.SetCursorPosition(0, 1);
                //Read in the users input.
                double sum = ReadSum();
                //Check if the sum is valid.
                if (ValidSum(account, chosen, sum))
                {
                    //If the sum is valid then we check the password, if the password is correct
                    //then the amount is removed from the account.
                    //If withdraw is successful or exited, the current user index is returned
                    //If password check fails three attempts then the account is locked and we return -2,
                    //returning us to the start menu in RunProgram.
                    return Withdraw(checkPassword(password, userIndex, lockedAccount), account, accountNames, chosen, sum, userIndex);
                    
                }
            }
            //Return index of current user.
            return userIndex;
            
        }
        //Method for three cases during withdrawel.
        static int Withdraw(int choice, double[] account, string[] accountNames, int chosen, double sum, int userIndex)
        {
            switch (choice)
            {
                //If password check is failed return -2;
                case -2:
                    return -2;
                //User chooses to exit.
                case -1:
                    return userIndex;
                //password is correct.
                case 1:
                    //Withdraw specified amount.
                    WithdrawAvailableAmount(account, accountNames, chosen, sum);
                    return userIndex;
            }
            return -1;
        }
        //Check if the sum is valid.
        static bool ValidSum(double[] account, int choosen, double sum)
        {
            //If the sum is available on the account and the sum is not 0 or negative.
            if (account[choosen] >= sum && sum > 0)
            {
                return true;
            }
            //If the sum is invalid.
            else if (sum <= 0)
            {
                Console.Clear();
                ErrorMessage("Ogiltig summa!\n\n\n\n\n\n\n\n");
            }
            //If the sum is to large, not sufficient funds are available.
            else
            {
                Console.Clear();
                ErrorMessage("Medges ej!\n\n\n\n\n\n\n\n");
            }
            return false;

        }
        //Method for removing withdrawel amount from chosen account.
        static void WithdrawAvailableAmount(double[] account, string[] accountNames, int chosen, double sum)
        {
            account[chosen] -= sum;
            Console.Clear();
            Console.WriteLine("Transaktion genomförd!\n\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($" {accountNames[chosen]} : {account[chosen]:C}\n\n\n\n\n\n\n");
            Console.ResetColor();
            ReturnToMenu();
        }
    }
}
