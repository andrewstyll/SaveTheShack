using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModalButtonUI : MonoBehaviour {

    private Text textOne;
    private Text textTwo;

    [SerializeField] private GameObject buttonOne;
    [SerializeField] private GameObject buttonTwo;

    // Start is called before the first frame update
    private void Awake() {
        ModalUI.ButtonControl += ButtonControlRecieve;
        this.textOne = buttonOne.GetComponentInChildren<Text>();
        this.textTwo = buttonTwo.GetComponentInChildren<Text>();
    }

    /**** Events ****/
    private void ButtonControlRecieve(bool oneOn, bool twoOn, string textOne, string textTwo) {
        Debug.Log("PrintMe");
        if(oneOn) {
            buttonOne.SetActive(true);
            this.textOne.text = textOne;
        } else {
            buttonOne.SetActive(false);
        }
        if (twoOn) {
            buttonTwo.SetActive(true);
            this.textTwo.text = textTwo;
        } else {
            buttonTwo.SetActive(false);
        }
    }

}
