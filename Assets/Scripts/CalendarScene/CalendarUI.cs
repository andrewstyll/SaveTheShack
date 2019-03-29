using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarUI : MonoBehaviour {

    private GameManager gameManager;

    private Camera mainCamera;

    private GameObject modal = null;

    private GameObject month;
    private GameObject[] days;
    private int daysPassed;
    private int numDays;

    [SerializeField] private GameObject modalPrefab;
    [SerializeField] private GameObject monthPrefab;
    [SerializeField] private int daysPassedUser;

    public delegate void ModalEvent(ModalUI.ModalState state, string displayString);
    public static ModalEvent SelectDayEvent;

    private void Awake() {
        Debug.Log("Awake");
        if (this == null) Debug.Log("Null this Awake");
        this.gameManager = GameManager.GetInstance();

        this.mainCamera = Camera.main;

        this.modal = null;

        this.month = Instantiate(this.monthPrefab, this.gameObject.transform, false);
        this.numDays = month.transform.childCount;
        this.days = new GameObject[this.numDays];

        ModalUI.NotifyCaller += ModalCloseEvent;
        DayUI.NotifyCalendarSelectDay += SpawnDaySelectModal;
    }

    // Start is called before the first frame update
    private void Start() {
        Debug.Log("Start");

        if (this.gameManager != null) {
            InitCalendar();
        } else {
            StartCoroutine("WaitForGameManager");
        }
    }

    private void OnDestroy() {
        ModalUI.NotifyCaller -= ModalCloseEvent;
        DayUI.NotifyCalendarSelectDay -= SpawnDaySelectModal;
    }

    private void SpawnDaySelectModal(int id) {
        // the calendar has to show the modal to fill screen, so we will send an event along with the ID of the day
        if (this.modal == null) {

            this.modal = Instantiate(this.modalPrefab, this.gameObject.transform, false);
        }
        this.modal.SetActive(true);
        SelectDayEvent(ModalUI.ModalState.DaySelect, "");
    }

    private void HideModal() {
        this.modal.SetActive(false);
    }

    private void InitCalendar() {
        if (daysPassedUser == -1 ) {
            this.daysPassed = this.gameManager.GetDaysPassed();
        } else {
            this.daysPassed = daysPassedUser;
        }

        for (int i = 0; i < days.Length; i++) {
            this.days[i] = this.month.transform.GetChild(i).gameObject;
            DayUI script = this.days[i].GetComponent<DayUI>();
            script.SetDay(i);
            if (i < this.daysPassed) {
                script.SetPast();
            } else if (i == this.daysPassed) {
                script.SetCurrent();
            } else {
                script.SetFuture();
            }
        }
    }

    /**** Events ****/
    private void ModalCloseEvent(ModalUI.ModalState modalState) {
        if(modalState == ModalUI.ModalState.DaySelect) {
            HideModal();
        }
    }

    /**** Coroutine ****/
    IEnumerator WaitForGameManager() {
        while (this.gameManager == null) {
            this.gameManager = GameManager.GetInstance();
            yield return null;
        }
        InitCalendar();
    }
}
