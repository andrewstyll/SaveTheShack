﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour {

    private enum RestaurantStates {
        Loading,
        Open,
        Closed
    };

    // Restaurant management helpers
    private GameManager gameManager;

    private RestaurantBuilder restaurantBuilder;
    private RestaurantInfo.Types currentType = RestaurantInfo.Types.NoType;

    // Current state
    private RestaurantStates state;

    // check Loaded status
    private bool statusUILoaded = false;
    private bool customerUILoaded = false;
    private bool foodUILoaded = false;

    // Modal stuff
    private GameObject modal = null;
    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private GameObject backgroundCanvas;

    // Events
    public delegate void RestaurantManagerNotification();
    public static event RestaurantManagerNotification LoadFoodUI;
    public static event RestaurantManagerNotification StartGame;

    public delegate void ModalNotification(ModalUI.ModalState state, string displayString);
    public static event ModalNotification ModalEvent;

    private void Awake() {
        // grab instances required to manage restaurant
        this.restaurantBuilder = RestaurantBuilder.GetInstance();

        // event subscriptions
        StatusBarUI.Loaded += StatusUILoaded;
        CustomerAreaUI.Loaded += CustomerUILoaded;
        FoodStationUI.Loaded += FoodUILoaded;
        ModalUI.CountDownComplete += SetupStartGame;
        StatusBarUI.EndOfDay += EndDayEvent;

        // spawn modal to block screen from being touched with loading graphic
        DisplayModal(ModalUI.ModalState.Loading, "");
        this.state = RestaurantStates.Loading;
    }

    // Start is called before the first frame update
    private void Start() {

        this.gameManager = GameManager.GetInstance();
        if (this.gameManager == null) {
            Debug.Log("Will have to wait for gameManager O_o");
        }

        if (this.restaurantBuilder.SetupComplete() && 
            this.gameManager.GetCurrentRestaurantType() != RestaurantInfo.Types.NoType) {
            this.CreateRestaurant();
        } else {
            StartCoroutine("WaitForSetupComplete");
        }
    }

    private void Update() {
        if (this.state == RestaurantStates.Loading && UIIsLoaded()) {
            this.state = RestaurantStates.Open;
            DisplayModal(ModalUI.ModalState.CountDown, "");
        }
    }

    private void OnDestroy() {
        StatusBarUI.Loaded -= StatusUILoaded;
        CustomerAreaUI.Loaded -= CustomerUILoaded;
        FoodStationUI.Loaded -= FoodUILoaded;
        ModalUI.CountDownComplete -= SetupStartGame;
        StatusBarUI.EndOfDay -= EndDayEvent;
    }

    private void CreateRestaurant() {
        this.currentType = this.gameManager.GetCurrentRestaurantType();
        this.restaurantBuilder.BuildRestaurant(this.currentType, this.gameManager.GetDaysPassed());
        LoadFoodUI();
    }

    private void DisplayModal(ModalUI.ModalState state, string displayString) {
        if(modal == null) {
            modal = Instantiate(this.modalPrefab, this.backgroundCanvas.transform, false);
            modal.transform.SetAsLastSibling();
        }
        modal.SetActive(true);
        ModalEvent(state, displayString);
    }

    private void HideModal() {
        modal.SetActive(false);
    }

    // UI Loading code;
    private void StatusUILoaded() {
        this.statusUILoaded = true;
    }

    private void CustomerUILoaded() {
        this.customerUILoaded = true;
    }

    private void FoodUILoaded() {
        this.foodUILoaded = true;
    }

    private bool UIIsLoaded() {
        return (this.statusUILoaded && this.customerUILoaded && this.foodUILoaded);
    }

    /**** Events ****/
    private void SetupStartGame(ModalUI.ModalState state) {
        // destroy modal
        HideModal();
        StartGame();
    }

    private void EndDayEvent(int score) {
        this.state = RestaurantStates.Closed;
        DisplayModal(ModalUI.ModalState.EndDay, score.ToString());
    }

    /**** Coroutines ****/
    IEnumerator WaitForSetupComplete() {
        while(!this.restaurantBuilder.SetupComplete() ||
            this.gameManager.GetCurrentRestaurantType() == RestaurantInfo.Types.NoType) {
            yield return null;
        }
        this.CreateRestaurant();
    }
}
