using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BurgerDrawer : MealDrawer {

    private const float SPACING_CONST = 10.0f;

    private string TopBun = "Top";
    private string BottomBun = "Bottom";

    Color alphaControl = Color.white;

    public override void StartDrawing(GameObject parentObject, GameObject baseObject) {
        AppendFood(parentObject, baseObject, BottomBun);
    }

    public override void FinishDrawing(GameObject parentObject, GameObject baseObject) {
        AppendFood(parentObject, baseObject, TopBun);
    }

    public override void AppendFood(GameObject parentObject, GameObject baseObject, string foodName) {

        Sprite nextFood = displaySprites[foodName];
        GameObject childObject = (GameObject)Instantiate(baseObject, parentObject.transform);
        childObject.GetComponent<Image>().sprite = nextFood;
        childObject.GetComponent<Image>().color = alphaControl;
        childObject.transform.SetParent(parentObject.transform);
        childObject.transform.SetAsLastSibling();
        childObject.transform.localPosition = new Vector3(0, childObject.transform.GetSiblingIndex() * SPACING_CONST);
    }
}