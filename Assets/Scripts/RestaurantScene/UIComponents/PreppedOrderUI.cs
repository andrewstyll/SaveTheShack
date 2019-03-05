using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreppedOrderUI : MonoBehaviour {

    private List<string> preppedFood = new List<string>();
    private string preppedDrink = null;

    private const float ALPHA_HIDDEN = 0.0f;
    private const float ALPHA_FULL = 1.0f;
    Color alphaControl = Color.white;

    private RestaurantBuilder restaurantBuilder;
    private MealDrawer mealDrawer;
    [SerializeField] private GameObject drawnFood;
    [SerializeField] private GameObject drawnDrink;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        MainUI.FoodSelected += AddFoodToOrderEvent;
        ToppingUI.FoodSelected += AddFoodToOrderEvent;
        DrinkUI.FoodSelected += AddFoodToOrderEvent;
        TrashUI.TrashClicked += ClearOrderEvent;
        CustomerUI.ServeMe += CheckMatchingOrder;

        alphaControl.a = ALPHA_HIDDEN;
        drawnDrink.GetComponent<Image>().color = alphaControl;
    }

    // Start is called before the first frame update
    private void Start() {
        if(this.restaurantBuilder.MealDrawerCreated()) {
            this.mealDrawer = this.restaurantBuilder.GetMealDrawer();
            InitializeFoodDrawing();
        } else {
            Debug.Log("Should have been able to grab Meal Drawer here, starting wait coroutine");
            StartCoroutine("WaitForMealDrawerCreated");
        }
    }

    // Update is called once per frame
    void Update() { }


    private void InitializeFoodDrawing() {
        mealDrawer.GetBaseDrawing(drawnFood);
        //drawnFood = Instantiate(, gameObject.transform, false);
    }

    private void AddFood(Food food) {
        // add on to topmost food entry
        this.preppedFood.Add(food.GetName());
        mealDrawer.AppendFood(drawnFood, food.GetName());
    }

    /**** Events ****/
    private void AddFoodToOrderEvent(Food food) {
        if (food.GetFoodType() == FoodType.Type.drink && preppedDrink == null) {
            this.preppedDrink = food.GetName();

            alphaControl.a = ALPHA_FULL;
            drawnDrink.GetComponent<Image>().color = alphaControl;
            drawnDrink.GetComponent<Image>().sprite = mealDrawer.ManuallyGetSprite(this.preppedDrink);
        } else {
            // food adding is more complex
            this.AddFood(food);
        }
    }

    public void ClearOrderEvent() {
        Debug.Log("Throwing out everything");
        this.preppedFood.Clear();
        this.preppedDrink = null;
    }

    public bool CheckMatchingOrder(Order order) {
        Order currentOrder = new Order(preppedFood, preppedDrink);
        if(currentOrder == order) {
            ClearOrderEvent();
            return true;
        }
        return false;
    }


    /**** Coroutines ****/
    IEnumerator WaitForMealDrawerCreated() {
        // the meal drawer should 100% be finished before people can even click an order
        // this is just hear in case to avoid grabbing a null reference
        while (!this.restaurantBuilder.MealDrawerCreated()) {
            yield return null;
        }
        this.mealDrawer = this.restaurantBuilder.GetMealDrawer();
        InitializeFoodDrawing();
    }
}
