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

    public List<Quest> quests;
    public List<Quest> currentQuests;

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
        this.quests = new List<Quest>();
        this.currentQuests = new List<Quest>();

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

        // These are the quests that the user can get (Main Quests for each Region)
        this.quests.Add(new Quest("Known All Heroes", "Talk to Mang Esterlito", 25, "Region 1", new TalkGoal("Mang Esterlito")));
        this.quests.Add(new Quest("Talk to your MILF", "Talk to Aling Nena", 30, "Region 1", new TalkGoal("Aling Nena")));
        this.quests.Add(new Quest("Best Friends", "Talk to Aling Missy", 30, "Region 2", new TalkGoal("Aling Nena")));
        this.quests.Add(new Quest("Gather Goal", "This is gather goal", 45, "Region 2", new GatherGoal()));
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

        this.collectibles.Add(new Collectible("Jose Rizal", "Collectibles/Rizal", this.NATIONAL_HEROES, "Region 1"));
        this.collectibles.Add(new Collectible("Anahaw", "Collectibles/Anahaw", this.NATIONAL_SYMBOLS, "Region 1"));
        this.collectibles.Add(new Collectible("Andres Bonifacio", "Collectibles/Bonifacio", this.NATIONAL_SYMBOLS, "Region 2"));
        this.collectibles.Add(new Collectible("Kalabaw", "Collectibles/Kalabaw", this.NATIONAL_SYMBOLS, "Region 3"));
    }
}

[System.Serializable]
public class Collectible
{
    public string name;
    public string imagePath;
    public bool isCollected;
    public string categoryName;
    public string regionName;

    public Collectible(string name, string imagePath, string categoryName, string regionName)
    {
        this.name = name;
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