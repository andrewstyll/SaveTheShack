using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour {

    private MenuBuilder menuBuilder;
    private RestaurantInfo.Types currentType = RestaurantInfo.Types.Burger;

    public delegate void FoodAreaEvent();
    public static event FoodAreaEvent MenuCreated;

    // access singleton in awake?
    private void Awake() {
        this.menuBuilder = MenuBuilder.GetInstance();

    }

    // Start is called before the first frame update
    private void Start() {
        if (this.menuBuilder.MenuSetupComplete()) {
            this.CreateMenu(currentType);
        } else {
            StartCoroutine("WaitForMenuComplete");
        }
    }

    // Update is called once per frame
    private void Update() { }

    private void CreateMenu(RestaurantInfo.Types restaurantType) {
        this.menuBuilder.BuildMenu(currentType);
        MenuCreated();
    }

    IEnumerator WaitForMenuComplete() {
        while(!this.menuBuilder.MenuSetupComplete()) {
            yield return null;
        }
        this.CreateMenu(currentType);
    }
}
