using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food {

    private string name;
    private FoodType.Type type;
    private Sprite sprite;

    public Food(string name, FoodType.Type type, Sprite sprite) {
        this.name = name;
        this.type = type;
        this.sprite = sprite;
    }

    public string GetName() {
        return name;
    }

    public FoodType.Type GetFoodType() {
        return type;
    }

    public Sprite GetSprite() {
        return sprite;
    }
}
