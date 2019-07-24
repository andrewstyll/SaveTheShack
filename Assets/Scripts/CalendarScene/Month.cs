using System;
using UnityEngine;

public class Month {
    // season
    // events array
    // name
    private MonthInfo.Months name;
    private MonthInfo.Seasons season;
    private int[] events;

    private GameObject calendarPrefab;

    public Month(MonthInfo.Months name, MonthInfo.Seasons season, GameObject calendarPrefab) {
        this.name = name;
        this.season = season;
        this.calendarPrefab = calendarPrefab;
    }

    public MonthInfo.Months GetName() {
        return this.name;
    }

    public MonthInfo.Seasons GetSeason() {
        return this.season;
    }

    public GameObject GetPrefab() {
        return this.calendarPrefab;
    }
}