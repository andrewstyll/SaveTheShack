using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinksAreaUI : MonoBehaviour {

    private const int MAX_DRINKS = 2;

    private MenuBuilder menuBuilder;
    private List<Food> drinks;
    [SerializeField] private GameObject drinkPrefab;

    // Start is called before the first frame update
    private void Start() {
        this.menuBuilder = MenuBuilder.GetInstance();

        this.drinks = this.menuBuilder.GetMenu().GetDrinks();
        for (int i = 0; i < MAX_DRINKS; i++) {
            DrinkUI UIScript = Instantiate(this.drinkPrefab, gameObject.transform, false).GetComponent<DrinkUI>();
            if (i < this.drinks.Count) {
                //UIScript.SetTopping(this.toppings[i]);
            }
        }
    }

    // Update is called once per frame
    private void Update() { }
}
