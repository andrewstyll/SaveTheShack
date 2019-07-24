using System;

[System.Serializable]
public struct JsonToRestSpriteContainer {
    public JsonToRestSprite[] Sprites;
    public string SpriteLocation;
}

[System.Serializable]
public struct JsonToRestSprite {
    public string SpriteName;
}
