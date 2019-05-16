using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour {

    private static GameManager instance = null;

    // states denoting the current gameplay state/scene
    private enum States {
        NoState,
        TitleScene,
        GameplayScene,
        CalendarScene
    };

    // names for the scenes to allow for loading them
    private const string TITLE = "TitleScene";
    private const string GAMEPLAY = "RestaurantScene";
    private const string CALENDAR = "CalendarScene";

    private States currentState; // the current state/scene that the game is in

    // data that will be saved on gamesave
    private RestaurantInfo.Types currentRestType = RestaurantInfo.Types.NoType; // currently selected restaurant type  
    private int totalScore = 0; // players total score 
    private int daysPassed = 0; // days passed in the current month
    private MonthInfo.Months month = MonthInfo.Months.NONE; // the currently selected month

    private int dailyRent = 0; // can be quickly calculated, but is stored to eliminated potentially lengthy computation on function call

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(this);
        }
        DontDestroyOnLoad(this);
        this.currentState = States.NoState;
        SetCurrentStateFromScene();

        // Don't need to remove on destroy as game manager can't be destroyed
        TitleScene.NewGame += NewGameEvent;
        StatusBarUI.EndOfDay += EndOfDayEvent;
        ModalUI.NotifyGameManager += HandleModalEvent;
    }

    // set up gamemanager state based on no prior input other than the scene name. use case would be
    // launching a scene out of order from regular gameplay for testing
    private void SetCurrentStateFromScene() {
        string scene = SceneManager.GetActiveScene().name;
        switch(scene) {
            case TITLE:
                this.currentState = States.TitleScene;
                break;
            case GAMEPLAY:
                this.currentState = States.GameplayScene;
                if(this.currentRestType == RestaurantInfo.Types.NoType) {
                    // set burger to default;
                    this.currentRestType = RestaurantInfo.Types.Burger;
                }
                break;
            case CALENDAR:
                this.currentState = States.CalendarScene;
                if(this.month == MonthInfo.Months.NONE) {
                    PickMonth();
                }
                break;
            default:
                throw new System.Exception("Invalid scene name for current scene");
        };
    }

    // Used for scene transitions during gameplay. very similar to SetCurrentStateFromScene()
    private void SetAndLoadNewState(States newState) {
        if(this.currentState != newState) {
            this.currentState = newState;
            switch(currentState) {
                case States.TitleScene:
                    SceneManager.LoadSceneAsync(TITLE, LoadSceneMode.Single);
                    break;
                case States.GameplayScene:
                    if (this.currentRestType == RestaurantInfo.Types.NoType) {
                        // set burger to default;
                        this.currentRestType = RestaurantInfo.Types.Burger;
                    }
                    this.SetRent();
                    SceneManager.LoadSceneAsync(GAMEPLAY, LoadSceneMode.Single);
                    break;
                case States.CalendarScene:
                    if (this.month == MonthInfo.Months.NONE) {
                        PickMonth();
                    }
                    SceneManager.LoadSceneAsync(CALENDAR, LoadSceneMode.Single);
                    break;
                case States.NoState:
                    throw new System.Exception("Cannot load NoState state, invalid state action");
                default:
                    throw new System.Exception("Invalid scene name for current scene");
            }
        }
    }

    private void StartNewGame(RestaurantInfo.Types selectedType) {
        this.currentRestType = selectedType;
        this.totalScore = 0;
        this.daysPassed = 0;
        // pick a month here as well
        SetAndLoadNewState(States.CalendarScene);
    }

    private void SetRent() {
        // make this tougher later
        this.dailyRent = CalculateRent(this.daysPassed);
    }

    private int CalculateRent(int daysPassed) {
        return 50;
    }

    private void PickMonth() {
        int monthMaxIndex = System.Enum.GetNames(typeof(MonthInfo.Months)).Length-1;
        int index = Random.Range(1, monthMaxIndex);
        this.month = (MonthInfo.Months)System.Enum.GetValues(typeof(MonthInfo.Months)).GetValue(index);
    }

    /**** Events ****/
    private void NewGameEvent(RestaurantInfo.Types selectedType) {
        // set type to by the selected type
        StartNewGame(selectedType);
    }

    private void EndOfDayEvent(int score) {
        this.totalScore = this.totalScore - this.dailyRent + score;
        this.daysPassed += 1;
    }

    private void HandleModalEvent(ModalUI.ModalState modalState) {
        switch(modalState) {
            case ModalUI.ModalState.EndGame:
                SetAndLoadNewState(States.CalendarScene);
                break;
            case ModalUI.ModalState.DaySelect:
                SetAndLoadNewState(States.GameplayScene);
                break;
            case ModalUI.ModalState.GameOver:
                SetAndLoadNewState(States.TitleScene);
                break;
        }
    }

    /**** Public API ****/
    public static GameManager GetInstance() {
        return instance;
    }

    public RestaurantInfo.Types GetCurrentRestaurantType() {
        return this.currentRestType;
    }

    public int GetDaysPassed() {
        return this.daysPassed;
    }

    public int GetTodaysRent() {
        return this.dailyRent;
    }

    public int GetRentByDay(int day) {
        return CalculateRent(day);
    }

    public int GetTotalScore() {
        return this.totalScore;
    }

    public MonthInfo.Months GetMonth() {
        return this.month;
    }

    public void SetMonth(MonthInfo.Months month) {
        this.month = month;
    }
}