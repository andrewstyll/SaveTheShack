﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToppingUI : MonoBehaviour {


    [SerializeField] private Sprite disabledSprite;

    private Food topping;

    private bool disabled = true;
    private Sprite sprite;
    private Button toppingButton;

    public delegate bool EventHandler(Food food);
    public static event EventHandler FoodSelected;

    private void Awake() {
        this.GetComponent<Image>().sprite = this.disabledSprite;
        this.toppingButton = this.GetComponent<Button>();
        this.toppingButton.onClick.AddListener(AddTopping);
    }

    // Start is called before the first frame update
    private void Start() { }

    // Update is called once per frame
    private void Update() { }

    private void SetSprite(Sprite newSprite) {
        if(newSprite != null) {
            this.sprite = newSprite;
            this.GetComponent<Image>().sprite = this.sprite;
            this.disabled = false;
        } else {
            Debug.Log("Adding null sprite");
        }
    }

    private void AddTopping() {
        // add an event that will be picked up by the serving area
        if (!this.disabled) {

            // returns boolean, but doesn't matter in this case
            FoodSelected(this.topping);
        }
    }

    /*** PUBLIC API ***/
    public void SetTopping(Food topping) {
        this.topping = topping;
        SetSprite(this.topping.GetPreppedSprite());
    }

    public string GetName() {
        return this.topping.GetName();
    }

}
