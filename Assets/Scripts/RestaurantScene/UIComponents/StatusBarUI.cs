using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarUI : MonoBehaviour {

    // score and timer interfaces
    private const int SINGLE_CUSTOMER_SCORE = 10;
    private const float TIME_PER_DAY = 60.0f;

    private int score;
    private float timeRemaining;
    private Slider timer;
    private Text scoreText;

    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject scoreObject;

    public delegate void EndOfDayEvent(int score);
    public static event EndOfDayEvent EndOfDay;

    private void Awake() {
        CustomerUI.SuccessfulOrder += AddToScoreEvent;

        this.score = 0;
        this.timeRemaining = TIME_PER_DAY;

        this.timer = timerObject.GetComponent<Slider>();
        this.scoreText = scoreObject.GetComponent<Text>();

        this.scoreText.text = score.ToString();
    }

    // Update is called once per frame
    private void Update() {
        if(this.timeRemaining > 0) {
            UpdateSlider();
        } else {
            EndOfDay(this.score);
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
}
