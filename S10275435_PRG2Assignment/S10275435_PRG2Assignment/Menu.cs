//==========================================================
// Student Number : S10275435F
// Student Name   : Bryan Mak Jing Yang
// Partner Name   : Loh Yu Wei Kyran
//==========================================================

using System;
using System.Collections.Generic;

public class Menu
{
    public string MenuID { get; set; }
    public string MenuName { get; set; }
    public List<FoodItem> FoodItems { get; set; } = new List<FoodItem>();

    public Menu(string id, string name)
    {
        MenuID = id;
        MenuName = name;
    }

    public void AddFoodItem(FoodItem item)
    {
        FoodItems.Add(item);
    }

    public override string ToString()
    {
        return $"Menu: {MenuName} ({MenuID})";
    }
}