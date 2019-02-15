using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour {

    private RestaurantBuilder restaurantBuilder;
    private RestaurantInfo.Types currentType = RestaurantInfo.Types.Burger;

    public delegate void FoodAreaEvent();
    public static event FoodAreaEvent MenuCreated;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
    }

    // Start is called before the first frame update
    private void Start() {
        if (this.restaurantBuilder.SetupComplete()) {
            this.CreateRestaurant(currentType);
        } else {
            StartCoroutine("WaitForMenuComplete");
        }
    }

    // Update is called once per frame
    private void Update() { }

    private void CreateRestaurant(RestaurantInfo.Types restaurantType) {
        this.restaurantBuilder.BuildRestaurant(restaurantType);
        MenuCreated();
    }

    IEnumerator WaitForMenuComplete() {
        while(!this.restaurantBuilder.SetupComplete()) {
            yield return null;
        }
        this.CreateRestaurant(currentType);
    }
}
