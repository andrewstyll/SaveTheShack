using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurgerDrawer : MealDrawer {

    private Sprite topBun;
    private Sprite bottomBun;
    private Dictionary<string, Sprite> displaySprites;

    public BurgerDrawer(string spritePath, string top, string bottom, JsonFoodDisplaySprites[] displayFood) {
        this.topBun = Resources.Load<Sprite>(spritePath + top);
        this.bottomBun = Resources.Load<Sprite>(spritePath + bottom);

        displaySprites = new Dictionary<string, Sprite>();
        foreach(JsonFoodDisplaySprites displayObj in displayFood) {
            displaySprites.Add(displayObj.Name, Resources.Load<Sprite>(spritePath + displayObj.SpriteName));
        }
    }

    public override void GetBaseDrawing(GameObject parentObject) {

        parentObject.AddComponent<VerticalLayoutGroup>();
        VerticalLayoutGroup layoutGroup = parentObject.GetComponent<VerticalLayoutGroup>();
        layoutGroup.childControlHeight = true;
        layoutGroup.childControlWidth = true;
        layoutGroup.childForceExpandWidth = false;
        layoutGroup.childForceExpandHeight = false;
        layoutGroup.childAlignment = TextAnchor.LowerCenter;
        layoutGroup.spacing = -107;
               

        GameObject childObject = new GameObject("bottomBun");
        childObject.AddComponent<Image>();
        childObject.GetComponent<Image>().sprite = bottomBun;

        childObject.transform.parent = parentObject.transform;

        //childObject.transform.localPosition = objPosition;

        childObject.transform.SetAsLastSibling();
    }

    public override void AppendFood(GameObject parentObject, string foodName) {
        Sprite nextFood = displaySprites[foodName];
        GameObject childObject = new GameObject(foodName);
        childObject.AddComponent<Image>();
        childObject.GetComponent<Image>().sprite = nextFood;
        childObject.transform.parent = parentObject.transform;
        childObject.transform.SetAsLastSibling();
    }

    public override void FinishDrawing(GameObject parentObject) {
        Sprite nextFood = topBun;
        GameObject childObject = new GameObject();
        childObject.AddComponent<Image>();
        childObject.GetComponent<Image>().sprite = nextFood;
        childObject.transform.parent = parentObject.transform;
        childObject.transform.SetAsLastSibling();
    }

    public override void ManuallyAddSprite(string foodName, Sprite sprite) {
        displaySprites.Add(foodName, sprite);
    }

    public override Sprite ManuallyGetSprite(string foodName) {
        return displaySprites[foodName];
    }
}