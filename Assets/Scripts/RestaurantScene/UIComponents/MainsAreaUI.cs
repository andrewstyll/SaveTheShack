﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainsAreaUI : MonoBehaviour {

    private const int MAX_MAINS = 3;

    private RestaurantBuilder restaurantBuilder;
    private Food main;
    [SerializeField] private GameObject mainPrefab;

    // Events
    public delegate void MainsAreaUINotification();
    public static event MainsAreaUINotification Loaded;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        RestaurantManager.LoadFoodUI += SpawnMainsEvent;
    }

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    void Update() { }

    private void OnDestroy() {
        RestaurantManager.LoadFoodUI -= SpawnMainsEvent;
    }

    /**** Events ****/
    private void SpawnMainsEvent() {
        this.main = this.restaurantBuilder.GetMenu().GetMain();
        for (int i = 0; i < MAX_MAINS; i++) {
            MainUI UIScript = Instantiate(this.mainPrefab, gameObject.transform, false).GetComponent<MainUI>();
            UIScript.SetMain(this.main);
        }
        Loaded();
    }
}
