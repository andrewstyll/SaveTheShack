using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDisplayUI : MonoBehaviour {

    RestaurantBuilder restaurantBuilder = null;
    RestaurantInfo.Types restaurantType = RestaurantInfo.Types.NoType;

    MealDrawer mealDrawer = null;

    [SerializeField] private GameObject parentDisplay;
    [SerializeField] private GameObject modelObjPrefab;

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
        this.mealDrawer.StartDrawing(parentDisplay, modelObjPrefab);
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
    public void AddFood(string foodName) {
        this.mealDrawer.AppendFood(parentDisplay, modelObjPrefab, foodName);
    }

    public void FinishDrawing() {
        this.mealDrawer.FinishDrawing(parentDisplay, modelObjPrefab);
    }

    public void ClearDrawing() {
        for(int i = 1; i < this.parentDisplay.transform.childCount; i++) {
            Transform child = this.parentDisplay.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    }
}
