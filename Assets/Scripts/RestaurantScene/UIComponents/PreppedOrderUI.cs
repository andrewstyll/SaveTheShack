using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreppedOrderUI : MonoBehaviour {

    private List<Food> currentPreppedFood = new List<Food>();
    private Food drink = null;

    private RestaurantBuilder restaurantBuilder;
    private MealDrawer mealDrawer;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
    }

    // Start is called before the first frame update
    private void Start() {
        MainUI.FoodSelected += AddFoodToOrderEvent;
        ToppingUI.FoodSelected += AddFoodToOrderEvent;
        DrinkUI.FoodSelected += AddFoodToOrderEvent;
        TrashUI.TrashClicked += ClearOrderEvent;
        if(this.restaurantBuilder.MealDrawerCreated()) {
            this.mealDrawer = this.restaurantBuilder.GetMealDrawer();
        } else {
            Debug.Log("Should have been able to grab Meal Drawer here, starting wait coroutine");
            StartCoroutine("WaitForMealDrawerCreated");
        }
    }

    // Update is called once per frame
    void Update() { }

    private void DisplayDrink(Food food) { }

    private void DisplayFood(Food food) { 
        // render returned gameobject from the burgerDrawer
    }

    private void AddFood(Food food) {
        // add on to topmost food entry
        //this.DisplayFood(food);
        this.currentPreppedFood.Add(food);
    }

    private void PrintCurrentOrder() {
        Debug.Log("Drink is: " + (this.drink != null ? this.drink.GetName() : "no drink set"));
        Debug.Log("Food Order is currently: " + (this.currentPreppedFood.Count == 0 ? "no food set" : ""));
        foreach(Food food in this.currentPreppedFood) {
            Debug.Log(food.GetName());
        }
    }

    /**** Events ****/
    private void AddFoodToOrderEvent(Food food) {
        if(food.GetFoodType() == FoodType.Type.drink && drink == null) {
            this.drink = food;
        } else {
            // food adding is more complex
            this.AddFood(food);
        }
        PrintCurrentOrder();
    }

    public void ClearOrderEvent() {
        Debug.Log("Throwing out everything");
        this.currentPreppedFood.Clear();
        this.drink = null;
        PrintCurrentOrder();
    }

    /**** Coroutines ****/
    IEnumerator WaitForMealDrawerCreated() {
        // the meal drawer should 100% be finished before people can even click an order
        // this is just hear in case to avoid grabbing a null reference
        while (!this.restaurantBuilder.MealDrawerCreated()) {
            yield return null;
        }
        this.mealDrawer = this.restaurantBuilder.GetMealDrawer();
    }
}
