using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class GameManager : MonoBehaviour, IDataPersistence
{
    public static GameManager instance;

    //[Header("Save Panel")]
    //[SerializeField] private RectTransform saveSlotsPanel;

    [Header("Settings UI")]
    [SerializeField] private RectTransform optionsPanel;

    [Header("Profile Confirmation Panel")]
    [SerializeField] private RectTransform profileConfirmationPanel;

    [Header("Volume UI")]
    [SerializeField] public Button soundButton;
    [SerializeField] public Button quitButton;
    [SerializeField] public RectTransform volumePanel;

    [Header("Canvas Groups")]
    public CanvasGroup menuSceneCanvasGroup;
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
        SceneManager.sceneLoaded += OnCharacterAndLoadSceneLoaded;
        SceneManager.sceneLoaded += OnHouseSceneLoaded;
        SceneManager.sceneLoaded += OnOutsideSceneLoaded;
        SceneManager.sceneLoaded += OnSchoolSceneLoaded;
        SceneManager.sceneLoaded += OnPhilippineMapSceneLoaded;
        SceneManager.sceneLoaded += OnAssessmentAndWordGamesSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMenuSceneLoaded;
        SceneManager.sceneLoaded -= OnCharacterAndLoadSceneLoaded;
        SceneManager.sceneLoaded -= OnHouseSceneLoaded;
        SceneManager.sceneLoaded -= OnOutsideSceneLoaded;
        SceneManager.sceneLoaded -= OnSchoolSceneLoaded;
        SceneManager.sceneLoaded -= OnPhilippineMapSceneLoaded;
        SceneManager.sceneLoaded -= OnAssessmentAndWordGamesSceneLoaded;
    }

    // For Menu Scene
    public void OnMenuSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
                // GET ALL THE NECESSARY COMPONENTS.
                this.menuSceneCanvasGroup = GameObject.Find("Menu Canvas Group").GetComponent<CanvasGroup>();
                this.optionsPanel = GameObject.Find("Options Panel").GetComponent<RectTransform>();
                this.soundButton = GameObject.Find("Sounds").GetComponent<Button>();
                this.quitButton = GameObject.Find("Quit").GetComponent<Button>();
                this.volumePanel = GameObject.Find("Volume Panel").GetComponent<RectTransform>();

                Button optionsPanelCloseBtn = GameObject.Find("Options Panel Close Button").GetComponent<Button>();
                Button playButton = GameObject.Find("Play Button").GetComponent<Button>();
                Button optionsButton = GameObject.Find("Options Button").GetComponent<Button>();

                print(optionsButton.onClick.GetPersistentEventCount());
                this.soundButton.onClick.AddListener(() =>
                {
                    this.ShowVolumeUI();
                    SoundManager.instance.PlaySound("Button Click 2");
                });

                this.quitButton.onClick.AddListener(() => { 
                    
                    this.Quit();
                    SoundManager.instance.PlaySound("Button Click 2");
                });

                optionsPanelCloseBtn.onClick.AddListener(() =>
                {
                    this.CloseOptionPanel();
                    SoundManager.instance.PlaySound("Button Click 1");
                });

                playButton.onClick.AddListener(() => {

                    this.LoadScene("CharacterAndLoad");
                    SoundManager.instance.PlaySound("Button Click 1");
                    //TransitionLoader.instance.StartAnimation("CharacterAndLoad");
                });
                optionsButton.onClick.AddListener(() => {
                    this.ShowOptionsPanel();
                    SoundManager.instance.PlaySound("Button Click 1");
                });
                // Hide the optionsPanel at first render
                this.optionsPanel.gameObject.SetActive(false);
                this.volumePanel.gameObject.SetActive(false);
        
        }
    }

    // For CharacterAndLoad Scene
    public void OnCharacterAndLoadSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("CharacterAndLoad"))
        {
            // Get the 'Back' button and Add Event to it.
            Button backButton = GameObject.Find("Back").GetComponent<Button>();
            backButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene("Menu");
                SoundManager.instance.PlaySound("Button Click 2");
            });
            //backButton.onClick.AddListener(() => TransitionLoader.instance.StartAnimation("Menu"));
        }
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

    // For Outside Scene
    public void OnOutsideSceneLoaded( Scene scene, LoadSceneMode mode )
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside") )
        {
            this.sceneToLoadFromPhilippineMap = "Outside";
            this.playerData.sceneToLoad = "Outside";
            this.SetUpHouseOrOutsideSceneButtons();
        }
    }

    // For Outside Scene
    public void OnSchoolSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School"))
        {
            this.sceneToLoadFromPhilippineMap = "School";
            this.playerData.sceneToLoad = "School";
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
            goToHouseButton.onClick.AddListener(() => {

                SoundManager.instance.PlaySound("Button Click 1");
                this.LoadScene(this.sceneToLoadFromPhilippineMap);
            });

            // Set the value of dunong points of a current player.
            GameObject.Find("DP Value").GetComponent<TMPro.TextMeshProUGUI>().text = this.playerData.dunongPoints.ToString();
        }
    }

    /**
     * <summary>
     *  This is the registered function for Assessment and Word Games scene.
     *  
     *  Since ang dalawang scene na iyon ay may parehas na replay and exit button.
     *  Pinagsama ko na lang sila sa isang function na ito.
     *  
     *  Also, to know kung anong scene ang iloload, magbabase ang iloload na scene
     *  sa current na active na scene.
     * </summary>
     */
    public void OnAssessmentAndWordGamesSceneLoaded(Scene scene, LoadSceneMode mode)
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
                    this.playerData.dunongPoints -= 5;

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

    /**
     * <summary>
     *  Ang function na ito ay isesetup ang common UI
     *  sa house and outside scene.
     *  
     *  Example ay ang mga optionsPanel, soundButton, volumePanel etc.
     * </summary>
     */
    public void SetUpHouseOrOutsideSceneButtons()
    {
        if (!this.playerData.isTutorialDone)
        {
            return;
        }

        // GET ALL THE NECESSARY COMPONENTS.
        GameObject.Find("Character Name").GetComponent<TMPro.TextMeshProUGUI>().text = DataPersistenceManager.instance.playerData.name;
        this.homeCanvasGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        this.optionsPanel = GameObject.Find("Options Panel").GetComponent<RectTransform>();
        this.soundButton = GameObject.Find("Sounds").GetComponent<Button>();
        this.quitButton = GameObject.Find("Quit").GetComponent<Button>();
        this.volumePanel = GameObject.Find("Volume Panel").GetComponent<RectTransform>();
        Button closeButton = GameObject.Find("Close Button").GetComponent<Button>();

        this.soundButton.onClick.AddListener(() => 
        {
            SoundManager.instance.PlaySound("Button Click 2");
            this.ShowVolumeUI(); 
        });
        closeButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 1");
            this.CloseOptionPanel();
        });

        this.quitButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 2");
            this.LoadScene("Menu");
        });

        Button showMapButton = GameObject.Find("Show Map").GetComponent<Button>();
        showMapButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 1");
            this.LoadScene("Philippine Map");
        });

        Button optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
        optionsButton.onClick.AddListener(() =>
        {
            SoundManager.instance.PlaySound("Button Click 1");
            this.ShowOptionsPanel();
        });

        // Hide the optionsPanel at first render
        this.optionsPanel.gameObject.SetActive(false);
        this.volumePanel.gameObject.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }


    public void EnableCanvasGroupWhenOptionPanelIsClosed()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            this.menuSceneCanvasGroup.interactable = true;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School"))
        {
            this.homeCanvasGroup.interactable = true;
            this.homeCanvasGroup.blocksRaycasts = true;
        }
    }


    public void DisableCanvasGroupWhenOptionPanelIsOpen()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            this.menuSceneCanvasGroup.interactable = false;
        }
        else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("School"))
        {
            this.homeCanvasGroup.interactable = false;
            this.homeCanvasGroup.blocksRaycasts = false;
        }
    }

    
    public void ShowOptionsPanel()
    {
        this.optionsPanel.gameObject.SetActive(true);
        this.DisableCanvasGroupWhenOptionPanelIsOpen();
        LeanTween.scale(optionsPanel.gameObject, new Vector3(1.586f, 1.586f, 1.586f), .2f)
            .setEase(LeanTweenType.easeInCubic);
    }

    // Function for closing the options panel.
    public void CloseOptionPanel()
    {
        if (soundButton.gameObject.activeInHierarchy)
        {
            LeanTween.scale(optionsPanel.gameObject, new Vector3(0, 0, 0), .2f)
            .setEase(LeanTweenType.easeInCubic)
            .setOnComplete(() => {
                optionsPanel.gameObject.SetActive(false);
                this.EnableCanvasGroupWhenOptionPanelIsClosed();
            });
        }
        else
        {
            CloseVolumeUI();
        }
    }

    // Function for showing the volume.
    public void ShowVolumeUI()
    {
        soundButton.gameObject.SetActive(false);
        quitButton.gameObject.SetActive(false);
        volumePanel.gameObject.SetActive(true);
    }

    public void CloseVolumeUI()
    {
        soundButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
        volumePanel.gameObject.SetActive(false);
    }

    public void UnloadScene()
    {
        SceneManager.LoadScene("Philippine Map");
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
