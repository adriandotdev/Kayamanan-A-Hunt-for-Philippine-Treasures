using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlayerData
{
    public bool isNewlyCreated;
    public string id;
    public string name;
    public string gender;
    public int dunongPoints;
    public int remainingTime;

    public string sceneToLoad;
    public float xPos;
    public float yPos;

    public Notebook notebook;

    public List<RegionData> regionsData;

    public PlayerData()
    {
        this.isNewlyCreated = true;
        this.id = null;
        this.name = null;
        this.gender = "male";
        this.dunongPoints = 25;
        this.remainingTime = 18000;
        this.sceneToLoad = "House";
        this.xPos = 0;
        this.yPos = 0;
        this.regionsData = new List<RegionData>();
        this.notebook = new Notebook();

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
            new Category[1] { new Category("Philippine Myths") }));
    }

    public void Copy(PlayerData playerData)
    {
        this.id = playerData.id;
        this.name = playerData.name;
        this.gender = playerData.gender;
        this.dunongPoints = playerData.dunongPoints;
        this.remainingTime = playerData.remainingTime;
        this.regionsData = playerData.regionsData;
        this.notebook = playerData.notebook;
    }
}

[System.Serializable]
public class Notebook
{
    public List<Collectible> collectibles;
    private string NATIONAL_HEROES = "National Heroes";
    private string NATIONAL_SYMBOLS = "National Symbols";
    private string NATIONAL_GAMES = "National Games";
    private string NATIONAL_FESTIVALS = "National Festivals";
    private string PHILIPPINE_MYTH = "Philippine Myth";
    public Notebook()
    {
        this.collectibles = new List<Collectible>();

        this.collectibles.Add(new Collectible("Collectibles/C1", this.NATIONAL_HEROES, "Region 1"));
        this.collectibles.Add(new Collectible("Collectibles/C2", this.NATIONAL_SYMBOLS, "Region 1"));
        this.collectibles.Add(new Collectible("Collectibles/C3", this.NATIONAL_SYMBOLS, "Region 2"));
        this.collectibles.Add(new Collectible("Collectibles/C4", this.NATIONAL_SYMBOLS, "Region 3"));
    }
}

[System.Serializable]
public class Collectible
{
    public string imagePath;
    public bool isCollected;
    public string categoryName;
    public string regionName;

    public Collectible(string imagePath, string categoryName, string regionName)
    {
        this.imagePath = imagePath;
        this.categoryName = categoryName;
        this.regionName = regionName;
        this.isCollected = false;
    }
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