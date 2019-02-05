using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Menu {
    Food main;
    Food[] toppings;
    Food[] drinks;
};

// create an instance of this scriptable object
public sealed class MenuBuilder : ScriptableObject {
    
    private static MenuBuilder instance = new MenuBuilder();

    private Dictionary<string, Food> dictionary;
    private Menu currentMenu;

    /**** MENU ASSET LIST ****/
    [SerializeField] private Sprite Burger;
    [SerializeField] private Sprite Taco;
    [SerializeField] private Sprite Lettuce;
    [SerializeField] private Sprite Tomato;
    [SerializeField] private Sprite Cheese;
    [SerializeField] private Sprite Onion;
    [SerializeField] private Sprite Guacamole;
    [SerializeField] private Sprite SourCream;
    [SerializeField] private Sprite Coffee;
    [SerializeField] private Sprite RootBeer;
    [SerializeField] private Sprite GreenSoda;
    [SerializeField] private Sprite IceTea;

    private MenuBuilder() {
        dictionary = new Dictionary<string, Food>();
        BuildDictionary();
    }

    private void BuildDictionary() {
        AddMains();
        AddToppings();
        AddDrinks();
    }

    private void AddMains() {
        foreach(string main in FoodType.Mains) {
            // need to map assets to strings
            switch(main) {
                case "Burger":
                    dictionary.Add(main, new Food(main, FoodType.Type.main, Burger));
                    break;
                case "Taco":
                    dictionary.Add(main, new Food(main, FoodType.Type.main, Taco));
                    break;
                default:
                    Debug.LogError("invalid main " + main);
                    break;
            }
        }
    }

    private void AddToppings() {
        foreach (string topping in FoodType.Toppings) {
            // need to map assets to strings
            switch (topping) {
                case "Lettuce":
                    dictionary.Add(topping, new Food(topping, FoodType.Type.topping, Lettuce));
                    break;
                case "Tomato":
                    dictionary.Add(topping, new Food(topping, FoodType.Type.topping, Tomato));
                    break;
                case "Cheese":
                    dictionary.Add(topping, new Food(topping, FoodType.Type.topping, Cheese));
                    break;
                case "Onion":
                    dictionary.Add(topping, new Food(topping, FoodType.Type.topping, Onion));
                    break;
                case "Guacamole":
                    dictionary.Add(topping, new Food(topping, FoodType.Type.topping, Guacamole));
                    break;
                case "SourCream":
                    dictionary.Add(topping, new Food(topping, FoodType.Type.topping, SourCream));
                    break;
                default:
                    Debug.LogError("invalid topping " + topping);
                    break;
            }
        }
    }

    private void AddDrinks() {
        foreach (string drink in FoodType.Drinks) {
            // need to map assets to strings
            switch (drink) {
                case "Coffee":
                    dictionary.Add(drink, new Food(drink, FoodType.Type.drink, Coffee));
                    break;
                case "RootBeer":
                    dictionary.Add(drink, new Food(drink, FoodType.Type.drink, RootBeer));
                    break;
                case "GreenSoda":
                    dictionary.Add(drink, new Food(drink, FoodType.Type.drink, GreenSoda));
                    break;
                case "IceTea":
                    dictionary.Add(drink, new Food(drink, FoodType.Type.drink, IceTea));
                    break;
                default:
                    Debug.LogError("invalid drink " + drink);
                    break;
            }
        }
    }

    /**** PUBLIC API ****/

    public static MenuBuilder GetInstance() {
        return instance;
    }

    public Menu GetMenu() {
        return currentMenu;
    }
}
