using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CustomerUIImageSelect : MonoBehaviour {

    private ConfigSetup configData;

    private Image customerImage;
    private List<string> customerSpritePaths;

    private void Awake() {
        this.configData = ConfigSetup.GetInstance();
        this.customerImage = gameObject.GetComponent<Image>();
    }

    // Start is called before the first frame update
    private void Start() {
        AddSpritePaths();
        SelectAndLoadSprite();
    }

    private void AddSpritePaths() {
        customerSpritePaths = new List<string>();
        JsonToCustomerSpriteContainer spriteData = this.configData.GetCustomerSpriteData();
        string spritePath = spriteData.SpriteLocation;
        foreach(JsonToCustomerSprite spriteNameObj in spriteData.Sprites) {
            customerSpritePaths.Add(spritePath + spriteNameObj.SpriteName);
        }
    }

    private void SelectAndLoadSprite() {
        string selectedSprite = customerSpritePaths[Random.Range(0, customerSpritePaths.Count)];
        customerImage.sprite = Resources.Load<Sprite>(selectedSprite);
    }
}
