using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Modal : MonoBehaviour {

    private const float COUNTDOWN = 3.0f;
    private const string LOADING_TEXT = "Loading...";
    private const string READY_TEXT = "Ready?";
    private const string TAP_TO_CONTINUE = "Tap to continue...";

    public enum ModalState {
        Loading,
        Ready,
        CountDown,
        EndGame,
        NoState
    };

    private ModalState state = ModalState.NoState;
    private float countDownLeft;

    [SerializeField] private GameObject modalTextObj;
    [SerializeField] private GameObject modalButtonObj;

    // Events
    public delegate void ModalNotification();
    public static event ModalNotification CountDownComplete;

    private void Awake() {
        RestaurantManager.ModalEvent += RecieveModalEvent;

        this.modalTextObj.SetActive(false);
        this.modalButtonObj.SetActive(false);

        this.SetState(ModalState.Loading);
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if(this.state == ModalState.CountDown) {
            UpdateCountDown();
        }
    }

    private void Loading() {
        this.state = ModalState.Loading;
        this.modalTextObj.SetActive(true);
        this.modalButtonObj.SetActive(false);

        Text loadingText = this.modalTextObj.GetComponentInChildren<Text>();
        loadingText.text = LOADING_TEXT;
    }

    private void Ready() {
        this.state = ModalState.Ready;
        this.modalTextObj.SetActive(false);
        this.modalButtonObj.SetActive(true);

        this.modalButtonObj.GetComponent<Button>().onClick.AddListener(CountDown);
    }

    private void UpdateCountDown() {
        this.countDownLeft -= Time.deltaTime;

        if (this.countDownLeft > 0) {
            Text countdownText = this.modalTextObj.GetComponentInChildren<Text>();
            countdownText.text = Mathf.Ceil(this.countDownLeft).ToString();
        } else {
            this.state = ModalState.NoState;
            CountDownComplete();
        }
    }

    private void CountDown() {
        this.state = ModalState.CountDown;
        this.modalTextObj.SetActive(true);
        this.modalButtonObj.SetActive(false);

        this.countDownLeft = COUNTDOWN;
    }

    private void EndGame() {

    }

    private void SetState(ModalState state) {
        if (this.state != state) {
            switch (state) {
                case ModalState.Loading:
                    this.Loading();
                    break;
                case ModalState.Ready:
                    this.Ready();
                    break;
                case ModalState.CountDown:
                    this.CountDown();
                    break;
            };
            this.state = state;
        }
    }

    /**** Events ****/
    private void RecieveModalEvent(ModalState state) {
        this.SetState(state);
    }
}
