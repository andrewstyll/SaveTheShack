using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrinkUI : MonoBehaviour {

    private enum State {
        NoDrink,
        Pouring,
        Poured
    };

    [SerializeField] private GameObject timerObject;

    private Food drink;

    private Color alphaControl = Color.white;
    private const float ALPHA_HIDDEN = 0.0f;
    private const float ALPHA_HALF = 0.5f;
    private const float ALPHA_FULL = 1.0f;

    private const float POUR_TIME = 3.0f;
    private float timeRemaining;

    private Button mainButton;

    private State state;

    public delegate bool EventHandler(Food food);
    public static event EventHandler FoodSelected;

    private void Awake() {
        this.state = State.NoDrink;
        this.timerObject.SetActive(false);

        // sprite will not be set yet here
        this.alphaControl.a = ALPHA_HALF;
        this.GetComponent<Image>().color = alphaControl;

        this.mainButton = this.GetComponent<Button>();
        this.mainButton.onClick.AddListener(PerformPouringAction);
    }

    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    private void Update() {
        HandlePouringTimes();
    }

    private void HandlePouringTimes() {
        if(this.state == State.Pouring) {
            if (this.timeRemaining < 0) {
                this.timerObject.SetActive(false);

                this.GetComponent<Image>().sprite = this.drink.GetPreppedSprite();
                this.state = State.Poured;
            } else {
                this.timeRemaining -= Time.deltaTime;
                this.timerObject.GetComponent<Slider>().value = (POUR_TIME - this.timeRemaining) / POUR_TIME;
            }
        }
    }

    private void SetTimerObject() {
        this.timeRemaining = POUR_TIME;
        this.timerObject.GetComponent<Slider>().value = this.timeRemaining / POUR_TIME;
        this.timerObject.SetActive(true);
    }

    private void PerformPouringAction() { 
        if(this.state == State.NoDrink) {
            this.SetTimerObject();

            this.alphaControl.a = ALPHA_FULL;
            this.GetComponent<Image>().color = this.alphaControl;
            this.state = State.Pouring;
        } else if(this.state == State.Poured) {
            if(FoodSelected(this.drink)) {
                this.alphaControl.a = ALPHA_HALF;
                this.GetComponent<Image>().color = this.alphaControl;
                this.GetComponent<Image>().sprite = this.drink.GetUnPreppedSprite();
                this.state = State.NoDrink;
            }
        }
    }

    /**** Public API ****/
    public void SetDrink(Food drink) {
        this.drink = drink;
        this.GetComponent<Image>().sprite = this.drink.GetUnPreppedSprite();
    }

    public string GetName() {
        return this.drink.GetName();
    }
}
