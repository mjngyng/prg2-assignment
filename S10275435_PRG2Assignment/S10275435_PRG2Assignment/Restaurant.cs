//==========================================================
// Student Number : S10275435F
// Student Name   : Bryan Mak Jing Yang
// Partner Name   : Loh Yu Wei Kyran
//==========================================================

using System;
using System.Collections.Generic;

public class Restaurant
{
    public string RestaurantID { get; set; }
    public string RestaurantName { get; set; }
    public string RestaurantEmail { get; set; }
    public List<Menu> Menus { get; set; } = new List<Menu>();
    public Queue<Order> OrderQueue { get; set; } = new Queue<Order>();

    public Restaurant(string id, string name, string email)
    {
        RestaurantID = id;
        RestaurantName = name;
        RestaurantEmail = email;
    }

    public void AddMenu(Menu menu)
    {
        Menus.Add(menu);
    }

    public override string ToString()
    {
        return $"{RestaurantName} ({RestaurantID})";
    }
}