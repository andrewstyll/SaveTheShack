using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerUI : MonoBehaviour {

    private Order myOrder;

    private Button customerButton;

    private void Awake() {
        this.customerButton = this.GetComponent<Button>();
        this.customerButton.onClick.AddListener(ServeCustomer);
    }

    // Update is called once per frame
    private void Update() {
        // update patience as time passes
        // if patience hits 0, remove customer
    }

    /**** EVENTS ****/
    private void ServeCustomer() {
        // needs to grab list of food on serving tray
        // needs to compare the lists
        // if true, notify custmer manager to remove customer
        List<Food> preppedOrder;

    }

    private void DisplayOrder() {

    }

    public void SetOrder(Order order) {
        this.myOrder = order;
    }
}
