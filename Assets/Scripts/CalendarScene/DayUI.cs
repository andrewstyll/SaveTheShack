using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayUI : MonoBehaviour {

    private Button button;
    private Image image;

    [SerializeField] private Sprite pastSprite;
    [SerializeField] private Sprite currentSprite;
    [SerializeField] private Text textPromt;

    private void Awake() {
        this.button = gameObject.GetComponent<Button>();
        this.image = gameObject.GetComponent<Image>();

        this.button.onClick.AddListener(StartDay);
    }

    // Start is called before the first frame update
    private void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    /**** Events ****/
    private void StartDay() {
        Debug.Log("Hi");
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
}
