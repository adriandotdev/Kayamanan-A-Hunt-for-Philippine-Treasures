using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Save Panel")]
    [SerializeField] private RectTransform saveSlotsPanel;

    [Header("Settings UI")]
    [SerializeField] private RectTransform optionsPanel;

    [Header("Volume UI")]
    [SerializeField] public Button soundButton;
    [SerializeField] public Button quitButton;
    [SerializeField] public RectTransform volumePanel;

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
        SceneManager.sceneLoaded += OnPhilippineMapSceneLoaded;
        SceneManager.sceneLoaded += OnAssessmentSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMenuSceneLoaded;
        SceneManager.sceneLoaded -= OnCharacterCreationSceneLoaded;
        SceneManager.sceneLoaded -= OnHouseSceneLoaded;
        SceneManager.sceneLoaded -= OnPhilippineMapSceneLoaded;
        SceneManager.sceneLoaded -= OnAssessmentSceneLoaded;
    }

    public void OnMenuSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Menu"))
        {
            // GET ALL THE NECESSARY COMPONENTS.
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
            playButton.onClick.AddListener(() => this.LoadScene("CharacterCreation") );
            optionsButton.onClick.AddListener(() => this.ShowOptionsPanel() );
            loadButton.onClick.AddListener(() => this.ShowSaveSlots(true) );
            loadPlayerProfileBTN.onClick.AddListener(() => this.LoadScene("House"));
            closeSlotsPanelBTN.onClick.AddListener(() => this.ShowSaveSlots(false));

            // Hide the optionsPanel at first render
            this.optionsPanel.gameObject.SetActive(false);
            this.volumePanel.gameObject.SetActive(false);
        }
    }
    public void OnCharacterCreationSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("CharacterCreation"))
        {
            Button backButton = GameObject.Find("Back Button").GetComponent<Button>();
            Button confirm = GameObject.Find("Confirm").GetComponent<Button>();

            backButton.onClick.AddListener(() => this.LoadScene("Menu") );
            confirm.onClick.AddListener(() => this.LoadScene("House") );
        }
    }
    public void OnHouseSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {
            print("FROM GAME MANAGER : HOUSE SCENE LOADED");

            // GET ALL THE NECESSARY COMPONENTS.
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
    }
    public void OnPhilippineMapSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Philippine Map"))
        {
            print("FROM GAME MANAGER : Philippine Map Loaded");
            Button goToHouseButton = GameObject.Find("Go to House").GetComponent<Button>();
            goToHouseButton.onClick.AddListener(() => this.LoadScene("House"));
        }
    }
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

    public void ShowSaveSlots(bool active)
    {
        if (active)
        {
            saveSlotsPanel.gameObject.SetActive(true);
            LeanTween.scale(saveSlotsPanel.gameObject, new Vector2(0.6188691f, 0.6188691f), .3f);
        }
        else
        {
            LeanTween.scale(saveSlotsPanel.gameObject, new Vector2(0, 0), .3f)
                .setOnComplete(() =>
                {
                    saveSlotsPanel.gameObject.SetActive(false);
                    DataPersistenceManager.instance.playerData = null;
                });
        }
    }

    public void ShowOptionsPanel()
    {
        optionsPanel.gameObject.SetActive(true);
        LeanTween.scale(optionsPanel.gameObject, new Vector3(1.586f, 1.586f, 1.586f), .3f)
            .setEase(LeanTweenType.easeInCubic);
    }

    // Function for closing the options panel.
    public void Close()
    {
        if (soundButton.gameObject.activeInHierarchy)
        {
            LeanTween.scale(optionsPanel.gameObject, new Vector3(0, 0, 0), .3f)
            .setEase(LeanTweenType.easeInCubic)
            .setOnComplete(() => {
                optionsPanel.gameObject.SetActive(false);
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
}
