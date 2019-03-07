using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreppedOrderUI : MonoBehaviour {

    private enum TopMeButtonState {
        Full,
        Hidden
    };

    // builders and data
    private RestaurantBuilder restaurantBuilder;
    private MealDrawer mealDrawer;
    private List<string> preppedFood = new List<string>();
    private string preppedDrink = null;

    // top me button effects
    private const string TOP_ME_SPRITE = "TopMe";
    private bool isTopped;
    private TopMeButtonState topMeState;
    private Button topMeButton;
    private Image topMeImage;
    [SerializeField] private GameObject topMeButtonObj;

    // visual effects
    private bool signalClearOrder;
    private const float ALPHA_HIDDEN = 0.0f;
    private const float ALPHA_FULL = 1.0f;
    Color alphaControl = Color.white;
    [SerializeField] private GameObject drawnFood;
    [SerializeField] private GameObject drawnDrink;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        MainUI.FoodSelected += AddFoodToOrderEvent;
        ToppingUI.FoodSelected += AddFoodToOrderEvent;
        DrinkUI.FoodSelected += AddFoodToOrderEvent;
        TrashUI.TrashClicked += SignalClearOrderEvent;
        CustomerUI.ServeMe += CheckMatchingOrder;

        this.topMeButton = this.topMeButtonObj.GetComponent<Button>();
        this.topMeImage = this.topMeButtonObj.GetComponent<Image>();
        this.topMeButton.onClick.AddListener(FinishFoodOrder);

        this.signalClearOrder = false;
    }

    // Start is called before the first frame update
    private void Start() {
        if(this.restaurantBuilder.MealDrawerCreated()) {
            this.mealDrawer = this.restaurantBuilder.GetMealDrawer();
            InitializeFoodDrawing();
            InitializeThemeSprites();
        } else {
            Debug.Log("Should have been able to grab Meal Drawer here, starting wait coroutine");
            StartCoroutine("WaitForMealDrawerCreated");
        }
    }

    // Update is called once per frame
    void Update() {

        if(EmptyFoodDrawing()) {
            this.mealDrawer.StartDrawing(this.drawnFood);
        }

        if (this.topMeState == TopMeButtonState.Hidden && CanTop()) {
            ShowTopMe();
        } else if(this.topMeState == TopMeButtonState.Full && !CanTop()) {
            HideTopMe();
        }

        if(signalClearOrder) {
            ClearOrder();
            signalClearOrder = false;
        }
    }

  
    private void InitializeThemeSprites() {
        this.topMeImage.sprite = this.restaurantBuilder.GetThemedSprite(TOP_ME_SPRITE);
        HideTopMe();
    }

    private void ShowTopMe() {
        this.alphaControl.a = ALPHA_FULL;
        this.topMeImage.color = alphaControl;
        this.topMeState = TopMeButtonState.Full;
    }

    private void HideTopMe() {
        this.alphaControl.a = ALPHA_HIDDEN;
        this.topMeImage.color = alphaControl;
        this.topMeState = TopMeButtonState.Hidden;
    }

    private void InitializeFoodDrawing() {
        alphaControl.a = ALPHA_HIDDEN;
        drawnDrink.GetComponent<Image>().color = alphaControl;

        mealDrawer.StartDrawing(drawnFood);
    }

    private bool HasDrink() {
        return this.preppedDrink != null;
    }

    private bool CanTop() {
        return (!this.isTopped && this.preppedFood.Count > 0);
    }

    private bool EmptyFoodDrawing() {
        return this.drawnFood.transform.childCount == 0;
    }

    private void ClearFoodDrawing() { 
        foreach(Transform child in this.drawnFood.transform) {
            Destroy(child.gameObject);
        }
    }

    private void AddFood(Food food) {
        // add on to topmost food entry
        this.preppedFood.Add(food.GetName());
        mealDrawer.AppendFood(drawnFood, food.GetName());
    }

    public void ClearOrder() {
        this.preppedFood.Clear();
        this.preppedDrink = null;
        this.isTopped = false;

        ClearFoodDrawing();

        alphaControl.a = ALPHA_HIDDEN;
        drawnDrink.GetComponent<Image>().color = alphaControl;
        drawnDrink.GetComponent<Image>().sprite = null;
    }

    /**** Events ****/
    private bool AddFoodToOrderEvent(Food food) {
        if (food.GetFoodType() == FoodType.Type.drink) {
            if(!this.HasDrink()) {

                this.preppedDrink = food.GetName();

                alphaControl.a = ALPHA_FULL;
                drawnDrink.GetComponent<Image>().color = alphaControl;
                drawnDrink.GetComponent<Image>().sprite = mealDrawer.ManuallyGetSprite(this.preppedDrink);

                return true;
            }
        } else if(!this.isTopped) {
            // food adding is more complex
            this.AddFood(food);
            return true;
        }
        return false;
    }

    private void FinishFoodOrder() {
        if (this.CanTop()) {
            this.isTopped = true;
            mealDrawer.FinishDrawing(drawnFood);
        }
    }

    private void SignalClearOrderEvent() {
        this.signalClearOrder = true;
    }

    private bool CheckMatchingOrder(Order order) {
        Order currentOrder = new Order(preppedFood, preppedDrink);
        if(this.isTopped && currentOrder == order) {
            this.SignalClearOrderEvent();
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
