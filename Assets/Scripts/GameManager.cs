using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager instance = null;

    private enum States {
        NoState,
        TitleScene,
        GameplayScene,
        CalendarScene
    };

    private const string TITLE = "TitleScene";
    private const string GAMEPLAY = "RestaurantScene";
    private const string CALENDAR = "CalendarScene";

    private States currentState;
    private RestaurantInfo.Types currentRestType = RestaurantInfo.Types.NoType;

    private int totalScore = 0;
    private int daysPassed = 0;

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

    // call this after loading every scene
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
                break;
            default:
                throw new System.Exception("Invalid scene name for current scene");
        };
    }

    // acts the same as SetCurrentStateFromScene, but bases state change on input state
    // rather than readon from scene title. Sets up andloads the new scene
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
                    SceneManager.LoadSceneAsync(GAMEPLAY, LoadSceneMode.Single);
                    break;
                case States.CalendarScene:
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

    /**** Events ****/
    private void NewGameEvent(RestaurantInfo.Types selectedType) {
        // set type to by the selected type
        StartNewGame(selectedType);
    }

    private void EndOfDayEvent(int score) {
        this.totalScore += score;
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
}