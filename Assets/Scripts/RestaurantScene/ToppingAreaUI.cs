using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingAreaUI : MonoBehaviour {

    private const int MAX_TOPPINGS = 6;

    private MenuBuilder menuBuilder;

    private List<Food> toppings;
    [SerializeField] private GameObject toppingPrefab;

    private void Awake() {
        this.menuBuilder = MenuBuilder.GetInstance();
        RestaurantManager.MenuCreated += SpawnToppingsEvent;
    }

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    private void Update() {
        
    }

    private void SpawnToppingsEvent() {
        this.toppings = this.menuBuilder.GetMenu().GetToppings();
        for (int i = 0; i < MAX_TOPPINGS; i++) {
            ToppingUI UIScript = Instantiate(this.toppingPrefab, gameObject.transform, false).GetComponent<ToppingUI>();
            if (i < this.toppings.Count) {
                UIScript.SetTopping(this.toppings[i]);
            }
        }
    }
}
