using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IslandHandler : MonoBehaviour, IDataPersistence
{
    public List<Button> m_RegionButtons;
    public RectTransform m_IslandInformationPanel;
    public RectTransform m_RegionInformationPanel;
    public RectTransform m_RegionInformationPanelContent;
    public PlayerData playerData;

    Dictionary<string, Image> m_Images = new Dictionary<string, Image>();

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnIslandSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnIslandSceneLoaded;
    }

    void OnIslandSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Luzon")
        {
            print("LUZON IS LOADED");
            /** FOR TESTING PURPOSES */
            //this.playerData = new PlayerData();

            Button majorIslandInformationBTN = null;
            Button closeRegionInformationPanelBTN = null;
            Button closeIslandInformationPanelBTN = null;

            // Set the scene to be loaded after assessment is Luzon.
            AssessmentManager.instance.previousSceneToLoad = "Luzon";
            WordManager.instance.previousSceneToLoad = "Luzon";

            this.m_IslandInformationPanel = GameObject.Find("Island Information Panel").GetComponent<RectTransform>();
            this.m_RegionInformationPanel = GameObject.Find("Region Information Panel").GetComponent<RectTransform>();
            this.m_RegionInformationPanelContent = this.m_RegionInformationPanel.GetChild(2).GetChild(0).GetChild(0).GetComponent<RectTransform>();

            majorIslandInformationBTN = GameObject.Find("Information BTN").GetComponent<Button>();
            closeRegionInformationPanelBTN = this.m_RegionInformationPanel.gameObject.transform.GetChild(0).GetComponent<Button>();
            closeIslandInformationPanelBTN = this.m_IslandInformationPanel.gameObject.transform.GetChild(0).GetComponent<Button>();

            GameObject.Find("DP Value").GetComponent<TMPro.TextMeshProUGUI>().text = this.playerData.dunongPoints.ToString(); // Get the text for setting up the DP Value.

            majorIslandInformationBTN.onClick.AddListener(() =>
            {
                LeanTween.scale(this.m_IslandInformationPanel.gameObject, Vector2.one, .2f)
                .setEaseSpring();
            });

            closeRegionInformationPanelBTN.onClick.AddListener(() =>
            {
                LeanTween.scale(this.m_RegionInformationPanel.gameObject, Vector2.zero, .2f)
                .setEaseSpring();
            });

            closeIslandInformationPanelBTN.onClick.AddListener(() =>
            {
                LeanTween.scale(this.m_IslandInformationPanel.gameObject, Vector2.zero, .2f)
                .setEaseSpring();
            });

            this.m_IslandInformationPanel.localScale = Vector2.zero;
            this.m_RegionInformationPanel.localScale = Vector2.zero;

            this.GetAllRegionImages();
            this.GetAllRegionButtons();

            if (QuestManager.instance != null)
                QuestManager.instance.GetListOfQuests();
        }
    }

    public void GetAllRegionImages()
    {
        Transform transform = GameObject.Find("Luzon").transform.GetChild(0);

        foreach (Transform image in transform)
        {
            if (!m_Images.ContainsKey(image.name))
                m_Images.Add(image.name, image.GetComponent<Image>());
        }
    }

    public void GetAllRegionButtons()
    {
        Transform locations = GameObject.Find("Luzon").transform.GetChild(1);

        foreach (Transform button in locations)
        {
            RegionData foundRegionData = this.FindRegionDataWithName(button.name);

            button.GetComponent<Button>().onClick.AddListener(() =>
            {
                LeanTween.scale(this.m_RegionInformationPanel.gameObject, Vector2.one, .2f)
                .setEaseSpring();

                // Set the regionName attributes for AssessmentManager and WordManager
                AssessmentManager.instance.regionName = button.name;
                WordManager.instance.regionName = button.name;

                AssessmentSetup typeOfAssessment = button.GetComponent<AssessmentSetup>();
                WordSetup typeOfWord = button.GetComponent<WordSetup>();

                Transform regionPanelInfoTransform = this.m_RegionInformationPanel.transform;

                regionPanelInfoTransform.GetChild(1).GetComponent<Image>().sprite = this.m_Images[button.name].sprite;

                m_RegionInformationPanelContent.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = foundRegionData.regionName;
                m_RegionInformationPanelContent.GetChild(1).gameObject.GetComponent<TMPro.TextMeshProUGUI>().text = foundRegionData.information;

                regionPanelInfoTransform.GetChild(regionPanelInfoTransform.childCount - 1).GetComponent<Button>().onClick.AddListener(() =>
                {
                    if (!this.IsAllQuestCompleted())
                        print("All Quest should be completed");

                    if (this.playerData.dunongPoints < this.playerData.requiredDunongPointsToPlay)
                        print("Dunong points does not reached the required dunong points to play.");

                    // Check if all the quest is completed AND Check if the dunong points is valid.
                    if (this.IsAllQuestCompleted() && this.playerData.dunongPoints >= this.playerData.requiredDunongPointsToPlay)
                    {
                        this.playerData.dunongPoints -= this.playerData.requiredDunongPointsToPlay;

                        if (typeOfAssessment != null)
                        {
                            AssessmentManager.instance.StartAssessments(typeOfAssessment.assessments);
                            SceneManager.LoadScene(typeOfAssessment.sceneToLoad);
                        }
                        else if (typeOfWord != null)
                        {
                            WordManager.instance.StartWordGames(typeOfWord.words);
                            SceneManager.LoadScene(typeOfWord.sceneToLoad);
                        }
                        return;
                    }
                });

            });

            // Change the color of the region image if the found RegionData is open.
            Color greenColor;
            ColorUtility.TryParseHtmlString("#B0FF96", out greenColor);

            if (foundRegionData.isOpen)
            { 
                button.gameObject.SetActive(true);
                this.m_Images[button.name].color = greenColor;
            }
               
        }
    }

    public RegionData FindRegionDataWithName(string regionName)
    {
        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == regionName.ToUpper())
            {
                return regionData;
            }
        }

        return null;
    }

    private bool IsAllQuestCompleted()
    {
        return this.playerData.currentQuests.Count == 0;
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
