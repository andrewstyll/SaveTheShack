using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/* Describes the interactions/events between a user and the title screen. Fills out layout
 * upon startup and user interaction.
 */
public class TitleScene : MonoBehaviour {

    private GameManager gameManager;

    // button strings
    private const string LOAD_GAME = "Load Game";
    private const string NEW_GAME = "New Game";

    private const string REST_ONE_STRING = "Burger Shack";
    private const string REST_TWO_STRING = "Fries Shack";

    // starting restaurant types for selection
    private const RestaurantInfo.Types REST_ONE_TYPE = RestaurantInfo.Types.Burger;
    private const RestaurantInfo.Types REST_TWO_TYPE = RestaurantInfo.Types.Fries;

    // load old game or create new game
    private bool isNewGame = true;

    private RestaurantInfo.Types selectedType;

    // menu selection buttons
    private GameObject currentlySelected;
    private Button buttonOne;
    private Button buttonTwo;
    [SerializeField] private GameObject buttonOneObject;
    [SerializeField] private GameObject buttonTwoObject;

    // start game button
    private Button startGameButton;
    [SerializeField] private GameObject startGameButtonObj;
    [SerializeField] private GameObject startGameText;

    // back button
    private Button buttonBack;
    [SerializeField] private GameObject buttonBackObject;

    [SerializeField] private EventSystem eventSystem;

    // notify that a button has been pressed
    public delegate void TitleScreenButtonEvent(RestaurantInfo.Types selectedType, bool isNewGame);
    public static TitleScreenButtonEvent NewGame;

    private void Awake() {
        this.gameManager = GameManager.GetInstance();

        buttonOne = buttonOneObject.GetComponent<Button>();
        buttonTwo = buttonTwoObject.GetComponent<Button>();
        buttonBack = buttonBackObject.GetComponent<Button>();
        startGameButton = startGameButtonObj.GetComponent<Button>();
    }

    private void Start() {
        if (this.gameManager != null) {
            InitButtons();
        } else {
            StartCoroutine("WaitForGameManager");
        }
    }

    // Update is called once per frame
    void Update() {
        // update the currently selected button if a new button has been selected
        if(this.eventSystem.currentSelectedGameObject != this.currentlySelected) {
            this.eventSystem.SetSelectedGameObject(this.currentlySelected);
        }
    }

    // will load the correct buttons wether or not a save file exists
    private void InitButtons() {
        if(this.gameManager.SaveFileExists()) {
            InitNewLoadGameButtons();
        } else {
            InitRestaurantSelectButtons();
        }
    }

    // deactivate older buttons and activate Restauraunt type select buttons. Event listeners need to be added
    // and removed to prevent multiple events from firing
    private void InitRestaurantSelectButtons() {
        buttonOne.onClick.RemoveListener(LoadGameSelect);
        buttonTwo.onClick.RemoveListener(NewGameSelect);

        buttonOne.onClick.AddListener(RestaurantOneSelect);
        buttonTwo.onClick.AddListener(RestaurantTwoSelect);

        buttonTwoObject.SetActive(false); // fries isn't ready, hide this for release

        startGameButton.onClick.AddListener(StartGameSelect);
        startGameText.SetActive(true);

        // display the back button if there is a save/load menu to return to
        if (this.gameManager.SaveFileExists()) {
            buttonBack.onClick.AddListener(BackSelect);
            buttonBackObject.SetActive(true);
        } else {
            buttonBack.onClick.RemoveListener(BackSelect);
            buttonBackObject.SetActive(false);
        }

        buttonOneObject.GetComponentInChildren<Text>().text = REST_ONE_STRING;
        buttonTwoObject.GetComponentInChildren<Text>().text = REST_TWO_STRING;

        // auto select the restaurant one type and button
        RestaurantOneSelect();
    }

    // deactivate potential older buttons and display the new/load save game menu
    private void InitNewLoadGameButtons() {
        buttonOne.onClick.RemoveListener(RestaurantOneSelect);
        buttonTwo.onClick.RemoveListener(RestaurantTwoSelect);
        startGameButton.onClick.RemoveListener(StartGameSelect);
        startGameText.SetActive(false);

        buttonTwoObject.SetActive(true);

        // need to hide the back button as you can't go "back" from this menu
        buttonBack.onClick.RemoveListener(BackSelect);
        buttonBackObject.SetActive(false);

        buttonOne.onClick.AddListener(LoadGameSelect);
        buttonTwo.onClick.AddListener(NewGameSelect);

        buttonOneObject.GetComponentInChildren<Text>().text = LOAD_GAME;
        buttonTwoObject.GetComponentInChildren<Text>().text = NEW_GAME;

        // select load game by default
        LoadGameSelect();
    }

    /**** Events ****/
    // load game button shas been selected
    private void LoadGameSelect() {
        isNewGame = false;
        this.currentlySelected = buttonOneObject;

        // if we can load a game, activate the start game big button
        startGameButton.onClick.AddListener(StartGameSelect);
        startGameText.SetActive(true);
    }

    // on new game select click, bring up the restaurant select buttons
    private void NewGameSelect() {
        isNewGame = true;
        this.currentlySelected = buttonTwoObject;
        InitRestaurantSelectButtons();
    }

    // select the first displayed restaurant
    private void RestaurantOneSelect() {
        this.selectedType = REST_ONE_TYPE;
        this.currentlySelected = buttonOneObject;
    }

    // select the second displayed restaurant
    private void RestaurantTwoSelect() {
        this.selectedType = REST_TWO_TYPE;
        this.currentlySelected = buttonTwoObject;
    }

    // go back the the new/load game menu
    private void BackSelect() {
        InitNewLoadGameButtons();
    }

    // start the game
    private void StartGameSelect() {
        // we don't want to "select" this button in the event manager, but still want to run the event
        NewGame(selectedType, isNewGame);
    }

    /**** Coroutine ****/
    // if the game manager hasn't started, we need to wait for it to be ready
    IEnumerator WaitForGameManager() {
        while (this.gameManager == null) {
            this.gameManager = GameManager.GetInstance();
            yield return null;
        }
        InitButtons();
    }
}