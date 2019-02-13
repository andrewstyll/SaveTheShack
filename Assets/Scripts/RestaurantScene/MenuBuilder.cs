using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

/* I had this idea for using a PQueue to store different master menus with 
 * random values determining priority, and then using each master menu to create
 * smaller menus that will change every day. Runs faster than linked list
 * removal at index strategy i'm using below but implementing a pqueue isn't what
 * this is about and I don't want to think about my own datastructure performance
 * at all so I'm doing this....
 */
public sealed class MenuBuilder : MonoBehaviour {

    private static MenuBuilder instance;

    private readonly ConfigSetup configData = ConfigSetup.GetInstance();
    private readonly string foodSpriteUrl = "Sprites/Food/";

    private Dictionary<string, Food> dictionary;
    private Menu currentMenu;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            this.dictionary = new Dictionary<string, Food>();
            FillDictionary();
        } else {
            Destroy(gameObject);
        }
    }

    private void FillDictionary() {
        Food newFood = null;

        foreach (FoodType.Type foodType in System.Enum.GetValues(typeof(FoodType.Type))) {
            JsonToFood[] jsonList = this.configData.GetJsonFood(foodType);
            if(jsonList != null) {  
                foreach(JsonToFood jsonFood in jsonList) {
                    newFood = new Food(jsonFood.name, foodType,
                                    Resources.Load<Sprite>(foodSpriteUrl + jsonFood.unPreppedSpriteName),
                                    Resources.Load<Sprite>(foodSpriteUrl + jsonFood.preppedSpriteName),
                                    Resources.Load<Sprite>(foodSpriteUrl + jsonFood.burntSpriteName));
                                   
                    this.dictionary.Add(jsonFood.name, newFood);
                }
            }
        }
    }
    
    private void BuildFullMenu(string[] restaurantMenu) {

        foreach (string item in restaurantMenu) {
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

    private void BuildMenu(string[] restaurantMenu) {
        this.currentMenu = new Menu();

        BuildFullMenu(restaurantMenu);
        // Now need to reduce list by removing elements
    }

    /**** PUBLIC API ****/
    public static MenuBuilder GetInstance() {
        return instance;
    }

    public void BuildMenu(RestaurantInfo.Types type) {

        switch (type) {
            case (RestaurantInfo.Types.Burger):
                BuildMenu(RestaurantInfo.Menus.Burger);
                break;
            case (RestaurantInfo.Types.Taco):
                BuildMenu(RestaurantInfo.Menus.Taco);
                break;
            default:
                throw new System.Exception("invalid menu type " + type);
        }
    }

    public Menu GetMenu() {
        return this.currentMenu;
    }
}
