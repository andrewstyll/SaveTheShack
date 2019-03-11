using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashUI : MonoBehaviour {

    private Button trashButton;

    // Events
    public delegate void EventHandler();
    public static event EventHandler TrashClicked;

    public delegate void PreppedOrderAreaUINotification();
    public static event PreppedOrderAreaUINotification Loaded;

    private void Awake() {
        this.trashButton = this.GetComponent<Button>();
        this.trashButton.onClick.AddListener(OnTrashClick);

    }

    // Start is called before the first frame update
    void Start() {
        Loaded();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTrashClick() {
        TrashClicked();
    }
}
