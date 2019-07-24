using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    private GameObject currentlySelected;
    private Button buttonOne;
    private Button buttonTwo;
    [SerializeField] private GameObject buttonOneObject;
    [SerializeField] private GameObject buttonTwoObject;

    // start game buttone
    private Button startGameButton;
    [SerializeField] private GameObject startGameButtonObj;
    [SerializeField] private GameObject startGameText;

    // back button
    private Button buttonBack;
    [SerializeField] private GameObject buttonBackObject;

    [SerializeField] private EventSystem eventSystem;

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
        if(this.eventSystem.currentSelectedGameObject != this.currentlySelected) {
            this.eventSystem.SetSelectedGameObject(this.currentlySelected);
        }
    }

    private void InitButtons() {
        if(this.gameManager.SaveFileExists()) {
            InitNewLoadGameButtons();
        } else {
            InitRestaurantSelectButtons();
        }
    }

    private void InitRestaurantSelectButtons() {
        buttonOne.onClick.RemoveListener(LoadGameSelect);
        buttonTwo.onClick.RemoveListener(NewGameSelect);

        buttonOne.onClick.AddListener(RestaurantOneSelect);
        buttonTwo.onClick.AddListener(RestaurantTwoSelect);

        buttonTwoObject.SetActive(false); // fries isn't ready, hide this for release

        startGameButton.onClick.AddListener(StartGameSelect);
        startGameText.SetActive(true);

        if (this.gameManager.SaveFileExists()) {
            buttonBack.onClick.AddListener(BackSelect);
            buttonBackObject.SetActive(true);
        } else {
            buttonBack.onClick.RemoveListener(BackSelect);
            buttonBackObject.SetActive(false);
        }

        buttonOneObject.GetComponentInChildren<Text>().text = REST_ONE_STRING;
        buttonTwoObject.GetComponentInChildren<Text>().text = REST_TWO_STRING;

        RestaurantOneSelect();
    }

    private void InitNewLoadGameButtons() {
        buttonOne.onClick.RemoveListener(RestaurantOneSelect);
        buttonTwo.onClick.RemoveListener(RestaurantTwoSelect);
        startGameButton.onClick.RemoveListener(StartGameSelect);
        startGameText.SetActive(false);

        buttonTwoObject.SetActive(true);

        buttonBack.onClick.RemoveListener(BackSelect);
        buttonBackObject.SetActive(false);

        buttonOne.onClick.AddListener(LoadGameSelect);
        buttonTwo.onClick.AddListener(NewGameSelect);

        buttonOneObject.GetComponentInChildren<Text>().text = LOAD_GAME;
        buttonTwoObject.GetComponentInChildren<Text>().text = NEW_GAME;

        LoadGameSelect();
    }

    /**** Events ****/

    private void LoadGameSelect() {
        isNewGame = false;
        this.currentlySelected = buttonOneObject;

        startGameButton.onClick.AddListener(StartGameSelect);
        startGameText.SetActive(true);
    }

    private void NewGameSelect() {
        isNewGame = true;
        this.currentlySelected = buttonTwoObject;
        InitRestaurantSelectButtons();
    }

    private void RestaurantOneSelect() {
        this.selectedType = REST_ONE_TYPE;
        this.currentlySelected = buttonOneObject;
    }

    private void RestaurantTwoSelect() {
        this.selectedType = REST_TWO_TYPE;
        this.currentlySelected = buttonTwoObject;
    }

    private void BackSelect() {
        InitNewLoadGameButtons();
    }

    private void StartGameSelect() {
        // we don't want to select this button, but still want to run the event
        NewGame(selectedType, isNewGame);
    }

    /**** Coroutine ****/
    IEnumerator WaitForGameManager() {
        while (this.gameManager == null) {
            this.gameManager = GameManager.GetInstance();
            yield return null;
        }
        InitButtons();
    }
}