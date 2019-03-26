using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    private static GameManager instance = null;

    private enum States {
        TitleScene,
        GameplayScene
    };

    private const string TITLE = "TitleScene";
    private const string GAMEPLAY = "RestaurantScene";

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
        SetCurrentState();
        TitleScene.StartGameEvent += StartGameEvent;
        StatusBarUI.EndOfDay += EndOfDayEvent;
    }
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void SetCurrentState() {
        string scene = SceneManager.GetActiveScene().name;
        switch(scene) {
            case TITLE:
                this.currentState = States.TitleScene;
                Debug.Log("TitleScene");
                break;
            case GAMEPLAY:
                this.currentState = States.GameplayScene;
                if(this.currentRestType == RestaurantInfo.Types.NoType) {
                    // set burger to default;
                    this.currentRestType = RestaurantInfo.Types.Burger;
                }
                break;
            default:
                throw new System.Exception("Invalid scene name for current scene");
        };
    }

    /**** Events ****/
    private void StartGameEvent(RestaurantInfo.Types selectedType) {
        // load restaurant scene
        // set type to by the selected type
        this.currentRestType = selectedType;
        SceneManager.LoadSceneAsync(GAMEPLAY, LoadSceneMode.Single);
    }

    private void EndOfDayEvent(int score) {
        this.totalScore += score;
        this.daysPassed += 1;
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