using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MealDrawer : MonoBehaviour {

    protected Dictionary<string, Sprite> displaySprites;

    public void InitDrawer(Dictionary<string, Sprite> displaySprites) {
        this.displaySprites = displaySprites;
    }

    public abstract void StartDrawing(GameObject parentObject, GameObject baseObject);

    public abstract void FinishDrawing(GameObject parentObject, GameObject baseObject);

    public abstract void AppendFood(GameObject parentObject, GameObject baseObject, string foodName);

    public void ManuallyAddSprite(string foodName, Sprite sprite) {
        displaySprites.Add(foodName, sprite);
    }

    public Sprite ManuallyGetSprite(string foodName) {
        return displaySprites[foodName];
    }
}
