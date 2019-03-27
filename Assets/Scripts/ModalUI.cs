using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalUI : MonoBehaviour {

    private const float COUNTDOWN = 4.0f;
    private const string LOADING_TEXT = "Loading...";
    private const string READY_TEXT = "Ready?";
    private const string YOUR_SCORE = "Score: ";
    private const string START_DAY = "Start Day?";

    public enum ModalState {
        NoState,
        Loading,
        CountDown,
        EndGame,
        DaySelect
    };

    private ModalState state = ModalState.NoState;
    private float countDownLeft;

    [SerializeField] private GameObject modalTextObj;

    // Events
    public delegate void ModalNotification();
    public static event ModalNotification CountDownComplete;

    public delegate void ModalButtonControl(bool buttonOne, bool buttonTwo, string textOne, string textTwo);
    public static event ModalButtonControl ButtonControl;

    private void Awake() {
        RestaurantManager.ModalEvent += RecieveModalEvent;
        DayUI.SelectDayEvent += RecieveModalEvent;
    }

    // Start is called before the first frame update
    void Start() {
        //this.SetState(ModalState.Loading, "");
    }

    // Update is called once per frame
    void Update() {
        if(this.state == ModalState.CountDown) {
            UpdateCountDown();
        }
    }

    private void Loading(string displayString) {
        ButtonControl(false, false, "", "");
        Text loadingText = this.modalTextObj.GetComponent<Text>();
        loadingText.text = (displayString != "") ? displayString : LOADING_TEXT;
    }

    private void CountDown(string displayString) {
        ButtonControl(false, false, "", "");
        this.countDownLeft = COUNTDOWN;
    }

    private void EndGame(string displayString) {
        ButtonControl(false, false, "", "");
        Text endgameText = this.modalTextObj.GetComponent<Text>();
        endgameText.text = YOUR_SCORE + displayString;
    }

    private void DaySelect(string displayString) {
        Debug.Log("Here");
        ButtonControl(true, true, "Yes", "No");
        Text daySelectText = this.modalTextObj.GetComponent<Text>();
        daySelectText.text = START_DAY + displayString;
    }

    private void UpdateCountDown() {
        this.countDownLeft -= Time.deltaTime;

        if (this.countDownLeft > 0) {
            Text countdownText = this.modalTextObj.GetComponent<Text>();

            int countDown = (int)Mathf.Ceil(this.countDownLeft);

            if (countDown == 4) {
                countdownText.text = READY_TEXT;
            } else {
                countdownText.text = Mathf.Ceil(this.countDownLeft).ToString();
            }
        } else {
            this.state = ModalState.NoState;
            CountDownComplete();
        }
    }

    // display string only used to pass endgame scores for now, kept because potentially useful in future
    private void SetState(ModalState state, string displayString) {
        if (this.state != state) {
            this.state = state;
            switch (state) {
                case ModalState.Loading:
                    this.Loading(displayString);
                    break;
                case ModalState.CountDown:
                    this.CountDown(displayString);
                    break;
                case ModalState.EndGame:
                    this.EndGame(displayString);
                    break;
                case ModalState.DaySelect:
                    this.DaySelect(displayString);
                    break;
            };
        }
    }

    /**** Events ****/
    private void RecieveModalEvent(ModalState state, string displayString) {
        this.SetState(state, displayString);
    }
}
