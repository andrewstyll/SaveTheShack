using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food {

    private string name;
    private FoodType.Type type;

    private Sprite unPreppedSprite;
    private Sprite preppedSprite;
    private Sprite burntSprite;

    // constructor for toppings
    public Food(string name, FoodType.Type type, Sprite preppedSprite) {
        this.name = name;
        this.type = type;
        this.preppedSprite = preppedSprite;
        this.unPreppedSprite = null;
        this.burntSprite = null;
    }

    // constructor for drinks
    public Food(string name, FoodType.Type type, Sprite unPreppedSprite,
                Sprite preppedSprite) {
        this.name = name;
        this.type = type;
        this.unPreppedSprite = unPreppedSprite;
        this.preppedSprite = preppedSprite;
        this.burntSprite = null;
    }

    // constructor for mains
    public Food(string name, FoodType.Type type, Sprite unPreppedSprite, 
                Sprite preppedSprite, Sprite burntSprite) {
        this.name = name;
        this.type = type;
        this.unPreppedSprite = unPreppedSprite;
        this.preppedSprite = preppedSprite;
        this.burntSprite = burntSprite;
    }

    public string GetName() {
        return name;
    }

    public FoodType.Type GetFoodType() {
        return type;
    }

    public Sprite GetUnPreppedSprite() {
        return this.unPreppedSprite;
    }

    public Sprite GetBurntSprite() {
        return this.burntSprite;
    }

    public Sprite GetPreppedSprite() {
        return this.preppedSprite;
    }
}
