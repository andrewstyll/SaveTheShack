using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData {
    public int restaurantType;
    public int month;
    public int totalScore;
    public int daysPassed;

    public GameData(RestaurantInfo.Types restaurantType, MonthInfo.Months month, 
                    int totalScore, int daysPassed) {
        this.restaurantType = (int)restaurantType;
        this.month = (int)month;
        this.totalScore = totalScore;
        this.daysPassed = daysPassed;
    }
}
