using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using System.Data;
using System.IO;

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

    private void dictionarySize() { Debug.Log("Dictionary Count: " + dictionary.Count); }

    private void AddToDictionary(string jsonString) {

        string json = @"{
          'Table1': [
            {
              'id': 0,
              'item': 'item 0'
            },
            {
              'id': 1,
              'item': 'item 1'
            }
          ]
        }";

        DataSet data = JsonConvert.DeserializeObject<DataSet>(json);

        if (File.Exists(jsonString)) {
            string jsonFile = File.ReadAllText(jsonString);
           
            DataSet dataSet = JsonConvert.DeserializeObject<DataSet>(jsonFile);
            Debug.Log("Here");
            DataTable list = dataSet.Tables["List"];

            foreach(DataRow obj in list.Rows) {
                Debug.Log(obj["name"]);
            }

        } else {
            Debug.Log("Config File Not Found: " + jsonString);
        }
    }
}