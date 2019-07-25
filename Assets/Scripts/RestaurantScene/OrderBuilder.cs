using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public sealed class OrderBuilder {

    private const int MAX_TOPPINGS = 5;
    private const int ODDS_NO_MAIN = 5;
    private const int ODDS_NO_DRINK = 10;

    private static readonly OrderBuilder instance = new OrderBuilder();
    private static RestaurantBuilder restaurantBuilder;

    private OrderBuilder() {
        restaurantBuilder = RestaurantBuilder.GetInstance();
    }

    private List<string> GetFood() {
        List<string> foodOrder = new List<string>();
        Menu menu = restaurantBuilder.GetMenu();
        int foodToAdd = MAX_TOPPINGS;
        if (menu == null) Debug.Log("Null menu");
        // determine if main used or no main, 5% chance of no main
        foodOrder.Add(menu.GetMain().GetName());
        foodToAdd--;

        // determine how many toppings to add
        foodToAdd = Random.Range(0, foodToAdd);

        // add toppings, ensuring no 2 same toppings in a row
        List<string> toppingList = new List<string>();
        foreach(Food food in menu.GetToppings()) {
            toppingList.Add(food.GetName());
        }
        string lastFoodAdded = null;
        string newFoodSelected = null;
        for(int i = 0; i < foodToAdd; i++) {
            newFoodSelected = toppingList[Random.Range(0, toppingList.Count)];
            foodOrder.Add(newFoodSelected);

            if (lastFoodAdded != null) {
                toppingList.Add(lastFoodAdded);
            }
            toppingList.Remove(newFoodSelected);
            lastFoodAdded = newFoodSelected;
        }

        return foodOrder;
    }

    private string GetDrink() {
        string drinkOrder = null;
        Menu menu = restaurantBuilder.GetMenu();

        if (Random.Range(0, 100) > ODDS_NO_DRINK) {
            drinkOrder = menu.GetDrinks()[Random.Range(0, menu.GetDrinksLength())].GetName();
        }
        return drinkOrder;
    }

    public static OrderBuilder GetInstance() {
        return instance;
    }

    public Order BuildOrder() {
        List<string> food = GetFood();
        string drink = GetDrink();

        return new Order(food, drink);
    }
}