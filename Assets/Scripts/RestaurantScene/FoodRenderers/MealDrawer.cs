using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MealDrawer {
    public abstract void GetBaseDrawing(GameObject parentObject);

    public abstract void StartDrawing(GameObject parentObject);

    public abstract void FinishDrawing(GameObject parentObject);

    public abstract void AppendFood(GameObject parentObject, string foodName);

    public abstract void ManuallyAddSprite(string foodName, Sprite sprite);

    public abstract Sprite ManuallyGetSprite(string foodName);
}
