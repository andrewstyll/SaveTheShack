﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurgerDrawer : MealDrawer {

    private string TopBun = "TopBun";
    private string BottomBun = "BottomBun";
    private Dictionary<string, Sprite> displaySprites;

    public BurgerDrawer(string spritePath, string top, string bottom, JsonSpritesObject[] displayFood) {
        displaySprites = new Dictionary<string, Sprite>();
        displaySprites.Add(this.TopBun, Resources.Load<Sprite>(spritePath + top));
        displaySprites.Add(this.BottomBun, Resources.Load<Sprite>(spritePath + bottom));

        foreach (JsonSpritesObject displayObj in displayFood) {
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
        layoutGroup.childAlignment = TextAnchor.MiddleCenter;
        layoutGroup.spacing = -107;

        StartDrawing(parentObject);
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