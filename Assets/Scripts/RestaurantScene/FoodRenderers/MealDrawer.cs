using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class MealDrawer : MonoBehaviour {

    protected Dictionary<string, Sprite> displaySprites;

    private const float ALPHA_HIDDEN = 0.0f;
    private const float ALPHA_FULL = 1.0f;
    Color alphaControl = Color.white;

    public void InitDrawer(Dictionary<string, Sprite> displaySprites) {
        this.displaySprites = displaySprites;
    }

    public abstract void StartDrawing(GameObject parentObject, GameObject baseObject);

    public abstract void FinishDrawing(GameObject parentObject, GameObject baseObject);

    public abstract void AppendFood(GameObject parentObject, GameObject baseObject, string foodName);

    public void AddDrink(GameObject drinkDisplay, string drinkName) {
        alphaControl.a = ALPHA_FULL;
        drinkDisplay.GetComponent<Image>().color = alphaControl;
        drinkDisplay.GetComponent<Image>().sprite = this.displaySprites[drinkName];
    }

    public void HideDrink(GameObject drinkDisplay) {
        alphaControl.a = ALPHA_HIDDEN;
        drinkDisplay.GetComponent<Image>().color = alphaControl;
        drinkDisplay.GetComponent<Image>().sprite = null;
    }

    public void ManuallyAddSprite(string foodName, Sprite sprite) {
        displaySprites.Add(foodName, sprite);
    }

    public Sprite ManuallyGetSprite(string foodName) {
        return displaySprites[foodName];
    }
}
