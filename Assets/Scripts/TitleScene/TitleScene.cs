using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScene : MonoBehaviour {

    private const string SUFFIX = " Shack";
    private const string REST_ONE_STRING = "Burger";
    private const string REST_TWO_STRING = "Taco";
    private const RestaurantInfo.Types REST_ONE_TYPE = RestaurantInfo.Types.Burger;
    private const RestaurantInfo.Types REST_TWO_TYPE = RestaurantInfo.Types.Taco;

    private RestaurantInfo.Types selectedType;

    private Button buttonOne;
    private Button buttonTwo;
    [SerializeField] private GameObject buttonOneObject;
    [SerializeField] private GameObject buttonTwoObject;

    private void Awake() {
        buttonOne = buttonOneObject.GetComponent<Button>();
        buttonTwo = buttonTwoObject.GetComponent<Button>();

        buttonOne.onClick.AddListener(RestaurantOneSelect);
        buttonTwo.onClick.AddListener(RestaurantTwoSelect);

        buttonOneObject.GetComponentInChildren<Text>().text = REST_ONE_STRING;
        buttonTwoObject.GetComponentInChildren<Text>().text = REST_TWO_STRING;
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void RestaurantOneSelect() {
        this.selectedType = REST_ONE_TYPE;
    }

    private void RestaurantTwoSelect() {
        this.selectedType = REST_TWO_TYPE;
    }
}
