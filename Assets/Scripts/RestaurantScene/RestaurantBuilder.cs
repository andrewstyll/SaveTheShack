using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public sealed class RestaurantBuilder {

    private static readonly RestaurantBuilder instance = new RestaurantBuilder();

    private ConfigSetup configData;
    private readonly string foodSpritePath = "Sprites/Food/";
    private Dictionary<string, Food> foodDictionary;
    private bool setupComplete = false;

    // Current restaurant data
    private RestaurantInfo.Types currentRestaurantType = RestaurantInfo.Types.NoType;
    private string[] restaurantMenu;
    private string restaurantSpritePath;
    private MealDrawer mealDrawer;
    private Menu menu;
    private bool mealDrawerCreated = false;

    private RestaurantBuilder() {
        this.configData = ConfigSetup.GetInstance();
        this.configData.RunConfigSetup();
        this.InitMenuBuilder();
        this.setupComplete = true;
    }

    private void InitMenuBuilder() {
        this.foodDictionary = new Dictionary<string, Food>();
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
                                   
                    this.foodDictionary.Add(jsonFood.name, newFood);
                }
            }
        }
    }

    private void BuildMealDrawer(JsonToRestaurant restaurantData) {
        switch(this.currentRestaurantType) {
            case RestaurantInfo.Types.Burger:
                this.mealDrawer = new BurgerDrawer(this.restaurantSpritePath, restaurantData.RestaurantDisplaySprites.Top,
                                                    restaurantData.RestaurantDisplaySprites.Bottom, restaurantData.FoodDisplaySprites);

                foreach (KeyValuePair<string, Food> entry in this.foodDictionary) {
                    // do something with entry.Value or entry.Key
                    if(entry.Value.GetFoodType() == FoodType.Type.drink) {
                        this.mealDrawer.ManuallyAddSprite(entry.Key, entry.Value.GetPreppedSprite());
                    }
                }

                break;
            default:
                throw new System.Exception("Can't create meal drawer of invalid type");
        }

    }

    private void BuildFullMenu() {

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

    public bool MealDrawerCreated() {
        return this.mealDrawerCreated;
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

            this.mealDrawerCreated = false;
            this.BuildMealDrawer(restaurantData);
            this.mealDrawerCreated = true;
        }
        this.BuildNewMenu();
    }

    public Menu GetMenu() {
        return this.menu;
    }

    public MealDrawer GetMealDrawer() {
        return this.mealDrawer;
    }
}
