using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JsonFoodContainer {
    public JsonToFood[] List;
}

[System.Serializable]
public class JsonToFood {
    public string name;
    public string unPreppedSpriteName;
    public string preppedSpriteName;
    public string burntSpriteName;
}