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

    private Food main;

    private Sprite baseSprite; // this needs to be set from it's parent, can't be a variable
    private Color baseColor = Color.white;
    private const float ALPHA_NO_FOOD = 0.5f;
    private const float ALPHA_FOOD = 1.0f;

    private Button mainButton;

    private MainState state;

    private void Awake() {
        this.state = MainState.NoFood;
        this.timerObject.SetActive(false);

        // sprite will not be set yet here
        this.baseColor.a = ALPHA_NO_FOOD;
        this.GetComponent<Image>().color = baseColor;

        this.mainButton = this.GetComponent<Button>();
        this.mainButton.onClick.AddListener(PerformCookingAction);
    }

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    private void SetSprite(Sprite sprite) {
        this.baseSprite = sprite;
        this.GetComponent<Image>().sprite = this.baseSprite;
    }

    private void SetTimer(float time) {
        this.timerObject.GetComponent<Slider>().value = time;
    }

    private void PerformCookingAction() {
        if (this.state == MainState.NoFood) {
            // set up burger to count down cooking timer
            this.timerObject.SetActive(true);

            this.baseColor.a = ALPHA_FOOD;
            this.GetComponent<Image>().color = this.baseColor;
            this.state = MainState.Cooked;

        } else if(this.state == MainState.Cooked || this.state == MainState.OverDone) {
            // if the burger is cooked or burnt, we have to remove it
            this.timerObject.SetActive(false);
            
            this.baseColor.a = ALPHA_NO_FOOD;
            this.GetComponent<Image>().color = this.baseColor;
            this.GetComponent<Image>().sprite =this.baseSprite;

            if (this.state == MainState.Cooked) {
                // add an event that will be picked up by the serving area
            }
            this.state = MainState.NoFood;
        }
    }

    /**** Public API ****/
    public void SetMain(Food main) {
        this.main = main;
        this.SetSprite(this.main.GetSprite());
        // call this.SetTimer when implemented
    }

    public string GetName() {
        return this.main.GetName();
    }
}