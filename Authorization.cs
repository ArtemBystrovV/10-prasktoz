using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

class Authorization1
{


    static string databaseFile = "userDatabase.txt";

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("\nВыберите действие:");
            Console.WriteLine("1. Регистрация");
            Console.WriteLine("2. Авторизация");
            Console.WriteLine("3. Выход");



            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Register();
                    break;
                case "2":
                    Login();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте еще раз.");
                    break;
            }
        }
    }

    static void MainMenu(string username)
    {


        Administrator admin = new Administrator();
        PersonalManager personal = new PersonalManager();
        Buxgalter buxgalter = new Buxgalter();
        Warehouse_manager warehouse_Manager = new Warehouse_manager();
        Cashier cashier = new Cashier();

        while (true)
        {
            Console.WriteLine($"\nВыберите действие для пользователя {username}:");
            Console.WriteLine("1. Администратор");
            Console.WriteLine("2. Менеджер персонала ");
            Console.WriteLine("3. Бухгалтер");
            Console.WriteLine("4. Склад-Менеджер");
            Console.WriteLine("5. Кассир ");
            Console.WriteLine("5. Выйти");




            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    admin.AdministratorMenu();
                    Console.Clear();
                    Console.WriteLine($"Вы выполнили Действие 1 для пользователя {username}.");
                    break;
                case "2":
                    personal.PersonalManagerMenu();
                    Console.Clear();
                    Console.WriteLine($"Вы выполнили Действие 2 для пользователя {username}.");
                    break;
                case "3":
                    buxgalter.BuxgalterMenu();
                    Console.Clear();
                    Console.WriteLine($"Вы выполнили Действие 2 для пользователя {username}.");
                    break;
                case "4":
                    warehouse_Manager.Warehouse_managerMenu();
                    Console.Clear();
                    Console.WriteLine($"Вы выполнили Действие 2 для пользователя {username}.");
                    break;
                case "5":
                    cashier.CashierMenu();
                    Console.Clear();
                    Console.WriteLine($"Вы выполнили Действие 2 для пользователя {username}.");
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте еще раз.");
                    break;
            }
        }
    }



    static void Register()
    {
        Console.WriteLine("=== Регистрация ===");
        Console.Write("Введите логин: ");
        string username = Console.ReadLine();

        // Проверяем, существует ли пользователь с таким логином
        if (UserExists(username))
        {
            Console.WriteLine("Пользователь с таким логином уже существует.");
            return;
        }

        Console.Write("Введите пароль: ");
        string password = Console.ReadLine();

        // Сохраняем информацию о пользователе в файл
        using (StreamWriter writer = new StreamWriter(databaseFile, true))
        {
            writer.WriteLine($"{username},{password}");
        }

        Console.WriteLine("Регистрация прошла успешно.");
    }

    static void Login()
    {
        Console.WriteLine("=== Авторизация ===");
        Console.Write("Введите логин: ");
        string username = Console.ReadLine();

        Console.Write("Введите пароль: ");
        string password = GetHiddenInput();

        // Проверяем, существует ли пользователь с введенным логином и паролем
        if (ValidateUser(username, password))
        {
            Console.WriteLine($"Вы успешно авторизованы как {username}.");
            MainMenu(username);
        }
        else
        {
            Console.WriteLine("Неверный логин или пароль.");
        }
    }

    static string GetHiddenInput()
    {
        List<char> passwordChars = new List<char>();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Игнорируем клавишу Enter
            if (key.Key != ConsoleKey.Enter)
            {
                // Добавляем символ в список для проверки пароля
                passwordChars.Add(key.KeyChar);
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        // Преобразуем список символов в строку
        return new string(passwordChars.ToArray());
    }


    static string GetPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            // Игнорируем клавишу Enter
            if (key.Key != ConsoleKey.Enter)
            {
                // Добавляем символ звездочки в пароль
                password += "*";
                Console.Write("*");
            }
        } while (key.Key != ConsoleKey.Enter);

        return password;
    }


    static bool UserExists(string username)
    {
        // Проверяем, существует ли пользователь с указанным логином
        if (File.Exists(databaseFile))
        {
            using (StreamReader reader = new StreamReader(databaseFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] userData = line.Split(',');
                    if (userData.Length >= 2 && userData[0] == username)
                    {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    static bool ValidateUser(string username, string password)
    {
        // Проверяем, существует ли пользователь с указанным логином и паролем
        if (File.Exists(databaseFile))
        {
            using (StreamReader reader = new StreamReader(databaseFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] userData = line.Split(',');

                    // Убедимся, что у нас достаточно данных для проверки логина и пароля
                    if (userData.Length >= 2)
                    {
                        // Удалим пробельные символы вокруг логина и пароля
                        string storedUsername = userData[0].Trim();
                        string storedPassword = userData[1].Trim();

                        if (storedUsername == username && storedPassword == password)
                        {
                            return true;
                        }
                    }
                }
            }
        }

        return false;
    }
}