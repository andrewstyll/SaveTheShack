using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

/* This only needs to be run once per game startup i believe? I will put this
 * with the title screen later. I think it's better for this to be a mono behaviour
 * so that everything runs at the same time
 */
public sealed class ConfigSetup {

    private static readonly ConfigSetup instance = new ConfigSetup();

    private JsonFoodContainer mainsList;
    private JsonFoodContainer toppingsList;
    private JsonFoodContainer drinksList;

    private readonly string mainsInfoFilePath = "Assets/Resources/Config/Mains.json";
    private readonly string toppingsInfoFilePath = "Assets/Resources/Config/Toppings.json";
    private readonly string drinksInfoFilePath = "Assets/Resources/Config/Drinks.json";

    private JsonToRestaurant burgerData; 

    private readonly string burgerRestaurantPath = "Assets/Resources/Config/BurgerRestaurant.json";

    private bool setupComplete = false;

    private ConfigSetup() {}

    public void RunConfigSetup() {
        this.mainsList = GetFoodConfig(mainsInfoFilePath);
        this.toppingsList = GetFoodConfig(toppingsInfoFilePath);
        this.drinksList = GetFoodConfig(drinksInfoFilePath);

        this.burgerData = GetRestaurantConfig(burgerRestaurantPath);
        this.setupComplete = true;
    }

    private JsonFoodContainer GetFoodConfig(string jsonString) {

        if (File.Exists(jsonString)) {
            string jsonFile = File.ReadAllText(jsonString);
            return JsonUtility.FromJson<JsonFoodContainer>(jsonFile);
        } else {
            throw new System.Exception("Config File Not Found: " + jsonString);
        }
    }

    private JsonToRestaurant GetRestaurantConfig(string jsonString) {
        if (File.Exists(jsonString)) {
            string jsonFile = File.ReadAllText(jsonString);
            return JsonUtility.FromJson<JsonToRestaurant>(jsonFile);
        } else {
            throw new System.Exception("Config File Not Found: " + jsonString);
        }
    }

    /**** Public API ****/
    public static ConfigSetup GetInstance() {
        return instance;
    }

    public JsonFoodContainer GetJsonFood(FoodType.Type foodType) {
        switch(foodType) {
            case FoodType.Type.main:
                return mainsList;
            case FoodType.Type.topping:
                return toppingsList;
            case FoodType.Type.drink:
                return drinksList;
            default:
                throw new System.Exception("Invalid foodtype config retrieval");
        }
    }

    public JsonToRestaurant GetRestaurantData(RestaurantInfo.Types restaurantType) {
        switch (restaurantType) {
            case RestaurantInfo.Types.Burger:
                return burgerData;
            default:
                throw new System.Exception("Unsupported Restaurant Type");
        }
    }

    public bool ConfigSetupComplete() {
        return setupComplete;
    }
}