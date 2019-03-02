using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order {

    private List<string> myFood;
    private string myDrink;

    public Order(List<string> food, string drink) {
        this.myFood = food;
        this.myDrink = drink;
    }
     /**** Operator Overload ****/
    public static bool operator == (Order a, Order b) {
        return (a.IsSameDrink(b.GetDrink()) && a.IsSameFood(b.GetFood()));
    }

    public static bool operator != (Order a, Order b) {
        return (!a.IsSameDrink(b.GetDrink()) && !a.IsSameFood(b.GetFood()));
    }
    /**** ****/

    private bool IsSameFood(List<string> food) {
        if(this.myFood.Count != food.Count) {
            return false;
        } else {
            for(int i = 0; i < this.myFood.Count; i++) {
                if(this.myFood[i] != food[i]) {
                    return false;
                }
            }
            return true;
        }
    }

    private bool IsSameDrink(string drink) {
        return (this.myDrink == drink);
    }

    public void PrintOrder() {
        Debug.Log("Drink is: " + (this.myDrink != null ? this.myDrink : "no drink set"));
        Debug.Log("Food Order is currently: " + (this.myFood.Count == 0 ? "no food set" : ""));
        foreach (string food in this.myFood) {
            Debug.Log(food);
        }
    }

    /**** PUBLIC API ****/
    public List<string> GetFood() {
        return this.myFood;
    }

    public string GetDrink() {
        return this.myDrink;
    }
}
