using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu {
    private Food main;
    private List<Food> toppings;
    private List<Food> drinks;

    public Menu() {
        this.toppings = new List<Food>();
        this.drinks = new List<Food>();
    }

    public Food GetMain() {
        return this.main;
    }

    public void SetMain(Food main) {
        this.main = main;
    }

    public List<Food> GetToppings() {
        return this.toppings;
    }

    public int GetToppingsLength() {
        return this.toppings.Count;
    }

    public void AddTopping(Food topping) {
        this.toppings.Add(topping);
    }

    public void RemoveToppingAtIndex(int i) {
        if (i < 0 || i > this.toppings.Count-1) {
            throw new System.Exception("Accessing out of bounds index in Menu.toppings");
        }
        this.toppings.RemoveAt(i);
    }

    public List<Food> GetDrinks() {
        return this.drinks;
    }

    public int GetDrinksLength() {
        return this.drinks.Count;
    }

    public void AddDrink(Food drink) {
        this.drinks.Add(drink);
    }

    public void RemoveDrinkAtIndex(int i) {
        if (i < 0 || i > this.drinks.Count - 1) {
            throw new System.Exception("Accessing out of bounds index in Menu.drinks");
        }
        this.drinks.RemoveAt(i);
    }
}
