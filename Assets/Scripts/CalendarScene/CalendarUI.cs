using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CalendarUI : MonoBehaviour {

    private GameManager gameManager;

    // game data to provide to the UI
    private int totalScore = 0; // the total score of the current game
    private GameObject month; // the current month

    // used to center and snap focus around the current day
    private Transform currentDayTransform; // the transform of the current day

    // UI status bar display
    [SerializeField] private Text totalScoreText;
    [SerializeField] private GameObject totalScoreDisplay;

    // modal display and removal variables
    private ModalUI.ModalState modalState;
    private GameObject modal;
    private float orthoCameraScale;
    private Vector3 baseModalScale;
    [SerializeField] private Camera mainCamera; // main camera, will be used to center modal on it

    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private GameObject monthPrefab;

    // event system
    public delegate void ModalEvent(ModalUI.ModalState state, string displayString);
    public static ModalEvent ModalNotification;
    public delegate void CameraNotification(bool state);
    public static CameraNotification BlockInput;

    private void Awake() {
        this.gameManager = GameManager.GetInstance();

        this.modal = Instantiate(this.modalPrefab, this.gameObject.transform, false);
        this.modalState = ModalUI.ModalState.NoState;
        this.modal.SetActive(false);

        this.month = Instantiate(this.monthPrefab, this.gameObject.transform, false);

        totalScoreDisplay.transform.SetAsLastSibling();

        ModalUI.NotifyCaller += ModalCloseEvent;
        DayUI.NotifyCalendarSelectDay += DaySelected;
        MonthUI.NotifyCurrentDay += SetCurrentDayTransform;
    }

    // Start is called before the first frame update
    private void Start() {
        if (this.gameManager != null) {
            InitBackground();
        } else {
            StartCoroutine("WaitForGameManager");
        }
    }

    private void Update() {
        string displayString = "";
        if (this.totalScore < 0 || this.gameManager.GetDaysPassed() == 21) { // TODO::arbitrary endgame, come back later
            // game is over, show game over modal
            displayString = "Days Lasted: " + (this.gameManager.GetDaysPassed() + 1).ToString() + " " +
            "Money Made: " + this.totalScore.ToString();
            this.modalState = ModalUI.ModalState.GameOver;
        }
        if(modalState != ModalUI.ModalState.HideModal && modalState != ModalUI.ModalState.NoState) {
            SpawnModal(this.modalState, displayString);
            AdjustModalPositionScale();
            BlockInput(true);
        } else if(modal.activeSelf && modalState == ModalUI.ModalState.HideModal) {
            HideModal();
            BlockInput(false);
        }
    }

    private void OnDestroy() {
        ModalUI.NotifyCaller -= ModalCloseEvent;
        DayUI.NotifyCalendarSelectDay -= DaySelected;
        MonthUI.NotifyCurrentDay -= SetCurrentDayTransform;
    }

    // set up the total score for display in the UI
    private void InitBackground() {
        this.totalScore = this.gameManager.GetTotalScore();
        this.totalScoreText.text = totalScore.ToString();
        this.orthoCameraScale = this.mainCamera.orthographicSize;
        this.baseModalScale = this.modal.transform.localScale;
    }

    // spawn a modal that will allow an action based on the modal state
    private void SpawnModal(ModalUI.ModalState state, string displayString) {
        this.modal.transform.SetAsLastSibling();
        this.modal.SetActive(true);
        ModalNotification(state, displayString);
    }

    // hides the modal to allow interaction with the calendar
    private void HideModal() {
        this.modalState = ModalUI.ModalState.NoState;
        this.modal.SetActive(false);
    }

    // resize/reposition the modal based on the camera's current position and orthographic view
    private void AdjustModalPositionScale() {
        this.modal.transform.position = new Vector3(this.mainCamera.transform.position.x,
                                                    this.mainCamera.transform.position.y,
                                                    this.modal.transform.position.z);

        float scaleFactor = this.mainCamera.orthographicSize / this.orthoCameraScale;
        this.modal.transform.localScale = new Vector3(this.baseModalScale.x * scaleFactor,
                                                        this.baseModalScale.y * scaleFactor);
    }

    /**** Coroutine ****/
    IEnumerator WaitForGameManager() {
        while (this.gameManager == null) {
            this.gameManager = GameManager.GetInstance();
            yield return null;
        }
        InitBackground();
    }

    /**** Events ****/

    // sets the current day transform
    private void SetCurrentDayTransform(Transform currentDay) {
        this.currentDayTransform = currentDay;
    }

    // notifies the calendar that the modal should be hidden
    private void ModalCloseEvent(ModalUI.ModalState currentModalState) {
        this.modalState = ModalUI.ModalState.HideModal;
    }

    // called when a day is selected, update the modal state to allow display
    private void DaySelected() {
        this.modalState = ModalUI.ModalState.DaySelect;
    }
}
