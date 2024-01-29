using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using Newtonsoft.Json;

internal class Warehouse_manager : ICrud
{
    static List<Product> products = new List<Product>();
    const string filePath = "products.json";

    public void Warehouse_managerMenu()
    {
        LoadData();

        while (true)
        {
            Console.WriteLine("Меню склад-менеджера:");
            Console.WriteLine("1. Просмотреть все товары");
            Console.WriteLine("2. Добавить товар");
            Console.WriteLine("3. Редактировать товар");
            Console.WriteLine("4. Удалить товар");
            Console.WriteLine("5. Поиск по атрибутам товара");
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
        Console.WriteLine("Список всеx товаров:");
        foreach (var user in products)
        {
            Console.WriteLine(user);
        }

        Console.WriteLine();
    }


    public void Add()
    {
        Console.Clear();
        Console.WriteLine("Введите данные нового товара:");

        Product newProduct = GetProductDetails();

        products.Add(newProduct);

        Console.WriteLine("Товар успешно добавлен.\n");
    }

    public void Edit()
    {
        Console.Clear();
        Console.WriteLine("Введите ID товара для редактирования:");
        int productId = GetChoice(1, int.MaxValue);

        Product productToEdit = products.FirstOrDefault(p => p.Id == productId);

        if (productToEdit != null)
        {
            Console.WriteLine($"Текущие данные товара:\n{productToEdit}");

            Console.WriteLine("Введите новые данные товара:");

            Product updatedProduct = GetProductDetails();

            products[products.IndexOf(productToEdit)] = updatedProduct;

            Console.WriteLine("Данные товара успешно обновлены.\n");
        }
        else
        {
            Console.WriteLine("Товар не найден.\n");
        }
    }



    public void Delete()
    {
        Console.Clear();
        Console.WriteLine("Введите ID товара для удаления:");
        int userId = GetChoice(1, int.MaxValue);

        Product userToDelete = products.FirstOrDefault(u => u.Id == userId);

        if (userToDelete != null)
        {
            products.Remove(userToDelete);
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
        Console.WriteLine("2. Наименование");
        Console.WriteLine("3. Сумма");
        Console.WriteLine("4. Количество");
        Console.WriteLine("4. Нажмите Enter,чтоб вернуться обратно");

        int searchOption = GetChoice(1, 4);

        Console.WriteLine("Введите значение для поиска:");
        string searchValue = Console.ReadLine();

        List<Product> searchResults = new List<Product>();

        switch (searchOption)
        {
            case 1:
                searchResults = products.Where(t => t.Id.ToString() == searchValue).ToList();
                break;
            case 2:
                searchResults = products.Where(t => t.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase)).ToList();
                break;
            case 3:
                searchResults = products.Where(t => t.Price.ToString() == searchValue).ToList();
                break;
            case 4:
                searchResults = products.Where(t => t.Quantity.ToString() == searchValue).ToList();
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





    static decimal GetDecimalValue()
    {
        decimal value;
        while (!decimal.TryParse(Console.ReadLine(), out value))
        {
            Console.WriteLine("Введите корректное значение для суммы:");
        }

        return value;
    }

    static Product GetProductDetails()
    {
        Console.Write("ID товара: ");
        int id = GetChoice(1, int.MaxValue);

        Console.Write("Наименование товара: ");
        string name = Console.ReadLine();

        Console.Write("Цена: ");
        decimal price = GetDecimalValue();

        Console.Write("Количество: ");
        int quantity = GetChoice(0, int.MaxValue);

        return new Product { Id = id, Name = name, Price = price, Quantity = quantity };
    }

    static void LoadData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            products = JsonConvert.DeserializeObject<List<Product>>(json);
        }
    }

    static void SaveData()
    {
        string json = JsonConvert.SerializeObject(products);
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

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }

    public override string ToString()
    {
        return $"ID: {Id}, Наименование: {Name}, Цена: {Price:C}, Количество: {Quantity}";
    }
}