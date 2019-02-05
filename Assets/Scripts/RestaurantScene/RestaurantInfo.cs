using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* don't see benefit yet of making this a scriptable object. Means that there
 * will have to be instances of it. Seems useful if i want to adjust the menu as
 * the game progresses, I'll worry about it later
 */
public static class RestaurantInfo {
    public enum Types {
        Burger,
        Taco
    };

    public static class Menus {

        /* need better way of differentiating between drinks food and mains and
         * drinks. Right now feels inefficient      
         */
        public static readonly string[] Drinks = {
            "Coffee",
            "RootBeer",
            "GreenSoda",
            "IceTea"
        };

        // assume only 1 main per menu for now.
        public static readonly string[] Burger = {
            "Burger",
            "Lettuce",
            "Tomato",
            "Cheese",
            "Onion"
        };

        public static readonly string[] Taco = {
            "Taco",
            "Lettuce",
            "Tomato",
            "Cheese",
            "Guacamole",
            "SourCream",
        };

        // Doesn't account for main being stored in the array with the toppings
        // need to seperate main from the toppings or come up with something else
        public static bool IsValidAmount(string[] array, int val) {
            return (val <= array.Length && val >= 0);
        }
    }
}
