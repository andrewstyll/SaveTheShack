using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour {

    // Properties
    private const float MAX_PATIENCE = 15.0f;
    private int id;
    private float patience;
    private Order myOrder;

    // Builders
    private RestaurantBuilder restaurantBuilder;
    private OrderBuilder orderBuilder;

    // Event Triggers
    private Button customerButton;

    // Effects
    private const float BEGIN_WARNING = MAX_PATIENCE / 2;
    private const float FINAL_WARNING = BEGIN_WARNING / 4;
    private const float FADE_IN = 0.35f;
    private const float FADE_OUT = 0.15f;
    private const float ALPHA_HIDDEN = 0.0f;
    private const float ALPHA_FULL = 1.0f;
    Color alphaControl = Color.white;

    private Image orderDisplay;
    private Image orderWarning;
    [SerializeField] private GameObject orderDisplayObject;
    [SerializeField] private GameObject orderWarningObject;

    private MealDrawer mealDrawer;
    [SerializeField] private GameObject foodDisplay;
    [SerializeField] private GameObject drinkDisplay;

    // Events
    public delegate bool CustomerEvent(Order order);
    public static event CustomerEvent ServeMe;

    public delegate void CustomerHandlerEvent(int id);
    public static event CustomerHandlerEvent DestroyMe;

    // created this in the event that we want to use time remaining to calcuate score,
    // right now only the status UI is subscribed to this
    public delegate void OrderSuccessEvent();
    public static event OrderSuccessEvent SuccessfulOrder;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        orderBuilder = OrderBuilder.GetInstance();

        this.orderDisplay = orderDisplayObject.GetComponent<Image>();
        this.orderWarning = orderWarningObject.GetComponent<Image>();
        this.alphaControl.a = ALPHA_HIDDEN;
        this.orderWarning.color = alphaControl;

        this.customerButton = this.GetComponent<Button>();
        this.customerButton.onClick.AddListener(ServeCustomer);

        this.patience = MAX_PATIENCE;
    }

    private void Start() {
        this.mealDrawer = this.restaurantBuilder.GetMealDrawer();
        this.myOrder = orderBuilder.BuildOrder();
        this.DisplayOrder();
    }

    // Update is called once per frame
    private void Update() {
        if(this.patience > 0) {
            this.patience -= Time.deltaTime;
            UpdateOrderDisplay();
        } else {
            StopCoroutine(FlickerWarning());
            DestroyMe(this.id);
        }
    }

    private void UpdateOrderDisplay() {
        if(this.patience <= FINAL_WARNING) {
            StartCoroutine(FlickerWarning());
        } else if(this.patience <= BEGIN_WARNING) {
            this.alphaControl.a = ((float)(this.patience - FINAL_WARNING) / (float)(BEGIN_WARNING - FINAL_WARNING));
            this.orderDisplay.color = this.alphaControl;

            this.alphaControl.a = 1.0f - this.alphaControl.a;
            this.orderWarning.color = this.alphaControl;
        }
    }

    private void DisplayOrder() {
        mealDrawer.StartDrawing(foodDisplay);

        List<string> myFood = this.myOrder.GetFood();
        foreach(string food in myFood) {
            mealDrawer.AppendFood(foodDisplay, food);
        }
        mealDrawer.FinishDrawing(foodDisplay);
        if(this.myOrder.GetDrink() != null) {
            drinkDisplay.GetComponent<Image>().sprite = mealDrawer.ManuallyGetSprite(this.myOrder.GetDrink());
        } else {
            alphaControl.a = ALPHA_HIDDEN;
            drinkDisplay.GetComponent<Image>().color = alphaControl;
        }
    }

    /**** EVENTS ****/
    private void ServeCustomer() {
        if(ServeMe(this.myOrder)) {
            SuccessfulOrder();
            DestroyMe(this.id);
        }
    }

    /**** COROUTINES ****/
    IEnumerator FlickerWarning() {
        float flickerTime = FADE_IN + FADE_OUT;
        while (this.patience <= FINAL_WARNING && this.patience >= 0) {
            while (flickerTime >= FADE_OUT) {
                flickerTime -= Time.deltaTime;
                this.alphaControl.a = Mathf.Min(((FADE_IN + FADE_OUT) - flickerTime) / FADE_OUT, 1.0f);
                this.orderWarning.color = this.alphaControl;

                this.alphaControl.a = 1.0f - this.alphaControl.a;
                this.orderDisplay.color = this.alphaControl;


                yield return null;
            }
            while (flickerTime >= 0) {
                flickerTime -= Time.deltaTime;
                this.alphaControl.a = Mathf.Max(flickerTime / FADE_IN, 0.0f);
                this.orderWarning.color = this.alphaControl;

                this.alphaControl.a = 1.0f - this.alphaControl.a;
                this.orderDisplay.color = this.alphaControl;

                yield return null;
            }
            flickerTime = FADE_IN + FADE_OUT;
            yield return null;
        }
    }

    /**** PUBLIC API ****/
    public void SetId(int id) {
        this.id = id;
    }
}
