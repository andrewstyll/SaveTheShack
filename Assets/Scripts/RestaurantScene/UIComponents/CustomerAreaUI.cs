﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CustomerAreaUI : MonoBehaviour {

    // should be some max number of customers at once
    private int MAX_CUSTOMERS = 5;

    // Customer scheduling
    private bool restaurantOpen = false;

    private float timeToNextCustomer;
    private float customerWindowSize = 2.0f;
    private float customerWindowMin = 5.0f;

    private GameObject[] customerList;
    private List<int> freeSpawnSlots;

    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject dummyPrefab;

    // Events
    public delegate void CustomerAreaUINotification();
    public static event CustomerAreaUINotification Loaded;

    private void Awake() {
        // maybe attach this to a different event like start game?
        RestaurantManager.StartGame += StartCustomerSpawn;
        CustomerUI.DestroyMe += DestroyCustomer;
        StatusBarUI.EndOfDay += StopCustomerSpawn;

        freeSpawnSlots = new List<int>();
        customerList = new GameObject[MAX_CUSTOMERS];

        for (int i = 0; i < MAX_CUSTOMERS; i++) {
            customerList[i] = Instantiate(this.dummyPrefab, gameObject.transform, false);
            customerList[i].transform.SetSiblingIndex(i);
            freeSpawnSlots.Add(i);
        }
    }

    private void Start() {
        Loaded();
    }

    // Update is called once per frame
    void Update() {
        if(this.restaurantOpen) {
            //Debug.Log("Spawning Customer");
            SpawnCustomer();
        }
    }

    private void OnDestroy() {
        RestaurantManager.StartGame -= StartCustomerSpawn;
        CustomerUI.DestroyMe -= DestroyCustomer;
        StatusBarUI.EndOfDay -= StopCustomerSpawn;
    }

    private void SpawnCustomer() {
        if (this.timeToNextCustomer <= 0.0f && this.freeSpawnSlots.Count > 0) {
            // only perform this action if there is a spot available or maybe perform it and auto add a customer
            // when a spot becomes available and the time is up
            int spawnSpot = this.freeSpawnSlots[Random.Range(0, this.freeSpawnSlots.Count - 1)];
            this.freeSpawnSlots.Remove(spawnSpot);

            Destroy(customerList[spawnSpot]);

            GameObject newCustomer = Instantiate(this.customerPrefab, gameObject.transform, false);
            newCustomer.transform.SetSiblingIndex(spawnSpot);
            newCustomer.GetComponent<CustomerUI>().SetId(spawnSpot);

            customerList[spawnSpot] = newCustomer;

            if (this.freeSpawnSlots.Count > 0) {
                this.timeToNextCustomer = Random.Range(customerWindowMin, customerWindowMin + customerWindowSize);
            }
        } else if (this.timeToNextCustomer > 0.0f) {
            this.timeToNextCustomer -= Time.deltaTime;
        }
    }

    private void StartCustomerSpawn() {
        this.restaurantOpen = true;
        this.timeToNextCustomer = Random.Range(customerWindowMin, customerWindowMin + customerWindowSize);
    }

    /**** Events ****/
    private void DestroyCustomer(int id) {
        Destroy(customerList[id]);
        customerList[id] = Instantiate(this.dummyPrefab, gameObject.transform, false);
        customerList[id].transform.SetSiblingIndex(id);
        this.freeSpawnSlots.Add(id);
    }

    private void StopCustomerSpawn(int score) {
        this.restaurantOpen = false;
    }
}
