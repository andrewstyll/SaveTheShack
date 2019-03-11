using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour {

    // Restaurant management helpers
    private GameManager gameManager;

    private RestaurantBuilder restaurantBuilder;
    private RestaurantInfo.Types currentType = RestaurantInfo.Types.NoType;

    // check Loaded status
    private bool restaurantOpen = false;
    private bool statusUILoaded = false;
    private bool customerUILoaded = false;
    private bool foodUILoaded = false;

    // Modal stuff
    private GameObject modal;
    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private GameObject backgroundCanvas;

    // Events
    public delegate void RestaurantManagerNotification();
    public static event RestaurantManagerNotification LoadUI;
    public static event RestaurantManagerNotification StartGame;

    public delegate void ModalNotification();
    public static event ModalNotification ModalMessage;

    private void Awake() {
        // grab instances required to manage restaurant
        this.gameManager = GameManager.GetInstance();
        if(this.gameManager == null) {
            throw new System.Exception("RestaurantManager failed to grab GameManager");
        }
        this.restaurantBuilder = RestaurantBuilder.GetInstance();

        // event subscriptions
        StatusBarUI.Loaded += StatusUILoaded;
        CustomerAreaUI.Loaded += CustomerUILoaded;
        FoodStationUI.Loaded += FoodUILoaded;

        // spawn modal to block screen from being touched with loading graphic
        DisplayModal();
    }

    // Start is called before the first frame update
    private void Start() {
        this.currentType = this.gameManager.GetCurrentRestaurantType();

        if (this.restaurantBuilder.SetupComplete() &&
            this.currentType != RestaurantInfo.Types.NoType) {
            this.CreateRestaurant(currentType);
        } else {
            StartCoroutine("WaitForSetupComplete");
        }
    }

    private void Update() {
        if(!this.restaurantOpen && UIIsLoaded()) {
            this.restaurantOpen = true;
            Debug.Log("Loaded UI");
            //ModalMessage();
        }
    }

    private void CreateRestaurant(RestaurantInfo.Types restaurantType) {
        this.restaurantBuilder.BuildRestaurant(restaurantType);
        LoadUI();
    }

    private void DisplayModal() {
        modal = Instantiate(this.modalPrefab, this.backgroundCanvas.transform, false);
        modal.transform.SetAsLastSibling();
        //ModalMessage();
    }

    private void HideModal() {
        Destroy(modal);
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
    private void SetupStartGame() {
        // destroy modal
        HideModal();
        StartGame();
    }

    /**** Coroutines ****/
    IEnumerator WaitForSetupComplete() {
        while(!this.restaurantBuilder.SetupComplete() || 
                this.gameManager.GetCurrentRestaurantType() == RestaurantInfo.Types.NoType) {
            yield return null;
        }
        this.currentType = this.gameManager.GetCurrentRestaurantType();
        this.CreateRestaurant(currentType);
    }
}
