using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {

    [SerializeField] private GameObject gameManagerObj;
    // Start is called before the first frame update
    private void Awake() {
        if(GameManager.GetInstance() == null) {
            Instantiate(gameManagerObj);
        }
    }
}