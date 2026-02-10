//==========================================================
// Student Number : S10275435F
// Student Name   : Bryan Mak Jing Yang
// Partner Name   : Loh Yu Wei Kyran
//==========================================================

using System;
public class FoodItem
{
    public string ItemName { get; set; }
    public string ItemDesc { get; set; }
    public double ItemPrice { get; set; }

    public FoodItem() { }

    public FoodItem(string itemName, string itemDesc, double itemPrice)
    {
        ItemName = itemName;
        ItemDesc = itemDesc;
        ItemPrice = itemPrice;
    }

    public override string ToString()
    {
        return $"{ItemName,-15} {ItemDesc,-40} ${ItemPrice:F2}";
    }
}