using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FoodType {
    public enum Type {
        main,
        topping,
        drink
    };

    public static readonly string[] Mains = {
        "burger",
        "taco"
    };

    public static readonly string[] Toppings = {
        "Lettuce",
        "Tomato",
        "Cheese",
        "Onion",
        "Guacamole",
        "SourCream"
    };

    public static readonly string[] Drinks = {
        "Coffee",
        "RootBeer",
        "GreenSoda",
        "IceTea"
    };
}
