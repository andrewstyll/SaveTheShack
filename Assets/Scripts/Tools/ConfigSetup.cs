using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public sealed class ConfigSetup {

    private static readonly ConfigSetup instance = new ConfigSetup();

    private JsonFoodContainer mainsList;
    private JsonFoodContainer toppingsList;
    private JsonFoodContainer drinksList;

    private string spritePath = "Sprites/";
    private string mainsInfoFilePath = "Assets/Config/Mains.json";
    private string toppingsInfoFilePath = "Assets/Config/Toppings.json";
    private string drinksInfoFilePath = "Assets/Config/Drinks.json";

    private ConfigSetup() {
        this.mainsList = GetConfig(mainsInfoFilePath);
        this.toppingsList = GetConfig(toppingsInfoFilePath);
        //GetConfig(drinksInfoFilePath, this.drinksList);
    }

    private JsonFoodContainer GetConfig(string jsonString) {

        if (File.Exists(jsonString)) {
            string jsonFile = File.ReadAllText(jsonString);
            return JsonUtility.FromJson<JsonFoodContainer>(jsonFile);
        } else {
            throw new System.Exception("Config File Not Found: " + jsonString);
        }
    }

    public static ConfigSetup GetInstance() {
        return instance;
    }

    public JsonToFood[] GetJsonFood(FoodType.Type foodType) {
        switch(foodType) {
            case FoodType.Type.main:
                return mainsList.List;
            case FoodType.Type.topping:
                return toppingsList.List;
            case FoodType.Type.drink:
                return drinksList.List;
            default:
                Debug.LogError("Invalid foodtype config retrieval");
                break;
        }
        return null;
    }
}