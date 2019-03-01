using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order {

    private List<Food> myFood;
    private Food myDrink;

    public Order(List<Food> food, Food drink) {
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

    private bool IsSameFood(List<Food> food) {
        if(this.myFood.Count != food.Count) {
            return false;
        } else {
            for(int i = 0; i < this.myFood.Count; i++) {
                if(this.myFood[i].GetName() != food[i].GetName()) {
                    return false;
                }
            }
            return true;
        }
    }

    private bool IsSameDrink(Food drink) {
        return (this.myDrink == drink);
    }

    /**** PUBLIC API ****/
    public List<Food> GetFood() {
        return this.myFood;
    }

    public Food GetDrink() {
        return this.myDrink;
    }
}
