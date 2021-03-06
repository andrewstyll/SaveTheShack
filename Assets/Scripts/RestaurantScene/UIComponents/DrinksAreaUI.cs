﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinksAreaUI : MonoBehaviour {

    private const int MAX_DRINKS = 2;

    private RestaurantBuilder restaurantBuilder;
    private List<Food> drinks;
    [SerializeField] private GameObject drinkPrefab;

    // Events
    public delegate void DrinksAreaUINotification();
    public static event DrinksAreaUINotification Loaded;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        RestaurantManager.LoadFoodUI += SpawnDrinksEvent;
    }

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    private void OnDestroy() {
        RestaurantManager.LoadFoodUI -= SpawnDrinksEvent;
    }

    /**** Events ****/
    private void SpawnDrinksEvent() {
        this.drinks = this.restaurantBuilder.GetMenu().GetDrinks();
        for (int i = 0; i < MAX_DRINKS; i++) {
            DrinkUI UIScript = Instantiate(this.drinkPrefab, gameObject.transform, false).GetComponent<DrinkUI>();
            if (i < this.drinks.Count) {
                UIScript.SetDrink(this.drinks[i]);
            }
        }
        Loaded();
    }
}
