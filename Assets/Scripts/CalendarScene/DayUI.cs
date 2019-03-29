using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayUI : MonoBehaviour {

    private int id;
    private Button button;
    private Image image;

    private GameObject modal;
    [SerializeField] private GameObject modalPrefab;

    [SerializeField] private Sprite pastSprite;
    [SerializeField] private Sprite currentSprite;

    public delegate void SelectDayEvent(int id);
    public static SelectDayEvent NotifyCalendarSelectDay;

    private void Awake() {
        this.button = gameObject.GetComponent<Button>();
        this.image = gameObject.GetComponent<Image>();

        this.button.onClick.AddListener(SelectDay);
    }

    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    /**** Events ****/
    private void SelectDay() {
        if (this == null) Debug.Log("Null this DayUI");
        NotifyCalendarSelectDay?.Invoke(this.id);
    }

    /**** Public API ****/
    public void SetPast() {
        this.button.enabled = false;
        this.image.sprite = pastSprite;
    }

    public void SetCurrent() {
        this.button.enabled = true;
        this.image.sprite = currentSprite;
    }

    public void SetFuture() {
        this.button.enabled = false;
        this.image.enabled = false;
    }

    public void SetDay(int id) {
        this.id = id;
    }
}
