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

    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup UICanvasGroup;
    [SerializeField] private CanvasGroup createCharacterPanelCanvasGroup;

    [Header("Confirmation Message UI")]
    [SerializeField] private RectTransform confirmationPanel;
    [SerializeField] private TMPro.TextMeshProUGUI confirmationMessage;
    [SerializeField] private TMPro.TextMeshProUGUI profileNameLabel;
    [SerializeField] private Button btnConfirm;
    [SerializeField] private Button btnCancel;
    
    [Header("Delete Confirmation UI")]
    [SerializeField] private RectTransform deletePanel;
    [SerializeField] private TMPro.TextMeshProUGUI lblMainMessage;
    [SerializeField] private TMPro.TextMeshProUGUI lblProfile;
    [SerializeField] private Button btnDelete;
    [SerializeField] private Button btnCancelDelete;

    [Header("Character Creation UI Elements")]
    [SerializeField] private RectTransform createCharacterPanel;
    [SerializeField] private TMPro.TMP_InputField btnCharacterName;
    [SerializeField] private Button btnMale;
    [SerializeField] private Button btnFemale;
    [SerializeField] private Button btnCheckButtonCharCreationPanel;
    [SerializeField] private Button btnCloseCharCreationPanel;
    [SerializeField] private Button btnCreateNewProfile;

    private bool isCharacterPanelOpen = false;

    public PlayerData playerData;

    private void Start()
    {
        this.createCharacterPanel.localScale = Vector2.zero;
        this.confirmationPanel.localScale = Vector2.zero;
        this.deletePanel.localScale = Vector2.zero;

        this.AddEventsToConfirmationPanelButtons();
        this.SetupCharacterPanelUIElements();
        this.SetupDeleteConfirmation();

        this.LoadAllSlots();
    }

    /**
     * <summary>
     *  Ang function na ito ay ang responsible
     *  para sa event ng button sa loob ng confirmation panel.
     *  
     *  Since ang load button sa confirmation panel ay magbabase
     *  depende kung ang player ay nag loload o nagcrecreate ng profile,
     *  kaya hindi siya direct na inimplement ang event dito.
     * </summary>
     */
    private void AddEventsToConfirmationPanelButtons()
    {
        /**
         * <summary>
         *  Ang btnCancel ay iche-check kung ang character panel ay 
         *  open. 
         *  
         *  Kung ang character panel ay hindi open, it means na ang
         *  confirmation panel ay triggered by load button from a 
         *  specific slot.
         *  
         *  Kung ang character panel ay true, it means na triggered ito
         *  by the check button sa character panel.
         *  
         *  So, i didisable natin ang interactable property ng canvas group
         *  based doon sa condition.
         * </summary> 
         */
        btnCancel.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 2"); // Click Sound
            if (this.isCharacterPanelOpen != true)
            {
                this.UICanvasGroup.interactable = true;
                DataPersistenceManager.instance.playerData = new PlayerData();
                this.playerData = DataPersistenceManager.instance.playerData;
            }
               
            this.createCharacterPanelCanvasGroup.interactable = true;
            LeanTween.scale(confirmationPanel.gameObject, new Vector2(0, 0), .2f);
        });
    }

    /**
     * <summary>
     *  Ang function na ito ay ang magseset up
     *  para sa event ng male at female button
     *  sa loob ng character panel.
     * </summary>
     */
    private void CharacterButtons()
    {
        btnMale.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Checkbox");

            // Show the check symbol under the btnMale.
            btnMale.transform.GetChild(0).gameObject.SetActive(true);

            // Hide the check symbol under the btnFemale.
            btnFemale.transform.GetChild(0).gameObject.SetActive(false);

            // Set the 'gender' property at playerData.
            this.playerData.gender = "male";
        });

        btnFemale.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Checkbox");

            // Show the check symbol at btnFemale.
            btnFemale.transform.GetChild(0).gameObject.SetActive(true);

            // Hide the check symbol at btnMale.
            btnMale.transform.GetChild(0).gameObject.SetActive(false);

            // Set the 'gender' property to 'female'.
            this.playerData.gender = "female";
        });
    }

    /**
     * <summary>
     *  Ang function na ito ay ang magseset up ng mga
     *  events ng button sa loob ng character panel.
     * </summary>
     */
    private void SetupCharacterPanelUIElements()
    {
        this.CharacterButtons(); // Invoke the function 'CharacterButtons()'.

        btnCreateNewProfile.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 2");

            this.UICanvasGroup.interactable = false;
            this.isCharacterPanelOpen = true;
            LeanTween.scale(createCharacterPanel.gameObject, new Vector2(1, 1), .2f);
        });

        btnCheckButtonCharCreationPanel.onClick.AddListener(() =>
        {
            if (this.btnCharacterName.text.Length != 0)
            {
                SoundManager.instance.PlaySound("Button Click 2");
                this.UICanvasGroup.interactable = false;
                this.createCharacterPanelCanvasGroup.interactable = false;
                LeanTween.scale(confirmationPanel.gameObject, new Vector2(1, 1), .2f);

                this.confirmationMessage.text = "Create new profile";
                this.profileNameLabel.text = "'" + this.btnCharacterName.text + "'?";
                this.btnConfirm.onClick.RemoveAllListeners();
                this.btnConfirm.onClick.AddListener(this.ConfirmBtnEventWhileCreatingProfile);
            }
        });

        btnCloseCharCreationPanel.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 2");

            this.UICanvasGroup.interactable = true;
            this.isCharacterPanelOpen = false;
            LeanTween.scale(createCharacterPanel.gameObject, new Vector2(0, 0), .2f);
            this.btnCharacterName.text = "";
        });
    }


    /**
     * <summary>
     *  Ang function na ito ay responsible 
     *  para i set up ang mga events ng buttons
     *  sa delete confirmation panel.
     * </summary>
     */
    private void SetupDeleteConfirmation()
    {
        this.btnDelete.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 2");

            // Create and Load all the slots.
            SlotsFileHandler fileHandler = new SlotsFileHandler();
            Slots slots = fileHandler.Load();

            LeanTween.scale(this.deletePanel.gameObject, Vector2.zero, .2f)
            .setOnComplete(() => this.UICanvasGroup.interactable = true);

            // Delete the file based on the playerData 'id'.
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
            SoundManager.instance.PlaySound("Button Click 2");

            DataPersistenceManager.instance.playerData = new PlayerData();
            this.playerData = DataPersistenceManager.instance.playerData;

            LeanTween.scale(this.deletePanel.gameObject, Vector2.zero, .2f).
            setOnComplete(() => this.UICanvasGroup.interactable = true );
        });
    }


    /**
     * <summary>
     * 
     * Ito ang ireregister na event sa confirm button
     * sa confirmation panel kapag ang player ay nag
     * loload ng profile. 
     * </summary>
     */
    private void ConfirmBtnEventWhileLoadingProfile()
    {
        SoundManager.instance.PlaySound("Button Click 2");
        SceneManager.LoadScene(this.playerData.sceneToLoad);
    }

    /**
     * <summary>
     *    Ito ang ireregister kapag ang player ay
     *    nagcrecreate ng bagong profile. 
     * </summary>
     */
    private void ConfirmBtnEventWhileCreatingProfile()
    {
        SoundManager.instance.PlaySound("Button Click 2");
        this.playerData.name = this.btnCharacterName.text;
        DataPersistenceManager.instance.ConfirmCharacter();

        SceneManager.LoadScene("House");
    }

    public void RemoveAllSlots()
    {
        foreach (Transform transform in this.content.transform)
        {
            Destroy(transform.gameObject);
        }
    }

    /**
     * <summary>
     *   Load and Display all created profile/slots.
     * </summary> 
     */
    private void LoadAllSlots()
    {
        Slots slots = DataPersistenceManager.instance.GetAllSlots();
        PlayerDataHandler playerDataHandler = null;

        /**
         * <summary>
         *  Traverse the slots ids list and instantiate a 
         *  slot game object at 'content' game object.
         * </summary> 
         */
        for (int i = 0; i < slots.ids.Count; i++)
        {
            GameObject gameObject = Instantiate(saveSlot, content.transform);

            playerDataHandler = new PlayerDataHandler(slots.ids[i]);

            PlayerData playerData = playerDataHandler.Load();

            gameObject.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = playerData.name;

            // LOAD Button Event
            gameObject.transform.GetChild(1).gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                UICanvasGroup.interactable = false;
                SoundManager.instance.PlaySound("Button Click 2"); // Sound Click

                this.playerData = playerData;
                DataPersistenceManager.instance.playerData = this.playerData;

                LeanTween.scale(confirmationPanel.gameObject, new Vector2(1, 1), .2f);
                this.confirmationMessage.text = "Are you sure you want to load";
                this.profileNameLabel.text = "'" + playerData.name + "' Profile?";
                this.btnConfirm.onClick.RemoveAllListeners();
                this.btnConfirm.onClick.AddListener(this.ConfirmBtnEventWhileLoadingProfile);
            });

            // DELETE Button Event
            gameObject.transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                this.playerData = playerData;
                SoundManager.instance.PlaySound("Button Click 2");
                DataPersistenceManager.instance.playerData = this.playerData;

                this.UICanvasGroup.interactable = false;
                LeanTween.scale(this.deletePanel.gameObject, Vector2.one, .2f);
                this.lblMainMessage.text = "Are you sure you want to remove";
                this.lblProfile.text = "'" + playerData.name + "' Profile?";
                
            });
        }

        // Delete the instance of slots and playerDataHandler to avoid memory leak.
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
