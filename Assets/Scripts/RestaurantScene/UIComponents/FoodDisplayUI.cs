using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDisplayUI : MonoBehaviour {

    RestaurantBuilder restaurantBuilder = null;
    RestaurantInfo.Types restaurantType = RestaurantInfo.Types.NoType;

    MealDrawer mealDrawer = null;

    private bool setupComplete = false;
    [SerializeField] private GameObject parentFoodDisplay;
    [SerializeField] private GameObject modelFoodObj;
    [SerializeField] private GameObject drinkDisplay;

    private void Awake() {
    }

    // Start is called before the first frame update
    void Start() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        if (this.restaurantBuilder.MealDrawerSetupComplete()) {
            SetMealDrawer();
        } else {
            StartCoroutine("WaitForSetupComplete");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SetMealDrawer() {
        this.restaurantType = this.restaurantBuilder.GetCurrentRestaurantType();
        switch (this.restaurantType) {
            case RestaurantInfo.Types.Burger:
                GameObject tmp = new GameObject();
                tmp.AddComponent<BurgerDrawer>();
                this.mealDrawer = tmp.GetComponent<BurgerDrawer>();
                this.mealDrawer.InitDrawer(this.restaurantBuilder.GetMealDrawerData());
                break;
        }
        this.mealDrawer.StartDrawing(parentFoodDisplay, modelFoodObj);
        this.mealDrawer.HideDrink(drinkDisplay);
        this.setupComplete = true;
    }

    /**** Coroutines ****/
    IEnumerator WaitForSetupComplete() {
        Debug.Log("Waiting for meal Drawer Setup");
        while (!this.restaurantBuilder.MealDrawerSetupComplete()) {
            yield return null;
        }
        SetMealDrawer();
    }

    /**** PUBLIC API ****/
    public void AddDrink(string drinkName) {
        this.mealDrawer.AddDrink(drinkDisplay, drinkName);
    }

    public void AddFood(string foodName) {
        this.mealDrawer.AppendFood(parentFoodDisplay, modelFoodObj, foodName);
    }

    public void FinishDrawing() {
        this.mealDrawer.FinishDrawing(parentFoodDisplay, modelFoodObj);
    }

    public void ClearDrawing() {
        for(int i = 2; i < this.parentFoodDisplay.transform.childCount; i++) {
            Transform child = this.parentFoodDisplay.transform.GetChild(i);
            Destroy(child.gameObject);
        }
        this.mealDrawer.HideDrink(drinkDisplay);
    }

    public bool SetUpComplete() {
        return this.setupComplete;
    }
}
