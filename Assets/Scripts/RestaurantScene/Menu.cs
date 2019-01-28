using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu {

    // use dictionary to map objects or something??? 
    // menu is enum, map food to properties?? should not hold actual objects
    private FoodStub main;
    private List<FoodStub> toppings;
    private List<FoodStub> drinks;

    public FoodStub GetMain() {
        return main;
    }

    public List<FoodStub> GetToppings() {
        return toppings;
    }

    public List<FoodStub> GetDrinks() {
        return drinks;
    }
}
