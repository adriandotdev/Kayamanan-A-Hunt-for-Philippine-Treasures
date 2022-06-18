using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public string id;
    public string name;
    public int dunongPoints;
    public int remainingTime;
    public struct PlayerPosition
    {
        float x;
        float y;
        float z;
    }

    public List<RegionData> regionsData;

    public PlayerData()
    {
        this.id = null;
        this.name = null;
        this.dunongPoints = 25;
        this.remainingTime = 18000;
        this.regionsData = new List<RegionData>();

        this.regionsData.Add(
            new RegionData(
                1,
                true, 
                "REGION 1", 
                "It is more about Region 1", 
                new Category[2] { new Category("National Heroes"), new Category("National Symbols")} ));

        this.regionsData.Add(new RegionData(
            2,
            false, 
            "REGION 2", 
            "",
            new Category[1] { new Category("National Games" )} ));

        this.regionsData.Add(new RegionData(
            3,
            false,
            "REGION 3",
            "",
            new Category[1] { new Category("National Heroes") }));
        //this.regionsData.Add(new RegionData(false, "REGION 3", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 4", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 5", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 6", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 7", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 8", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 9", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 10", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 11", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 12", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 13", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 14", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 15", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 16", ""));
        //this.regionsData.Add(new RegionData(false, "REGION 17", ""));
    }
}

[System.Serializable]
public class Notebook
{

}

[System.Serializable]
public class RegionData
{
    public int regionNumber;
    public bool isOpen;
    public string regionName;
    public string information;
    public int currentScore;
    public int highestScore;

    public List<Category> categories;

    public RegionData(int regionNumber, bool isOpen, string regionName, string information, Category[] categories)
    {
        this.regionNumber = regionNumber;
        this.isOpen = isOpen;
        this.regionName = regionName;
        this.information = information;
        this.currentScore = 0;
        this.highestScore = 0;
        this.categories = new List<Category>(categories);
    }

    public RegionData()
    {
        this.isOpen = false;
        this.regionName = "";
        this.information = "";
    }
}

[System.Serializable]
public class Category
{
    public string categoryName;
    public int highestScore;
    public int noOfStars;

    public Category(string categoryName)
    {
        this.categoryName = categoryName;
        this.highestScore = 0;
        this.noOfStars = 0;
    }
}