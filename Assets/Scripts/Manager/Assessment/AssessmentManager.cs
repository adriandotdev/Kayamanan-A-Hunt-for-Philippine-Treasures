using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class AssessmentManager : MainGame, IDataPersistence
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
    public Assessment[] assessments;
    private string answer;
    private int currentIndex;

    // Buttons
    public GameObject[] choices = null;

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

                foreach (GameObject star in stars)
                {
                    star.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Empty Star");
                }
                choices = GameObject.FindGameObjectsWithTag("Choices");

                FisherYates.Shuffle(this.shuffled);
                questionLabel.text = ((Assessment)this.shuffled[this.currentIndex]).question.ToString();
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
            this.questionLabel.text = ((Assessment)this.shuffled[this.currentIndex]).question.ToString();
            this.SetChoices();
        }
        // If all the questions are loaded.
        else
        {
            questionsPanel.gameObject.SetActive(false); // Disable the question panel.
            //scorePanel.gameObject.SetActive(true); // Enable the score panel.

            int noOfCorrectAns = this.CountCorrectAnswers();

            scoreLabel.text = noOfCorrectAns + "/" + this.shuffled.Length;

            this.SetRegionHighestScore(noOfCorrectAns);

            this.ShowStars(noOfCorrectAns);
            this.ShowScorePanel(this.CountNoOfStarsToShow(noOfCorrectAns));

            this.CheckIfNextRegionIsReadyToOpen();

            //this.CollectAllRewards();

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
                bool isCorrect = this.answer.ToUpper().Equals(((Assessment)this.shuffled[this.currentIndex]).correctAnswer.ToUpper());

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
        string[] shuffledAssessmentChoices = new string[((Assessment)this.shuffled[this.currentIndex]).choices.Length];

        Array.Copy(((Assessment)this.shuffled[this.currentIndex]).choices, shuffledAssessmentChoices, shuffledAssessmentChoices.Length);

        FisherYates.Shuffle(shuffledAssessmentChoices);

        for (int i = 0; i < shuffledAssessmentChoices.Length; i++)
        {
            this.choices[i].transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = shuffledAssessmentChoices[i];
        }
    }

    // This function updates the highest score of the specified region.
    //public void SetRegionHighscore(int noOfCorrectAnswers)
    //{
    //    // Get all the regions from 'playerData.regionsData'.
    //    foreach(RegionData regionData in playerData.regionsData)
    //    {
    //        if (regionData.regionName == this.regionName.ToUpper())
    //        {
    //            foreach (Category category in regionData.categories)
    //            {
    //                if (category.categoryName.ToUpper() == this.categoryName.ToUpper())
    //                {
    //                    if (noOfCorrectAnswers > category.highestScore)
    //                    {
    //                        category.highestScore = noOfCorrectAnswers;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //}

    public void ShowScorePanel(int noOfCorrectAnswers)
    {
        TweeningManager.instance.OpenScorePanel(noOfCorrectAnswers, () =>
        {
            this.CollectAllRewards();
        });
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

    public static System.Object[] Shuffle(System.Object[] toShuffle)
    {
        System.Object[] newArray = toShuffle;

        System.Random random = new System.Random();

        for (int i = newArray.Length - 1; i > 0; i--)
        {
            int randomNum = random.Next(newArray.Length - 1);

            System.Object temp = newArray[i];
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
