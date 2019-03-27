using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalendarUI : MonoBehaviour {

    private GameManager gameManager;

    private Camera mainCamera;

    private GameObject[] days;
    private int daysPassed;
    private int numDays;

    [SerializeField] private int daysPassedUser;

    private void Awake() {
        this.gameManager = GameManager.GetInstance();

        this.mainCamera = Camera.main;
        this.numDays = gameObject.transform.childCount;

        this.days = new GameObject[this.numDays];
        for(int i = 0; i < days.Length; i++) {
            this.days[i] = gameObject.transform.GetChild(i).gameObject;
        }

    }

    // Start is called before the first frame update
    private void Start() {
        if(daysPassedUser == -1) {
            this.daysPassed = gameManager.GetDaysPassed();
        } else {
            this.daysPassed = daysPassedUser;
        }

        for (int i = 0; i < days.Length; i++) {
            DayUI script = this.days[i].GetComponent<DayUI>();
            if(i < this.daysPassed) {
                script.SetPast();
            } else if(i == this.daysPassed) {
                script.SetCurrent();
            } else {
                script.SetFuture();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
