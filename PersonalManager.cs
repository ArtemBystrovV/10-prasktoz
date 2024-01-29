using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

public class PersonalManager : ICrud
{
    static List<User> users = new List<User>();
    static List<Employee> employees = new List<Employee>();
    const string usersFilePath = "users.json";
    const string employeesFilePath = "employees.json";

    public void PersonalManagerMenu()
    {
        LoadData();

        while (true)
        {
            Console.WriteLine("Меню менеджера персонала:");
            Console.WriteLine("1. Просмотреть всех сотрудников");
            Console.WriteLine("2. Добавить сотрудника");
            Console.WriteLine("3. Редактировать сотрудника");
            Console.WriteLine("4. Удалить сотрудника");
            Console.WriteLine("5. Поиск по атрибутам сотрудника");
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
        Console.WriteLine("Список всех сотрудников:");
        foreach (var employee in employees)
        {
            Console.WriteLine(employee);
        }

        Console.WriteLine();
    }

    public void Add()
    {
        Console.Clear();
        Console.WriteLine("Введите данные нового сотрудника:");

        Employee newEmployee = GetEmployeeDetails();

        // Проверка, что сотрудник не привязан к другому пользователю
        if (employees.Any(e => e.UserId == newEmployee.UserId))
        {
            Console.WriteLine("Ошибка: Этот пользователь уже привязан к другому сотруднику.\n");
            return;
        }

        employees.Add(newEmployee);

        Console.WriteLine("Сотрудник успешно добавлен.\n");
    }

    public void Edit()
    {
        Console.Clear();
        Console.WriteLine("Введите ID сотрудника для редактирования:");
        int employeeId = GetChoice(1, int.MaxValue);

        Employee employeeToEdit = employees.FirstOrDefault(e => e.Id == employeeId);

        if (employeeToEdit != null)
        {
            Console.WriteLine($"Текущие данные сотрудника:\n{employeeToEdit}");

            Console.WriteLine("Введите новые данные сотрудника:");

            Employee updatedEmployee = GetEmployeeDetails();

            // Проверка, что сотрудник не привязан к другому пользователю
            if (updatedEmployee.UserId != employeeToEdit.UserId &&
                employees.Any(e => e.UserId == updatedEmployee.UserId))
            {
                Console.WriteLine("Ошибка: Этот пользователь уже привязан к другому сотруднику.\n");
                return;
            }

            employees[employees.IndexOf(employeeToEdit)] = updatedEmployee;

            Console.WriteLine("Данные сотрудника успешно обновлены.\n");
        }
        else
        {
            Console.WriteLine("Сотрудник не найден.\n");
        }
    }

    public void Delete()
    {
        Console.Clear();
        Console.WriteLine("Введите ID сотрудника для удаления:");
        int employeeId = GetChoice(1, int.MaxValue);

        Employee employeeToDelete = employees.FirstOrDefault(e => e.Id == employeeId);

        if (employeeToDelete != null)
        {
            employees.Remove(employeeToDelete);
            Console.WriteLine("Сотрудник успешно удален.\n");
        }
        else
        {
            Console.WriteLine("Сотрудник не найден.\n");
        }
    }

    public void Search()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        Console.Clear();
        Console.WriteLine("Выберите атрибут для поиска:");
        Console.WriteLine("1. ID");
        Console.WriteLine("2. Имя");
        Console.WriteLine("3. Должность");
        Console.WriteLine("4. ID пользователя");
        Console.WriteLine("5. Нажмите Enter,чтоб вернуться обратно");

        int searchOption = GetChoice(1, 5);

        Console.WriteLine("Введите значение для поиска:");
        string searchValue = Console.ReadLine();

        List<Employee> searchResults = new List<Employee>();

        switch (searchOption)
        {
            case 1:
                searchResults = employees.Where(e => e.Id.ToString() == searchValue).ToList();
                break;
            case 2:
                searchResults = employees.Where(e => e.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case 3:
                searchResults = employees.Where(e => e.Position.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case 4:
                searchResults = employees.Where(e => e.UserId.ToString() == searchValue).ToList();
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

    static Employee GetEmployeeDetails()
    {
        Console.Write("ID сотрудника: ");
        int id = GetChoice(1, int.MaxValue);

        Console.Write("Имя сотрудника: ");
        string name = Console.ReadLine();

        Console.Write("Должность: ");
        string position = Console.ReadLine();

        Console.Write("ID пользователя (оставьте пустым, если сотрудник сам по себе): ");
        int userId;
        int.TryParse(Console.ReadLine(), out userId);

        return new Employee { Id = id, Name = name, Position = position, UserId = userId };
    }

    static void SaveData()
    {
        string usersJson = JsonConvert.SerializeObject(users, Formatting.Indented);
        File.WriteAllText(usersFilePath, usersJson);

        string employeesJson = JsonConvert.SerializeObject(employees, Formatting.Indented);
        File.WriteAllText(employeesFilePath, employeesJson);
    }

    static void LoadData()
    {
        if (File.Exists(usersFilePath))
        {
            string usersJson = File.ReadAllText(usersFilePath);
            users = JsonConvert.DeserializeObject<List<User>>(usersJson);
        }

        if (File.Exists(employeesFilePath))
        {
            string employeesJson = File.ReadAllText(employeesFilePath);
            employees = JsonConvert.DeserializeObject<List<Employee>>(employeesJson);
        }
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
}

class Employee
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Position { get; set; }
    public int UserId { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Имя: {Name}, Должность: {Position}, ID пользователя: {UserId}";
    }
}

class User1
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Имя: {Name}";
    }
}