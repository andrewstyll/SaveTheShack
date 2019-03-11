using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public sealed class RestaurantBuilder {

    private static readonly RestaurantBuilder instance = new RestaurantBuilder();

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

    private void BuildMealDrawer(JsonToRestaurant restaurantData) {
        switch(this.currentRestaurantType) {
            case RestaurantInfo.Types.Burger:
                this.mealDrawer = new BurgerDrawer(this.restaurantSpritePath, restaurantData.RestaurantFoodDisplaySprites.Top,
                                                    restaurantData.RestaurantFoodDisplaySprites.Bottom, restaurantData.FoodDisplaySprites);

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

    private void FillThemeSpriteDictionary(JsonToRestaurant restaurantData) {
        this.themeSpriteDictionary = new Dictionary<string, Sprite>();
        switch (this.currentRestaurantType) {
            case RestaurantInfo.Types.Burger:
                foreach(JsonSpritesObject spritePair in restaurantData.RestaurantThemeSprites) {
                    this.themeSpriteDictionary.Add(spritePair.Name, 
                                                    Resources.Load<Sprite>(this.restaurantSpritePath + spritePair.SpriteName));
                }

                break;
            default:
                throw new System.Exception("Can't Fill theme sprites for invalid type");
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

            this.FillThemeSpriteDictionary(restaurantData);

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

    public Sprite GetThemedSprite(string name) {
        return this.themeSpriteDictionary[name];
    }
}
