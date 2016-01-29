using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Refactoring
{
    public class Tusc
    {
        public static void Start(List<User> usrs, List<Product> prods)
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
            Console.WriteLine();

            // Return to Login when user or password is invalid
            Login:

            string userName = PromptForUserInput("Enter Username:");

            // Validate Username
            if (!string.IsNullOrEmpty(userName))
            {
                int totalUsers = usrs.Count();

                if (IsUserValid(usrs, userName, totalUsers))
                {
                    string userPassword = PromptForUserInput("Enter Password:");

                    if (IsPasswordValid(usrs, userName, totalUsers, userPassword))
                    {
                        // Show welcome message
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine();
                        Console.WriteLine("Login successful! Welcome " + userName + "!");
                        Console.ResetColor();
                        
                        // Show remaining balance
                        double userBalance = 0;
                        for (int i = 0; i < totalUsers; i++)
                        {
                            User user = usrs[i];

                            // Check that name and password match
                            if (user.Name == userName && user.Pwd == userPassword)
                            {
                                userBalance = user.Bal;

                                // Show balance 
                                Console.WriteLine();
                                Console.WriteLine("Your balance is " + user.Bal.ToString("C"));
                            }
                        }

                        // Show product list
                        while (true)
                        {
                            // Prompt for user input
                            Console.WriteLine();
                            Console.WriteLine("What would you like to buy?");
                            for (int i = 0; i < 7; i++)
                            {
                                Product prod = prods[i];
                                Console.WriteLine(i + 1 + ": " + prod.Name + " (" + prod.Price.ToString("C") + ")");
                            }
                            Console.WriteLine(prods.Count + 1 + ": Exit");

                            string answer = PromptForUserInput("Enter a number:");
                            int productNumber = Convert.ToInt32(answer);
                            productNumber = productNumber - 1; /* Subtract 1 from number
                            num = num + 1 // Add 1 to number */

                            // Check if user entered number that equals product count
                            if (productNumber == 7)
                            {
                                // Update balance
                                foreach (var usr in usrs)
                                {
                                    // Check that name and password match
                                    if (usr.Name == userName && usr.Pwd == userPassword)
                                    {
                                        usr.Bal = userBalance;
                                    }
                                }

                                // Write out new balance
                                string json = JsonConvert.SerializeObject(usrs, Formatting.Indented);
                                File.WriteAllText(@"Data/Users.json", json);

                                // Write out new quantities
                                string json2 = JsonConvert.SerializeObject(prods, Formatting.Indented);
                                File.WriteAllText(@"Data/Products.json", json2);


                                // Prevent console from closing
                                Console.WriteLine();
                                Console.WriteLine("Press Enter key to exit");
                                Console.ReadLine();
                                return;
                            }
                            else
                            {
                                Console.WriteLine();
                                Console.WriteLine("You want to buy: " + prods[productNumber].Name);
                                Console.WriteLine("Your balance is " + userBalance.ToString("C"));

                                // Prompt for user input
                                answer = PromptForUserInput("Enter amount to purchase:");
                                int productQuantity = Convert.ToInt32(answer);

                                // Check if balance - quantity * price is less than 0
                                if (userBalance - prods[productNumber].Price * productQuantity < 0)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine();
                                    Console.WriteLine("You do not have enough money to buy that.");
                                    Console.ResetColor();
                                    continue;
                                }

                                // Check if quantity is less than quantity
                                if (prods[productNumber].Qty <= productQuantity)
                                {
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.WriteLine();
                                    Console.WriteLine("Sorry, " + prods[productNumber].Name + " is out of stock");
                                    Console.ResetColor();
                                    continue;
                                }

                                // Check if quantity is greater than zero
                                if (productQuantity > 0)
                                {
                                    // Balance = Balance - Price * Quantity
                                    userBalance = userBalance - prods[productNumber].Price * productQuantity;

                                    // Quanity = Quantity - Quantity
                                    prods[productNumber].Qty = prods[productNumber].Qty - productQuantity;

                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine("You bought " + productQuantity + " " + prods[productNumber].Name);
                                    Console.WriteLine("Your new balance is " + userBalance.ToString("C"));
                                    Console.ResetColor();
                                }
                                else
                                {
                                    // Quantity is less than zero
                                    Console.Clear();
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine();
                                    Console.WriteLine("Purchase cancelled");
                                    Console.ResetColor();
                                }
                            }
                        }
                    }
                    else
                    {
                        // Invalid Password
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine();
                        Console.WriteLine("You entered an invalid password.");
                        Console.ResetColor();

                        goto Login;
                    }
                }
                else
                {
                    // Invalid User
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine("You entered an invalid user.");
                    Console.ResetColor();

                    goto Login;
                }
            }

            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }

        private static bool IsPasswordValid(List<User> usrs, string userName, int totalUsers, string userPassword)
        {
            bool isPasswordValid = false;
            for (int i = 0; i < totalUsers; i++)
            {
                User user = usrs[i];

                // Check that name and password match
                if (user.Name == userName && user.Pwd == userPassword)
                {
                    isPasswordValid = true;
                }
            }
            return isPasswordValid;
        }

        private static bool IsUserValid(List<User> usrs, string userName, int totalUsers)
        {
            bool isUserValid = false;

            for (int i = 0; i < totalUsers; i++)
            {
                User user = usrs[i];
                if (user.Name == userName)
                {
                    isUserValid = true;
                }
            }

            return isUserValid;
        }

        private static string PromptForUserInput(string message)
        {
            Console.WriteLine(message);
            string userInput = Console.ReadLine();
            return userInput;
        }
    }
}
