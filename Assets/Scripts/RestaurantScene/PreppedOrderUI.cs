using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreppedOrderUI : MonoBehaviour {

    private List<Food> currentPreppedFood = new List<Food>();
    private Food drink = null;

    private void Awake() { }

    // Start is called before the first frame update
    private void Start() {
        MainUI.FoodSelected += AddFoodToOrderEvent;
        ToppingUI.FoodSelected += AddFoodToOrderEvent;
        TrashUI.TrashClicked += ClearOrderEvent;
    }

    // Update is called once per frame
    void Update() { }

    private void DisplayDrink(Food food) { }

    private void DisplayFood(Food food) { }

    private void AddFood(Food food) {
        // add on to topmost food entry
        this.DisplayFood(food);
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
    public void AddFoodToOrderEvent(Food food) {
        Debug.Log("Adding " + food.GetName() + " to order");
        if(food.GetFoodType() == FoodType.Type.drink && drink == null) {
            DisplayDrink(food);
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
}
