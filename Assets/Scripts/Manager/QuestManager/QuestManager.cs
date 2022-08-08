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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnHouseSceneLoaded;
        SceneManager.sceneLoaded += OnOutsideSceneLoaded;
        SceneManager.sceneLoaded += OnSchoolSceneLoaded;
        //SceneManager.sceneLoaded += OnSceneWithQuestManagerSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnHouseSceneLoaded;
        SceneManager.sceneLoaded -= OnOutsideSceneLoaded;
        SceneManager.sceneLoaded -= OnSchoolSceneLoaded;
        //SceneManager.sceneLoaded -= OnSceneWithQuestManagerSceneLoaded;
    }

    //void OnSceneWithQuestManagerSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
    //        || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
    //        || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School"))
    //    {
    //        this.GetAllNecessaryGameObjects();
    //        this.GetListOfQuests();
    //        this.SetupScriptsForDeliveryQuestToNPCs();
    //    }
    //}

    void OnHouseSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {
            if (DataPersistenceManager.instance.playerData.isTutorialDone == false)
                return;

            this.GetAllNecessaryGameObjects();
            this.GetListOfQuests();
            this.SetupScriptsForDeliveryQuestToNPCs();
        }
    }

    void OnOutsideSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside"))
        {
            this.GetAllNecessaryGameObjects();
            this.GetListOfQuests();
            this.SetupScriptsForDeliveryQuestToNPCs();
        }
    }

    void OnSchoolSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School"))
        {
            this.GetAllNecessaryGameObjects();
            this.GetListOfQuests();
            this.SetupScriptsForDeliveryQuestToNPCs();
        }
    }
    /** Gets all the quest that is related to the 
     current open region. */
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

    /** Get the last open region */
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
        Button closeQuestPanel = questPanel.GetChild(3).GetComponent<Button>();

        RectTransform questContentScrollView = GameObject.Find("Quest Content Scroll View").GetComponent<RectTransform>();

        CanvasGroup canvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        Button openQuestPanel = GameObject.Find("Quest Button").GetComponent<Button>();
        Button pendingBtn = GameObject.Find("Pending").GetComponent<Button>();
        Button completedBtn = GameObject.Find("Completed").GetComponent<Button>();

        this.questAlertBox = GameObject.Find("Alert Box");

        pendingBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 2");
            this.RemoveAllQuest(questContentScrollView); // reset
            this.GetAllCurrentQuests(questContentScrollView);

            this.ChangeButtonColor(pendingBtn, completedBtn);
        });

        completedBtn.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 2");
            this.RemoveAllQuest(questContentScrollView);
            this.GetAllCompletedQuest(questContentScrollView);

            this.ChangeButtonColor(completedBtn, pendingBtn);
        });

        closeQuestPanel.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 1");
            LeanTween.scale(questPanel.gameObject, Vector2.zero, .2f)
            .setEaseSpring()
            .setOnComplete(() =>
            {
                this.RemoveAllQuest(questContentScrollView);
                canvasGroup.interactable = true;
            });
            this.ChangeButtonColor(pendingBtn, completedBtn);
        });

        openQuestPanel.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 1");
            this.GetAllCurrentQuests(questContentScrollView);
            LeanTween.scale(questPanel.gameObject, new Vector2(296.8722f, 296.8722f), .2f)
                .setEaseSpring();
            canvasGroup.interactable = false;
        });

        this.ChangeButtonColor(pendingBtn, completedBtn);
        this.questAlertBox.SetActive(false);
        questPanel.transform.localScale = Vector2.zero;

    }

    /** This function is responsible for changing the color
     of 'Pending' and 'Completed' buttons in Quest Panel. */
    void ChangeButtonColor(Button buttonToChange, Button buttonToRevert)
    {
        // Setup the color for selecting button.
        Color selectedColor;
        ColorUtility.TryParseHtmlString("#331313", out selectedColor);

        buttonToChange.GetComponent<Image>().color = selectedColor;
        buttonToChange.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = Color.white;

        buttonToRevert.GetComponent<Image>().color = Color.white;
        buttonToRevert.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().color = selectedColor;
    }

    /** Display all the current quest at Quest Panel. */
    public void GetAllCurrentQuests(RectTransform content)
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            GameObject questSlot = Instantiate(questPrefab, content.transform);

            questSlot.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.title;
            questSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.description;
            questSlot.transform.GetChild(2).transform.GetChild(0)
                .GetComponent<TMPro.TextMeshProUGUI>()
                .text = "Pending";
            questSlot.transform.GetChild(3).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.dunongPointsRewards.ToString();
        }
    }

    /** Display all the complete quest at Quest Panel. */
    public void GetAllCompletedQuest(RectTransform content)
    {
        foreach (Quest quest in this.playerData.completedQuests)
        {
            // Instantiate the Quest Prefab
            GameObject questSlot = Instantiate(questPrefab, content.transform); 

            // Quest Title
            questSlot.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.title;

            // Quest Description
            questSlot.transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.description;

            if (quest.isCompleted)
            {
                Button claimBtn = questSlot.transform.GetChild(2).GetComponent<Button>();

                if (!quest.isClaimed)
                {
                    claimBtn.onClick.AddListener(() =>
                    {
                        questSlot.transform.GetChild(2).transform.GetChild(0)
                         .GetComponent<TMPro.TextMeshProUGUI>()
                         .text = "Claimed";

                        this.playerData.dunongPoints += quest.dunongPointsRewards;
                        quest.isClaimed = true;
                        claimBtn.interactable = false;
                        DataPersistenceManager.instance.SaveGame();
                    });

                    // If it is not yet claimed, we display 'claim'
                    questSlot.transform.GetChild(2).transform.GetChild(0)
                     .GetComponent<TMPro.TextMeshProUGUI>()
                     .text = "Claim";
                }
                else
                {
                    // If this function is called again, then if it is claimed, we display different text.
                    claimBtn.interactable = false;
                    questSlot.transform.GetChild(2).transform.GetChild(0)
                     .GetComponent<TMPro.TextMeshProUGUI>()
                     .text = "Claimed";
                }
            }

            // Dunong Points Rewards
            questSlot.transform.GetChild(3).transform.GetChild(1)
                .GetComponent<TMPro.TextMeshProUGUI>()
                .text = quest.dunongPointsRewards.ToString();
        }
    }

    /** Remove all quest at Quest Panel */
    public void RemoveAllQuest(RectTransform content)
    {
        foreach (Transform questSlot in content)
        {
            Destroy(questSlot.gameObject);
        }
    }

    /** Setups all the gameobject who handles DeliveryGoalGiver and DeliveryGoalReceiver Quest. */
    public void SetupScriptsForDeliveryQuestToNPCs()
    {
        DeliveryGoalGiver[] dgg = GameObject.FindObjectsOfType<DeliveryGoalGiver>();
        DeliveryGoalReceiver[] dgr = GameObject.FindObjectsOfType<DeliveryGoalReceiver>();

        foreach (DeliveryGoalGiver dg in dgg)
        {
            dg.quest = null;
        }

        foreach (DeliveryGoalReceiver dr in dgr)
        {
            dr.quest = null;
        }

        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.deliveryGoal != null)
            {
                GameObject giver = GameObject.Find(quest.deliveryGoal.giverName);
                GameObject receiver = GameObject.Find(quest.deliveryGoal.receiverName);

                if (giver != null)
                {
                    giver.GetComponent<DeliveryGoalGiver>().quest = quest.CopyQuestDeliveryGoal();
                }

                if (receiver != null)
                {
                    receiver.GetComponent<DeliveryGoalReceiver>().quest = quest.CopyQuestDeliveryGoal();
                }
            }
        }
    }

    /** Find the delivery quest based on ID and set it as completed. */
    public void FindDeliveryQuestGoal(string deliveryQuestID)
    {
        foreach (Quest quest in this.playerData.currentQuests)
        {
            if (quest.questID == deliveryQuestID)
            {
                this.questAlertBox.SetActive(true);
                SoundManager.instance.PlaySound("Quest Notification");

                // Find and Copy the Delivery Quest
                Quest questFound = this.playerData.quests.Find((questToFind) => questToFind.questID == quest.questID).CopyQuestDeliveryGoal();

                this.playerData.currentQuests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);
                this.playerData.quests.RemoveAll(questToRemove => questToRemove.questID == quest.questID);
                //questFound = this.playerData.currentQuests.Find(questToFind => questToFind.questID == quest.questID);
                //this.playerData.currentQuests.Remove(questFound);

                //questFound = this.playerData.quests.Find(questToFind => questToFind.questID == quest.questID);
                //this.playerData.quests.Remove(questFound); // NEED TO TEST.

                questFound.isCompleted = true;

                this.playerData.completedQuests.Add(questFound);

                this.GetListOfQuests();

                DataPersistenceManager.instance.SaveGame();

                StartCoroutine(HideQuestAlertBox());

                return;
            }
        }
    }

    /** Find the talk quest and set it as completed. */
    public void FindTalkQuestGoal(string npcName)
    {
        foreach(Quest quest in this.playerData.currentQuests)
        {
            if (!quest.isCompleted 
                && quest.talkGoal != null 
                && quest.talkGoal.GetNPCName().ToUpper() == npcName.ToUpper())
            {
                quest.isCompleted = true;
                this.questAlertBox.SetActive(true);
                SoundManager.instance.PlaySound("Quest Notification");

                Quest questFound = this.playerData.quests.Find((questToFind) => questToFind.questID == quest.questID);
                
                questFound = this.playerData.currentQuests.Find(questToFind => questToFind.questID == quest.questID);
                this.playerData.currentQuests.Remove(questFound);

                questFound = this.playerData.quests.Find(questToFind => questToFind.questID == quest.questID);
                this.playerData.quests.Remove(questFound); // NEED TO TEST.

                questFound.isCompleted = true;

                this.playerData.completedQuests.Add(questFound);

                this.GetListOfQuests();

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
