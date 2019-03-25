using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random; 

public sealed class RestaurantBuilder {

    private struct FoodKey {
        public RestaurantInfo.Types restType;
        public string foodName;
        public FoodKey(RestaurantInfo.Types restType, string foodName) {
            this.restType = restType;
            this.foodName = foodName;
        }
    }

    private static readonly RestaurantBuilder instance = new RestaurantBuilder();

    private const int FIVE_TOPPINGS_THRESH = 2;
    private const int SIX_TOPPINGS_THRESH = 3;
    private const int MAX_DRINKS_LENGTH = 2;

    // config/setup variables
    private ConfigSetup configData;
    private Dictionary<FoodKey, Food> foodDictionary;
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
        this.foodDictionary = new Dictionary<FoodKey, Food>();
        this.FillFoodDictionary();
    }

    private void FillFoodDictionary() {
        Food newFood = null;

        foreach (FoodType.Type foodType in System.Enum.GetValues(typeof(FoodType.Type))) {

            foreach (RestaurantInfo.Types restType in System.Enum.GetValues(typeof(RestaurantInfo.Types))) {
                // all of this fancy stuff to make sure we don't double store drink sprites as there currently
                // aren't restaurant specific drinks
                if ((restType != RestaurantInfo.Types.NoType && this.configData.HasRestaurantType(foodType, restType)) ||
                    (restType == RestaurantInfo.Types.NoType && foodType == FoodType.Type.drink)) {

                    JsonFoodContainer jsonFoodData = this.configData.GetJsonFood(foodType, restType);
                    JsonToFood[] jsonList = jsonFoodData.List;
                    string foodSpritePath = jsonFoodData.SpriteLocation;

                    foreach (JsonToFood jsonFood in jsonList) {
                        newFood = new Food(jsonFood.name, foodType,
                                        Resources.Load<Sprite>(foodSpritePath + jsonFood.unPreppedSpriteName),
                                        Resources.Load<Sprite>(foodSpritePath + jsonFood.preppedSpriteName),
                                        Resources.Load<Sprite>(foodSpritePath + jsonFood.burntSpriteName));

                        this.foodDictionary.Add(new FoodKey(restType, jsonFood.name), newFood);
                    }
                }
            }
        }
    }

    private void BuildNewMenu(int daysPassed) {
        this.menu = new Menu();
        int numToppingsToRemove = 0;

        // more shenanigens due to lack of restaurant specific drinks
        FoodKey tmpKey = new FoodKey(this.currentRestaurantType, "");
        FoodKey drinkKey = new FoodKey(RestaurantInfo.Types.NoType, ""); 

        foreach (string item in this.restaurantMenu) {
            tmpKey.foodName = item;
            drinkKey.foodName = item;
            // check to see if it exists in dictionary
            Food foodItem = foodDictionary.ContainsKey(tmpKey) ? foodDictionary[tmpKey] : null;
            if(foodItem == null && foodDictionary.ContainsKey(drinkKey)) {
                foodItem = foodDictionary[drinkKey];
            }
            if (foodItem != null) {
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

        if (daysPassed >= SIX_TOPPINGS_THRESH && menu.GetToppingsLength() > 6) {
            numToppingsToRemove = menu.GetToppingsLength() - 6;
        } else if (daysPassed >= FIVE_TOPPINGS_THRESH && menu.GetToppingsLength() > 5) {
            numToppingsToRemove = menu.GetToppingsLength() - 5;
        } else if (menu.GetToppingsLength() > 4) {
            numToppingsToRemove = menu.GetToppingsLength() - 4;
        }

        while (numToppingsToRemove > 0) {
            menu.RemoveToppingAtIndex(Random.Range(0, menu.GetToppingsLength()));
            numToppingsToRemove--;
        }

        while (menu.GetDrinksLength() > MAX_DRINKS_LENGTH) {
            menu.RemoveDrinkAtIndex(Random.Range(0, menu.GetDrinksLength()));
        }
    }

    private void BuildMealDrawer(JsonToRestaurant restaurantData) {

        switch (this.currentRestaurantType) {
            case RestaurantInfo.Types.Burger:
                this.mealDrawer = new BurgerDrawer(this.restaurantSpritePath, restaurantData.RestaurantFoodDisplaySprites.Top,
                                                    restaurantData.RestaurantFoodDisplaySprites.Bottom, restaurantData.FoodDisplaySprites);
                break;
            case RestaurantInfo.Types.Fries:
                Debug.Log("Fries drawer not available atm");
                this.mealDrawer = new FriesDrawer();
                break;
            default:
                throw new System.Exception("Can't create meal drawer of invalid type");
        }

        // add drinks as they re-use sprites from the menu
        foreach (KeyValuePair<FoodKey, Food> entry in this.foodDictionary) {
            // do something with entry.Value or entry.Key
            if (entry.Value.GetFoodType() == FoodType.Type.drink) {
                this.mealDrawer.ManuallyAddSprite(entry.Key.foodName, entry.Value.GetPreppedSprite());
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

            this.mealDrawerCreated = false;
            this.BuildMealDrawer(restaurantData);
            this.mealDrawerCreated = true;
        }
        this.BuildNewMenu(daysPassed);
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
