using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* I had this idea for using a PQueue to store different master menus with 
 * random values determining priority, and then using each master menu to create
 * smaller menus that will change every day. Runs faster than linked list
 * removal at index strategy i'm using below but implementing a pqueue isn't what
 * this is about and I don't want to think about my own datastructure performance
 * at all so I'm doing this....
 */
public sealed class MenuBuilder : MonoBehaviour {

    Random random = new Random();
    private static MenuBuilder instance;

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

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            dictionary = new Dictionary<string, Food>();
            BuildDictionary();
        } else {
            Destroy(gameObject);
        }
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
                    throw new System.Exception("invalid main " + main);
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
                    throw new System.Exception("invalid topping " + topping);
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
                    throw new System.Exception("invalid drink " + drink);
            }
        }
    }

    // This method is inefficient due to poor convention in seperating menu items
    private void BuildFullMenu(string[] food, string[] drinks) {

        foreach (string item in food) {
            // check to see if it exists in dictionary
            if (dictionary.ContainsKey(item)) {
                Food foodItem = dictionary[item];
                switch (foodItem.GetFoodType()) {
                    case FoodType.Type.main:
                        currentMenu.SetMain(foodItem);
                        break;
                    case FoodType.Type.topping:
                        currentMenu.AddTopping(foodItem);
                        break;
                    default:
                        throw new System.Exception("invalid food type " + foodItem.GetFoodType());
                }
            } else {
                throw new System.Exception("invalid menu item " + item);
            }
        }

        foreach (string item in drinks) {
            // check to see if it exists in dictionary
            if (dictionary.ContainsKey(item)) {
                Food foodItem = dictionary[item];
                switch (foodItem.GetFoodType()) {
                    case FoodType.Type.drink:
                        currentMenu.AddDrink(foodItem);
                        break;
                    default:
                        throw new System.Exception("invalid food type " + foodItem.GetFoodType());
                }
            } else {
                throw new System.Exception("invalid menu item " + item);
            }
        }
    }

    private void BuildMenu(string[] food, string[] drinks, int numToppings, int numDrinks) {

        if (!RestaurantInfo.Menus.IsValidAmount(food, numToppings) ||
                !RestaurantInfo.Menus.IsValidAmount(drinks, numDrinks)) {
            throw new System.Exception("invalid number of toppings or drinks");
        }

        this.currentMenu = new Menu();

        BuildFullMenu(food, drinks);

        // Now need to reduce list by removing elements
    }

    /**** PUBLIC API ****/
    public static MenuBuilder GetInstance() {
        return instance;
    }

    public void BuildMenu(RestaurantInfo.Types type) {
        // On API update I will implement using these values, removed atm to 
        // avoid confusing API
        int numToppings = 0;
        int numDrinks = 0;

        switch (type) {
            case (RestaurantInfo.Types.Burger):
                BuildMenu(RestaurantInfo.Menus.Burger, RestaurantInfo.Menus.Drinks, 
                            numToppings, numDrinks);
                break;
            case (RestaurantInfo.Types.Taco):
                BuildMenu(RestaurantInfo.Menus.Taco, RestaurantInfo.Menus.Drinks, 
                            numToppings, numDrinks);
                break;
            default:
                throw new System.Exception("invalid menu type " + type);
        }
    }

    public Menu GetMenu() {
        return this.currentMenu;
    }
}
