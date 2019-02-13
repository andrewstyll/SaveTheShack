﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JsonFoodContainer {
    public JsonToFood[] List;
}

[System.Serializable]
public class JsonToFood {
    public string name;
    public string unPreppedSpritePath;
    public string preppedSpritePath;
    public string burntSpritePath;
}