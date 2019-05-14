using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarUI : MonoBehaviour {

    private GameManager gameManager;

    // to store current game state locally, to avoid calling game manager
    private bool canPlayOn = true;
    private int totalScore = 0;
    private int daysPassed;
    private GameObject month;
    private Transform currentDayTransform;

    private GameObject modal = null;

    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private GameObject monthPrefab;

    // event system
    public delegate void ModalEvent(ModalUI.ModalState state, string displayString);
    public static ModalEvent ModalNotification;

    private void Awake() {
        this.gameManager = GameManager.GetInstance();

        this.modal = null;

        this.month = Instantiate(this.monthPrefab, this.gameObject.transform, false);

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
        if(this.totalScore < 0) {
            // game is over, show game over modal
            string displayString = "Days Lasted: " + (this.daysPassed + 1).ToString() + " " +
            "Money Made: " + this.totalScore.ToString();
            SpawnModal(ModalUI.ModalState.GameOver, displayString);
        }
    }

    private void OnDestroy() {
        ModalUI.NotifyCaller -= ModalCloseEvent;
        DayUI.NotifyCalendarSelectDay -= DaySelected;
        MonthUI.NotifyCurrentDay -= SetCurrentDayTransform;
    }

    private void InitBackground() {
        this.totalScore = this.gameManager.GetTotalScore();
        this.daysPassed = this.gameManager.GetDaysPassed();
    }

    // spawn a modal that will allow an action based on the modal state
    private void SpawnModal(ModalUI.ModalState state, string displayString) {
        // the calendar has to show the modal to fill screen, so we will send an event along with the ID of the day
        // id will be used to grab transform from days array
        if (this.modal == null) {

            this.modal = Instantiate(this.modalPrefab, this.gameObject.transform, false);
        }
        this.modal.SetActive(true);
        ModalNotification(state, displayString);
    }

    // hides the modal to allow interaction with the calendar
    private void HideModal() {
        this.modal.SetActive(false);
    }

    // called when a day is selected, the modal will be centered on the day
    private void DaySelected() {
        //SpawnModal(ModalUI.ModalState.DaySelect, "");
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
    private void ModalCloseEvent(ModalUI.ModalState modalState) {
        if(modalState == ModalUI.ModalState.DaySelect) {
            HideModal();
        }
    }

    private void SetCurrentDayTransform(Transform currentDay) {
        this.currentDayTransform = currentDay;
    }
}
