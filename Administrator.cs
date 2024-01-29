using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;

public class Administrator : ICrud
{
    static List<User> users = new List<User>();
    const string filePath = "users.json";

    public void AdministratorMenu()
    {
        Console.Clear();
        LoadData();

        while (true)
        {
            Console.WriteLine("Меню администратора:");
            Console.WriteLine("1. Просмотреть всех пользователей");
            Console.WriteLine("2. Добавить пользователя");
            Console.WriteLine("3. Редактировать пользователя");
            Console.WriteLine("4. Удалить пользователя");
            Console.WriteLine("5. Поиск по атрибутам пользователя");
            Console.WriteLine("6. Выход");

            int choice = GetChoice(1, 6);

            switch (choice)
            {
                case 1:
                    DisplayAll();
                    break;
                case 2:
                    Add();
                    break;
                case 3:
                    Edit();
                    break;
                case 4:
                    Delete();
                    break;
                case 5:
                    Search();
                    break;
                case 6:
                    SaveData();
                    Environment.Exit(0);
                    break;
            }
        }
    }

    public void DisplayAll()
    {
        Console.Clear();
        Console.WriteLine("Список пользователей:");
        foreach (var user in users)
        {
            Console.WriteLine(user);
        }

        Console.WriteLine();
    }

    public void Add()
    {
        Console.Clear();
        Console.WriteLine("Введите данные нового пользователя:");

        User newUser = GetUserDetails();

        users.Add(newUser);

        Console.WriteLine("Пользователь успешно добавлен.\n");
    }

    public void Edit()
    {
        Console.Clear();
        Console.WriteLine("Введите ID пользователя для редактирования:");
        int userId = GetChoice(1, int.MaxValue);

        User userToEdit = users.FirstOrDefault(u => u.Id == userId);

        if (userToEdit != null)
        {
            Console.WriteLine($"Текущие данные пользователя:\n{userToEdit}");

            Console.WriteLine("Введите новые данные пользователя:");

            User updatedUser = GetUserDetails();

            users[users.IndexOf(userToEdit)] = updatedUser;

            Console.WriteLine("Данные пользователя успешно обновлены.\n");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.\n");
        }
    }

    public void Delete()
    {
        Console.Clear();
        Console.WriteLine("Введите ID пользователя для удаления:");
        int userId = GetChoice(1, int.MaxValue);

        User userToDelete = users.FirstOrDefault(u => u.Id == userId);

        if (userToDelete != null)
        {
            users.Remove(userToDelete);
            Console.WriteLine("Пользователь успешно удален.\n");
        }
        else
        {
            Console.WriteLine("Пользователь не найден.\n");
        }
    }

    public void Search()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        Console.Clear();
        Console.WriteLine("Выберите атрибут для поиска:");
        Console.WriteLine("1. ID");
        Console.WriteLine("2. Имя");
        Console.WriteLine("3. Возраст");
        Console.WriteLine("4. Email");
        Console.WriteLine("5. Нажмите Enter,чтоб вернуться обратно");

        int searchOption = GetChoice(1, 5);

        Console.WriteLine("Введите значение для поиска:");
        string searchValue = Console.ReadLine();

        List<User> searchResults = new List<User>();

        switch (searchOption)
        {
            case 1:
                searchResults = users.Where(u => u.Id.ToString() == searchValue).ToList();
                break;
            case 2:
                searchResults = users.Where(u => u.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case 3:
                searchResults = users.Where(u => u.Age.ToString() == searchValue).ToList();
                break;
            case 4:
                searchResults = users.Where(u => u.Email.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case 5:
                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    // Ваш код для возвращения обратно
                    Console.WriteLine("\nВозвращение обратно...");
                }
                else
                {
                    // Ваш код для обработки других клавиш (если нужно)
                    Console.WriteLine("\nНеверная клавиша...");
                }
                break;
            default:
                // Ваш код для обработки других случаев (если нужно)
                Console.WriteLine("\nНеверный вариант поиска...");
                break;
        }

        if (searchResults.Any())
        {
            Console.Clear();
            Console.WriteLine("Результаты поиска:");
            foreach (var result in searchResults)
            {
                Console.WriteLine(result);
            }
        }
        else
        {
            Console.WriteLine("Ничего не найдено.\n");
        }
    }

    static User GetUserDetails()
    {
        Console.Write("ID: ");
        int id = GetChoice(1, int.MaxValue);

        Console.Write("Имя: ");
        string name = Console.ReadLine();

        Console.Write("Возраст: ");
        int age = GetChoice(1, 150);

        Console.Write("Email: ");
        string email = Console.ReadLine();

        return new User { Id = id, Name = name, Age = age, Email = email };
    }

    static int GetChoice(int minValue, int maxValue)
    {
        int choice;
        while (!int.TryParse(Console.ReadLine(), out choice) || choice < minValue || choice > maxValue)
        {
            Console.WriteLine($"Введите корректное значение (от {minValue} до {maxValue}):");
        }

        return choice;
    }

    static void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            users = JsonConvert.DeserializeObject<List<User>>(json);
        }
    }

    static void SaveData()
    {
        string json = JsonConvert.SerializeObject(users, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
}

class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Email { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Имя: {Name}, Возраст: {Age}, Email: {Email}";
    }
}




public class MyUtilityClass
{
    public void MyMethod()
    {
        Console.WriteLine("Метод из другого класса был вызван!");
    }
}