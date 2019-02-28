using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order {

    private List<Food> myFood;
    private Food myDrink;

    Order(List<Food> food, Food drink) {
        this.myFood = food;
        this.myDrink = drink;
    }

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

    public bool IsMatchingOrder(Order order) {
        return (this.IsSameDrink(order.GetDrink()) && this.IsSameFood(order.GetFood()));
    }
}
