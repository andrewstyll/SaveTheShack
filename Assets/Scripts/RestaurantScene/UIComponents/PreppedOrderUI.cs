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

    private FoodDisplayUI foodDisplayScript;
    [SerializeField] private GameObject foodDisplayUI;

    // Events
    public delegate void PreppedOrderAreaUINotification();
    public static event PreppedOrderAreaUINotification Loaded;

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
        this.foodDisplayScript = foodDisplayUI.GetComponent<FoodDisplayUI>();
        if(this.restaurantBuilder.MealDrawerSetupComplete()) {
            InitPreppedOrderArea();
        } else {
            StartCoroutine("WaitForMealDrawerCreated");
        }
    }

    // Update is called once per frame
    void Update() {

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

    private void OnDestroy() {
        MainUI.FoodSelected -= AddFoodToOrderEvent;
        ToppingUI.FoodSelected -= AddFoodToOrderEvent;
        DrinkUI.FoodSelected -= AddFoodToOrderEvent;
        TrashUI.TrashClicked -= SignalClearOrderEvent;
        CustomerUI.ServeMe -= CheckMatchingOrder;
    }

    // will soon be not needed
    private void InitPreppedOrderArea() {
        // food is done on own for now. Will update later to do drinks on own too
        InitializeThemeSprites();
        Loaded();
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

    private bool HasDrink() {
        return this.preppedDrink != null;
    }

    private bool CanTop() {
        return (!this.isTopped && this.preppedFood.Count > 0);
    }

    private void ClearFoodDrawing() { 
        this.foodDisplayScript.ClearDrawing();
    }

    public void ClearOrder() {
        this.preppedFood.Clear();
        this.preppedDrink = null;
        this.isTopped = false;

        ClearFoodDrawing();
    }

    /**** Events ****/
    private bool AddFoodToOrderEvent(Food food) {
        if (food.GetFoodType() == FoodType.Type.drink) {
            if(!this.HasDrink()) {

                this.preppedDrink = food.GetName();
                this.foodDisplayScript.AddDrink(food.GetName());
                return true;
            }
        } else if(!this.isTopped) {
            // food adding is more complex
            this.preppedFood.Add(food.GetName());
            this.foodDisplayScript.AddFood(food.GetName());
            return true;
        }
        return false;
    }

    private void FinishFoodOrder() {
        if (this.CanTop()) {
            this.isTopped = true;
            this.foodDisplayScript.FinishDrawing();
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
        while (!this.restaurantBuilder.MealDrawerSetupComplete()) {
            yield return null;
        }
        InitPreppedOrderArea();
    }
}
