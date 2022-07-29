using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
            try
            {
                this.currentIndex = 0; // reset the current index to '0'.
                this.correctAnswers = new List<bool>(); // instantiate again the correctAnswers variable.

                questionsPanel = GameObject.Find("Layout").GetComponent<RectTransform>();
                questionLabel = GameObject.Find("Question").GetComponent<TMPro.TextMeshProUGUI>();
                scorePanel = GameObject.Find("Score Panel").GetComponent<RectTransform>();
                scoreLabel = GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
                stars = GameObject.FindGameObjectsWithTag("Score Star");

                scorePanel.gameObject.SetActive(false);

                foreach (GameObject star in stars)
                {
                    star.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Empty Star");
                }
                choices = GameObject.FindGameObjectsWithTag("Choices");

                FisherYates.Shuffle(this.shuffled);
                questionLabel.text = this.shuffled[this.currentIndex].question.ToString();
                this.SetChoices();
                this.AddEvents();
            }
            catch (System.Exception e)
            {

            }
        }
    }

    public void StartAssessments(Assessment[] assessments)
    {
        this.assessments = assessments;
        this.shuffled = new Assessment[this.assessments.Length];
        Array.Copy(this.assessments, this.shuffled, this.assessments.Length);
    }

    public void SetNextQuestion()
    {
        this.currentIndex += 1;

        // Check if there are still questions to load.
        if (this.currentIndex < this.shuffled.Length)
        {
            this.questionLabel.text = this.shuffled[this.currentIndex].question.ToString();
            this.SetChoices();
        }
        // If all the questions are loaded.
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

            scoreLabel.text = noOfCorrectAns + "/" + this.shuffled.Length;

            this.SetRegionHighscore(noOfCorrectAns);

            this.SetNumberOfStars(noOfCorrectAns);

            this.CheckIfNextRegionIsReadyToOpen();

            this.CollectAllRewards();

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
                bool isCorrect = this.answer.ToUpper().Equals(this.shuffled[this.currentIndex].correctAnswer.ToUpper());

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
        string[] shuffledAssessmentChoices = new string[this.shuffled[this.currentIndex].choices.Length];

        Array.Copy(this.shuffled[this.currentIndex].choices, shuffledAssessmentChoices, shuffledAssessmentChoices.Length);

        FisherYates.Shuffle(shuffledAssessmentChoices);

        for (int i = 0; i < shuffledAssessmentChoices.Length; i++)
        {
            this.choices[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = shuffledAssessmentChoices[i];
        }
    }

    // This function updates the highest score of the specified region.
    public void SetRegionHighscore(int noOfCorrectAnswers)
    {
        // Get all the regions from 'playerData.regionsData'.
        foreach(RegionData regionData in playerData.regionsData)
        {
            if (regionData.regionName == this.regionName.ToUpper())
            {
                foreach (Category category in regionData.categories)
                {
                    if (category.categoryName.ToUpper() == this.categoryName.ToUpper())
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

    // This function will set the number of stars in a specified category of a current region.
    public void SetNumberOfStars(int noOfCorrectAnswers)
    {
        int noOfStars = 0;

        /** 
         <summary>
            The passing score is computed by
            dividing the number of assessment items
            by 2 and adding 1.
        </summary>
         */
        int passingScore = this.shuffled.Length / 2 + 1;

        // If the result of assessment is 0, then the number of stars will remain.
        if (noOfCorrectAnswers == 0)
        {
            return;
        }

        /** 
         * Evaluate the number of stars that the 
            player will get based on assessment's result.
         */
        if (noOfCorrectAnswers == this.shuffled.Length)
            noOfStars = 3;
        else if (noOfCorrectAnswers >= passingScore)
            noOfStars = 2;
        else if (noOfCorrectAnswers < passingScore)
            noOfStars = 1;
        
        // Load the fill star asset from the 'Resources' folder.
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

    // This function determines if the next region is going to be opened.
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

        if (regionNumber < this.playerData.regionsData.Count)
        {
            print("TEST : REGION IS OPEN: " + (regionNumber + 1));
            this.playerData.regionsData[regionNumber].isOpen = true;
        }
    }

    public bool AllCategoriesCompleted()
    {
        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
            {
                foreach (Category category in regionData.categories)
                {
                    if (category.noOfStars < 3)
                        return false;
                }
            }
        }
        return true;
    }

    public bool CheckIfRegionCollectiblesIsCollected()
    {
        foreach (Collectible collectible in playerData.notebook.collectibles)
        {
            if (collectible.regionName.ToUpper() == this.regionName.ToUpper())
            {
                if (!collectible.isCollected)
                    return false;
            }
        }
        return true;
    }

    public void CollectAllRewards()
    {
        if (AllCategoriesCompleted() != true)
            return;

        if (!CheckIfRegionCollectiblesIsCollected())
            SoundManager.instance.PlaySound("Unlock Item");

        foreach (Collectible collectible in playerData.notebook.collectibles)
        {
            if (collectible.regionName.ToUpper() == this.regionName.ToUpper())
            {
                if (collectible.isCollected)
                    return;

                collectible.isCollected = true;
            }
        }

        SceneManager.LoadSceneAsync("Collectibles", LoadSceneMode.Additive);
    }

    public void LoadScene ()
    {
        SceneManager.LoadScene("Assessment");

        FisherYates.Shuffle(this.shuffled);
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
    static System.Random random = new System.Random();

    public static Assessment[] Shuffle(Assessment[] toShuffle)
    {
        Assessment[] newArray = toShuffle;
        System.Random random = new System.Random();

        for (int i = newArray.Length - 1; i > 0; i--)
        {
            int randomNum = random.Next(newArray.Length - 1);

            Assessment temp = newArray[i];
            newArray[i] = newArray[randomNum];
            newArray[randomNum] = temp;
        }

        return newArray;
    }

    public static Word[] Shuffle(Word[] toShuffle)
    {
        Word[] newArray = toShuffle;
        System.Random random = new System.Random();

        for (int i = newArray.Length - 1; i > 0; i--)
        {
            int randomNum = random.Next(newArray.Length - 1);

            Word temp = newArray[i];
            newArray[i] = newArray[randomNum];
            newArray[randomNum] = temp;
        }

        return newArray;
    }

    public static char[] Shuffle(char[] toShuffle)
    {
        char[] newArray = toShuffle;
        System.Random random = new System.Random();

        for (int i = newArray.Length - 1; i > 0; i--)
        {
            int randomNum = random.Next(newArray.Length - 1);

            char temp = newArray[i];
            newArray[i] = newArray[randomNum];
            newArray[randomNum] = temp;
        }

        return newArray;
    }

    public static string[] Shuffle(string[] toShuffle)
    {
        string[] newArray = toShuffle;
        System.Random random = new System.Random();

        for (int i = newArray.Length - 1; i > 0; i--)
        {
            int randomNum = random.Next(newArray.Length - 1);

            string temp = newArray[i];
            newArray[i] = newArray[randomNum];
            newArray[randomNum] = temp;
        }

        return newArray;
    }

    //public static Assessment[] shuffle(Assessment[] assessments)
    //{
    //    Assessment[] newAssessments = new Assessment[assessments.Length];

    //    Array.Copy(assessments, newAssessments, assessments.Length);

    //    int randomNumber = 0;
    //    int endPointer = assessments.Length - 1;

    //    for (int i = 0; i < (endPointer - 1); i++)
    //    {
    //        randomNumber = i + random.Next(endPointer - 1);
    //        //randomNumber = Random.Range(0, endPointer);

    //        Assessment temp = newAssessments[randomNumber];
    //        newAssessments[randomNumber] = newAssessments[i];
    //        newAssessments[endPointer] = temp;

    //        //endPointer--;

    //        //if (endPointer == 0) return newAssessments;
    //    }

    //    return newAssessments;
    //}

    //public static Word[] shuffle(Word[] assessments)
    //{
    //    Word[] newAssessments = new Word[assessments.Length];

    //    Array.Copy(assessments, newAssessments, assessments.Length);

    //    int randomNumber = 0;
    //    int endPointer = assessments.Length - 1;

    //    for (int i = 0; i < (endPointer - 1); i++)
    //    {
    //        randomNumber = i + random.Next(endPointer - 1);
    //        //randomNumber = Random.Range(0, endPointer);

    //        Word temp = newAssessments[randomNumber];
    //        newAssessments[randomNumber] = newAssessments[i];
    //        newAssessments[endPointer] = temp;

    //        //endPointer--;

    //        //if (endPointer == 0) return newAssessments;
    //    }

    //    return newAssessments;
    //}

    //public static char[] shuffle(char[] characters)
    //{
    //    // a b c d
    //    char[] newAssessments = characters;

    //    for (int i = newAssessments.Length - 1; i >= 1; i--)
    //    {
    //        int j = random.Next(0, i);

    //    }

    //    return null;
    //}
}
