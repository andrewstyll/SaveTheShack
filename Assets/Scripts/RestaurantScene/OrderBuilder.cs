using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class OrderBuilder {
    private static readonly OrderBuilder instance = new OrderBuilder();
    private static RestaurantBuilder restaurantBuilder;

    private OrderBuilder() {
        restaurantBuilder = RestaurantBuilder.GetInstance();
    }

    private List<Food> GetFood() {
        List<Food> food = new List<Food>();

        return food;
    }

    private Food GetDrink() {
        return null;
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