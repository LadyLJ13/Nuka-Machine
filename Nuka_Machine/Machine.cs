using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using NAudio.Wave;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Nuka_Machine
{
    public class Machine
    {
        Dictionary<int, Drinks> Drink { get; set; }
        Dictionary<int, Select> Menu { get; set; }
        Dictionary<int, Actions> Action { get; set; }
        public class Select
        {
            public string Name { get; set; }
            public Select (string name)
            {
                Name = name;
            }

        }
        public class Actions
        {
            public string NameA { get; set; }
            public Actions(string nameA)
            {
                NameA = nameA;
            }
        }
        public Machine()
        {
            Drink = new Dictionary<int, Drinks>()
            {
                {1, new Drinks ("Nuka Cola", 20, "Refreshing and classic, straight from the Vault!",ConsoleColor.Red, 10) },
                {2, new Drinks ("Nuka Cherry", 40, "A sweet cherry explosion in every sip!", ConsoleColor.DarkRed, 10) },
                {3, new Drinks ("Nuka Quantum", 50, "Glows in the dark and gives you a boost!", ConsoleColor.Cyan, 10) },
                {4, new Drinks ("Nuka Void", 55, "Dark and mysterious... Not for the faint-hearted!", ConsoleColor.DarkMagenta, 10) },
                {5, new Drinks ("Rad X", 300, "Not exactly a drink... But it keeps you safe from the radiation!", ConsoleColor.Yellow, 1) },
                {6, new Drinks ("Admin Mode", 0, "Wanna modify the machine?", ConsoleColor.Green, 1) }
            };
            Menu = new Dictionary<int, Select>()
            {
                {1, new Select ("Add Drink") },
                {2, new Select ("Modify Drink") },
                {3, new Select ("Remove Drink") },
                {4, new Select ("Manage Inventory") },
                {5, new Select ("Go back to drinks menu") }
            };
            Action = new Dictionary<int, Actions>()
            {
                {1, new Actions ("Change Name") },
                {2, new Actions ("Change Price") },
                {3, new Actions ("Change Description") },
                {4, new Actions ("Change Stock") },
                {5, new Actions ("Change Color") },
                {6, new Actions ("Cancel") }
            };
        }
        public void PlaySound (string fileName)
        {
            try
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "sounds", fileName);
                if (File.Exists(path))
                {
                    using (var audioFile = new AudioFileReader(path))
                    using (var outputDevice = new WaveOutEvent())
                    {
                        outputDevice.Init(audioFile);
                        outputDevice.Play(); // Joue le son en arrière-plan
                        while (outputDevice.PlaybackState == PlaybackState.Playing)
                        {
                            System.Threading.Thread.Sleep(100);  // Attendre un peu avant de vérifier à nouveau
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Sound file not found: {path}"); //Permet de repérer si le son ne marche pas
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing sound: {ex.Message}");
            }
        }
        public void WelcomeDrinks()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            PlaySound("admin_mode.wav");
            Console.WriteLine("======== Welcome to your post-apocalyptic drink machine ========");
            Console.WriteLine("Please, make a choice:");
            Console.WriteLine();

            Console.WriteLine($"{"ID",-5} | {"Name",-25} | {"Price (caps)", -10} | {"Stock", -8}");
            Console.WriteLine(new string('-', 65));

            foreach (var item in Drink)
            {
                if(item.Value.Name == "Admin Mode")
                {
                    Console.WriteLine($"{item.Key,-5} | {item.Value.Name,-25} | {"-", -10}   | {"-", -11}");
                }
                else
                {
                    Console.WriteLine($"{item.Key,-5} | {item.Value.Name,-25} | {item.Value.Price ,-10}   | {item.Value.Stock, -8}");
                }
                
            }

            Console.ResetColor();
        }
        public void ShowDrinks()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            PlaySound("admin_mode.wav");
            Console.WriteLine("Here are the drinks:");
            Console.WriteLine();

            Console.WriteLine($"{"ID",-5} | {"Name",-25} | {"Price (caps)",-10} | {"Stock", -8}");
            Console.WriteLine(new string('-', 65));
            foreach (var item in Drink.Where(d => d.Key != 6))
            {
                Console.WriteLine($"{item.Key,-5} | {item.Value.Name,-25} | {item.Value.Price,-10}   | {item.Value.Stock,-8}");
            }
            return;
        }
        private static bool isAdmin = false;
        public void AdminMode()
        {
            while (true)
            {
                if (!isAdmin)
                {
                    Console.Clear();
                    Console.Write("Enter admin password: ");
                    string password = Console.ReadLine();

                    if (password != "Vault Tech")
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Access Denied! Incorrect password! Stranger Danger!");
                        PlaySound("too_poor.mp3");
                        System.Threading.Thread.Sleep(1000);
                        return;
                    }
                    isAdmin = true;
                    Console.WriteLine("Access granted");
                    PlaySound("access_granted.mp3");
                    Thread.Sleep(1000);
                }
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                PlaySound("admin_mode.wav");
                Console.WriteLine("================== Admin Mode ==================");
                Console.WriteLine("\nWhat do you want to do?");
                Console.WriteLine();

                Console.WriteLine($"{"ID",-5} | {"Selection",-20}");
                Console.WriteLine(new string('-', 40));
                foreach (var item in Menu)
                {
                    Console.WriteLine($"{item.Key,-5} | {item.Value.Name,-20}");
                }

                Console.WriteLine("Enter your selection:");
                if (!int.TryParse(Console.ReadLine(), out int selection) || selection <= 0)
                {
                    Console.ResetColor();
                    Console.WriteLine("Come on... You're that dumb?");
                    PlaySound("Erreur.wav");
                    Console.Clear();
                    continue;
                }
                if (!Menu.ContainsKey(selection))
                {
                    Console.ResetColor();
                    Console.WriteLine("Come on man! Try again.");
                    PlaySound("Erreur.wav");
                    Console.Clear();
                    continue;
                }
                Console.WriteLine($"You selected: {Menu[selection].Name}");

                switch (selection)
                {
                    case 1:
                        AddDrink();
                        break;
                    case 2:
                        ModifyDrink();
                        break;
                    case 3:
                        DeleteDrink();
                        break;
                    case 4:
                        ManageInventory();
                        break;
                    case 5:
                        Console.WriteLine("Going back to drinks menu...");
                        PlaySound("access_granted.mp3");
                        System.Threading.Thread.Sleep(1000);
                        Console.Clear();
                        return;
                    default:
                        Console.WriteLine("You've really got a problem man...");
                        PlaySound("Erreur.wav");
                        break;
                }
            }
        }
        public void AddDrink()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==== Adding a new drink ====");
            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine("Enter the drink name:");
            string name = Console.ReadLine();
            if(name == null)
            {
                Console.WriteLine("You didn't enter a name! Operation cancelled...");
                PlaySound("Erreur.wav");
                return;
            }
            if (Drink.Values.Any(d => d.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine($"A drink named '{name}' already exists. Choose another name.");
                return;
            }
            Console.WriteLine("Enter the drink price (in caps):");
            if (!int.TryParse(Console.ReadLine(), out int price) || price <= 0)
            {
                Console.WriteLine("You can't give drinks for free! Operation cancelled...");
                PlaySound("Erreur.wav");
                return;
            }
            Console.WriteLine("Add a small description:");
            string description = Console.ReadLine();
            if (description == null)
            {
                Console.WriteLine("Come on, write a little something... Operation cancelled...");
                return;
            }
            Console.WriteLine("Enter the stock for this drink:");
            int stock;
            while (!int.TryParse(Console.ReadLine(), out stock) || stock < 0)
            {
                Console.WriteLine("Enter a valid stock quantity man!");
            }
            Console.WriteLine("Choose a color for the drink (e.g., Red, Blue, Green, Yellow): ");
            string colorInput = Console.ReadLine();

            if (!Enum.TryParse(colorInput, true, out ConsoleColor color))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid color. Defaulting to White.");
                color = ConsoleColor.White;
            }
            int newId = Drink.Keys.Max() + 1;

            
            Drink.Add(newId, new Drinks(name, price, description, color, stock));
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Drink '{name}' added successfully with description and a price of {price} caps!");
            if (name.Equals("Dean", StringComparison.OrdinalIgnoreCase))
            {
                // Jouer le son spécifique pour "Pudding"
                PlaySound("pudding.wav");
            }
            else
            {
                PlaySound("task_done.wav");
            }
            Console.WriteLine("Press any key to go back to the admin menu");
            Console.ReadKey();
        }
        public void ModifyDrink()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==== Modifying a drink ====");
            ShowDrinks();
            Console.WriteLine("\nEnter the ID of the drink you want to modify:");
            if (int.TryParse(Console.ReadLine(), out int drinkId) && Drink.ContainsKey(drinkId))
            {
                var selectedDrink = Drink[drinkId];

                Console.WriteLine($"{"ID",-5} | {"Selection",-20}");
                Console.WriteLine(new string('-', 40));
                foreach (var item in Action)
                {
                    Console.WriteLine($"{item.Key,-5} | {item.Value.NameA,-20}");
                }
                Console.WriteLine("Choose an option: ");
                if (!int.TryParse(Console.ReadLine(), out int option) || option <= 0)
                {
                    Console.ResetColor();
                    Console.WriteLine("Come on... You're that dumb?");
                    PlaySound("Erreur.wav");
                    return;
                }
                if (!Action.ContainsKey(option))
                {
                    Console.ResetColor();
                    Console.WriteLine("Come on man! Try again.");
                    PlaySound("Erreur.wav");
                    return;
                }
                Console.WriteLine($"You selected: {Action[option].NameA}");

                switch (option)
                {
                    case 1:
                        Console.Write("Enter new name: ");
                        selectedDrink.Name = Console.ReadLine();

                        Console.WriteLine($"Drink '{selectedDrink.Name}' updated successfully!");
                        PlaySound("task_done.wav");
                        break;

                    case 2:
                        Console.Write("Enter new price: ");
                        if (int.TryParse(Console.ReadLine(), out int newPrice))
                        {
                            selectedDrink.Price = newPrice;
                            Console.WriteLine($"Drink '{selectedDrink.Name}' price updated successfully!");
                            PlaySound("task_done.wav");
                        }
                        else
                        {
                            Console.WriteLine("Invalid price.");
                            PlaySound("Erreur.wav");
                        }
                        break;

                    case 3:
                        Console.Write("Enter new description: ");
                        selectedDrink.Description = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine($"Drink '{selectedDrink.Name}' description updated successfully!");
                        PlaySound("task_done.wav");
                        break;

                    case 4:
                        Console.Write("Enter new stock quantity: ");
                        if (int.TryParse(Console.ReadLine(), out int newStock))
                        {
                            selectedDrink.Stock = newStock;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Drink '{selectedDrink.Name}' stock updated successfully!");
                            PlaySound("task_done.wav");
                        }
                        else
                        {
                            Console.WriteLine("Invalid stock quantity.");
                            PlaySound("Erreur.wav");
                        }
                        break;

                    case 5:
                        Console.WriteLine("Available colors: ");
                        foreach (var color in Enum.GetValues(typeof(ConsoleColor)))
                        {
                            Console.WriteLine($"- {color}");
                        }
                        Console.WriteLine("Enter new drink color: ");
                        string colorInput = Console.ReadLine();

                        if (Enum.TryParse<ConsoleColor>(colorInput, true, out ConsoleColor newColor))
                        {
                            selectedDrink.Color = newColor;
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine($"Drink '{selectedDrink.Name}' color updated to {newColor}!");
                            PlaySound("task_done.wav");
                        }
                        else
                        {
                            Console.WriteLine("Invalid color choice.");
                        }
                        break;

                    case 6:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Modification canceled");
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid ID. Returning to admin menu...");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n==== Going back to admin menu ====");
            Thread.Sleep(2000);
        }
         
        public void DeleteDrink()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==== Removing a drink ====");
            ShowDrinks();
            Console.WriteLine("Enter the ID of the drink you want to remove:");
            if (!int.TryParse(Console.ReadLine(), out int drinkId))
            {
                Console.WriteLine("Bro... Enter a valid number.");
                PlaySound("Erreur.wav");
                return;
            }

            // Vérifier si l'ID existe dans le dictionnaire
            if (!Drink.ContainsKey(drinkId))
            {
                Console.WriteLine($"No drink found with ID {drinkId}. Try again.");
                return;
            }

            string drinkName = Drink[drinkId].Name;

            // Supprimer la boisson du dictionnaire
            Drink.Remove(drinkId);

            Console.WriteLine($"Drink '{drinkName}' (ID {drinkId}) has been removed successfully!");
            PlaySound("task_done.wav");
            Console.WriteLine("Press any key to go back to the admin menu");
            Console.ReadKey();
        }
        public void ManageInventory()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("==== Managing drink stock ====");

            ShowDrinks();

            Console.WriteLine("Enter the ID of the drink to modify the stock:");
            if (!int.TryParse(Console.ReadLine(), out int drinkId))
            {
                Console.WriteLine("Bro... Enter a valid number.");
                PlaySound("Erreur.wav");
                return;
            }

            // Vérifier si l'ID existe dans le dictionnaire
            if (!Drink.ContainsKey(drinkId))
            {
                Console.WriteLine($"No drink found with ID {drinkId}. Try again.");
                return;
            }

            Drinks drink = Drink[drinkId];
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Modifying stock for {drink.Name}...");

            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("Enter new stock (current: " + drink.Stock + "): ");
            if (int.TryParse(Console.ReadLine(), out int newStock) && newStock >= 0)
            {
                drink.Stock = newStock;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"Stock for '{drink.Name}' updated successfully!");
                PlaySound("task_done.wav");
            }
            else
            {
                Console.WriteLine("Invalid stock value. Operation cancelled...");
                PlaySound("Erreur.wav");
            }

            Console.WriteLine("\nPress any key to go back to the admin menu...");
            Thread.Sleep(1000);
            Console.ReadKey();
        }
        public void Start()
        {
            while (true)
            {
                WelcomeDrinks();
                Console.ForegroundColor= ConsoleColor.Green;
                Console.Write("\nEnter the drink ID or 0 to exit: ");
                string userInput = Console.ReadLine();
                if (!int.TryParse(userInput, out int choice) || choice < 0 || choice > Drink.Count)
                {
                    Console.WriteLine("Come on, you're not that dumb...");
                    PlaySound("Erreur.wav");
                    Console.Clear();
                    continue;
                }

                if (choice == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine();
                    Console.WriteLine("Bye! Stay S.P.E.C.I.A.L.");
                    PlaySound("leaves.wav");
                    break;
                }

                if (choice == 6)
                {
                    AdminMode();
                    continue;
                }

                Drinks selectedDrink = Drink[choice];
                Console.WriteLine();
                if (selectedDrink.Name != "Admin Mode")
                {
                    if (selectedDrink.Stock > 0)
                    {
                        Console.WriteLine($"||| You selected {selectedDrink.Name}, price: {selectedDrink.Price} caps |||");

                        // Paiement
                        int capsInserted = 0;

                        //Permettre à l'user de revenir au drink menu + vérifier s'il insère des caps
                        while (true)
                        {
                            Console.Write($"Insert {selectedDrink.Price} caps (or press * to go back to the drinks menu): ");
                            string UInput = Console.ReadLine();
                            if(UInput == "*")
                            {
                                Console.WriteLine("\nGoing back to drinks menu...");
                                PlaySound("access_granted.mp3");
                                Thread.Sleep(1000);
                                Console.Clear();
                                Start();
                                return;
                            }
                            else if (int.TryParse(UInput, out capsInserted))
                            {
                                break;
                            }

                            Console.WriteLine("Hey! Those ain't caps! Try again!");
                            PlaySound("those_aint_caps.wav");
                        }

                        // Vérification du montant inséré
                        while (capsInserted < selectedDrink.Price)
                        {
                            Console.WriteLine($"Not enough caps! You need at least {selectedDrink.Price} caps");
                            PlaySound("not_enough_cash.wav");
                            Console.Write("Gimme more caps: ");

                            if (int.TryParse(Console.ReadLine(), out int additionalCaps))
                            {
                                capsInserted += additionalCaps;
                            }
                            else
                            {
                                Console.WriteLine("Hey! Those ain't caps! Try again!");
                                PlaySound("those_aint_caps.wav");
                            }
                        }

                        //Paiement réussi
                        Console.WriteLine($"Great! You inserted {capsInserted} caps");
                        PlaySound("Payment.wav");

                        if (capsInserted >= selectedDrink.Price)
                        {
                            int change = capsInserted - selectedDrink.Price; //Calculer la monnaie à rendre
                            selectedDrink.Stock--;  // Réduire le stock

                            if (selectedDrink.Name == "Rad X")
                            {
                                Console.ForegroundColor = selectedDrink.Color;
                                Console.WriteLine($"Here's your {selectedDrink.Name}!");
                                Console.WriteLine(selectedDrink.Description);
                                PlaySound("received_RadX.wav");
                            }
                            else
                            {
                                Console.ForegroundColor = selectedDrink.Color;
                                Console.WriteLine($"Here's your {selectedDrink.Name}!");
                                Console.WriteLine(selectedDrink.Description);
                                PlaySound("received_drink.wav");
                            }

                            Console.ResetColor();
                            if (change > 0)
                            {
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Don't forget your change man! {change} caps.");
                                PlaySound("Change.wav");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Sorry, out of stock!");
                        PlaySound("out_of_stock.mp3");
                    }
                }
                else
                {
                    Console.WriteLine("Admin Mode selected. No stock changes.");
                }
                Console.WriteLine();
                Console.WriteLine("\nPress any key to go back to the drinks menu...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}