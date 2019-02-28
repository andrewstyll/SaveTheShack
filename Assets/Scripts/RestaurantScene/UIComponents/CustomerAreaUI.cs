using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerAreaUI : MonoBehaviour {

    // should be some max number of customers at once
    private int MAX_CUSTOMERS = 5;

    private int successfulServes;
    private float customerWindow;
    private float customerWindowMin;

    private RestaurantBuilder restaurantBuilder;
    private GameObject[] customerList;
    [SerializeField] private GameObject customerPrefab;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        RestaurantManager.MenuCreated += StartCustomerSpawn;

        this.successfulServes = 0;

        customerList = new GameObject[MAX_CUSTOMERS];
        for (int i = 0; i < MAX_CUSTOMERS; i++) {
            customerList[i] = Instantiate(this.customerPrefab, gameObject.transform, false);
        }
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        SpawnCustomer();
    }

    private void SpawnCustomer() {

    }

    private void StartCustomerSpawn() {

    }
}
