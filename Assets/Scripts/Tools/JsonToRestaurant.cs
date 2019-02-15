using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JsonServingSpritePath {
    public string Bottom;
    public string Top;
}

[System.Serializable]
public class JsonToRestaurant {
    public string[] Menu;
    public JsonServingSpritePath ServingSprites;
}
