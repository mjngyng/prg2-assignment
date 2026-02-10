//==========================================================
// Student Number : S10275435F
// Student Name   : Bryan Mak Jing Yang
// Partner Name   : Loh Yu Wei Kyran
//==========================================================

using System;
using System.Collections.Generic;

public class Order
{
    public int OrderID { get; set; }
    public DateTime OrderDateTime { get; set; }
    public string OrderStatus { get; set; }
    public DateTime DeliveryDateTime { get; set; }
    public string DeliveryAddress { get; set; }
    public string OrderPaymentMethod { get; set; }
    public bool OrderPaid { get; set; }
    public List<OrderedFoodItem> OrderedFoodItems { get; set; } = new List<OrderedFoodItem>();

    public Order() { }

    public Order(int id, DateTime orderDate)
    {
        OrderID = id;
        OrderDateTime = orderDate;
        OrderStatus = "Pending";
    }

    public void AddOrderedFoodItem(OrderedFoodItem item)
    {
        OrderedFoodItems.Add(item);
    }

    public double CalculateOrderTotal()
    {
        double total = 0;
        foreach (var item in OrderedFoodItems)
        {
            total += item.CalculateSubtotal();
        }
        return total;
    }

    public override string ToString()
    {
        return $"{OrderID,-8} {OrderStatus,-12} {DeliveryDateTime,-22} ${CalculateOrderTotal():F2}";
    }
}