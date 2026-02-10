//==========================================================
// Student Number : S10275435F
// Student Name   : Bryan Mak Jing Yang
// Partner Name   : Loh Yu Wei Kyran
//==========================================================

using System;
using System.Collections.Generic;

public class Customer
{
    public string CustomerName { get; set; }
    public string EmailAddress { get; set; }
    public List<Order> OrderList { get; set; } = new List<Order>();

    public Customer(string name, string email)
    {
        CustomerName = name;
        EmailAddress = email;
    }

    public void AddOrder(Order order)
    {
        OrderList.Add(order);
    }

    public override string ToString()
    {
        return $"{CustomerName} ({EmailAddress})";
    }
}