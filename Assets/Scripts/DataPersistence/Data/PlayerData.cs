using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class PlayerData 
{
    public string id;
    public bool isNewlyCreated;
    public bool isTutorialDone;
    public bool isIntroductionDone;
    public string name;
    public string gender;
    public int dunongPoints;
    public int requiredDunongPointsToPlay;
    public int remainingTime;

    public string sceneToLoad;
    public float xPos;
    public float yPos;

    public Notebook notebook;

    public List<RegionData> regionsData;

    public List<Quest> quests;
    public List<Quest> currentQuests;
    public List<Quest> completedQuests;
    public Inventory inventory;
    public PlayerTime playerTime;

    // LIST OF REGIONS FOR LUZON
    const string REGION_1 = "Ilocos Region";
    const string REGION_2 = "Cagayan Valley";
    const string CAR = "Cordillera Administrative Region";
    const string REGION_3 = "Central Luzon";
    const string REGION_4A = "CALABARZON";
    const string MIMAROPA = "MIMAROPA";
    const string REGION_5 = "Bicol Region";
    const string NCR = "National Capital Region";

    // LIST OF CATEGORIES
    const string HEROES = "National Heroes";
    const string FESTIVALS = "National Festivals";
    const string TOURIST_ATTRACTIONS = "Tourist Attractions";
    const string GENERAL_KNOWLEDGE = "General Knowledge";

    public PlayerData()
    {
        this.isNewlyCreated = true;
        this.isTutorialDone = false;
        this.isIntroductionDone = false;
        this.id = null;
        this.name = null;
        this.gender = "male";
        this.dunongPoints = 0;
        this.requiredDunongPointsToPlay = 5;
        this.remainingTime = 18000;
        this.sceneToLoad = "House";
        this.xPos = 0;
        this.yPos = 0;
        this.regionsData = new List<RegionData>();
        this.notebook = new Notebook();
        this.quests = new List<Quest>();
        this.currentQuests = new List<Quest>();
        this.completedQuests = new List<Quest>();
        this.inventory = new Inventory();
        this.playerTime = new PlayerTime();

        this.AddRegionsForLuzon();

        // These are the quests that the user can get (Main Quests for each Region)
        this.quests.Add(new Quest("Known All Heroes", "Talk to Mang Esterlito", 5, REGION_1, new TalkGoal("Mang Esterlito")));
        this.quests.Add(new Quest("Talk to your Mother", "Talk to Aling Nena", 5, REGION_2, new TalkGoal("Aling Nena")));

        this.quests.Add(new Quest("Buko Pie Ayayay!", "Get the Buko Pie from Aling Marites and give it to Mang Esterlito", 5, CAR,
            new DeliveryGoal("Aling Marites", "Mang Esterlito", "Can you give this Buko Pie to Mang Esterlito?",
            new Item("Buko Pie", "", false))));

        this.quests.Add(new Quest("Mango Aloho!", "Help Aling Julia to give Aling Marites a Mango", 5, REGION_3,
            new DeliveryGoal("Aling Julia", "Aling Marites", "Hey! Would you mind if you give this to Aling Marites?",
            new Item("Mango", "", false))));

        this.quests.Add(new Quest("Talk to Aling Julia", "Talk to Aling Julia", 5, REGION_4A, new TalkGoal("Aling Julia")));

        this.quests.Add(new Quest("Talk to Aling Marites", "Talk to Aling Marites", 5, MIMAROPA, new TalkGoal("Aling Marites")));

        this.quests.Add(new Quest("Mango Aloho!", "Help Aling Julia to give Aling Marites a Mango", 5, REGION_5,
            new DeliveryGoal("Aling Nena", "Aling Marites", "Hey! Would you mind if you give this to Aling Marites?",
            new Item("Mango", "", false))));

        this.quests.Add(new Quest("Mango Aloho!", "Help Aling Julia to give Aling Marites a Mango", 5, NCR,
            new DeliveryGoal("Mang Esterlito", "Aling Marites", "Hey! Would you mind if you give this to Aling Marites?",
            new Item("Mango", "", false))));
        //this.quests.Add(new Quest("Gumamela Pula!", "Help Mang Esterlito give a gumamela to Aling Nena", 5, REGION_1,
        //    new DeliveryGoal("Mang Esterlito", "Aling Nena", "Hey! Would you mind if you give this to ur mother?",
        //    new Item("Gumamela", "", false))));

        //this.quests.Add(new Quest("Talk to Melchora", "Talk to Aling Melchora", 10, REGION_2, new TalkGoal("Aling Melchora")));
    }

    public void AddRegionsForLuzon()
    {
        this.regionsData.Add(
            new RegionData(
                1,
                true,
                REGION_1,
                "Ilocos is a region in the Philippines, encompassing the northwestern coast of Luzon island. It’s " +
                "known for its historic sites, beaches and the well-preserved Spanish colonial city of Vigan. " +
                "Dating from the 16th century, Vigan’s Mestizo district is characterized by cobblestone streets and " +
                "mansions with wrought-iron balconies. Farther north, Laoag City is a jumping-off point for the huge La Paz Sand Dunes.",
                new Category[2] { new Category(HEROES), new Category(FESTIVALS) }));

        this.regionsData.Add(new RegionData(
                2,
                false,
                REGION_2,
                "Cagayan Valley, designated as Region II, is an administrative region in the Philippines, " +
                "located in the northeastern section of Luzon Island. It is composed of five Philippine " +
                "provinces: Batanes, Cagayan, Isabela, Nueva Vizcaya, and Quirino.",
                new Category[1] { new Category(TOURIST_ATTRACTIONS) }));

        this.regionsData.Add(new RegionData(
                3,
                false,
                CAR,
                "The Cordillera Administrative Region, also known as the Cordillera Region, or simply, Cordillera, " +
                "is an administrative region in the Philippines, situated within the island of Luzon.",
                new Category[3] { new Category(HEROES), new Category(FESTIVALS), new Category(GENERAL_KNOWLEDGE) }));

        this.regionsData.Add(new RegionData(
                4,
                false,
                REGION_3,
                "",
                new Category[2] { new Category(FESTIVALS), new Category(GENERAL_KNOWLEDGE) }));

        this.regionsData.Add(new RegionData(
                5,
                false,
                REGION_4A,
                "",
                new Category[2] { new Category(HEROES), new Category(FESTIVALS) }));

        this.regionsData.Add(new RegionData(
                6,
                false,
                MIMAROPA,
                "",
                new Category[2] { new Category(FESTIVALS), new Category(TOURIST_ATTRACTIONS) }));

        this.regionsData.Add(new RegionData(
                7,
                false,
                REGION_5,
                "",
                new Category[1] { new Category(HEROES) }));

        this.regionsData.Add(new RegionData(
                8,
                false,
                NCR,
                "",
                new Category[2] { new Category(HEROES), new Category(FESTIVALS) }));
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

    // LIST OF REGIONS FOR LUZON
    const string REGION_1 = "Ilocos Region";
    const string REGION_2 = "Cagayan Valley";
    const string CAR = "Cordillera Administrative Region";
    const string REGION_3 = "Central Luzon";
    const string REGION_4A = "CALABARZON";
    const string MIMAROPA = "MIMAROPA";
    const string REGION_5 = "Bicol Region";
    const string NCR = "National Capital Region";

    public Notebook()
    {
        this.collectibles = new List<Collectible>();

        this.collectibles.Add(new Collectible("Jose Rizal", "Collectibles/Rizal", this.NATIONAL_HEROES, REGION_1));
        this.collectibles.Add(new Collectible("Anahaw", "Collectibles/Anahaw", this.NATIONAL_SYMBOLS, REGION_2));
        this.collectibles.Add(new Collectible("Andres Bonifacio", "Collectibles/Bonifacio", this.NATIONAL_SYMBOLS, CAR));
        this.collectibles.Add(new Collectible("Kalabaw", "Collectibles/Kalabaw", this.NATIONAL_SYMBOLS, REGION_3));
        this.collectibles.Add(new Collectible("Kalabaw", "Collectibles/Kalabaw", this.NATIONAL_SYMBOLS, REGION_4A));
        this.collectibles.Add(new Collectible("Jose Rizal", "Collectibles/Rizal", this.NATIONAL_HEROES, MIMAROPA));
        this.collectibles.Add(new Collectible("Anahaw", "Collectibles/Anahaw", this.NATIONAL_SYMBOLS, REGION_5));
        this.collectibles.Add(new Collectible("Andres Bonifacio", "Collectibles/Bonifacio", this.NATIONAL_SYMBOLS, NCR));
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
    public int noOfStars;

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

[System.Serializable]
public class Inventory
{
    public List<Item> items;
    private int MAX_SLOT = 7;
    
    public Inventory()
    {
        this.items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if (IsItemExisting(item))
        {
            this.UpdateItem(item);
        }
        else
        {
            if (items.Count != MAX_SLOT)
                this.items.Add(item);
        }
    }

    public void UpdateItem(Item item)
    {
        foreach (Item itemLoop in this.items)
        {
            if (itemLoop.itemName.ToUpper() == item.itemName.ToUpper())
            {
                itemLoop.quantity += item.quantity;
                return;
            }
        }
    }

    public bool IsItemExisting(Item item)
    {
        foreach (Item itemLoop in this.items)
        {
            if (itemLoop.itemName.ToUpper() == item.itemName.ToUpper())
            {
                return true;
            }
        }
        return false;
    }
}

[System.Serializable]
public class PlayerTime 
{
    public int m_ActualHourInRealLife;
    public bool m_IsDaytime;
    public bool m_IsAllEstablishmentsOpen;

    public PlayerTime()
    {
        this.m_ActualHourInRealLife = 8;
        this.m_IsDaytime = true;
        this.m_IsAllEstablishmentsOpen = true;
    }
}