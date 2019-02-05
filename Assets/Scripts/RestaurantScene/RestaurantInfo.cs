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
            "SourCream"
        };

        public static bool IsValidAmount(string[] array, int val) {
            return (array.Length - 1 <= val && val >= 0);
        }
    }
}
