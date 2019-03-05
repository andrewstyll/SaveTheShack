using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JsonServingSpritePath {
    public string Bottom;
    public string Top;
}

[System.Serializable]
public struct JsonSpritesObject {
    public string Name;
    public string SpriteName;
}

[System.Serializable]
public class JsonToRestaurant {
    public string[] Menu;
    public JsonServingSpritePath RestaurantFoodDisplaySprites;
    public JsonSpritesObject[] FoodDisplaySprites;
    public JsonSpritesObject[] RestaurantThemeSprites;
    public string SpriteLocation;
}
