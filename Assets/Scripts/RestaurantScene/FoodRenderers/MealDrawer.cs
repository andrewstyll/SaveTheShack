using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MealDrawer {

    protected Dictionary<string, Sprite> displaySprites;

    public abstract void StartDrawing(GameObject parentObject);

    public abstract void FinishDrawing(GameObject parentObject);

    public abstract void AppendFood(GameObject parentObject, string foodName);

    public void ManuallyAddSprite(string foodName, Sprite sprite) {
        displaySprites.Add(foodName, sprite);
    }

    public Sprite ManuallyGetSprite(string foodName) {
        return displaySprites[foodName];
    }
}
