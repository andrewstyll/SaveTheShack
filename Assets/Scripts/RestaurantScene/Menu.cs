using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MenuItem<T> {
    public T name;
    public Sprite sprite;
}

public struct Menu {
    public List<MenuItem<Food.Drinks>> drinks;
    public List<MenuItem<Food.Toppings>> toppings;
    public MenuItem<Food.Mains> main;
}

public class MenuBuilder : ScriptableObject {

    Menu menu;

    public List<MenuItem> fillMenu() {
        menu = new List<MenuItem>();

        return menu;
    }
}
