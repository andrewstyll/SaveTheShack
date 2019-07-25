using System;

[System.Serializable]
public struct JsonToCustomerSpriteContainer {
    public JsonToRestSprite[] Sprites;
    public string SpriteLocation;
}

[System.Serializable]
public struct JsonToCustomerSprite {
    public string SpriteName;
}