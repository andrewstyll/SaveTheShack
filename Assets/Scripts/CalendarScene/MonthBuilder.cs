using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class MonthBuilder {
    private static readonly MonthBuilder instance = new MonthBuilder();
    private ConfigSetup config;

    private JsonMonthContainer monthData;
    private Dictionary<MonthInfo.Months, Month> monthPrefabs;

    MonthBuilder() {
        this.config = ConfigSetup.GetInstance();
        this.monthData = this.config.GetMonthData();
        GetMonthPrefabs();
    }

    private void GetMonthPrefabs() {
        monthPrefabs = new Dictionary<MonthInfo.Months, Month>();
        string monthPrefabPath = this.monthData.PrefabLocation;

        foreach(JsonToMonth month in this.monthData.Months) {
            MonthInfo.Months monthShortcut;
            MonthInfo.Seasons seasonShortcut;

            GameObject monthPrefab = (GameObject)Resources.Load(monthPrefabPath + month.Name);

            switch(month.Name) {
                case "January":
                    monthShortcut = MonthInfo.Months.JAN;
                    break;
                case "February":
                    monthShortcut = MonthInfo.Months.FEB;
                    break;
                case "March":
                    monthShortcut = MonthInfo.Months.MAR;
                    break;
                case "April":
                    monthShortcut = MonthInfo.Months.APR;
                    break;
                case "May":
                    monthShortcut = MonthInfo.Months.MAY;
                    break;
                case "June":
                    monthShortcut = MonthInfo.Months.JUN;
                    break;
                case "July":
                    monthShortcut = MonthInfo.Months.JUL;
                    break;
                case "August":
                    monthShortcut = MonthInfo.Months.AUG;
                    break;
                case "September":
                    monthShortcut = MonthInfo.Months.SEPT;
                    break;
                case "October":
                    monthShortcut = MonthInfo.Months.OCT;
                    break;
                case "November":
                    monthShortcut = MonthInfo.Months.NOV;
                    break;
                case "December":
                    monthShortcut = MonthInfo.Months.DEC;
                    break;
                default:
                    throw new System.Exception("Invalid month name passed");
            }

            if(monthShortcut == MonthInfo.Months.DEC || monthShortcut == MonthInfo.Months.JAN 
                || monthShortcut == MonthInfo.Months.FEB ) {
                seasonShortcut = MonthInfo.Seasons.WINTER;
            } else if (monthShortcut == MonthInfo.Months.MAR || monthShortcut == MonthInfo.Months.APR
                || monthShortcut == MonthInfo.Months.MAY) {
                seasonShortcut = MonthInfo.Seasons.SPRING;
            } else if (monthShortcut == MonthInfo.Months.JUN || monthShortcut == MonthInfo.Months.JUL
                || monthShortcut == MonthInfo.Months.AUG) {
                seasonShortcut = MonthInfo.Seasons.SUMMER;
            } else if (monthShortcut == MonthInfo.Months.SEPT || monthShortcut == MonthInfo.Months.OCT
                || monthShortcut == MonthInfo.Months.NOV) {
                seasonShortcut = MonthInfo.Seasons.FALL;
            } else {
                seasonShortcut = MonthInfo.Seasons.NONE;
            }
            monthPrefabs.Add(monthShortcut, new Month(monthShortcut, seasonShortcut, monthPrefab));
        }
    }

    /**** Public API ****/
    public static MonthBuilder GetInstance() {
        return instance;
    }

    public GameObject GetMonthPrefab(MonthInfo.Months month) {
        return monthPrefabs[month].GetPrefab();
    }
}