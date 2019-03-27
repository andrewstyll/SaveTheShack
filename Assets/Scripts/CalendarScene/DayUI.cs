using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayUI : MonoBehaviour {

    private Button button;
    private Image image;

    private GameObject modal;
    [SerializeField] private GameObject modalPrefab;

    [SerializeField] private Sprite pastSprite;
    [SerializeField] private Sprite currentSprite;

    public delegate void ModalEvent(ModalUI.ModalState state, string displayString);
    public static ModalEvent SelectDayEvent;

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

    private void ShowModal() {
        if (this.modal == null) {
            this.modal = Instantiate(this.modalPrefab, this.transform, false);
        }
    }

    /**** Events ****/
    private void SelectDay() {
        ShowModal();
        SelectDayEvent(ModalUI.ModalState.DaySelect, "");
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
