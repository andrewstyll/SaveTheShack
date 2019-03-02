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

    public delegate bool CustomerEvent(Order order);
    public static event CustomerEvent ServeMe;

    public delegate void CustomerHandlerEvent(int id);
    public static event CustomerHandlerEvent DestroyMe;

    private void Awake() {
        orderBuilder = OrderBuilder.GetInstance();
        this.customerButton = this.GetComponent<Button>();
        this.customerButton.onClick.AddListener(ServeCustomer);
    }

    private void Start() {
        this.myOrder = orderBuilder.BuildOrder();
        this.myOrder.PrintOrder();
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
