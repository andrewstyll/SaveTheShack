using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour {

    private bool restaurantOpen = false;

    // score and timer interfaces
    private const int SINGLE_CUSTOMER_SCORE = 10;
    private const float TIME_PER_DAY = 10.0f;
    //private const float TIME_PER_DAY = 60.0f;

    private int score;
    private float timeRemaining;
    private Slider timer;
    private Text scoreText;

    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject scoreObject;

    // Events
    public delegate void EndOfDayEvent(int score);
    public static event EndOfDayEvent EndOfDay;

    public delegate void StatusBarUINotification();
    public static event StatusBarUINotification Loaded;

    private void Awake() {
        RestaurantManager.StartGame += StartDay;
        CustomerUI.SuccessfulOrder += AddToScoreEvent;

        this.score = 10;
        this.timeRemaining = TIME_PER_DAY;

        this.timer = timerObject.GetComponent<Slider>();
        this.scoreText = scoreObject.GetComponent<Text>();

        this.scoreText.text = score.ToString();
    }

    private void Start() {
        Loaded();
    }

    // Update is called once per frame
    private void Update() {
        if(this.restaurantOpen) {
            if (this.timeRemaining > 0) {
                UpdateSlider();
            } else {
                this.restaurantOpen = false;
                EndOfDay(this.score);
            }
        }
    }

    private void UpdateSlider() {
        this.timeRemaining -= Time.deltaTime;
        this.timer.value = (TIME_PER_DAY - this.timeRemaining) / TIME_PER_DAY;
    }

    /**** EVENTS ****/
    private void AddToScoreEvent() {
        score += SINGLE_CUSTOMER_SCORE;
        this.scoreText.text = score.ToString();
    }

    private void StartDay() {
        this.restaurantOpen = true;
    }
}
