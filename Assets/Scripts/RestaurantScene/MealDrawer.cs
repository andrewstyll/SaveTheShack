using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MealDrawer {
    public abstract void AppendSprite(Sprite sprite);

    public abstract void BuildFromList(List<Sprite> list);
}
