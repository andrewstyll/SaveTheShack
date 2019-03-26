using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurgerDrawer : MealDrawer {

    private const float SPACING_CONST = 10.0f;

    private string TopBun = "TopBun";
    private string BottomBun = "BottomBun";
 
    public BurgerDrawer(string spritePath, string top, string bottom, JsonSpritesObject[] displayFood) {
        displaySprites = new Dictionary<string, Sprite>();
        displaySprites.Add(this.TopBun, Resources.Load<Sprite>(spritePath + top));
        displaySprites.Add(this.BottomBun, Resources.Load<Sprite>(spritePath + bottom));

        foreach (JsonSpritesObject displayObj in displayFood) {
            displaySprites.Add(displayObj.Name, Resources.Load<Sprite>(spritePath + displayObj.SpriteName));
        }
    }

    public override void StartDrawing(GameObject parentObject) {
        AppendFood(parentObject, BottomBun);
    }

    public override void FinishDrawing(GameObject parentObject) {
        AppendFood(parentObject, TopBun);
    }

    public override void AppendFood(GameObject parentObject, string foodName) {
        Sprite nextFood = displaySprites[foodName];
        GameObject childObject = new GameObject(foodName);
        childObject.AddComponent<Image>();
        childObject.GetComponent<Image>().sprite = nextFood;
        childObject.transform.SetParent(parentObject.transform);
        childObject.transform.SetAsLastSibling();
        childObject.transform.localPosition = new Vector3(0, childObject.transform.GetSiblingIndex() * SPACING_CONST);
    }
}