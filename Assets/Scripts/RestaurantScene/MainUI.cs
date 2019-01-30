using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    private enum MainState {
        NoFood,
        Cooking,
        Cooked,
        OverDone
    };

    [SerializeField] private GameObject timerObject;

    [SerializeField] private Sprite burntSprite;
    [SerializeField] private Sprite baseSprite; // this needs to be set from it's parent, can't be a variable
    private Color baseColor = Color.white;
    private const float ALPHA_NO_FOOD = 0.5f;
    private const float ALPHA_FOOD = 1.0f;

    private Button burgerButton;

    private MainState state;

    void Awake() {
        state = MainState.NoFood;
        timerObject.SetActive(false);

        // sprite will not be set yet here
        baseColor.a = ALPHA_NO_FOOD;
        this.GetComponent<Image>().color = baseColor;

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
            timerObject.SetActive(true);

            baseColor.a = ALPHA_FOOD;
            this.GetComponent<Image>().color = baseColor;
            state = MainState.Cooked;

        } else if(state == MainState.Cooked || state == MainState.OverDone) {
            // if the burger is cooked or burnt, we have to remove it
            timerObject.SetActive(false);
            
            baseColor.a = ALPHA_NO_FOOD;
            this.GetComponent<Image>().color = baseColor;
            this.GetComponent<Image>().sprite = baseSprite;

            if (state == MainState.Cooked) {
                // call listener to add it to the serve tray?
            }
            state = MainState.NoFood;
        }
    }

    /**** Public API ****/
    public void SetBaseImage(Sprite sprite) {
        baseSprite = sprite;
    }

    public void setTimer(float time) {
        this.timerObject.GetComponent<Slider>().value = time;
    }
}