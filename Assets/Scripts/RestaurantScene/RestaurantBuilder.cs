﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public sealed class RestaurantBuilder {

    private static readonly RestaurantBuilder instance = new RestaurantBuilder();

    private const int FIVE_TOPPINGS_THRESH = 2;
    private const int SIX_TOPPINGS_THRESH = 3;

    // config/setup variables
    private ConfigSetup configData;
    private Dictionary<string, Food> foodDictionary;
    private bool setupComplete = false;

    // Restaurant Sprite Setup
    private Dictionary<string, Sprite> themeSpriteDictionary;

    // Current restaurant data
    private RestaurantInfo.Types currentRestaurantType = RestaurantInfo.Types.NoType;
    private string[] restaurantMenu;
    private string restaurantSpritePath;

    // Meal Drawer
    private bool mealDrawerSetupComplete = false;
    private Dictionary<string, Sprite> mealDrawerData;
    private Menu menu;


    private RestaurantBuilder() {
        this.configData = ConfigSetup.GetInstance();
        this.configData.RunFoodConfigSetup();
        this.InitMenuBuilder();
        this.setupComplete = true;
    }

    private void InitMenuBuilder() {
        this.foodDictionary = new Dictionary<string, Food>();
        this.FillFoodDictionary();
    }

    private void FillFoodDictionary() {
        Food newFood = null;

        foreach (FoodType.Type foodType in System.Enum.GetValues(typeof(FoodType.Type))) {
            JsonFoodContainer jsonFoodData = this.configData.GetJsonFood(foodType);
            JsonToFood[] jsonList = jsonFoodData.List;
            string foodSpritePath = jsonFoodData.SpriteLocation;
         
            foreach(JsonToFood jsonFood in jsonList) {
                newFood = new Food(jsonFood.name, foodType,
                                Resources.Load<Sprite>(foodSpritePath + jsonFood.unPreppedSpriteName),
                                Resources.Load<Sprite>(foodSpritePath + jsonFood.preppedSpriteName),
                                Resources.Load<Sprite>(foodSpritePath + jsonFood.burntSpriteName));
                               
                this.foodDictionary.Add(jsonFood.name, newFood);
            }
        }
    }

    private void MealDrawerSetup(JsonToRestaurant restaurantData) {
        this.mealDrawerData = new Dictionary<string, Sprite>();
        foreach (JsonSpritesObject displayObj in restaurantData.FoodDisplaySprites) {
            this.mealDrawerData.Add(displayObj.Name, Resources.Load<Sprite>(this.restaurantSpritePath + displayObj.SpriteName));
        }
        // add drinks as they re-use sprites from the menu
        foreach (KeyValuePair<string, Food> entry in this.foodDictionary) {
            // do something with entry.Value or entry.Key
            if (entry.Value.GetFoodType() == FoodType.Type.drink) {
                this.mealDrawerData.Add(entry.Key, entry.Value.GetPreppedSprite());
            }
        }
    }

    private void FillThemeSpriteDictionary(JsonToRestaurant restaurantData) {
        this.themeSpriteDictionary = new Dictionary<string, Sprite>();
        foreach(JsonSpritesObject spritePair in restaurantData.RestaurantThemeSprites) {
            this.themeSpriteDictionary.Add(spritePair.Name, 
                                            Resources.Load<Sprite>(this.restaurantSpritePath + spritePair.SpriteName));
        }
    }

    private void BuildNewMenu(int daysPassed) {
        this.menu = new Menu();
        int numToppingsToRemove = 0;
        foreach (string item in this.restaurantMenu) {
            // check to see if it exists in dictionary
            if (foodDictionary.ContainsKey(item)) {
                Food foodItem = foodDictionary[item];
                switch (foodItem.GetFoodType()) {
                    case FoodType.Type.main:
                        menu.SetMain(foodItem);
                        break;
                    case FoodType.Type.topping:
                        menu.AddTopping(foodItem);
                        break;
                    case FoodType.Type.drink:
                        menu.AddDrink(foodItem);
                        break;
                    default:
                        throw new System.Exception("invalid food type " + foodItem.GetFoodType());
                }
            } else {
                throw new System.Exception("invalid menu item " + item);
            }
        }

        if(daysPassed >= SIX_TOPPINGS_THRESH && menu.GetToppingsLength() > 6) {
            numToppingsToRemove = menu.GetToppingsLength() - 6;
        } else if(daysPassed >= FIVE_TOPPINGS_THRESH && menu.GetToppingsLength() > 5) {
            numToppingsToRemove = menu.GetToppingsLength() - 5;
        } else if(menu.GetToppingsLength() > 4) {
            numToppingsToRemove = menu.GetToppingsLength() - 4;
        }

        while(numToppingsToRemove > 0) {
            menu.RemoveToppingAtIndex(Random.Range(0, menu.GetToppingsLength()-1));
            numToppingsToRemove--;
        }

        while(menu.GetDrinksLength() > 2) {
            menu.RemoveDrinkAtIndex(Random.Range(0, menu.GetDrinksLength()-1));
        }

    }

    /**** PUBLIC API ****/
    public static RestaurantBuilder GetInstance() {
        return instance;
    }

    // Denotes the the menu database has been filled
    public bool SetupComplete() {
        return this.setupComplete;
    }

    public bool MealDrawerSetupComplete() {
        return this.mealDrawerSetupComplete;
    }

    public RestaurantInfo.Types GetCurrentRestaurantType() {
        return this.currentRestaurantType;
    }

    public void BuildRestaurant(RestaurantInfo.Types type, int daysPassed) {
        if(type == RestaurantInfo.Types.NoType) {
            throw new System.Exception("Can't build restaurant of type NoType");
        } else if(type != this.currentRestaurantType) {
            // new restaurant setup
            this.currentRestaurantType = type;
            JsonToRestaurant restaurantData = configData.GetRestaurantData(this.currentRestaurantType);
            this.restaurantMenu = restaurantData.Menu;
            this.restaurantSpritePath = restaurantData.SpriteLocation;

            this.FillThemeSpriteDictionary(restaurantData);

            this.mealDrawerSetupComplete = false;
            this.MealDrawerSetup(restaurantData);
            this.mealDrawerSetupComplete = true;
        }
        this.BuildNewMenu(daysPassed);
    }

    public Menu GetMenu() {
        return this.menu;
    }

    public Dictionary<string, Sprite> GetMealDrawerData() {
        return this.mealDrawerData;
    }
    
    public Sprite GetThemedSprite(string name) {
        return this.themeSpriteDictionary[name];
    }
}
