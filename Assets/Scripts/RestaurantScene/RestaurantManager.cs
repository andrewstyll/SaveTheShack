using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestaurantManager : MonoBehaviour {

    private MenuBuilder menuBuilder;
    private RestaurantInfo.Types currentType = RestaurantInfo.Types.Taco;

    // access singleton in awake
    private void Awake() {
        // assume we have recieved a type from somewhere, set manu with it
        menuBuilder = MenuBuilder.GetInstance();
        menuBuilder.BuildMenu(currentType);
    }

    // Start is called before the first frame update
    private void Start() {

    }

    // Update is called once per frame
    private void Update() {
        
    }
}
