using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class AssessmentTrigger : MonoBehaviour, IDataPersistence
{
    public static bool initialized = false;

    [Header("Region Canvas Group")]
    [SerializeField] public GameObject regionsCanvasGroup;

    [Header("Categories Panel")]
    [SerializeField] private RectTransform categoriesPanel;

    [Header("Categories Button")]
    [SerializeField] public GameObject[] categoriesButtons;

    [Header("Heroes")] 
    [SerializeField] public Assessment[] heroes;

    [Header("Symbols")]
    [SerializeField] public Assessment[] symbols;

    [Header("Myths")]
    [SerializeField] public Assessment[] myths;

    [Header("Festivals")]
    [SerializeField] public Assessment[] festivals;

    [Header("Games")]
    [SerializeField] public Assessment[] games;

    [Header("Player Data")]
    public PlayerData playerData;

    public void StartHeroesAssessment()
    {
        if (this.playerData.dunongPoints >= 5)
        {
            AssessmentManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
            AssessmentManager.instance.StartAssessments(this.heroes);
            SceneManager.LoadScene("Assessment");
        }
        else
        {
            Debug.Log("Insufficient Dunong Points");
        }
    }

    public void StartSymbolsAssessment()
    {
        if (this.playerData.dunongPoints >= 5)
        {
            AssessmentManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
            AssessmentManager.instance.StartAssessments(this.symbols);
            SceneManager.LoadScene("Assessment");
        }
        else
        {
            Debug.Log("Insufficient Dunong Points");
        }
    }

    public void StartMythsAssessment()
    {
        AssessmentManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
        AssessmentManager.instance.StartAssessments(this.myths);
        SceneManager.LoadScene("Assessment");
    }

    public void StartFestivalsAssessment()
    {
        AssessmentManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
        AssessmentManager.instance.StartAssessments(this.festivals);
        SceneManager.LoadScene("Assessment");
    }

    public void StartGamesAssessment()
    {
        AssessmentManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
        AssessmentManager.instance.StartAssessments(this.games);
        SceneManager.LoadScene("Assessment");
    }

    public void ShowCategories(bool show)
    {
        // Set the name of the region.
        AssessmentManager.instance.regionName = EventSystem.current.currentSelectedGameObject.name.ToString();

        if (this.IsRegionOpen(AssessmentManager.instance.regionName) || AssessmentManager.instance.regionName == "Close")
        {
            if (show)
            {
                regionsCanvasGroup.GetComponent<CanvasGroup>().blocksRaycasts = false;
                Camera.main.gameObject.GetComponent<PanZoom>().enabled = false;
                ResetAllCategories();
                ShowOnlyAvailableCategories();
                RemoveEventsToCategoryButton();
                AddEventsToCategoryButton();
                categoriesPanel.gameObject.SetActive(show);
            }
            else
            {
                regionsCanvasGroup.GetComponent<CanvasGroup>().blocksRaycasts = true;
                Camera.main.gameObject.GetComponent<PanZoom>().enabled = true;
                categoriesPanel.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.Log(AssessmentManager.instance.regionName + " is closed");
        }
    }

    public bool IsRegionOpen(string nameOfRegion)
    {
        foreach(RegionData region in this.playerData.regionsData)
        {
            if (region.regionName.ToUpper().Equals(nameOfRegion.ToUpper()))
            {
                if (region.isOpen) return true;
            }
        }
        return false;
    }

    public void AddEventsToCategoryButton()
    {
        categoriesButtons[0].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(StartHeroesAssessment);
        categoriesButtons[0].transform.GetChild(2).gameObject.name = "National Heroes";

        categoriesButtons[1].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(StartSymbolsAssessment);
        categoriesButtons[1].transform.GetChild(2).gameObject.name = "National Symbols";

        categoriesButtons[2].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(StartMythsAssessment);
        categoriesButtons[2].transform.GetChild(2).gameObject.name = "Philippine Myths";

        categoriesButtons[3].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(StartFestivalsAssessment);
        categoriesButtons[3].transform.GetChild(2).gameObject.name = "National Festivals";

        categoriesButtons[4].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.AddListener(StartGamesAssessment);
        categoriesButtons[4].transform.GetChild(2).gameObject.name = "National Games";
    }

    public void RemoveEventsToCategoryButton()
    {
        categoriesButtons[0].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        categoriesButtons[1].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        categoriesButtons[2].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        categoriesButtons[3].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
        categoriesButtons[4].transform.GetChild(2).gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void ResetAllCategories()
    {
        categoriesButtons[0].SetActive(true);
        categoriesButtons[1].SetActive(true);
        categoriesButtons[2].SetActive(true);
        categoriesButtons[3].SetActive(true);
        categoriesButtons[4].SetActive(true);
    }

    // Show only available categories for specific region.
    public void ShowOnlyAvailableCategories()
    {
        if (heroes.Length == 0)
        {
            categoriesButtons[0].SetActive(false);
        }

        if (symbols.Length == 0)
        {
            categoriesButtons[1].SetActive(false);
        }

        if (myths.Length == 0)
        {
            categoriesButtons[2].SetActive(false);
        }

        if (festivals.Length == 0)
        {
            categoriesButtons[3].SetActive(false);
        }

        if (games.Length == 0)
        {
            categoriesButtons[4].SetActive(false);
        }
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
