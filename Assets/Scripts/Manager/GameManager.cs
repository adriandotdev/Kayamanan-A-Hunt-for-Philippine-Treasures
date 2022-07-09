using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    [Header("Save Panel")]
    [SerializeField] private RectTransform saveSlotsPanel;

    [Header("Settings UI")]
    [SerializeField] private RectTransform optionsPanel;

    [Header("Profile Confirmation Panel")]
    [SerializeField] private RectTransform profileConfirmationPanel;

    [Header("Volume UI")]
    [SerializeField] public Button soundButton;
    [SerializeField] public Button quitButton;
    [SerializeField] public RectTransform volumePanel;

    [Header("Home Canvas Group")]
    public CanvasGroup menuCanvasGroup;
    public CanvasGroup homeCanvasGroup;
    public CanvasGroup characterCreationGroup;

    [Header("Player Data")]
    public PlayerData playerData;

    public string sceneToLoadFromPhilippineMap;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnMenuSceneLoaded;
        SceneManager.sceneLoaded += OnCharacterCreationSceneLoaded;
        SceneManager.sceneLoaded += OnHouseSceneLoaded;
        SceneManager.sceneLoaded += OnOutsideSceneLoaded;
        SceneManager.sceneLoaded += OnPhilippineMapSceneLoaded;
        SceneManager.sceneLoaded += OnAssessmentSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMenuSceneLoaded;
        SceneManager.sceneLoaded -= OnCharacterCreationSceneLoaded;
        SceneManager.sceneLoaded -= OnHouseSceneLoaded;
        SceneManager.sceneLoaded -= OnOutsideSceneLoaded;
        SceneManager.sceneLoaded -= OnPhilippineMapSceneLoaded;
        SceneManager.sceneLoaded -= OnAssessmentSceneLoaded;
    }

    // For Menu Scene
    public void OnMenuSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            // GET ALL THE NECESSARY COMPONENTS.
            this.menuCanvasGroup = GameObject.Find("Menu Canvas Group").GetComponent<CanvasGroup>();
            this.saveSlotsPanel = GameObject.Find("Save Slots Panel").GetComponent<RectTransform>();
            this.optionsPanel = GameObject.Find("Options Panel").GetComponent<RectTransform>();
            this.soundButton = GameObject.Find("Sounds").GetComponent<Button>();
            this.quitButton = GameObject.Find("Quit").GetComponent<Button>();
            this.volumePanel = GameObject.Find("Volume Panel").GetComponent<RectTransform>();
            Button closeButton = GameObject.Find("Close Button").GetComponent<Button>();

            Button playButton = GameObject.Find("Play Button").GetComponent<Button>();
            Button optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
            Button loadButton = GameObject.Find("Load Button").GetComponent<Button>();
            Button loadPlayerProfileBTN = GameObject.Find("Load Player Profile Button").GetComponent<Button>();
            Button closeSlotsPanelBTN = GameObject.Find("Close Slots Panel Button").GetComponent<Button>();
  
            this.soundButton.onClick.AddListener(() => this.ShowVolume());
            this.quitButton.onClick.AddListener(() => this.Quit());

            closeButton.onClick.AddListener(() => this.Close());
            playButton.onClick.AddListener(() => {

                this.LoadScene("CharacterAndLoad");
                //TransitionLoader.instance.StartAnimation("CharacterAndLoad");
            });
            optionsButton.onClick.AddListener(() => this.ShowOptionsPanel() );
            loadButton.onClick.AddListener(() => this.ShowSaveSlots(true) );
            loadPlayerProfileBTN.onClick.AddListener(() => {

                if (DataPersistenceManager.instance.playerData != null && DataPersistenceManager.instance.playerData.id != null)
                    this.ShowConfirmProfilePanel(true);
                //if (DataPersistenceManager.instance.playerData != null && DataPersistenceManager.instance.playerData.id != null)
                //    this.LoadScene("House");
            });
            closeSlotsPanelBTN.onClick.AddListener(() => this.ShowSaveSlots(false));

            // Get the UI Elements for profile confirmation.
            this.profileConfirmationPanel = GameObject.Find("Load Profile Confirmation").GetComponent<RectTransform>();
            Button confirmProfile = GameObject.Find("Confirm Profile").GetComponent<Button>();
            Button cancelProfile = GameObject.Find("Cancel Profile").GetComponent<Button>();
            confirmProfile.onClick.AddListener(() => {
                this.LoadScene("House");
            });
            cancelProfile.onClick.AddListener(() => this.ShowConfirmProfilePanel(false));

            // Hide the optionsPanel at first render
            this.optionsPanel.gameObject.SetActive(false);
            this.volumePanel.gameObject.SetActive(false);
            this.saveSlotsPanel.gameObject.SetActive(false);
            this.profileConfirmationPanel.gameObject.SetActive(false);
        }
    }

    // For CharacterAndLoad Scene
    public void OnCharacterCreationSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("CharacterAndLoad"))
        {
            // Get the 'Back' button and Add Event to it.
            Button backButton = GameObject.Find("Back").GetComponent<Button>();
            backButton.onClick.AddListener(() => SceneManager.LoadScene("Menu"));
            //backButton.onClick.AddListener(() => TransitionLoader.instance.StartAnimation("Menu"));
        }
    }

    public void SetUpHouseOrOutsideSceneButtons()
    {
        // GET ALL THE NECESSARY COMPONENTS.
        GameObject.Find("Character Name").GetComponent<TMPro.TextMeshProUGUI>().text = DataPersistenceManager.instance.playerData.name;
        this.homeCanvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        this.optionsPanel = GameObject.Find("Options Panel").GetComponent<RectTransform>();
        this.soundButton = GameObject.Find("Sounds").GetComponent<Button>();
        this.quitButton = GameObject.Find("Quit").GetComponent<Button>();
        this.volumePanel = GameObject.Find("Volume Panel").GetComponent<RectTransform>();
        Button closeButton = GameObject.Find("Close Button").GetComponent<Button>();

        this.soundButton.onClick.AddListener(() => this.ShowVolume());
        closeButton.onClick.AddListener(() => this.Close());

        this.quitButton.onClick.AddListener(() => this.LoadScene("Menu"));
        Button showMapButton = GameObject.Find("Show Map").GetComponent<Button>();
        showMapButton.onClick.AddListener(() => this.LoadScene("Philippine Map"));

        Button optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
        optionsButton.onClick.AddListener(() => this.ShowOptionsPanel());

        // Hide the optionsPanel at first render
        this.optionsPanel.gameObject.SetActive(false);
        this.volumePanel.gameObject.SetActive(false);
    }

    // For House Scene
    public void OnHouseSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {
            this.sceneToLoadFromPhilippineMap = "House";
            this.playerData.sceneToLoad = "House";
            this.SetUpHouseOrOutsideSceneButtons();
        }
    }

    public void OnOutsideSceneLoaded( Scene scene, LoadSceneMode mode )
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside") )
        {
            this.sceneToLoadFromPhilippineMap = "Outside";
            this.playerData.sceneToLoad = "Outside";
            this.SetUpHouseOrOutsideSceneButtons();
        }
    }

    // For Philippine Map Scene
    public void OnPhilippineMapSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Philippine Map"))
        {
            // Get the Go To House button and add an event to it.
            Button goToHouseButton = GameObject.Find("Go to House").GetComponent<Button>();
            goToHouseButton.onClick.AddListener(() => this.LoadScene(this.sceneToLoadFromPhilippineMap));

            // Set the value of dunong points of a current player.
            GameObject.Find("DP Value").GetComponent<TMPro.TextMeshProUGUI>().text = this.playerData.dunongPoints.ToString();
        }
    }

    // For Assessment Scene
    public void OnAssessmentSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Assessment")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Word Games"))
        {
            Button replayButton = GameObject.Find("Replay Button").GetComponent<Button>();
            Button exit = GameObject.Find("Exit Button").GetComponent<Button>();
            Button close = GameObject.Find("Close").GetComponent<Button>();

            replayButton.onClick.AddListener(() =>
            {
                if (DataPersistenceManager.instance.playerData.dunongPoints >= 5)
                {
                    if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Assessment"))
                    {
                        this.LoadScene("Assessment");
                        return;
                    }
                    this.LoadScene("Word Games");
                }
            });

            exit.onClick.AddListener(() => this.LoadScene("Philippine Map"));
            close .onClick.AddListener(() => this.LoadScene("Philippine Map"));
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void DisableCanvasGroup()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            this.menuCanvasGroup.interactable = false;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {
            this.homeCanvasGroup.interactable = false;
            this.homeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void EnableCanvasGroup()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            this.menuCanvasGroup.interactable = true;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {
            this.homeCanvasGroup.interactable = true;
            this.homeCanvasGroup.blocksRaycasts = true;
        }
    }

    public void ShowSaveSlots(bool active)
    {
        if (active)
        {
            saveSlotsPanel.gameObject.SetActive(true);
            this.DisableCanvasGroup();
            LeanTween.scale(saveSlotsPanel.gameObject, new Vector2(0.6188691f, 0.6188691f), .3f);
        }
        else
        {
            LeanTween.scale(saveSlotsPanel.gameObject, new Vector2(0, 0), .3f)
                .setOnComplete(() =>
                {
                    saveSlotsPanel.gameObject.SetActive(false);
                    this.EnableCanvasGroup();
                    DataPersistenceManager.instance.playerData = null;
                });
        }
    }

    public void ShowConfirmProfilePanel(bool show)
    {
        if (show)
        {
            this.profileConfirmationPanel.gameObject.SetActive(show);
            this.saveSlotsPanel.GetComponent<CanvasGroup>().interactable = false;
            this.saveSlotsPanel.GetComponent<CanvasGroup>().alpha = 0.5f;
            LeanTween.scale(this.profileConfirmationPanel.gameObject, new Vector2(1, 1), .2f);
        }
        else
        {
            this.saveSlotsPanel.GetComponent<CanvasGroup>().interactable = true;
            this.saveSlotsPanel.GetComponent<CanvasGroup>().alpha = 1f;
            LeanTween.scale(this.profileConfirmationPanel.gameObject, new Vector2(0, 0), .2f)
                .setOnComplete(() =>
                {
                    this.profileConfirmationPanel.gameObject.SetActive(false);
                });
        }
    }

    public void ShowOptionsPanel()
    {
        this.optionsPanel.gameObject.SetActive(true);
        this.DisableCanvasGroup();
        LeanTween.scale(optionsPanel.gameObject, new Vector3(1.586f, 1.586f, 1.586f), .2f)
            .setEase(LeanTweenType.easeInCubic);
    }

    // Function for closing the options panel.
    public void Close()
    {
        if (soundButton.gameObject.activeInHierarchy)
        {
            LeanTween.scale(optionsPanel.gameObject, new Vector3(0, 0, 0), .2f)
            .setEase(LeanTweenType.easeInCubic)
            .setOnComplete(() => {
                optionsPanel.gameObject.SetActive(false);
                this.EnableCanvasGroup();
            });
        }
        else
        {
            CloseVolume();
        }
    }

    // Function for showing the volume.
    public void ShowVolume()
    {
        soundButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        volumePanel.gameObject.SetActive(true);
    }

    public void UnloadScene()
    {
        SceneManager.LoadScene("Philippine Map");
    }

    public void CloseVolume()
    {
        soundButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        volumePanel.gameObject.SetActive(false);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadPlayerData(PlayerData playerData)
    {
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
