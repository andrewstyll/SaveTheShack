using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Customer {
    // has an order List
    private List<FoodStub> foodOrder;
    private FoodStub drinkOrder;

    // used to track customer status
    private float patience;
    private float timeRemaining;
    private int relationshipScore;

    // pass in these constants later
    Customer(Menu menu) {
        patience = 10.0f;
        relationshipScore = 3;
        InitCustomer(menu);
    }

    public List<FoodStub> GetFoodOrder() {
        return foodOrder;
    }

    public FoodStub GetDrinkOrder() {
        return drinkOrder;
    }

    public int GetRelationshipScore() {
        return relationshipScore;
    }

    // returned as a percentage (100 means no time remaining)
    public float GetTimeRemaining() {
        return (patience - timeRemaining) / patience;
    }

    public void UpdateTimeRemaining(float deltaTime) {
        timeRemaining -= deltaTime;
    }

    // take menu argument
    public void InitCustomer(Menu menu) {
        timeRemaining = patience;
        // create orders here using menu.
        // grab main, then random number of toppings, then random order including repeats
        
        // determine if yes or no drink
    }

    public bool MatchesFood(List<FoodStub> foodServed) {
        if (foodServed.Count == foodOrder.Count) {
            int i = 0;
            while (i < foodOrder.Count) {
                if (foodOrder[i] != foodServed[i]) {
                    break;
                }
                i++;
            }
            if (i == foodOrder.Count) {
                return true;
            }
        }
        return false;
    }

    public bool MatchesDrink(FoodStub drinkServed) {
        return drinkServed == drinkOrder;
    }
}
