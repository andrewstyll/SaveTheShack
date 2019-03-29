using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingAreaUI : MonoBehaviour {

    private const int MAX_TOPPINGS = 6;

    private RestaurantBuilder restaurantBuilder;

    private List<Food> toppings;
    [SerializeField] private GameObject toppingPrefab;

    // Events
    public delegate void ToppingsAreaUINotification();
    public static event ToppingsAreaUINotification Loaded;

    private void Awake() {
        this.restaurantBuilder = RestaurantBuilder.GetInstance();
        RestaurantManager.LoadFoodUI += SpawnToppingsEvent;
    }

    private void OnDestroy() {
        RestaurantManager.LoadFoodUI -= SpawnToppingsEvent;
    }

    private void SpawnToppingsEvent() {
        this.toppings = this.restaurantBuilder.GetMenu().GetToppings();
        for (int i = 0; i < MAX_TOPPINGS; i++) {
            ToppingUI UIScript = Instantiate(this.toppingPrefab, gameObject.transform, false).GetComponent<ToppingUI>();
            if (i < this.toppings.Count) {
                UIScript.SetTopping(this.toppings[i]);
            }
        }
        Loaded();
    }
}
