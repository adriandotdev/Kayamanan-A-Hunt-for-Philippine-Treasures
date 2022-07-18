using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance;
    public List<IDataPersistence> dataPersistenceObjects;
    public PlayerData playerData;
    public Slots slots;

    public PlayerDataHandler playerDataHandler;
    public SlotsFileHandler slotsHandler;
    public GameObject confirmButton;
    private TMPro.TMP_InputField inputField;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        print("Data Persistence On Enable");
        SceneManager.sceneLoaded += MenuSceneLoaded;
        SceneManager.sceneLoaded += CharacterCreationSceneLoaded;
        SceneManager.sceneLoaded += HouseSceneLoaded;
        SceneManager.sceneLoaded += PhilippineMapSceneLoaded;
        SceneManager.sceneLoaded += OutsideSceneLoaded;
        //SceneManager.sceneLoaded += WordSceneLoaded;
        //SceneManager.sceneLoaded += AssessmentSceneLoaded;
    }

    private void OnDisable()
    {
        print("Data Persistence On Disable");
        SceneManager.sceneLoaded -= MenuSceneLoaded;
        SceneManager.sceneLoaded -= CharacterCreationSceneLoaded;
        SceneManager.sceneLoaded -= HouseSceneLoaded;
        SceneManager.sceneLoaded -= PhilippineMapSceneLoaded;
        SceneManager.sceneLoaded -= OutsideSceneLoaded;
        //SceneManager.sceneLoaded -= WordSceneLoaded;
        //SceneManager.sceneLoaded -= AssessmentSceneLoaded;
    }

    /** All registered function to SceneManager delegates. */
    public void MenuSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            DataPersistenceManager.instance.playerData = null;
        }
    }

    private void CharacterCreationSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Kunin lahat ng nag implement ng IDataPersistence na interface.
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();

        // If nag loaded na ang 'CharacterCreation' Scene
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("CharacterAndLoad"))
        {
            this.inputField = GameObject.Find("Character Name TXT").GetComponent<TMPro.TMP_InputField>();
            // Create o mag instantiate ng new data.
            this.playerData = new PlayerData();
            this.LoadGame(); // i-load sa lahat ng nag implement ng IDataPersistence na interface.
        }
    }

    private void HouseSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {
            this.LoadGame();
        }
    }

    private void OutsideSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside"))
        {
            this.LoadGame();
        }
    }

    private void PhilippineMapSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Philippine Map"))
        {
            this.LoadGame();
        }
    }

    public void ConfirmCharacter()
    {
        string id = Guid.NewGuid().ToString(); // Generate a new ID.

        // Instantiate the playerDataHandler which handles to create a file for a new created player profile
        // and slotsHandler which creates or override the slot.txt file.
        this.playerDataHandler = new PlayerDataHandler(id); 
        this.slotsHandler = new SlotsFileHandler();

        this.playerData.id = id; // Set the id to the playerData's id.

        this.slots = slotsHandler.Load();

        this.slots.ids.Add(id);

        slotsHandler.Save(this.slots);

        this.SaveGame();
    }

    public void LoadGame()
    {
        // Load Any Data From File IF ID OF PLAYER DATA IS NOT NULL
        foreach(IDataPersistence dp in this.dataPersistenceObjects)
        {
            dp.LoadPlayerData(this.playerData);
        }
    }

    public void SaveGame()
    {
        if (this.playerData.id != null)
        {
            this.playerDataHandler = new PlayerDataHandler(this.playerData.id);

            this.playerDataHandler.Save(this.playerData);

            this.playerDataHandler = null;
        }
    }

    public List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public Slots GetAllSlots()
    {
        this.slotsHandler = new SlotsFileHandler();

        this.slots = slotsHandler.Load();

        return this.slots;
    }

    private void OnApplicationQuit()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("CharacterCreation"))
        {
            
        }
    }
}

public class SlotsFileHandler  
{
    public Slots Load()
    {
        if (!File.Exists(Application.persistentDataPath + "/slots.txt"))
        {
            string slotsInJson = JsonUtility.ToJson(new Slots());
            File.WriteAllText(Application.persistentDataPath + "/slots.txt", slotsInJson);
        }

        string result = File.ReadAllText(Application.persistentDataPath + "/slots.txt");
        Slots slots = JsonUtility.FromJson<Slots>(result);

        return slots;
    }

    public void Save(Slots slots)
    {
        string slotsInJson = JsonUtility.ToJson(slots);

        File.WriteAllText(Application.persistentDataPath + "/slots.txt", slotsInJson);
    }
}

public class PlayerDataHandler
{
    public string id;

    public PlayerDataHandler(string id)
    {
        this.id = id;
    }

    public PlayerData Load()
    {
        string json = File.ReadAllText(Application.persistentDataPath + "/" + id + ".txt");
        PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);

        return playerData;
    }

    public void Save(PlayerData playerData) 
    {
        string json = JsonUtility.ToJson(playerData, true);

        File.WriteAllText(Application.persistentDataPath + "/" + id + ".txt", json);
    }
}