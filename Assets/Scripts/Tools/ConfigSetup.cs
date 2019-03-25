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

    private Dictionary<RestaurantInfo.Types, JsonFoodContainer> mainsList;
    private Dictionary<RestaurantInfo.Types, JsonFoodContainer> toppingsList;
    private JsonFoodContainer drinksList;

    private readonly string basePath = "Assets/Resources/Config/";
    private readonly string mainsFilePath = "Mains.json";
    private readonly string toppingsFilePath = "Toppings.json";
    private readonly string drinksFilePath = "Drinks.json";

    private readonly string burgerRestaurantConfigPath = "BurgerRestaurant/";
    private readonly string burgerFilePath = "BurgerRestaurant.json";

    private JsonToRestaurant burgerData;

    private bool setupComplete = false;

    private ConfigSetup() {
        this.mainsList = new Dictionary<RestaurantInfo.Types, JsonFoodContainer>();
        this.toppingsList = new Dictionary<RestaurantInfo.Types, JsonFoodContainer>();
    }

    public void RunConfigSetup() {
        foreach (RestaurantInfo.Types type in System.Enum.GetValues(typeof(RestaurantInfo.Types))) {
            string filePath = null;
            switch (type) {
                case RestaurantInfo.Types.Burger:
                    filePath = this.basePath + this.burgerRestaurantConfigPath;
                    break;
                case RestaurantInfo.Types.Fries:
                    break;
            }
            if (filePath != null) {
                this.mainsList.Add(type, GetFoodConfig(filePath + this.mainsFilePath));
                this.toppingsList.Add(type, GetFoodConfig(filePath + this.toppingsFilePath));
            }

        }

        this.drinksList = GetFoodConfig(this.basePath + this.drinksFilePath);

        this.burgerData = GetRestaurantConfig(this.basePath + this.burgerRestaurantConfigPath + this.burgerFilePath);
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

    public bool HasRestaurantType(FoodType.Type foodType, RestaurantInfo.Types restType) {
        switch (foodType) {
            case FoodType.Type.main:
                return mainsList.ContainsKey(restType);
            case FoodType.Type.topping:
                return toppingsList.ContainsKey(restType);
            case FoodType.Type.drink: // no restaurant specific drinks atm so return false
                return false;
            default:
                throw new System.Exception("Invalid foodtype config retrieval");
        }
    }

    public JsonFoodContainer GetJsonFood(FoodType.Type foodType, RestaurantInfo.Types restType) {
        switch(foodType) {
            case FoodType.Type.main:
                return mainsList[restType];
            case FoodType.Type.topping:
                return toppingsList[restType];
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