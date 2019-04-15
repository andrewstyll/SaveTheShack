using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriesDrawer : MealDrawer {

    private const float SPACING_CONST = 10.0f;

    private string Fork = "Top";
    private string FriesTray = "Bottom";

    Color alphaControl = Color.white;

    public override void StartDrawing(GameObject parentObject, GameObject baseObject) {
        AppendFood(parentObject, baseObject, FriesTray);
    }

    public override void FinishDrawing(GameObject parentObject, GameObject baseObject) {
        AppendFood(parentObject, baseObject, Fork);
    }

    public override void AppendFood(GameObject parentObject, GameObject baseObject, string foodName) {

        Sprite nextFood = displaySprites[foodName];
        GameObject childObject = (GameObject)Instantiate(baseObject, parentObject.transform);
        childObject.GetComponent<Image>().sprite = nextFood;
        childObject.GetComponent<Image>().color = alphaControl;
        childObject.transform.SetParent(parentObject.transform);
        childObject.transform.SetAsLastSibling();
        childObject.transform.localPosition = new Vector3(0, (childObject.transform.GetSiblingIndex()-1) * SPACING_CONST);
    }
}