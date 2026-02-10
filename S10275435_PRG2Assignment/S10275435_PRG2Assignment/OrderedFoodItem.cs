//==========================================================
// Student Number : S10275435F
// Student Name   : Bryan Mak Jing Yang
// Partner Name   : Loh Yu Wei Kyran
//==========================================================

using System;
using System.Collections.Generic;

public class OrderedFoodItem : FoodItem
{
    public int QtyOrdered { get; set; }

    public OrderedFoodItem() { }

    public OrderedFoodItem(string itemName, string itemDesc, double itemPrice, int qty)
        : base(itemName, itemDesc, itemPrice)
    {
        QtyOrdered = qty;
    }

    public double CalculateSubtotal()
    {
        return ItemPrice * QtyOrdered;
    }
}