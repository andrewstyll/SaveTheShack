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

    private ModalState state;
    private ModalState newState;
    private float countDownLeft;
    private string displayString;

    [SerializeField] private GameObject modalTextObj;
    [SerializeField] private GameObject buttonOne;
    [SerializeField] private GameObject buttonTwo;

    // Events
    public delegate void ModalNotification(ModalState state);
    public static event ModalNotification CountDownComplete;
    public static event ModalNotification NotifyCaller;
    public static event ModalNotification NotifyGameManager;

    private void Awake() {
        RestaurantManager.ModalEvent += RecieveModalEvent;
        CalendarUI.SelectDayEvent += RecieveModalEvent;

        buttonOne.GetComponent<Button>().onClick.AddListener(ButtonOneListener);
        buttonTwo.GetComponent<Button>().onClick.AddListener(ButtonTwoListener);

        this.state = ModalState.NoState;
        this.newState = ModalState.NoState;
        this.NoState();
    }

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() {
        if(this.state != this.newState) {

            this.state = this.newState;
            switch (this.state) {
                case ModalState.NoState:
                    this.NoState();
                    break;
                case ModalState.Loading:
                    this.Loading();
                    break;
                case ModalState.CountDown:
                    this.CountDown();
                    break;
                case ModalState.EndGame:
                    this.EndGame();
                    break;
                case ModalState.DaySelect:
                    this.DaySelect();
                    break;
            }
        }

        if (this.state == ModalState.CountDown) {
            UpdateCountDown();
        }
    }

    private void OnDestroy() {
        RestaurantManager.ModalEvent -= RecieveModalEvent;
        CalendarUI.SelectDayEvent -= RecieveModalEvent;
    }

    private void NoState() {
        this.ButtonControl(false, false);
        this.modalTextObj.SetActive(false);
    }

    private void Loading() {
        this.ButtonControl(false, false);
        this.modalTextObj.SetActive(true);
        Text loadingText = this.modalTextObj.GetComponent<Text>();
        loadingText.text = (this.displayString != "") ? this.displayString : LOADING_TEXT;
    }

    private void CountDown() {
        this.ButtonControl(false, false);
        this.modalTextObj.SetActive(true);
        this.countDownLeft = COUNTDOWN;
    }

    private void EndGame() {
        this.ButtonControl(true, false);
        this.SetButtonText("Continue?", "");
        this.modalTextObj.SetActive(true);
        Text endgameText = this.modalTextObj.GetComponent<Text>();
        endgameText.text = YOUR_SCORE + ((this.displayString != "") ? this.displayString : "0");
    }

    private void DaySelect() {
        this.ButtonControl(true, true);
        this.SetButtonText("Yes", "No");
        this.modalTextObj.SetActive(true);
        Text daySelectText = this.modalTextObj.GetComponent<Text>();
        daySelectText.text = START_DAY + this.displayString;
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
            this.newState = ModalState.NoState;
            this.state = ModalState.NoState;
            this.NoState();
            CountDownComplete?.Invoke(this.state);
        }
    }

    // display string only used to pass endgame scores for now, kept because potentially useful in future
    private void SetState(ModalState state, string displayString) {
        this.newState = state;
        this.displayString = displayString;      
    }

    /**** Button control functions ****/
    private void ButtonControl(bool oneOn, bool twoOn) {
        if(oneOn) {
            this.buttonOne.SetActive(true);
        } else {
            this.buttonOne.SetActive(false);
        }
        if(twoOn) {
            this.buttonTwo.SetActive(true);
        } else {
            this.buttonTwo.SetActive(false);
        }
    }

    private void SetButtonText(string textOne, string textTwo) {
        this.buttonOne.GetComponentInChildren<Text>().text = textOne;
        this.buttonTwo.GetComponentInChildren<Text>().text = textTwo;
    }

    /**** Events ****/
    private void RecieveModalEvent(ModalState state, string displayString) {
        this.SetState(state, displayString);
    }

    private void ButtonOneListener() {
        if(this.state == ModalState.EndGame || this.state == ModalState.DaySelect) {
            NotifyGameManager?.Invoke(this.state);
        }
    }

    private void ButtonTwoListener() {
        if(this.state == ModalState.DaySelect) {
            NotifyCaller?.Invoke(this.state);
        }
    }
}
