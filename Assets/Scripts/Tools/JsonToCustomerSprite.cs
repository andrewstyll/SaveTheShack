using System;

[System.Serializable]
public struct JsonToCustomerSpriteContainer {
    public JsonToCustomerSprite[] Sprites;
    public string SpriteLocation;
}

[System.Serializable]
public struct JsonToCustomerSprite {
    public string SpriteName;
}