using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonthUI : MonoBehaviour {

    private GameManager gameManager;

    private int daysPassed = -1;
    private int numDays;

    private void Awake() {
        this.gameManager = GameManager.GetInstance();

        this.numDays = gameObject.transform.childCount;
    }

    // Start is called before the first frame update
    void Start() {
        if (this.gameManager != null) {
            InitMonth();
        } else {
            StartCoroutine("WaitForGameManager");
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void InitMonth() {
        if(this.daysPassed == -1) {
            this.daysPassed = this.gameManager.GetDaysPassed();
        }

        for (int i = 0; i < this.numDays; i++) {
            GameObject day = gameObject.transform.GetChild(i).gameObject;
            DayUI script = day.GetComponent<DayUI>();
            script.SetDay(i);
            script.SetRent(this.gameManager.GetRentByDay(i));
            if (i < this.daysPassed) {
                script.SetPast();
            } else if (i == this.daysPassed) {
                script.SetCurrent();
            } else {
                script.SetFuture();
            }
        }
    }

    /**** Coroutine ****/
    IEnumerator WaitForGameManager() {
        while (this.gameManager == null) {
            this.gameManager = GameManager.GetInstance();
            yield return null;
        }
        InitMonth();
    }
}
