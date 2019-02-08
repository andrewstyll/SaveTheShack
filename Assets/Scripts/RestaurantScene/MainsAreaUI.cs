using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainsAreaUI : MonoBehaviour {

    private MenuBuilder menuBuilder;
    [SerializeField] private GameObject mainPrefab;

    Menu currentMenu;

    void Awake() {
        menuBuilder = MenuBuilder.GetInstance();
    }

    // Start is called before the first frame update
    void Start() {
       
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
