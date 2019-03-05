using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour {

    private int id;
    private float patience = 15.0f;

    private OrderBuilder orderBuilder;
    private Order myOrder;

    private Button customerButton;

    private RestaurantBuilder restaurantBuilder;
    private MealDrawer mealDrawer;
    [SerializeField] private GameObject drawnFood;
    [SerializeField] private GameObject drawnDrink;

    public delegate bool CustomerEvent(Order order);
    public static event CustomerEvent ServeMe;

    public delegate void CustomerHandlerEvent(int id);
    public static event CustomerHandlerEvent DestroyMe;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        orderBuilder = OrderBuilder.GetInstance();
        this.customerButton = this.GetComponent<Button>();
        this.customerButton.onClick.AddListener(ServeCustomer);
    }

    private void Start() {
        this.mealDrawer = this.restaurantBuilder.GetMealDrawer();
        this.myOrder = orderBuilder.BuildOrder();
        this.DisplayOrder();
    }

    // Update is called once per frame
    private void Update() {
        if(patience > 0) {
            patience -= Time.deltaTime;
        } else {
            DestroyMe(this.id);
        }
    }

    private void DisplayOrder() {
        mealDrawer.GetBaseDrawing(drawnFood);

        List<string> myFood = this.myOrder.GetFood();
        foreach(string food in myFood) {
            mealDrawer.AppendFood(drawnFood, food);
        }
        mealDrawer.FinishDrawing(drawnFood);

        drawnDrink.GetComponent<Image>().sprite = mealDrawer.ManuallyGetSprite(this.myOrder.GetDrink());
    }

    /**** EVENTS ****/
    private void ServeCustomer() {
        if(ServeMe(this.myOrder)) {
            Debug.Log("Good Order");
            DestroyMe(this.id);
        } else {
            Debug.Log("Bad Order");
        }
    }

    public void SetId(int id) {
        this.id = id;
    }

}
