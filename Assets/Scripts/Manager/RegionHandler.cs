using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegionHandler : MonoBehaviour, IDataPersistence
{
    public static RegionHandler instance; 

    [SerializeField] public CanvasGroup regionsCanvasGroup;
    [SerializeField] public RectTransform categoriesPanel;
    [SerializeField] public PlayerData playerData;
    [SerializeField] public GameObject[] buttons;

    public void ShowCategories(bool show)
    {
        // Set the name of the region.
        string regionName = EventSystem.current.currentSelectedGameObject.name.ToString();

        WordManager.instance.regionName = regionName;
        AssessmentManager.instance.regionName = regionName;

        if (this.IsRegionOpen(regionName) || regionName == "Close")
        {
            this.ResetCategories();
            GetAllAssessments();
            GetAllWordGames();
            this.HideUnavailableCategories();

            if (show)
            {
                regionsCanvasGroup.GetComponent<CanvasGroup>().blocksRaycasts = false;
                Camera.main.gameObject.GetComponent<PanZoom>().enabled = false;
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
            Debug.Log(regionName + " is closed");
        }
    }

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
                    //if (playerData.dunongPoints >= 5)
                    //{
                        AddButtonEventForAssessment(setup);
                    //}
                });
            }
            else if (setup.categoryName == AssessmentSetup.CategoryType.NATIONAL_SYMBOLS)
            {
                buttons[1].transform.GetChild(2).GetComponent<Button>().name = "National Symbols";
                buttons[1].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    //if (playerData.dunongPoints >= 5)
                    //{
                    AddButtonEventForAssessment(setup);
                    //}
                });
            }
            else if (setup.categoryName == AssessmentSetup.CategoryType.PHILIPPINE_MYTHS)
            {
                buttons[2].transform.GetChild(2).GetComponent<Button>().name = "Philippine Myths";
                buttons[2].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    //if (playerData.dunongPoints >= 5)
                    //{
                    AddButtonEventForAssessment(setup);
                    //}
                });
            }
            else if (setup.categoryName == AssessmentSetup.CategoryType.NATIONAL_FESTIVALS)
            {
                buttons[3].transform.GetChild(2).GetComponent<Button>().name = "National Festivals";
                buttons[3].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    //if (playerData.dunongPoints >= 5)
                    //{
                    AddButtonEventForAssessment(setup);
                    //}
                });
            }
            else if (setup.categoryName == AssessmentSetup.CategoryType.NATIONAL_GAMES)
            {
                buttons[4].transform.GetChild(2).GetComponent<Button>().name = "National Games";
                buttons[4].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    //if (playerData.dunongPoints >= 5)
                    //{
                    AddButtonEventForAssessment(setup);
                    //}
                });
            }
            
        }
    }

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
                    if (playerData.dunongPoints >= 5)
                    {
                        AddButtonEventForWordGames(setup);
                    }
                });
            }
            else if (setup.categoryName == WordSetup.CategoryType.NATIONAL_SYMBOLS)
            {
                buttons[1].transform.GetChild(2).GetComponent<Button>().name = "National Symbols";
                buttons[1].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    //if (playerData.dunongPoints >= 5)
                    //{
                        AddButtonEventForWordGames(setup);
                    //}
                });
            }
            else if (setup.categoryName == WordSetup.CategoryType.PHILIPPINE_MYTHS)
            {
                buttons[2].transform.GetChild(2).GetComponent<Button>().name = "Philippine Myths";
                buttons[2].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    //if (playerData.dunongPoints >= 5)
                    //{
                    AddButtonEventForWordGames(setup);
                    //}
                });
            }
            else if (setup.categoryName == WordSetup.CategoryType.NATIONAL_FESTIVALS)
            {
                buttons[3].transform.GetChild(2).GetComponent<Button>().name = "National Festivals";
                buttons[3].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    //if (playerData.dunongPoints >= 5)
                    //{
                    AddButtonEventForWordGames(setup);
                    //}
                });
            }
            else if (setup.categoryName == WordSetup.CategoryType.NATIONAL_GAMES)
            {
                buttons[4].transform.GetChild(2).GetComponent<Button>().name = "National Games";
                buttons[4].transform.GetChild(2).GetComponent<Button>().onClick.AddListener(() =>
                {
                    //if (playerData.dunongPoints >= 5)
                    //{
                    AddButtonEventForWordGames(setup);
                    //}
                });
            }
        }
    }

    public void AddButtonEventForAssessment(AssessmentSetup setup)
    {
        AssessmentManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
        AssessmentManager.instance.StartAssessments(setup.assessments);
        SceneManager.LoadScene(setup.sceneToLoad);
    }

    public void AddButtonEventForWordGames(WordSetup setup)
    {
        WordManager.instance.categoryName = EventSystem.current.currentSelectedGameObject.name.ToString();
        WordManager.instance.StartWordGames(setup.words);
        SceneManager.LoadScene(setup.sceneToLoad);
    }

    public void HideUnavailableCategories()
    {
        foreach (GameObject obj in this.buttons)
        {
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
}
