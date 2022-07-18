using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour, IDataPersistence
{
    public static QuestManager instance;
    //public List<Quest> quests;

    public GameObject questPrefab;
    private GameObject questAlertBox;

    public PlayerData playerData;

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

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnHouseSceneLoaded;
        SceneManager.sceneLoaded += OnOutsideSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnHouseSceneLoaded;
        SceneManager.sceneLoaded -= OnOutsideSceneLoaded;
    }

    
    void OnHouseSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {
            this.GetAllNecessaryGameObjects();
            this.GetListOfQuests();
        }
    }

    void OnOutsideSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside"))
        {
            this.GetAllNecessaryGameObjects();
            this.GetListOfQuests();
        }
    }

    public void GetListOfQuests()
    {
        string currentRegion = this.GetCurrentOpenRegion();

        foreach (Quest quest in this.playerData.quests)
        {
            if (quest.region.ToUpper() == currentRegion.ToUpper())
            {
                Quest foundQuest = this.playerData.currentQuests.Find(questToFind => questToFind.questID == quest.questID);

                if (foundQuest == null)
                {
                    this.playerData.currentQuests.Add(quest);
                }
            }
        }
    }

    public string GetCurrentOpenRegion()
    {
        string regionNameOpened = "Region 1";

        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.isOpen)
            {
                regionNameOpened = regionData.regionName;
            }
        }

        return regionNameOpened;
    }

    public void GetAllNecessaryGameObjects()
    {
        Transform questPanel = GameObject.Find("Quest Panel").transform;
        Button closeQuestPanel = questPanel.GetChild(2).GetComponent<Button>();

        RectTransform questContentScrollView = GameObject.Find("Quest Content Scroll View").GetComponent<RectTransform>();
        CanvasGroup canvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        Button openQuestPanel = GameObject.Find("Quest Button").GetComponent<Button>();
        this.questAlertBox = GameObject.Find("Alert Box");

        closeQuestPanel.onClick.AddListener(() =>
        {
            LeanTween.scale(questPanel.gameObject, Vector2.zero, .2f)
            .setOnComplete(() => {

                canvasGroup.interactable = true;
                this.RemoveAllQuest(questContentScrollView);
            });
        });

        openQuestPanel.onClick.AddListener(() =>
        {
            this.GetAllQuests(questContentScrollView);
            LeanTween.scale(questPanel.gameObject, Vector2.one, .2f);
            canvasGroup.interactable = false;
        });

        questPanel.transform.localScale = Vector2.zero;
        this.questAlertBox.SetActive(false);
    }

    public void GetAllQuests(RectTransform content)
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            GameObject questSlot = Instantiate(questPrefab, content.transform);

            questSlot.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.title;
            questSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.description;

            if (quest.isCompleted)
            {
                questSlot.transform.GetChild(2).transform.GetChild(0)
                .GetComponent<TMPro.TextMeshProUGUI>()
                .text = "Receive";
            }
            else
            {
                questSlot.transform.GetChild(2).transform.GetChild(0)
                .GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.dunongPointsRewards.ToString();
            }
        }
    }

    public void RemoveAllQuest(RectTransform content)
    {
        foreach (Transform questSlot in content)
        {
            Destroy(questSlot.gameObject);
        }
    }


    // Functions to call to complete all quests.
    public void FindTalkQuestGoal(string npcName)
    {
        foreach(Quest quest in this.playerData.currentQuests)
        {
            if (!quest.isCompleted && quest.talkGoal.GetNPCName().ToUpper() == npcName.ToUpper())
            {
                quest.isCompleted = true;
                this.questAlertBox.SetActive(true);
                Quest questFound = this.playerData.quests.Find((questToFind) => questToFind.questID == quest.questID);
                questFound.isCompleted = true;

                DataPersistenceManager.instance.SaveGame();

                StartCoroutine(HideQuestAlertBox());

                return;
            }
        }
    }

    IEnumerator HideQuestAlertBox()
    {
        yield return new WaitForSeconds(1f);

        this.questAlertBox.SetActive(false);
    }

    public void LoadPlayerData(PlayerData playerData)
    {
        print("LOAD PLAYER DATA FROM QUEST MANAGER");
        this.playerData = playerData;
    }

    public void LoadSlotsData(Slots slots)
    {
        throw new System.NotImplementedException();
    }

    public void SaveSlotsData(ref Slots slots)
    {
        throw new System.NotImplementedException();
    }
}
