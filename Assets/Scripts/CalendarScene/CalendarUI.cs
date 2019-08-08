using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/* describes the modal behaviour/position relative to the camera.
 */
public class CalendarUI : MonoBehaviour {

    private GameManager gameManager;
    private MonthBuilder monthBuilder;

    // game data to provide to the UI
    private int totalScore = 0; // the total score of the current game
    private GameObject month; // the current month

    // used to center and snap focus around the current day::UNUSED?
    private Transform currentDayTransform; // the transform of the current day

    // UI total game score display
    [SerializeField] private Text totalScoreText;
    [SerializeField] private GameObject totalScoreDisplay;

    // modal display and removal variables
    private ModalUI.ModalState modalState;
    private GameObject modal;
    private float orthoCameraScale; // controls modal scale relative to camera orthographic size
    private Vector3 baseModalScale; // controls modal position relative to camera zoom

    // main camera, will be used to center modal on it
    [SerializeField] private Camera mainCamera; 

    [SerializeField] private GameObject modalPrefab;

    // event system
    public delegate void ModalEvent(ModalUI.ModalState state, string displayString);
    public static ModalEvent ModalNotification; // notify a modal event has occurred
    public delegate void CameraNotification(bool state);
    public static CameraNotification BlockInput; // used to block user input on all areas of screen except for the modal when spawned

    private void Awake() {
        this.gameManager = GameManager.GetInstance();
        this.monthBuilder = MonthBuilder.GetInstance();

        // spawn the modal but hide it. The modal base size is the size of the entire calendar
        this.modal = Instantiate(this.modalPrefab, this.gameObject.transform, false);
        this.modalState = ModalUI.ModalState.NoState;
        this.modal.SetActive(false);

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
        // arbitrary end date, will modify when months become objects, not just prefabs
        if (this.totalScore < 0 || this.gameManager.GetDaysPassed() >= 26) { 
            // game is over, complete game-over string with score and days passed
            displayString = "Days Lasted: " + (this.gameManager.GetDaysPassed() + 1).ToString() + " " +
            "Money Made: " + this.totalScore.ToString();
            this.modalState = ModalUI.ModalState.GameOver;
        }
        // if the modal is intended to be in a state where it is displayed, display it. Otherwise hide it.
        if(modalState != ModalUI.ModalState.HideModal && modalState != ModalUI.ModalState.NoState) {
            SpawnModal(this.modalState, displayString);
            AdjustModalPositionScale();
            BlockInput(true);
        } else if(modal.activeSelf && modalState == ModalUI.ModalState.HideModal) {
            HideModal();
            BlockInput(false);
        }
    }

    // remove event listeners after calendarUI is destroyed to prevent listeners attached to null objects
    private void OnDestroy() {
        ModalUI.NotifyCaller -= ModalCloseEvent;
        DayUI.NotifyCalendarSelectDay -= DaySelected;
        MonthUI.NotifyCurrentDay -= SetCurrentDayTransform;
    }

    // set up the total score for display in the UI along with the correct month image via prefab.
    // days will be colored/delt with via the monthUI and the dayUI
    private void InitBackground() {
        GameObject monthPrefab = monthBuilder.GetMonthPrefab(this.gameManager.GetMonth());
        this.month = Instantiate(monthPrefab, this.gameObject.transform, false);

        this.totalScore = this.gameManager.GetTotalScore();
        this.totalScoreText.text = totalScore.ToString();
        this.orthoCameraScale = this.mainCamera.orthographicSize;
        this.baseModalScale = this.modal.transform.localScale;

        // move the total score display to be in front of the calendar image
        totalScoreDisplay.transform.SetAsLastSibling();
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

        // use the new camera size/the base camera size to scale the x and y properties of the modal
        float scaleFactor = this.mainCamera.orthographicSize / this.orthoCameraScale;
        this.modal.transform.localScale = new Vector3(this.baseModalScale.x * scaleFactor,
                                                        this.baseModalScale.y * scaleFactor);
    }

    /**** Coroutine ****/
    // wait for game manager before running init
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

    // called when a day is selected, update the modal state to allow display of modal
    // centered on the day
    private void DaySelected() {
        this.modalState = ModalUI.ModalState.DaySelect;
    }
}
