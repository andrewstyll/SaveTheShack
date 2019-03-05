using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    private enum State {
        NoFood,
        Cooking,
        Cooked,
        Burnt
    };

    [SerializeField] private GameObject timerObject;
    [SerializeField] private GameObject warningSpriteObject;

    [SerializeField] private GameObject unpreppedSpriteObject;
    [SerializeField] private GameObject preppedSpriteObject;
    [SerializeField] private GameObject burntSpriteObject;
    private Image unpreppedSprite;
    private Image preppedSprite;
    private Image burntSprite;

    private Food main;

    // control alpha for all sprites
    private Color alphaControl = Color.white;
    private const float ALPHA_HIDDEN = 0.0f;
    private const float ALPHA_HALF = 0.5f;
    private const float ALPHA_FULL = 1.0f;

    // cooking constants and time control
    private const float COOK_TIME = 5.0f;
    private const float BURN_TIME = 5.0f;
    private float timeRemaining;

    // controls for warning light
    private bool burnLightOn = false;
    private const float FADE_IN = 0.35f;
    private const float FADE_OUT = 0.15f;

    private Button mainButton;

    private State state;

    // Event to add food to serving plate
    public delegate bool PreppedOrderEvent(Food food);
    public static event PreppedOrderEvent FoodSelected;

    private void Awake() {
        this.state = State.NoFood;
        this.timerObject.SetActive(false);

        this.alphaControl.a = ALPHA_HIDDEN;
        this.warningSpriteObject.GetComponent<Image>().color = this.alphaControl;

        // sprite will not be set yet here
        this.unpreppedSprite = this.unpreppedSpriteObject.GetComponent<Image>();
        this.preppedSprite = this.preppedSpriteObject.GetComponent<Image>();
        this.burntSprite = this.burntSpriteObject.GetComponent<Image>();

        UpdateMainSprite();

        this.mainButton = this.GetComponent<Button>();
        this.mainButton.onClick.AddListener(PerformCookingAction);
    }

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    private void Update() {
        HandleCookingTimes();
    }

    private void HandleCookingTimes() {
        if (this.state == State.Cooking) {
            if (this.timeRemaining < 0) {
                this.state = State.Cooked;

                this.timeRemaining = BURN_TIME;
                this.timerObject.SetActive(false);

                this.UpdateMainSprite();
            } else {
                this.timeRemaining -= Time.deltaTime;
                this.timerObject.GetComponent<Slider>().value = (COOK_TIME - this.timeRemaining) / COOK_TIME;
                this.UpdateMainSprite();
            }
        } else if (this.state == State.Cooked) {
            if (this.timeRemaining < 0) {
                this.state = State.Burnt;

                TurnOffWarning();

                this.UpdateMainSprite();
            } else {
                // put if statement to show burn indicator
                if(!this.burnLightOn && this.timeRemaining <= BURN_TIME/2) {
                    StartCoroutine(FlickerWarning());
                }
                this.timeRemaining -= Time.deltaTime;
            }
        }
    }

    private void UpdateMainSprite() {
        if(this.state == State.NoFood) {
            this.alphaControl.a = ALPHA_HALF;
            this.unpreppedSprite.color = this.alphaControl;

            this.alphaControl.a = ALPHA_HIDDEN;
            this.preppedSprite.color = this.alphaControl;
            this.burntSprite.color = this.alphaControl;
        } else if(this.state == State.Cooking) {

            this.alphaControl.a = (float)(this.timeRemaining / COOK_TIME);
            this.unpreppedSprite.color = this.alphaControl;

            this.alphaControl.a = (float)(1.0 - (this.timeRemaining / COOK_TIME));
            this.preppedSprite.color = this.alphaControl;

        } else if(this.state == State.Cooked) {
            this.alphaControl.a = ALPHA_FULL;
            this.preppedSprite.color = this.alphaControl;


            this.alphaControl.a = ALPHA_HIDDEN;
            this.unpreppedSprite.color = this.alphaControl;
            this.burntSprite.color = this.alphaControl;
        } else if(this.state == State.Burnt) {
            this.alphaControl.a = ALPHA_FULL;
            this.burntSprite.color = this.alphaControl;

            this.alphaControl.a = ALPHA_HIDDEN;
            this.unpreppedSprite.color = this.alphaControl;
            this.preppedSprite.color = this.alphaControl;
        }
    }

    private void SetTimerObject() {
        this.timeRemaining = COOK_TIME;
        this.timerObject.GetComponent<Slider>().value = this.timeRemaining/COOK_TIME;
        this.timerObject.SetActive(true);
    }

    private void TurnOffWarning() {
        this.burnLightOn = false;
        this.alphaControl.a = ALPHA_HIDDEN;
        this.warningSpriteObject.GetComponent<Image>().color = this.alphaControl;

        StopCoroutine(FlickerWarning());
    }

    /**** OnClick ****/
    private void PerformCookingAction() {
        if (this.state == State.NoFood) {
            // set up burger to count down cooking timer
            this.SetTimerObject();

            this.state = State.Cooking;

        } else if(this.state == State.Burnt || (this.state == State.Cooked && FoodSelected(this.main))) {
            // if the burger is cooked or burnt, we have to remove it
            if(this.burnLightOn) {
                TurnOffWarning();
            }
            this.state = State.NoFood;

            this.UpdateMainSprite();
        }
    }

    /**** COROUTINES ****/
    IEnumerator FlickerWarning() {
        burnLightOn = true;
        float flickerTime = FADE_IN + FADE_OUT;
        while (this.state == State.Cooked) {
            while(this.state == State.Cooked && flickerTime >= FADE_OUT) {
                flickerTime -= Time.deltaTime;
                this.alphaControl.a = Mathf.Min(((FADE_IN + FADE_OUT) - flickerTime) / FADE_OUT, 1.0f);
                this.warningSpriteObject.GetComponent<Image>().color = this.alphaControl;
                yield return null;
            }
            while(this.state == State.Cooked && flickerTime >= 0) {
                flickerTime -= Time.deltaTime;
                this.alphaControl.a = Mathf.Max(flickerTime / FADE_IN, 0.0f);
                this.warningSpriteObject.GetComponent<Image>().color = this.alphaControl;
                yield return null;
            }
            flickerTime = FADE_IN + FADE_OUT;
            yield return null;
        }
    }

    /**** Public API ****/
    public void SetMain(Food main) {
        this.main = main;
        this.unpreppedSprite.GetComponent<Image>().sprite = this.main.GetUnPreppedSprite();
        this.preppedSprite.GetComponent<Image>().sprite = this.main.GetPreppedSprite();
        this.burntSprite.GetComponent<Image>().sprite = this.main.GetBurntSprite();
    }

    public string GetName() {
        return this.main.GetName();
    }
}