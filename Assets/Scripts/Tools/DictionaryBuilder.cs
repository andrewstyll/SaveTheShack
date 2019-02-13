using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public sealed class DictionaryBuilder {
    private static readonly DictionaryBuilder instance = new DictionaryBuilder();
    private Dictionary<string, Food> dictionary;

    private string mainsInfoFilePath = "Assets/Config/Mains.json";

    private DictionaryBuilder() {
        Debug.Log("Running");
        dictionary = new Dictionary<string, Food>();
        AddToDictionary(mainsInfoFilePath);
    }

    public static DictionaryBuilder GetInstance() {
        return instance;
    }

    private void AddToDictionary(string jsonString) {
        if(System.IO.File.Exists(jsonString)) {
            Debug.Log("Parse Me: " + jsonString);
        } else {
            Debug.Log("File Not Found: " + jsonString);
        }
    }
}