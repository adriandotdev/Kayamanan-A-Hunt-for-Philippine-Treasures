using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegionHandler : MonoBehaviour, IDataPersistence
{
    public static RegionHandler instance;

    [SerializeField] public CanvasGroup UICanvasGroup;
    [SerializeField] public RectTransform alertBox;
    [SerializeField] public TMPro.TextMeshProUGUI categoriesPanelTitle;
    [SerializeField] public CanvasGroup regionsCanvasGroup;
    [SerializeField] public RectTransform categoriesPanel;
    [SerializeField] public PlayerData playerData;
    [SerializeField] public GameObject[] buttons;

    public float categoriesPanelScale;

    private void Start()
    {
        this.categoriesPanelScale = 0.86f;
        this.categoriesPanel.localScale = Vector2.zero;
    }

    public void ShowCategories(bool show)
    {
        // Set the name of the region.
        string regionName = EventSystem.current.currentSelectedGameObject.name.ToString();

        this.categoriesPanelTitle.text = regionName + " Categories";

        WordManager.instance.regionName = regionName;
        AssessmentManager.instance.regionName = regionName;

        if (this.IsRegionOpen(regionName) || regionName == "Close")
        {
            this.ResetCategories();
            this.GetAllAssessments();
            this.GetAllWordGames();
            this.HideUnavailableCategories();

            if (show)
            {
                this.UICanvasGroup.interactable = false;
                this.regionsCanvasGroup.blocksRaycasts = false;
                Camera.main.gameObject.GetComponent<PanZoom>().enabled = false;
                categoriesPanel.gameObject.SetActive(show);
                LeanTween.scale(this.categoriesPanel.gameObject, new Vector3(this.categoriesPanelScale, this.categoriesPanelScale, 1), .2f);
            }
            else
            {
                this.UICanvasGroup.interactable = true;
                this.regionsCanvasGroup.blocksRaycasts = true;
                Camera.main.gameObject.GetComponent<PanZoom>().enabled = true;
                LeanTween.scale(this.categoriesPanel.gameObject, Vector2.zero, .2f)
                    .setOnComplete(() => categoriesPanel.gameObject.SetActive(false));
            }
        }
        else
        {
            this.alertBox.gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = regionName + " is still closed";
            this.alertBox.gameObject.SetActive(true);
            StartCoroutine(CloseAlertBox());
        }
    }

    IEnumerator CloseAlertBox()
    {
        yield return new WaitForSeconds(.9f);

        this.alertBox.gameObject.SetActive(false);
    }

    /**<summary>
     *  Ang function na ito ay hahanapin ang lahat ng game
     *  object na may 'AssessmentSetup' na script.
     * </summary> */
    public void GetAllAssessments()
    {
        AssessmentSetup[] assessmentSetups = EventSystem.current.currentSelectedGameObject.GetComponents<AssessmentSetup>();

        foreach(AssessmentSetup setup in assessmentSetups)
        {
            if (setup.categoryName == AssessmentSetup.CategoryType.NATIONAL_HEROES)
            {
                buttons[0].transform.GetChild(2).GetComponent<Button>().name = "National Heroes";
                buttons[0].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    AddButtonEventForAssessment(setup);
                });
            }
            else if (setup.categoryName == AssessmentSetup.CategoryType.NATIONAL_SYMBOLS)
            {
                buttons[1].transform.GetChild(2).GetComponent<Button>().name = "National Symbols";
                buttons[1].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                  AddButtonEventForAssessment(setup);
                });
            }
            else if (setup.categoryName == AssessmentSetup.CategoryType.PHILIPPINE_MYTHS)
            {
                buttons[2].transform.GetChild(2).GetComponent<Button>().name = "Philippine Myths";
                buttons[2].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    AddButtonEventForAssessment(setup);
                });
            }
            else if (setup.categoryName == AssessmentSetup.CategoryType.NATIONAL_FESTIVALS)
            {
                buttons[3].transform.GetChild(2).GetComponent<Button>().name = "National Festivals";
                buttons[3].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    AddButtonEventForAssessment(setup);
                });
            }
            else if (setup.categoryName == AssessmentSetup.CategoryType.NATIONAL_GAMES)
            {
                buttons[4].transform.GetChild(2).GetComponent<Button>().name = "National Games";
                buttons[4].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                  AddButtonEventForAssessment(setup);
                });
            }
            
        }
    }

    /**
     * <summary>
     *  Ang function na ito ay hahanapin ang lahat ng game
     *  object na may 'WordSetup' script.
     * </summary> 
     */
    public void GetAllWordGames()
    {
        WordSetup[] wordGamesSetups = EventSystem.current.currentSelectedGameObject.GetComponents<WordSetup>();

        foreach (WordSetup setup in wordGamesSetups)
        {
            if (setup.categoryName == WordSetup.CategoryType.NATIONAL_HEROES)
            {
                buttons[0].transform.GetChild(2).GetComponent<Button>().name = "National Heroes";
                buttons[0].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                  AddButtonEventForWordGames(setup);
                });
            }
            else if (setup.categoryName == WordSetup.CategoryType.NATIONAL_SYMBOLS)
            {
                buttons[1].transform.GetChild(2).GetComponent<Button>().name = "National Symbols";
                buttons[1].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {

                        AddButtonEventForWordGames(setup);
                    
                });
            }
            else if (setup.categoryName == WordSetup.CategoryType.PHILIPPINE_MYTHS)
            {
                buttons[2].transform.GetChild(2).GetComponent<Button>().name = "Philippine Myths";
                buttons[2].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
              
                        AddButtonEventForWordGames(setup);
                    
                });
            }
            else if (setup.categoryName == WordSetup.CategoryType.NATIONAL_FESTIVALS)
            {
                buttons[3].transform.GetChild(2).GetComponent<Button>().name = "National Festivals";
                buttons[3].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
 
                        AddButtonEventForWordGames(setup);
                    
                });
            }
            else if (setup.categoryName == WordSetup.CategoryType.NATIONAL_GAMES)
            {
                buttons[4].transform.GetChild(2).GetComponent<Button>().name = "National Games";
                buttons[4].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
     
                        AddButtonEventForWordGames(setup);
                    
                });
            }
        }
    }

    public void ShowInsufficientDunongPointsMessage()
    {
        SoundManager.instance.PlaySound("Warning Notification");

        if (!this.alertBox.gameObject.activeInHierarchy)
        {
            this.alertBox.gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = "Insufficient Dunong Points.";
            this.alertBox.gameObject.SetActive(true);
            StartCoroutine(CloseAlertBox());
        }
    }

    public void AddButtonEventForAssessment(AssessmentSetup setup)
    {
        if (playerData.dunongPoints >= 5)
        {
            this.playerData.dunongPoints -= 5;
            AssessmentManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
            AssessmentManager.instance.StartAssessments(setup.assessments);
            SceneManager.LoadScene(setup.sceneToLoad);

            DataPersistenceManager.instance.SaveGame();
        }
        else
        {
            this.ShowInsufficientDunongPointsMessage();
        }
    }

    public void AddButtonEventForWordGames(WordSetup setup)
    {
        if (this.playerData.dunongPoints >= 5)
        {
            this.playerData.dunongPoints -= 5;
            WordManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
            WordManager.instance.StartWordGames(setup.words);
            SceneManager.LoadScene(setup.sceneToLoad);
        }
        else
        {
            this.ShowInsufficientDunongPointsMessage();
        }
    }

    public void HideUnavailableCategories()
    {
        foreach (GameObject obj in this.buttons)
        {
            // If the name of the gameobject is equal to 'Category Name BTN' then it means that it is not available to current region.
            if (obj.transform.GetChild(2).GetComponent<Button>().name == "Category Name BTN")
            {
                obj.gameObject.SetActive(false);
            }
        }
    }

    public void ResetCategories()
    {
        foreach (GameObject obj in this.buttons)
        {
            obj.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
            obj.transform.GetChild(2).GetComponent<Button>().name = "Category Name BTN";
            obj.gameObject.SetActive(true);
        }
    }

    public bool IsRegionOpen(string nameOfRegion)
    {
        foreach (RegionData region in this.playerData.regionsData)
        {
            if (region.regionName.ToUpper().Equals(nameOfRegion.ToUpper()))
            {
                if (region.isOpen) return true;
            }
        }
        return false;
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

    public void Test()
    {
        DataPersistenceManager.instance.playerData.dunongPoints = 25;
        DataPersistenceManager.instance.SaveGame();
        GameObject.Find("DP Value").GetComponent<TMPro.TextMeshProUGUI>().text = "25";
    }
}
