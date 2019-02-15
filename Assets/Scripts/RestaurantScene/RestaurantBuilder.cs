using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public sealed class RestaurantBuilder {

    private static readonly RestaurantBuilder instance = new RestaurantBuilder();

    private ConfigSetup configData;
    private readonly string foodSpritePath = "Sprites/Food/";
    private Dictionary<string, Food> dictionary;
    private bool setupComplete = false;

    // Current restaurant data
    private RestaurantInfo.Types currentRestaurantType = RestaurantInfo.Types.NoType;
    private string[] restaurantMenu;
    private string restaurantSpritePath;
    private MealDrawer mealDrawer;
    private Menu menu;

    private RestaurantBuilder() {
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
                                    Resources.Load<Sprite>(foodSpritePath + jsonFood.unPreppedSpriteName),
                                    Resources.Load<Sprite>(foodSpritePath + jsonFood.preppedSpriteName),
                                    Resources.Load<Sprite>(foodSpritePath + jsonFood.burntSpriteName));
                                   
                    this.dictionary.Add(jsonFood.name, newFood);
                }
            }
        }
    }

    private void BuildMealDrawer(JsonToRestaurant restaurantData) {
        switch(this.currentRestaurantType) {
            case RestaurantInfo.Types.Burger:
                this.mealDrawer = new BurgerDrawer(
                                    Resources.Load<Sprite>(restaurantSpritePath + restaurantData.ServingSprites.Top),
                                    Resources.Load<Sprite>(restaurantSpritePath + restaurantData.ServingSprites.Bottom));
                break;
            default:
                throw new System.Exception("Can't create meal drawer of invalid type");
        }
    }

    private void BuildFullMenu() {

        foreach (string item in this.restaurantMenu) {
            // check to see if it exists in dictionary
            if (dictionary.ContainsKey(item)) {
                Food foodItem = dictionary[item];
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
    }

    private void BuildNewMenu() {
        this.menu = new Menu();
        BuildFullMenu();
        // Now need to reduce list by removing elements
    }

    /**** PUBLIC API ****/
    public static RestaurantBuilder GetInstance() {
        return instance;
    }

    public bool SetupComplete() {
        return this.setupComplete;
    }

    public void BuildRestaurant(RestaurantInfo.Types type) {
        if(type == RestaurantInfo.Types.NoType) {
            throw new System.Exception("Can't build restaurant of type NoType");
        } else if(type != this.currentRestaurantType) {
            // new restaurant setup
            this.currentRestaurantType = type;
            JsonToRestaurant restaurantData = configData.GetRestaurantData(this.currentRestaurantType);
            this.restaurantMenu = restaurantData.Menu;
            this.restaurantSpritePath = restaurantData.SpriteLocation;

            //this.BuildMealDrawer(restaurantData);
        }
        this.BuildNewMenu();
    }

    public Menu GetMenu() {
        return this.menu;
    }
}
