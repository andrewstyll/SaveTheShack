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

    private List<Food> GetFood() {
        List<Food> foodOrder = new List<Food>();
        Menu menu = restaurantBuilder.GetMenu();
        int foodToAdd = MAX_TOPPINGS;

        // determine if main used or no main, 5% chance of no main
        if (Random.Range(0,100) > ODDS_NO_MAIN) {
            foodOrder.Add(menu.GetMain());
            foodToAdd--;
        }

        // determine how many toppings to add
        foodToAdd = Random.Range(0, foodToAdd);

        // add toppings, ensuring no 2 same toppings in a row
        List<Food> toppingList = new List<Food>(menu.GetToppings());
        Food lastFoodAdded = null;
        Food newFoodSelected = null;
        for(int i = 0; i < foodToAdd; i++) {
            newFoodSelected = toppingList[Random.Range(0, toppingList.Count - 1)];
            foodOrder.Add(newFoodSelected);

            if (lastFoodAdded != null) {
                toppingList.Add(lastFoodAdded);
            }
            toppingList.Remove(newFoodSelected);
            lastFoodAdded = newFoodSelected;
        }

        return foodOrder;
    }

    private Food GetDrink() {
        Food drinkOrder = null;
        Menu menu = restaurantBuilder.GetMenu();

        if (Random.Range(0, 100) > ODDS_NO_DRINK) {
            drinkOrder = menu.GetDrinks()[Random.Range(0, menu.GetDrinksLength() - 1)];
        }
        return drinkOrder;
    }

    public static OrderBuilder GetInstance() {
        return instance;
    }

    public Order BuildOrder() {
        List<Food> food = GetFood();
        Food drink = GetDrink();

        return new Order(food, drink);
    }


}