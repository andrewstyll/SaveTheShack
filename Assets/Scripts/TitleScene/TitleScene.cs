using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScene : MonoBehaviour {

    // select starting restaurant type
    private const string SUFFIX = " Shack";
    private const string REST_ONE_STRING = "Burger";
    private const string REST_TWO_STRING = "Fries";
    private const RestaurantInfo.Types REST_ONE_TYPE = RestaurantInfo.Types.Burger;
    private const RestaurantInfo.Types REST_TWO_TYPE = RestaurantInfo.Types.Fries;

    private RestaurantInfo.Types selectedType;

    private GameObject currentlySelected;
    private Button buttonOne;
    private Button buttonTwo;
    [SerializeField] private GameObject buttonOneObject;
    [SerializeField] private GameObject buttonTwoObject;

    // start game
    private Button startGameButton;
    [SerializeField] private GameObject startGameButtonObj;

    [SerializeField] private EventSystem eventSystem;

    public delegate void TitleScreenButtonEvent(RestaurantInfo.Types selectedType, bool isNewGame);
    public static TitleScreenButtonEvent NewGame;

    private void Awake() {
        buttonOne = buttonOneObject.GetComponent<Button>();
        buttonTwo = buttonTwoObject.GetComponent<Button>();
        startGameButton = startGameButtonObj.GetComponent<Button>();

        buttonOne.onClick.AddListener(RestaurantOneSelect);
        buttonTwo.onClick.AddListener(RestaurantTwoSelect);
        startGameButton.onClick.AddListener(StartGameSelect);

        buttonOneObject.GetComponentInChildren<Text>().text = REST_ONE_STRING;
        buttonTwoObject.GetComponentInChildren<Text>().text = REST_TWO_STRING;

        RestaurantOneSelect();
    }

    private void Start() {

    }

    // Update is called once per frame
    void Update() {
        if(this.eventSystem.currentSelectedGameObject != this.currentlySelected) {
            this.eventSystem.SetSelectedGameObject(this.currentlySelected);
        }
    }

    /**** Events ****/
    private void RestaurantOneSelect() {
        this.selectedType = REST_ONE_TYPE;
        this.currentlySelected = buttonOneObject;
    }

    private void RestaurantTwoSelect() {
        this.selectedType = REST_TWO_TYPE;
        this.currentlySelected = buttonTwoObject;
    }

    private void StartGameSelect() {
        // we don't want to select this button, but still want to run the event
        NewGame(selectedType, false);
    }
}