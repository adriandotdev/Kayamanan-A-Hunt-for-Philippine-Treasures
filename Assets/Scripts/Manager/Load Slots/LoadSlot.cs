using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class LoadSlot : MonoBehaviour, IDataPersistence
{
    [SerializeField] private GameObject content; // Container of all the slots.
    [SerializeField] private GameObject saveSlot; // Save slot to render or to put inside of 'content'.
    [SerializeField] private RectTransform confirmationPanel;
    [SerializeField] private Button confirm;
    [SerializeField] private Button cancel;
    [SerializeField] private CanvasGroup UICanvasGroup;

    [Header("Confirmation Message UI")]
    [SerializeField] private TMPro.TextMeshProUGUI confirmationMessage;
    [SerializeField] private TMPro.TextMeshProUGUI profileNameLabel;

    [Header("Delete Confirmation UI")]
    [SerializeField] private RectTransform deletePanel;
    [SerializeField] private TMPro.TextMeshProUGUI lblMainMessage;
    [SerializeField] private TMPro.TextMeshProUGUI lblProfile;
    [SerializeField] private Button btnDelete;
    [SerializeField] private Button btnCancelDelete;

    [Header("Character Creation UI Elements")]
    [SerializeField] private CanvasGroup createCharacterPanelCanvasGroup;
    [SerializeField] private RectTransform createCharacterPanel;
    [SerializeField] private Button create;
    [SerializeField] private Button checkButtonCharCreationPanel;
    [SerializeField] private Button closeCharCreationPanel;
    [SerializeField] private TMPro.TMP_InputField characterName;
    [SerializeField] private Button male;
    [SerializeField] private Button female;

    private bool isCharacterPanelOpen = false;

    public PlayerData playerData;

    private void SetupConfirmationPanelButtons()
    {
        cancel.onClick.AddListener(() =>
        {
            if (isCharacterPanelOpen != true)
            {
                this.UICanvasGroup.interactable = true;
                DataPersistenceManager.instance.playerData = new PlayerData();
                this.playerData = DataPersistenceManager.instance.playerData;
            }
               
            this.createCharacterPanelCanvasGroup.interactable = true;
            LeanTween.scale(confirmationPanel.gameObject, new Vector2(0, 0), .2f);
        });
    }

    private void SetupCharacterPanelUIElements()
    {
        this.CharacterButtons();
        create.onClick.AddListener(() =>
        {
            this.UICanvasGroup.interactable = false;
            this.isCharacterPanelOpen = true;
            LeanTween.scale(createCharacterPanel.gameObject, new Vector2(1, 1), .2f);
        });

        checkButtonCharCreationPanel.onClick.AddListener(() =>
        {
            if (this.characterName.text.Length != 0)
            {
                this.UICanvasGroup.interactable = false;
                this.createCharacterPanelCanvasGroup.interactable = false;
                LeanTween.scale(confirmationPanel.gameObject, new Vector2(1, 1), .2f);
                this.confirmationMessage.text = "Create new profile";
                this.profileNameLabel.text = "'" + this.characterName.text + "'?";
                this.confirm.onClick.RemoveAllListeners();
                this.confirm.onClick.AddListener(this.ConfirmBtnEventWhileCreatingProfile);
            }
        });

        closeCharCreationPanel.onClick.AddListener(() =>
        {
            this.UICanvasGroup.interactable = true;
            this.isCharacterPanelOpen = false;
            LeanTween.scale(createCharacterPanel.gameObject, new Vector2(0, 0), .2f);
            this.characterName.text = "";
        });
    }

    private void SetupDeleteConfirmation()
    {
        this.btnDelete.onClick.AddListener(() =>
        {
            SlotsFileHandler fileHandler = new SlotsFileHandler();
            Slots slots = fileHandler.Load();

            LeanTween.scale(this.deletePanel.gameObject, Vector2.zero, .2f)
            .setOnComplete(() => this.UICanvasGroup.interactable = true);

            File.Delete(Application.persistentDataPath + "/" + this.playerData.id + ".txt");

            slots.ids.Remove(this.playerData.id);
            fileHandler.Save(slots);

            DataPersistenceManager.instance.playerData = new PlayerData();
            this.playerData = DataPersistenceManager.instance.playerData;

            this.RemoveAllSlots();
            this.LoadAllSlots();
        });

        this.btnCancelDelete.onClick.AddListener(() =>
        {
            LeanTween.scale(this.deletePanel.gameObject, Vector2.zero, .2f).
            setOnComplete(() => this.UICanvasGroup.interactable = true );
        });
    }

    private void CharacterButtons()
    {
        male.onClick.AddListener(() =>
        {
            male.transform.GetChild(0).gameObject.SetActive(true);
            female.transform.GetChild(0).gameObject.SetActive(false);
            this.playerData.gender = "male";
        });

        female.onClick.AddListener(() =>
        {
            female.transform.GetChild(0).gameObject.SetActive(true);
            male.transform.GetChild(0).gameObject.SetActive(false);
            this.playerData.gender = "female";
        });
    }

    private void ConfirmBtnEventWhileLoadingProfile()
    {
        SceneManager.LoadScene(this.playerData.sceneToLoad);
    }

    private void ConfirmBtnEventWhileCreatingProfile()
    {
        this.playerData.name = this.characterName.text;
        DataPersistenceManager.instance.ConfirmCharacter();

        SceneManager.LoadScene("House");
    }

    private void Start()
    {
        this.createCharacterPanel.localScale = Vector2.zero;
        this.confirmationPanel.localScale = Vector2.zero;
        this.deletePanel.localScale = Vector2.zero;

        this.SetupConfirmationPanelButtons();
        this.SetupCharacterPanelUIElements();
        this.SetupDeleteConfirmation();

        this.LoadAllSlots();
        //Slots slots = DataPersistenceManager.instance.GetAllSlots();
        //PlayerDataHandler playerDataHandler = null;

        //for (int i = 0; i < slots.ids.Count; i++)
        //{
        //    GameObject gameObject = Instantiate(saveSlot, content.transform);

        //    playerDataHandler = new PlayerDataHandler(slots.ids[i]);

        //    PlayerData playerData = playerDataHandler.Load();

        //    gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = playerData.name;
        //    gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() =>
        //    {
        //        UICanvasGroup.interactable = false;
        //        DataPersistenceManager.instance.playerData = playerData;
        //        LeanTween.scale(confirmationPanel.gameObject, new Vector2(1, 1), .2f);
        //        this.confirmationMessage.text = "Are you sure you want to load";
        //        this.profileNameLabel.text = "'" + playerData.name + "' Profile?";
        //        this.confirm.onClick.RemoveAllListeners();
        //        this.confirm.onClick.AddListener(this.ConfirmBtnEventWhileLoadingProfile);
        //    });
        //    gameObject.AddComponent<SlotScript>();
        //}
    }

    public void RemoveAllSlots()
    {
        foreach (Transform transform in this.content.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    private void LoadAllSlots()
    {
        Slots slots = DataPersistenceManager.instance.GetAllSlots();
        PlayerDataHandler playerDataHandler = null;

        for (int i = 0; i < slots.ids.Count; i++)
        {
            GameObject gameObject = Instantiate(saveSlot, content.transform);

            playerDataHandler = new PlayerDataHandler(slots.ids[i]);

            PlayerData playerData = playerDataHandler.Load();

            gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = playerData.name;
            gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                UICanvasGroup.interactable = false;

                this.playerData = playerData;
                DataPersistenceManager.instance.playerData = this.playerData;

                LeanTween.scale(confirmationPanel.gameObject, new Vector2(1, 1), .2f);
                this.confirmationMessage.text = "Are you sure you want to load";
                this.profileNameLabel.text = "'" + playerData.name + "' Profile?";
                this.confirm.onClick.RemoveAllListeners();
                this.confirm.onClick.AddListener(this.ConfirmBtnEventWhileLoadingProfile);
            });

            // DELETE Button Event
            gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                this.playerData = playerData;
                DataPersistenceManager.instance.playerData = this.playerData;

                this.UICanvasGroup.interactable = false;
                LeanTween.scale(this.deletePanel.gameObject, Vector2.one, .2f);
                this.lblMainMessage.text = "Are you sure you want to remove";
                this.lblProfile.text = "'" + playerData.name + "' Profile?";
                
            });
            gameObject.AddComponent<SlotScript>();
        }

        slots = null;
        playerDataHandler = null;

    }

    public void LoadPlayerData(PlayerData playerData)
    {
        print("LOADED PLAYER DATA : LOAD SLOT");
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

public class SlotScript : MonoBehaviour
{
    private void OnDisable()
    {
        Destroy(gameObject);
    }
}


//private void OnEnable()
//{
//    Slots slots = DataPersistenceManager.instance.GetAllSlots();
//    PlayerDataHandler playerDataHandler = null;

//    for (int i = 0; i < slots.ids.Count; i++)
//    {
//        GameObject gameObject = Instantiate(saveSlot, content.transform);

//        playerDataHandler = new PlayerDataHandler(slots.ids[i]);

//        PlayerData playerData = playerDataHandler.Load();

//        gameObject.GetComponent<Button>().onClick.AddListener(() => 
//        {
//            DataPersistenceManager.instance.playerData = playerData;
//        });

//        gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = playerData.name;
//        gameObject.AddComponent<SlotScript>();
//    }

//    //if (content.transform.childCount > 0)
//    //{
//    //    content.transform.GetChild(0).GetComponent<Button>().Select();

//    //    playerDataHandler = new PlayerDataHandler(slots.ids[0]);

//    //    PlayerData playerData = playerDataHandler.Load();

//    //    DataPersistenceManager.instance.playerData = playerData;
//    //}

//     //gameObject.GetComponent<Button>().Select();
//}