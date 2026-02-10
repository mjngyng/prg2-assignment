//==========================================================
// Student Number : S10275435F
// Student Name   : Bryan Mak Jing Yang
// Partner Name   : Loh Yu Wei Kyran
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static List<Restaurant> restaurantList = new List<Restaurant>();
    static List<Customer> customerList = new List<Customer>();
    static int orderIdCounter = 0;

    static void Main(string[] args)
    {
        LoadRestaurantsAndFoodItems();
        LoadCustomersAndOrders();

        int maxId = 0;
        foreach (var c in customerList)
        {
            foreach (var o in c.OrderList)
            {
                if (o.OrderID > maxId) maxId = o.OrderID;
            }
        }
        orderIdCounter = maxId;

        while (true)
        {
            Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
            Console.WriteLine("1. List all restaurants and menu items");
            Console.WriteLine("2. List all orders");
            Console.WriteLine("3. Create a new order");
            Console.WriteLine("4. Process an order");
            Console.WriteLine("5. Modify an existing order");
            Console.WriteLine("6. Delete an existing order");
            Console.WriteLine("0. Exit");
            Console.Write("Enter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ListRestaurantsAndMenus();
                    break;
                case "2":
                    ListAllOrders();
                    break;
                case "3":
                    CreateNewOrder();
                    break;
                case "4":
                    ProcessOrder();
                    break;
                case "5":
                    ModifyOrder();
                    break;
                case "6":
                    DeleteOrder();
                    break;
                case "0":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void LoadRestaurantsAndFoodItems()
    {
        try
        {
            string[] rLines = File.ReadAllLines("restaurants.csv");
            for (int i = 1; i < rLines.Length; i++)
            {
                string[] data = rLines[i].Split(',');
                Restaurant r = new Restaurant(data[0], data[1], data[2]);
                Menu m = new Menu("M" + data[0], "Main Menu");
                r.AddMenu(m);
                restaurantList.Add(r);
            }
            Console.WriteLine($"{restaurantList.Count} restaurants loaded!");

            string[] fLines = File.ReadAllLines("fooditems.csv");
            int foodCount = 0;
            for (int i = 1; i < fLines.Length; i++)
            {
                string[] data = fLines[i].Split(',');
                string rID = data[0];
                string name = data[1];
                string desc = data[2];
                double price = double.Parse(data[3]);

                FoodItem item = new FoodItem(name, desc, price);

                Restaurant r = restaurantList.Find(x => x.RestaurantID == rID);
                if (r != null)
                {
                    r.Menus[0].AddFoodItem(item);
                    foodCount++;
                }
            }
            Console.WriteLine($"{foodCount} food items loaded!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading restaurant/food files: " + ex.Message);
        }
    }

    static void LoadCustomersAndOrders()
    {
        try
        {
            string[] cLines = File.ReadAllLines("customers.csv");
            for (int i = 1; i < cLines.Length; i++)
            {
                string[] data = cLines[i].Split(',');
                Customer c = new Customer(data[0], data[1]);
                customerList.Add(c);
            }
            Console.WriteLine($"{customerList.Count} customers loaded!");

            string[] oLines = File.ReadAllLines("orders.csv");
            int orderCount = 0;
            for (int i = 1; i < oLines.Length; i++)
            {
                string[] data = oLines[i].Split(',');
                int orderId = int.Parse(data[0]);
                string email = data[1];
                string rId = data[2];
                double total = double.Parse(data[3]);
                string status = data[4];

                Order o = new Order(orderId, DateTime.Now);
                o.OrderStatus = status;

                Customer c = customerList.Find(x => x.EmailAddress == email);
                if (c != null) c.AddOrder(o);

                Restaurant r = restaurantList.Find(x => x.RestaurantID == rId);
                if (r != null && (status == "Pending" || status == "Preparing"))
                {
                    r.OrderQueue.Enqueue(o);
                }
                orderCount++;
            }
            Console.WriteLine($"{orderCount} orders loaded!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error loading customer/order files: " + ex.Message);
        }
    }

    static void ListRestaurantsAndMenus()
    {
        Console.WriteLine("\nAll Restaurants and Menu Items");
        Console.WriteLine("==============================");
        foreach (var r in restaurantList)
        {
            Console.WriteLine($"Restaurant: {r.RestaurantName} ({r.RestaurantID})");
            foreach (var m in r.Menus)
            {
                foreach (var f in m.FoodItems)
                {
                    Console.WriteLine($" - {f.ItemName}: {f.ItemDesc} - ${f.ItemPrice:F2}");
                }
            }
            Console.WriteLine();
        }
    }

    static void ListAllOrders()
    {
        Console.WriteLine("\nAll Orders");
        Console.WriteLine("==========");
        Console.WriteLine($"{"Order ID",-10} {"Customer",-15} {"Restaurant",-15} {"Delivery Time",-20} {"Amount",-10} {"Status",-10}");

        foreach (var c in customerList)
        {
            foreach (var o in c.OrderList)
            {
                string rName = "Unknown";
                foreach (var r in restaurantList)
                {
                    if (r.OrderQueue.Contains(o)) rName = r.RestaurantName;
                }

                Console.WriteLine($"{o.OrderID,-10} {c.CustomerName,-15} {rName,-15} {o.DeliveryDateTime,-20} ${o.CalculateOrderTotal():F2} {o.OrderStatus,-10}");
            }
        }
    }

    static void CreateNewOrder()
    {
        Console.WriteLine("\nCreate New Order");
        Console.WriteLine("================");

        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine();
        Customer cust = customerList.Find(c => c.EmailAddress == email);
        if (cust == null) { Console.WriteLine("Customer not found."); return; }

        Console.Write("Enter Restaurant ID: ");
        string rId = Console.ReadLine();
        Restaurant rest = restaurantList.Find(r => r.RestaurantID == rId);
        if (rest == null) { Console.WriteLine("Restaurant not found."); return; }

        Console.Write("Enter Delivery Date (dd/MM/yyyy): ");
        string dateStr = Console.ReadLine();
        Console.Write("Enter Delivery Time (HH:mm): ");
        string timeStr = Console.ReadLine();
        DateTime deliveryDT;
        if (!DateTime.TryParseExact($"{dateStr} {timeStr}", "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out deliveryDT))
        {
            Console.WriteLine("Invalid date/time format."); return;
        }

        Console.Write("Enter Delivery Address: ");
        string address = Console.ReadLine();

        Order newOrder = new Order(++orderIdCounter, DateTime.Now);
        newOrder.DeliveryDateTime = deliveryDT;
        newOrder.DeliveryAddress = address;

        List<FoodItem> menuItems = rest.Menus[0].FoodItems;

        while (true)
        {
            Console.WriteLine("\nAvailable Food Items:");
            for (int i = 0; i < menuItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {menuItems[i].ItemName} - ${menuItems[i].ItemPrice:F2}");
            }
            Console.Write("Enter item number (0 to finish): ");
            int itemChoice;
            if (!int.TryParse(Console.ReadLine(), out itemChoice)) continue;
            if (itemChoice == 0) break;

            if (itemChoice > 0 && itemChoice <= menuItems.Count)
            {
                Console.Write("Enter quantity: ");
                int qty = int.Parse(Console.ReadLine());
                FoodItem selected = menuItems[itemChoice - 1];

                OrderedFoodItem orderedItem = new OrderedFoodItem(selected.ItemName, selected.ItemDesc, selected.ItemPrice, qty);
                newOrder.AddOrderedFoodItem(orderedItem);
            }
        }

        Console.Write("Add special request? [Y/N]: ");
        if (Console.ReadLine().ToUpper() == "Y")
        {
            Console.Write("Enter request: ");
            string req = Console.ReadLine();
        }

        double total = newOrder.CalculateOrderTotal() + 5.00;
        Console.WriteLine($"Order Total: ${newOrder.CalculateOrderTotal():F2} + $5.00 (delivery) = ${total:F2}");
        Console.Write("Proceed to payment? [Y/N]: ");
        if (Console.ReadLine().ToUpper() != "Y") return;

        Console.Write("Payment method [CC/PP/CD]: ");
        newOrder.OrderPaymentMethod = Console.ReadLine().ToUpper();
        newOrder.OrderStatus = "Pending";
        newOrder.OrderPaid = true;

        cust.AddOrder(newOrder);
        rest.OrderQueue.Enqueue(newOrder);

        string csvLine = $"{newOrder.OrderID},{cust.EmailAddress},{rest.RestaurantID},{total},{newOrder.OrderStatus}";
        File.AppendAllText("orders.csv", "\n" + csvLine);

        Console.WriteLine($"Order {newOrder.OrderID} created successfully! Status: Pending");
    }

    static void ProcessOrder()
    {
        Console.WriteLine("\nProcess Order");
        Console.Write("Enter Restaurant ID: ");
        string rId = Console.ReadLine();
        Restaurant rest = restaurantList.Find(r => r.RestaurantID == rId);
        if (rest == null) { Console.WriteLine("Restaurant not found."); return; }

        if (rest.OrderQueue.Count == 0)
        {
            Console.WriteLine("No orders in queue.");
            return;
        }

        Order currentOrder = rest.OrderQueue.Peek();

        Console.WriteLine($"Order {currentOrder.OrderID}");
        Console.WriteLine($"Status: {currentOrder.OrderStatus}");
        Console.WriteLine("Items:");
        foreach (var item in currentOrder.OrderedFoodItems)
        {
            Console.WriteLine($"- {item.ItemName} x {item.QtyOrdered}");
        }

        Console.Write("[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
        string action = Console.ReadLine().ToUpper();

        if (action == "C" && currentOrder.OrderStatus == "Pending")
        {
            currentOrder.OrderStatus = "Preparing";
            Console.WriteLine("Order confirmed. Status: Preparing");
        }
        else if (action == "R" && currentOrder.OrderStatus == "Pending")
        {
            currentOrder.OrderStatus = "Rejected";
            rest.OrderQueue.Dequeue();
            Console.WriteLine("Order rejected and refunded.");
        }
        else if (action == "D" && currentOrder.OrderStatus == "Preparing")
        {
            currentOrder.OrderStatus = "Delivered";
            rest.OrderQueue.Dequeue();
            Console.WriteLine("Order delivered.");
        }
        else if (action == "S")
        {
            Console.WriteLine("Skipped.");
        }
        else
        {
            Console.WriteLine("Invalid action for current status.");
        }
    }

    static void ModifyOrder()
    {
        Console.WriteLine("\nModify Order");
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine();
        Customer cust = customerList.Find(c => c.EmailAddress == email);
        if (cust == null) { Console.WriteLine("Customer not found."); return; }

        Console.WriteLine("Pending Orders:");
        foreach (var o in cust.OrderList)
        {
            if (o.OrderStatus == "Pending") Console.WriteLine(o.OrderID);
        }

        Console.Write("Enter Order ID: ");
        int oId = int.Parse(Console.ReadLine());
        Order order = cust.OrderList.Find(o => o.OrderID == oId);

        if (order == null || order.OrderStatus != "Pending")
        {
            Console.WriteLine("Order not found or not pending.");
            return;
        }

        Console.WriteLine("1. Modify Items (Not implemented in this snippet)");
        Console.WriteLine("2. Modify Address");
        Console.WriteLine("3. Modify Delivery Time");
        Console.Write("Select modification: ");
        string mod = Console.ReadLine();

        if (mod == "2")
        {
            Console.Write("Enter new address: ");
            order.DeliveryAddress = Console.ReadLine();
            Console.WriteLine("Address updated.");
        }
        else if (mod == "3")
        {
            Console.Write("Enter new time (HH:mm): ");
            string timeStr = Console.ReadLine();
            DateTime newTime = DateTime.ParseExact(timeStr, "HH:mm", null);
            order.DeliveryDateTime = new DateTime(order.DeliveryDateTime.Year, order.DeliveryDateTime.Month, order.DeliveryDateTime.Day, newTime.Hour, newTime.Minute, 0);
            Console.WriteLine("Time updated.");
        }
    }

    static void DeleteOrder()
    {
        Console.WriteLine("\nDelete Order");
        Console.Write("Enter Customer Email: ");
        string email = Console.ReadLine();
        Customer cust = customerList.Find(c => c.EmailAddress == email);
        if (cust == null) { Console.WriteLine("Customer not found."); return; }

        Console.Write("Enter Order ID: ");
        int oId = int.Parse(Console.ReadLine());
        Order order = cust.OrderList.Find(o => o.OrderID == oId);

        if (order == null || order.OrderStatus != "Pending")
        {
            Console.WriteLine("Order not found or cannot be deleted (must be Pending).");
            return;
        }

        Console.WriteLine($"Order Total: ${order.CalculateOrderTotal() + 5.00}");
        Console.Write("Confirm deletion? [Y/N]: ");
        if (Console.ReadLine().ToUpper() == "Y")
        {
            order.OrderStatus = "Cancelled";
            foreach (var r in restaurantList)
            {
            }
            Console.WriteLine($"Order {oId} cancelled. Refund processed.");
        }
    }
}