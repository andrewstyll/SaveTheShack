using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food {

    private readonly string name;
    private readonly FoodType.Type type;

    private readonly Sprite unPreppedSprite;
    private readonly Sprite preppedSprite;
    private readonly Sprite burntSprite;

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
