using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AssessmentManager : MonoBehaviour, IDataPersistence
{
    public static AssessmentManager instance; 

    [Header("UI Elements")]
    public TMPro.TextMeshProUGUI questionLabel;
    public RectTransform questionsPanel;
    public RectTransform scorePanel;
    public TMPro.TextMeshProUGUI scoreLabel;

    [Header("UI Elements (Stars)")]
    public GameObject[] stars;

    [Header("Properties for Assessment")]
    public string regionName;
    public string categoryName;
    public Assessment[] assessments;
    public Assessment[] shuffled;
    private string answer;
    public List<bool> correctAnswers = new List<bool>();
    private int currentIndex;

    // Buttons
    public GameObject[] choices = null;

    [Header("Player Data")]
    public PlayerData playerData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnAssessmentSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnAssessmentSceneLoaded;
    }

    private void OnAssessmentSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Assessment"))
        {
            Debug.Log("Assessment Scene Loaded");

            this.currentIndex = 0;
            this.correctAnswers = new List<bool>();

            questionLabel = GameObject.Find("Question").GetComponent<TMPro.TextMeshProUGUI>();
            questionsPanel = GameObject.Find("Layout").GetComponent<RectTransform>();
            scorePanel = GameObject.Find("Score Panel").GetComponent<RectTransform>();
            scoreLabel = GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
            stars = GameObject.FindGameObjectsWithTag("Score Star");

            scorePanel.gameObject.SetActive(false);
            foreach (GameObject star in stars)
            {
                star.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Empty Star");
            }
            choices = GameObject.FindGameObjectsWithTag("Choices");

            FisherYates.shuffle(this.assessments);
            questionLabel.text = assessments[this.currentIndex].question.ToString();
            this.SetChoices();
            this.AddEvents();
        }
    }

    public void StartAssessments(Assessment[] assessments)
    {
        this.assessments = assessments;
    }

    public void SetNextQuestion()
    {
        this.currentIndex += 1;

        // Check if there are still questions to load.
        if (this.currentIndex < this.assessments.Length)
        {
            this.questionLabel.text = assessments[this.currentIndex].question.ToString();
            this.SetChoices();
        }
        else
        {
            questionsPanel.gameObject.SetActive(false); // Disable the question panel.
            scorePanel.gameObject.SetActive(true); // Enable the score panel.

            int noOfCorrectAns = 0;

            for (int i = 0; i < correctAnswers.Count; i++)
            {
                if (this.correctAnswers[i])
                {
                    noOfCorrectAns++;
                }
            }

            scoreLabel.text = noOfCorrectAns + "/" + this.assessments.Length;

            this.SetRegionScore(noOfCorrectAns);

            this.ShowStars(noOfCorrectAns);

            this.CheckIfNextRegionIsReadyToOpen();

            DataPersistenceManager.instance.SaveGame();
        }
    }

    public void AddEvents()
    {
        /** A function that adds events to all 4 buttons in the assessment or quiz. */
        foreach(GameObject choice in choices)
        {
            choice.GetComponent<Button>().onClick.AddListener(() =>
            {
                // When the button is clicked, get the text of the text mesh pro under the clicked button.
                this.answer = choice.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;

                /** I-Check if ang text ng clinicked na button (which is from the TextMeshPro under ng button) is equal
                doon sa correct answer na naka store sa assessment instance based doon sa currentIndex natin. */ 
                bool isCorrect = this.answer.ToUpper().Equals(assessments[this.currentIndex].correctAnswer.ToUpper());

                // I-add sa boolean na List.
                this.correctAnswers.Add(isCorrect);

                // Set the next question.
                this.SetNextQuestion();
            });
        }
    }

    // SET ALL THE CHOICES TO 4 BUTTONS.
    public void SetChoices()
    {
        for (int i = 0; i < assessments[this.currentIndex].choices.Length; i++)
        {
            this.choices[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = assessments[this.currentIndex].choices[i];
        }
    }

    public void SetRegionScore(int noOfCorrectAnswers)
    {
        // Get all the regions.
        foreach(RegionData regionData in playerData.regionsData)
        {
            if (regionData.regionName == this.regionName.ToUpper())
            {
                foreach (Category category in regionData.categories)
                {
                    if (category.categoryName == this.categoryName)
                    {
                        if (noOfCorrectAnswers > category.highestScore)
                        {
                            category.highestScore = noOfCorrectAnswers;
                        }
                    }
                }
            }
        }
    }

    public void ShowStars(int noOfCorrectAnswers)
    {
        int noOfStars = 0;
        int passingScore = this.assessments.Length / 2 + 1;

        if (noOfCorrectAnswers == 0)
        {
            return;
        }

        if (noOfCorrectAnswers == this.assessments.Length)
            noOfStars = 3;
        else if (noOfCorrectAnswers >= passingScore)
            noOfStars = 2;
        else if (noOfCorrectAnswers < passingScore)
            noOfStars = 1;
        
        for (int i = 0; i < noOfStars; i++)
        {
            this.stars[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Fill Star");
        }
        
        foreach(RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
            {
                foreach(Category category in regionData.categories)
                {
                    if (category.categoryName.ToUpper() == this.categoryName.ToUpper())
                    {
                        if (category.noOfStars < noOfStars)
                            category.noOfStars = noOfStars;
                    }
                }
            }
        }
    }

    public void CheckIfNextRegionIsReadyToOpen()
    {
        int regionNumber = 0;

        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
            {
                regionNumber = regionData.regionNumber;

                foreach (Category category in regionData.categories)
                {
                    if (category.noOfStars < 2)
                    {
                        return;
                    }
                }
            }
        }

        print("REGION IS OPEN: " + regionNumber);
        this.playerData.regionsData[regionNumber].isOpen = true;
    }

    public void LoadScene ()
    {
        SceneManager.LoadScene("Assessment");

        FisherYates.shuffle(this.assessments);
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

[System.Serializable]
public class Assessment
{
    public string question;
    public string correctAnswer; // Heto yung correct answer
    public string[] choices; // Mga wrong answers
}

[System.Serializable]
public class FisherYates
{
    public static Assessment[] shuffle(Assessment[] assessments)
    {
        Assessment[] newAssessments = assessments;

        int randomNumber = 0;
        int endPointer = assessments.Length - 1;

        for (int i = 0; i < newAssessments.Length; i++)
        {
            randomNumber = Random.Range(0, endPointer);

            Assessment temp = assessments[randomNumber];
            assessments[randomNumber] = assessments[endPointer];
            assessments[endPointer] = temp;

            endPointer--;

            if (endPointer == 0) return newAssessments;
        }

        return newAssessments;
    }

    public static Word[] shuffle(Word[] assessments)
    {
        Word[] newAssessments = assessments;

        int randomNumber = 0;
        int endPointer = assessments.Length - 1;

        for (int i = 0; i < newAssessments.Length; i++)
        {
            randomNumber = Random.Range(0, endPointer);

            Word temp = assessments[randomNumber];
            assessments[randomNumber] = assessments[endPointer];
            assessments[endPointer] = temp;

            endPointer--;

            if (endPointer == 0) return newAssessments;
        }

        return newAssessments;
    }

    public static char[] shuffle(char[] characters)
    {
        char[] charactersTemp = characters;

        int randomNumber = 0;
        int endPointer = characters.Length - 1;

        for (int i = 0; i < charactersTemp.Length; i++)
        {
            randomNumber = Random.Range(0, endPointer);

            char temp = characters[randomNumber];
            characters[randomNumber] = characters[endPointer];
            characters[endPointer] = temp;

            endPointer--;

            if (endPointer == 0) return characters;
        }

        return charactersTemp;
    }
}
