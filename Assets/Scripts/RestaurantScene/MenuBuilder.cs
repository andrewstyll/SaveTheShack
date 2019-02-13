using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public sealed class MenuBuilder {

    private static readonly MenuBuilder instance = new MenuBuilder();

    private ConfigSetup configData;
    private readonly string foodSpriteUrl = "Sprites/Food/";

    private Dictionary<string, Food> dictionary;
    private Menu currentMenu;
    private bool setupComplete = false;

    private MenuBuilder() {
        this.configData = ConfigSetup.GetInstance();
        this.configData.RunConfigSetup();
        this.InitMenuBuilder();
        this.setupComplete = true;
    }

    private void InitMenuBuilder() {
        this.dictionary = new Dictionary<string, Food>();
        this.FillDictionary();
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

    public bool MenuSetupComplete() {
        return this.setupComplete;
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
