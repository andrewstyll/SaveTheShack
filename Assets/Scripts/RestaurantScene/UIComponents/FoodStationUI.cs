using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodStationUI : MonoBehaviour {

    private bool isLoadedSent = false;

    private bool mainsUILoaded = false;
    private bool toppingsUILoaded = false;
    private bool drinksUILoaded = false;
    private bool preppedUILoaded = false;
    private bool trashUILoaded = false;

    public delegate void FoodStationUINotification();
    public static event FoodStationUINotification Loaded;

    private void Awake() {
        MainsAreaUI.Loaded += MainsUILoaded;
        ToppingAreaUI.Loaded += ToppingsUILoaded;
        DrinksAreaUI.Loaded += DrinksUILoaded;
        PreppedOrderUI.Loaded += PreppedUILoaded;
        TrashUI.Loaded += TrashUILoaded;
    }

    private void Update() {
        if (!this.isLoadedSent && IsLoaded()) {
            this.isLoadedSent = true;
            Loaded();
        }
    }

    private void OnDestroy() {
        MainsAreaUI.Loaded -= MainsUILoaded;
        ToppingAreaUI.Loaded -= ToppingsUILoaded;
        DrinksAreaUI.Loaded -= DrinksUILoaded;
        PreppedOrderUI.Loaded -= PreppedUILoaded;
        TrashUI.Loaded -= TrashUILoaded;
    }

    private void MainsUILoaded() {
        this.mainsUILoaded = true;
    }

    private void ToppingsUILoaded() {
        this.toppingsUILoaded = true;
    }

    private void DrinksUILoaded() {
        this.drinksUILoaded = true;
    }

    private void PreppedUILoaded() {
        this.preppedUILoaded = true;
    }

    private void TrashUILoaded() {
        this.trashUILoaded = true;
    }

    private bool IsLoaded() {
        return (mainsUILoaded && toppingsUILoaded && drinksUILoaded && preppedUILoaded && trashUILoaded);
    }
}
