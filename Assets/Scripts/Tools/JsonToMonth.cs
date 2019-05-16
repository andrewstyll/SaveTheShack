using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct JsonMonthContainer {
    public JsonToMonth[] Months;
    public string PrefabLocation;
}

[System.Serializable]
public struct JsonToMonth {
    public string Name;
}