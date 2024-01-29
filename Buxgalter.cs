using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

public class Buxgalter : ICrud
{
    static List<Transaction> transactions = new List<Transaction>();
    const string filePath = "transactions.json";

    public void BuxgalterMenu()
    {
        LoadData();

        while (true)
        {
            Console.WriteLine("Меню бухгалтера:");
            Console.WriteLine("1. Просмотреть все записи о денежных транзакциях");
            Console.WriteLine("2. Добавить запись");
            Console.WriteLine("3. Редактировать запись");
            Console.WriteLine("4. Удалить запись");
            Console.WriteLine("5. Поиск по атрибутам записи");
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
        Console.WriteLine("Список денежных транзакциях:");
        foreach (var user in transactions)
        {
            Console.WriteLine(user);
        }

        Console.WriteLine();
    }

    public void Add()
    {
        Console.Clear();

        Console.WriteLine("Введите данные новой записи о денежной транзакции:");

        Transaction newTransaction = GetTransactionDetails();

        transactions.Add(newTransaction);

        Console.WriteLine("Запись успешно добавлена.\n");
    }

    public void Edit()
    {
        Console.Clear();
        Console.WriteLine("Введите ID записи для редактирования:");
        int transactionId = GetChoice(1, int.MaxValue);

        Transaction transactionToEdit = transactions.FirstOrDefault(t => t.Id == transactionId);

        if (transactionToEdit != null)
        {
            Console.WriteLine($"Текущие данные записи:\n{transactionToEdit}");

            Console.WriteLine("Введите новые данные записи:");

            Transaction updatedTransaction = GetTransactionDetails();

            transactions[transactions.IndexOf(transactionToEdit)] = updatedTransaction;

            Console.WriteLine("Данные записи успешно обновлены.\n");
        }
        else
        {
            Console.WriteLine("Запись не найдена.\n");
        }
    }

    public void Delete()
    {
        Console.Clear();
        Console.WriteLine("Введите ID записи для удаления:");
        int transactionId = GetChoice(1, int.MaxValue);

        Transaction transactionToDelete = transactions.FirstOrDefault(t => t.Id == transactionId);

        if (transactionToDelete != null)
        {
            transactions.Remove(transactionToDelete);
            Console.WriteLine("Запись успешно удалена.\n");
        }
        else
        {
            Console.WriteLine("Запись не найдена.\n");
        }
    }


    public void Search()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey();
        Console.Clear();
        Console.WriteLine("Выберите атрибут для поиска:");
        Console.WriteLine("1. ID");
        Console.WriteLine("2. Описание");
        Console.WriteLine("3. Сумма");
        Console.WriteLine("4. Нажмите Enter,чтоб вернуться обратно");

        int searchOption = GetChoice(1, 4);

        Console.WriteLine("Введите значение для поиска:");
        string searchValue = Console.ReadLine();

        List<Transaction> searchResults = new List<Transaction>();

        switch (searchOption)
        {
            case 1:
                searchResults = transactions.Where(t => t.Id.ToString() == searchValue).ToList();
                break;
            case 2:
                searchResults = transactions.Where(t => t.Description.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case 3:
                searchResults = transactions.Where(t => t.Amount.ToString() == searchValue).ToList();
                break;
            case 4:
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

    static decimal GetDecimalValue()
    {
        decimal value;
        while (!decimal.TryParse(Console.ReadLine(), out value))
        {
            Console.WriteLine("Введите корректное значение для суммы:");
        }

        return value;
    }

    static Transaction GetTransactionDetails()
    {
        Console.Write("ID записи: ");
        int id = GetChoice(1, int.MaxValue);

        Console.Write("Описание: ");
        string description = Console.ReadLine();

        Console.Write("Сумма: ");
        decimal amount = GetDecimalValue();

        return new Transaction { Id = id, Description = description, Amount = amount };
    }




    static void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            transactions = JsonConvert.DeserializeObject<List<Transaction>>(json);
        }
    }



    static void SaveData()
    {
        string json = JsonConvert.SerializeObject(transactions, Formatting.Indented);
        File.WriteAllText(filePath, json);
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

class Transaction
{
    public int Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Описание: {Description}, Сумма: {Amount:C}";
    }
}