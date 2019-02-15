using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerDrawer : MealDrawer {

    private Sprite topBun;
    private Sprite bottomBun;

    public BurgerDrawer(Sprite top, Sprite bottom) {
        this.topBun = top;
        this.bottomBun = bottom;
    }

    public override void AppendSprite(Sprite sprite) {
        throw new System.NotImplementedException();
    }

    public override void BuildFromList(List<Sprite> list) {
        throw new System.NotImplementedException();
    }
}