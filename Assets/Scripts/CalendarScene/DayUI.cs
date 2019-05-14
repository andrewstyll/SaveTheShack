using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayUI : MonoBehaviour {

    private const float ALPHA_HALF = 0.5f;
    private const float ALPHA_FULL = 1.0f;

    private int id;
    int rent = 0;

    private GameObject modal;
    [SerializeField] private GameObject modalPrefab;

    [SerializeField] private Sprite pastSprite;
    [SerializeField] private Sprite currentSprite;
    [SerializeField] private Image markedDay;

    [SerializeField] private Image rentDisplay;
    [SerializeField] private Image coinImage;
    [SerializeField] private Text rentText;

    [SerializeField] private Button button;

    public delegate void SelectDayEvent();
    public static SelectDayEvent NotifyCalendarSelectDay;

    private void Awake() {
        this.button.onClick.AddListener(SelectDay);
        Color alphaControl = this.markedDay.color;
        alphaControl.a = ALPHA_FULL;
        this.markedDay.color = alphaControl;
    }

    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    void Update() {
    }

    private void ToggleRent(bool rentEnable) {
        this.rentDisplay.enabled = rentEnable;
        this.coinImage.enabled = rentEnable;
        this.rentText.enabled = rentEnable;
    }

    private void RentIsDim(bool rentIsDim) {
        Color alphaControl;
        if(rentIsDim) {
            alphaControl = this.rentDisplay.color;
            alphaControl.a = ALPHA_HALF;
            this.rentDisplay.color = alphaControl;

            alphaControl = this.coinImage.color;
            alphaControl.a = ALPHA_HALF;
            this.coinImage.color = alphaControl;

            alphaControl = this.rentText.color;
            alphaControl.a = ALPHA_HALF;
            this.rentText.color = alphaControl;
        } else {
            alphaControl = this.rentDisplay.color;
            alphaControl.a = ALPHA_FULL;
            this.rentDisplay.color = alphaControl;

            alphaControl = this.coinImage.color;
            alphaControl.a = ALPHA_FULL;
            this.coinImage.color = alphaControl;

            alphaControl = this.rentText.color;
            alphaControl.a = ALPHA_FULL;
            this.rentText.color = alphaControl;
        }
    }

    /**** Events ****/
    private void SelectDay() {
        if (this == null) Debug.Log("Null this DayUI");
        NotifyCalendarSelectDay?.Invoke();
    }

    /**** Public API ****/
    public void SetPast() {
        this.button.enabled = false;
        this.markedDay.sprite = pastSprite;
        this.ToggleRent(true);
        this.RentIsDim(true);
    }

    public void SetCurrent() {
        this.button.enabled = true;
        this.markedDay.sprite = currentSprite;
        this.rentDisplay.enabled = true;
        this.ToggleRent(true);
        this.RentIsDim(false);
    }

    public void SetFuture() {
        this.button.enabled = false;
        this.markedDay.enabled = false;
        this.rentDisplay.enabled = false;
        this.ToggleRent(false);
    }

    public void SetDay(int id) {
        this.id = id;
    }

    public void SetRent(int rent) {
        this.rent = rent;
        this.rentText.text = this.rent.ToString();
    }
}
