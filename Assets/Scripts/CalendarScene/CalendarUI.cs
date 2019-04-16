using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarUI : MonoBehaviour {

    private GameManager gameManager;

    private Camera mainCamera;

    private bool canPlayOn = true;
    private int totalScore = 0;
    private GameObject modal = null;

    private GameObject month;
    private int daysPassed;

    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private GameObject monthPrefab;

    public delegate void ModalEvent(ModalUI.ModalState state, string displayString);
    public static ModalEvent ModalNotification;

    private void Awake() {
        this.gameManager = GameManager.GetInstance();

        this.mainCamera = Camera.main;

        this.modal = null;

        this.month = Instantiate(this.monthPrefab, this.gameObject.transform, false);

        ModalUI.NotifyCaller += ModalCloseEvent;
        DayUI.NotifyCalendarSelectDay += DaySelected;
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
            SpawnModal(-1, ModalUI.ModalState.GameOver, displayString);
        }
    }

    private void OnDestroy() {
        ModalUI.NotifyCaller -= ModalCloseEvent;
        DayUI.NotifyCalendarSelectDay -= DaySelected;
    }

    private void SpawnModal(int id, ModalUI.ModalState state, string displayString) {
        // the calendar has to show the modal to fill screen, so we will send an event along with the ID of the day
        // id will be used to grab transform from days array
        if (this.modal == null) {

            this.modal = Instantiate(this.modalPrefab, this.gameObject.transform, false);
        }
        this.modal.SetActive(true);
        ModalNotification(state, displayString);
    }

    private void HideModal() {
        this.modal.SetActive(false);
    }

    private void DaySelected(int id) {
        SpawnModal(id, ModalUI.ModalState.DaySelect, "");
    }

    private void InitBackground() {
        this.totalScore = this.gameManager.GetTotalScore();
        this.daysPassed = this.gameManager.GetDaysPassed();
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
}
