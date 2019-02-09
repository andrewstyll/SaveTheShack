using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainsAreaUI : MonoBehaviour {

    private const int MAX_MAINS = 3;

    private MenuBuilder menuBuilder;

    private Food main;
    [SerializeField] private GameObject mainPrefab;

    void Awake() { }

    // Start is called before the first frame update
    void Start() {
        this.menuBuilder = MenuBuilder.GetInstance();

        this.main = this.menuBuilder.GetMenu().GetMain();
        for(int i = 0; i < MAX_MAINS; i++) {
            MainUI UIScript = Instantiate(this.mainPrefab, gameObject.transform, false).GetComponent<MainUI>();
            UIScript.SetMain(this.main);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
}
