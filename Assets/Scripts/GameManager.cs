using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

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

    // Player save data path
    private string SAVE_PATH;

    // names for the scenes to allow for loading them
    private const string TITLE = "TitleScene";
    private const string GAMEPLAY = "RestaurantScene";
    private const string CALENDAR = "CalendarScene";

    private States currentState; // the current state/scene that the game is in

    // data that will be saved on gamesave
    private RestaurantInfo.Types currentRestType = RestaurantInfo.Types.NoType; // currently selected restaurant type  
    private MonthInfo.Months month = MonthInfo.Months.NONE; // the currently selected month
    private int totalScore = 0; // players total score 
    private int daysPassed = 27; // days passed in the current month

    private int dailyRent = 0; // can be quickly calculated, but is stored to eliminated potentially lengthy computation on function call

    private void Awake() {
        if(instance == null) {
            instance = this;
        } else if(instance != this) {
            Destroy(this);
        }

        DontDestroyOnLoad(this);
        this.currentState = States.NoState;
        SAVE_PATH = Application.persistentDataPath + "/player.save";
        LoadSceneFromAwake();

        // Don't need to remove on destroy as game manager can't be destroyed
        TitleScene.NewGame += StartGameEvent;
        StatusBarUI.EndOfDay += EndOfDayEvent;
        ModalUI.NotifyGameManager += HandleModalEvent;
    }

    // set up gamemanager state based on no prior input other than the scene name. use case would be
    // launching a scene out of order from regular gameplay for testing
    private void LoadSceneFromAwake() {
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

    // Used for scene transitions during gameplay. very similar to LoadSceneFromAwake()
    private void SetAndLoadNewState(States newState) {
        if(this.currentState != newState) {
            if(this.currentState == States.GameplayScene) {
                SaveGame();
            }

            switch (newState) {
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
            this.currentState = newState;
        }
    }

    private void DeleteOldGame() { 
        if(SaveFileExists()) {
            File.Delete(SAVE_PATH);
        }
    }

    private void SaveGame() {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream stream = new FileStream(SAVE_PATH, FileMode.Create);

        GameData data = new GameData(this.currentRestType, this.month, this.totalScore, this.daysPassed);
        binaryFormatter.Serialize(stream, data);
        stream.Close();
    }

    private void LoadGame() { 
        if(SaveFileExists()) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(SAVE_PATH, FileMode.Open);
            GameData gameData = (GameData)binaryFormatter.Deserialize(stream);

            this.currentRestType = (RestaurantInfo.Types)gameData.restaurantType;
            this.month = (MonthInfo.Months)gameData.month;
            this.totalScore = gameData.totalScore;
            this.daysPassed = gameData.daysPassed;

        } else {
            throw new System.Exception("Save file not found at " + SAVE_PATH);
        }
    }

    private void SetRent() {
        // make this tougher later
        this.dailyRent = CalculateRent(this.daysPassed);
    }

    private int CalculateRent(int daysPassed) {
        return (50*(daysPassed+1)) - (daysPassed*3);
    }

    private void PickMonth() {
        int monthMaxIndex = System.Enum.GetNames(typeof(MonthInfo.Months)).Length-1;
        int index = Random.Range(1, monthMaxIndex);
        this.month = (MonthInfo.Months)System.Enum.GetValues(typeof(MonthInfo.Months)).GetValue(index);
    }

    /**** Events ****/
    private void StartGameEvent(RestaurantInfo.Types selectedType, bool isNewGame) {

        if(isNewGame) {
            // delete old save file if it exists
            DeleteOldGame();
            PickMonth();
            this.currentRestType = selectedType;
            this.totalScore = 0;
            this.daysPassed = 0;
            SaveGame();
        } else {
            LoadGame();
        }

        SetAndLoadNewState(States.CalendarScene);
    }

    private void EndOfDayEvent(int score) {
        this.totalScore = this.totalScore - this.dailyRent + score;
        this.daysPassed += 1;
    }

    private void HandleModalEvent(ModalUI.ModalState modalState) {
        switch(modalState) {
            case ModalUI.ModalState.EndDay:
                SetAndLoadNewState(States.CalendarScene);
                break;
            case ModalUI.ModalState.DaySelect:
                SetAndLoadNewState(States.GameplayScene);
                break;
            case ModalUI.ModalState.GameOver:
                DeleteOldGame();
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

    public bool SaveFileExists() {
        return File.Exists(SAVE_PATH);
    }
}