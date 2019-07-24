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

    private const string RESOURCE_LOCATION = "Assets/Resources/";
    private const string JSON_FILE = ".json";

    private JsonFoodContainer mainsList;
    private JsonFoodContainer toppingsList;
    private JsonFoodContainer drinksList;

    private readonly string mainsInfoFilePath = "Config/Mains";
    private readonly string toppingsInfoFilePath = "Config/Toppings";
    private readonly string drinksInfoFilePath = "Config/Drinks";

    private JsonToRestaurant burgerData;
    private JsonToRestaurant friesData;

    private readonly string burgerRestaurantPath = "Config/BurgerRestaurant";
    private readonly string friesRestaurantPath = "Config/FriesRestaurant";

    private bool foodSetupComplete = false;

    private readonly string monthsPath = "Config/Months";

    private readonly string restSpritesPath = "Config/RestSprites";

    private ConfigSetup() {}

    public void RunFoodConfigSetup() {
        this.mainsList = GetFoodConfig(mainsInfoFilePath);
        this.toppingsList = GetFoodConfig(toppingsInfoFilePath);
        this.drinksList = GetFoodConfig(drinksInfoFilePath);

        this.burgerData = GetRestaurantConfig(burgerRestaurantPath);
        this.friesData = GetRestaurantConfig(friesRestaurantPath);
        this.foodSetupComplete = true;
    }

    private JsonFoodContainer GetFoodConfig(string jsonString) {
            TextAsset jsonFileAsset = Resources.Load<TextAsset>(jsonString);
            if(jsonFileAsset != null) { 
                string jsonFile = jsonFileAsset.text;
                return JsonUtility.FromJson<JsonFoodContainer>(jsonFile);
            } else {
                throw new System.Exception("Config File Not Found: " + RESOURCE_LOCATION + jsonString + JSON_FILE);
            }
    }

    private JsonToRestaurant GetRestaurantConfig(string jsonString) {
        TextAsset jsonFileAsset = (TextAsset)Resources.Load(jsonString);
        if (jsonFileAsset != null) {
            string jsonFile = jsonFileAsset.text;
            return JsonUtility.FromJson<JsonToRestaurant>(jsonFile);
        } else {
            throw new System.Exception("Config File Not Found: " + RESOURCE_LOCATION + jsonString + JSON_FILE);
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
            case RestaurantInfo.Types.Fries:
                return friesData;
            default:
                throw new System.Exception("Unsupported Restaurant Type");
        }
    }

    public bool FoodConfigSetupComplete() {
        return foodSetupComplete;
    }

    public JsonMonthContainer GetMonthData() {
        TextAsset jsonFileAsset = (TextAsset)Resources.Load(monthsPath);
        if (jsonFileAsset != null) {
            string jsonFile = jsonFileAsset.text;
            return JsonUtility.FromJson<JsonMonthContainer>(jsonFile);
        } else {
            throw new System.Exception("Config File Not Found: " + RESOURCE_LOCATION + monthsPath);
        }
    }

    public JsonToRestSpriteContainer GetRestaurantSpriteData() {
        TextAsset jsonFileAsset = (TextAsset)Resources.Load(restSpritesPath);
        if (jsonFileAsset != null) {
            string jsonFile = jsonFileAsset.text;
            return JsonUtility.FromJson<JsonToRestSpriteContainer>(jsonFile);
        } else {
            throw new System.Exception("Config File Not Found: " + RESOURCE_LOCATION + monthsPath);
        }
    }
}