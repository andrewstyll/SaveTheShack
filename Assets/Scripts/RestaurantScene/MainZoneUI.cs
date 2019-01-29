using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainZoneUI : MonoBehaviour {

    private enum MainState {
        NoFood,
        Cooking,
        Cooked,
        OverDone
    };

    [SerializeField] private Slider timer;

    private Color burgerColor;
    private const float ALPHA_NO_FOOD = 0.5f;

    // replace with images later
    private Color baseColor = new Color(139/255.0f, 69/255.0f, 19/255.0f);
    private Color burntColor = new Color(68/255.0f, 34/255.0f, 10/255.0f);

    private Color currentColor;
    private Button burgerButton;
    private MainState state;

    void Awake() {
        state = MainState.NoFood;
        timer.enabled = false;

        currentColor = baseColor;
        currentColor.a = ALPHA_NO_FOOD;
        this.GetComponent<Image>().color = currentColor;

        burgerButton = this.GetComponent<Button>();
        burgerButton.onClick.AddListener(PerformBurgerAction);
    }

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    void PerformBurgerAction() {
        if (state == MainState.NoFood) {
            // set up burger to count down cooking timer
            timer.enabled = true;
            currentColor.a = 1.0f;
            this.GetComponent<Image>().color = currentColor;
            state = MainState.Cooked;

        } else if(state == MainState.Cooked || state == MainState.OverDone) {
            // if the burger is cooked or burnt, we have to remove it
            timer.enabled = false;
             currentColor = baseColor;
            currentColor.a = ALPHA_NO_FOOD;
            this.GetComponent<Image>().color = currentColor;

            if(state == MainState.Cooked) {
                // call listener to add it to the serve tray?
            }
            state = MainState.NoFood;
        }
    }

    /**** Public API ****/
    public void SetImage(Sprite sprite) {
        this.GetComponent<Image>().sprite = sprite;
    }

    public void setTimer(float time) {
        this.timer.value = time;
    }
}