using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using Newtonsoft.Json;

public class Cashier
{
    static List<Product> products = new List<Product>();
    static List<Sale> sales = new List<Sale>();
    static Accounting accounting = new Accounting();
    const string productsFilePath = "products.json";
    const string salesFilePath = "sales.json";
    const string accountingFilePath = "accounting.json";

    public void CashierMenu()
    {
        LoadData();

        while (true)
        {
            Console.WriteLine("Меню кассира:");
            Console.WriteLine("1. Просмотреть все товары");
            Console.WriteLine("2. Создать заказ");
            Console.WriteLine("3. Выход");

            int choice = GetChoice(1, 3);

            switch (choice)
            {
                case 1:
                    DisplayAllProducts();
                    break;
                case 2:
                    CreateOrder();
                    break;
                case 3:
                    SaveData();
                    Environment.Exit(0);
                    break;
            }
        }
    }




    static void DisplayAllProducts()
    {
        Console.Clear();
        Console.WriteLine("Список всех товаров:");
        foreach (var product in products)
        {
            Console.WriteLine(product);
        }

        Console.WriteLine();
    }




    static void CreateOrder()
    {
        Console.Clear();
        Console.WriteLine("Создание заказа:");

        List<Product> order = new List<Product>();
        decimal totalCost = 0;

        while (true)
        {
            Console.Clear();
            DisplayAllProducts();

            Console.WriteLine("\nУправление заказом:");
            Console.WriteLine("Используйте клавиши + и - для изменения количества товара");
            Console.WriteLine("Нажмите S для завершения заказа");

            int selectedProductIndex = GetChoice(1, products.Count) - 1;
            Product selectedProduct = products[selectedProductIndex];

            Console.WriteLine($"Выбранный товар: {selectedProduct}");

            Console.Write("Введите количество товара (+/- или S для завершения): ");
            string input = Console.ReadLine();

            if (input.ToUpper() == "S")
            {
                break;
            }

            int quantityChange;
            if (!int.TryParse(input, out quantityChange))
            {
                Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
                Console.ReadLine(); // Ожидание ввода для удобства пользователя
                continue;
            }

            if (quantityChange < 0 && -quantityChange > selectedProduct.Quantity)
            {
                Console.WriteLine("Нельзя купить больше, чем есть на складе.");
                Console.ReadLine(); // Ожидание ввода для удобства пользователя
                continue;
            }

            selectedProduct.Quantity += quantityChange;
            totalCost += selectedProduct.Price * quantityChange;

            Console.Clear();
            Console.WriteLine("Товар добавлен в заказ.");
            Console.WriteLine($"Общая стоимость заказа: {totalCost:C}");
            Console.ReadLine(); // Ожидание ввода для удобства пользователя
        }

        if (totalCost > 0)
        {
            // Создание чека и списание товаров со склада
            Sale sale = new Sale { Products = order, TotalCost = totalCost };
            sales.Add(sale);

            foreach (var product in order)
            {
                int index = products.FindIndex(p => p.Id == product.Id);
                products[index].Quantity -= product.Quantity;
            }

            // Учет в бухгалтерии
            accounting.AddSale(totalCost);

            Console.WriteLine("Заказ успешно завершен.");

            SaveData();
        }
    }



    static void SaveData()
    {
        string productsJson = JsonConvert.SerializeObject(products, Formatting.Indented);
        File.WriteAllText(productsFilePath, productsJson);

        string salesJson = JsonConvert.SerializeObject(sales, Formatting.Indented);
        File.WriteAllText(salesFilePath, salesJson);

        string accountingJson = JsonConvert.SerializeObject(accounting, Formatting.Indented);
        File.WriteAllText(accountingFilePath, accountingJson);
    }

    static void LoadData()
    {
        if (File.Exists(productsFilePath))
        {
            string productsJson = File.ReadAllText(productsFilePath);
            products = JsonConvert.DeserializeObject<List<Product>>(productsJson);
        }

        if (File.Exists(salesFilePath))
        {
            string salesJson = File.ReadAllText(salesFilePath);
            sales = JsonConvert.DeserializeObject<List<Sale>>(salesJson);
        }

        if (File.Exists(accountingFilePath))
        {
            string accountingJson = File.ReadAllText(accountingFilePath);
            accounting = JsonConvert.DeserializeObject<Accounting>(accountingJson);
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

class Sale
{
    public List<Product> Products { get; set; } = new List<Product>();
    public decimal TotalCost { get; set; }
}

class Accounting
{
    public decimal TotalRevenue { get; private set; } = 0;

    public void AddSale(decimal amount)
    {
        TotalRevenue += amount;
    }
}

