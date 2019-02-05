using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainsAreaUI : MonoBehaviour {

    [SerializeField] private GameObject mainPrefab;
    private MenuBuilder menuBuilder = MenuBuilder.GetInstance();
    Menu currentMenu;

    void Awake() {

    }

    // Start is called before the first frame update
    void Start() {
        menuBuilder.BuildMenu(RestaurantInfo.Types.Taco, 4, 4);
        currentMenu = menuBuilder.GetMenu();

        Debug.Log(currentMenu.GetMain().GetName());
        foreach(Food topping in currentMenu.GetToppings()) {
            Debug.Log(topping.GetName());
        }
        foreach (Food drink in currentMenu.GetDrinks()) {
            Debug.Log(drink.GetName());
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
