using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestaurantSpriteLoader : MonoBehaviour {

    private GameManager gameManager;
    private ConfigSetup config;
    private MonthBuilder monthBuilder;

    [SerializeField] private Image backgroundWindow;
    [SerializeField] private Image foodStation;
    [SerializeField] private Image mainsArea;
    [SerializeField] private Image toppingsArea;
    [SerializeField] private Image drinksArea;
    [SerializeField] private Image preppedOrderArea;

    private Dictionary<MonthInfo.Seasons, string> seasonalBackgroundSpritePaths;

    private void Awake() {
        this.gameManager = GameManager.GetInstance();
        this.config = ConfigSetup.GetInstance();
    }

    // Start is called before the first frame update
    private void Start() {
        if (this.gameManager != null && this.config != null && this.monthBuilder != null) {
            InitBackgroundSetup();
        } else {
            StartCoroutine("WaitForGameManagerAndConfig");
        }
    }

    private void InitBackgroundSetup() {
        FillSpriteDictionary();
        SetSprites();
    }

    private void FillSpriteDictionary() {
        seasonalBackgroundSpritePaths = new Dictionary<MonthInfo.Seasons, string>();
        JsonToRestSpriteContainer spriteDataList = this.config.GetRestaurantSpriteData();
        string spritePath = spriteDataList.SpriteLocation;
        MonthInfo.Seasons season = MonthInfo.Seasons.NONE;

        foreach (JsonToRestSprite spriteData in spriteDataList.Sprites) {
            switch(spriteData.SpriteName) {
                case "Window_Plain":
                    season = MonthInfo.Seasons.NONE;
                    break;
                case "Window_Spring":
                    season = MonthInfo.Seasons.SPRING;
                    break;
                case "Window_Summer":
                    season = MonthInfo.Seasons.SUMMER;
                    break;
                case "Window_Fall":
                    season = MonthInfo.Seasons.FALL;
                    break;
                case "Window_Winter":
                    season = MonthInfo.Seasons.WINTER;
                    break;
            }
            seasonalBackgroundSpritePaths.Add(season, spritePath + spriteData.SpriteName);
        }
    }

    private void SetSprites() {
        MonthInfo.Months month = this.gameManager.GetMonth();
        MonthInfo.Seasons season = this.monthBuilder.GetMonthSeason(month);
        backgroundWindow.sprite = Resources.Load<Sprite>(seasonalBackgroundSpritePaths[season]);
    }

    /**** Coroutine ****/
    IEnumerator WaitForGameManagerAndConfig() {
        while (this.gameManager == null || this.config == null || this.monthBuilder == null) {
            this.gameManager = GameManager.GetInstance();
            this.config = ConfigSetup.GetInstance();
            this.monthBuilder = MonthBuilder.GetInstance();
            yield return null;
        }
        InitBackgroundSetup();
    }

}
