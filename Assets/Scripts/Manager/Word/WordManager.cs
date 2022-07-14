using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordManager : MonoBehaviour, IDataPersistence
{
    public static WordManager instance;

    [Header("UI")]
    public RectTransform wordContainer;
    public RectTransform shuffledContainer;
    public TMPro.TextMeshProUGUI questionLabel;
    public Button confirmButton;

    [Header("Letter Button (Prefab)")]
    public Button letter;

    [Header("Word Games Properties")]
    public string regionName;
    public string categoryName;

    public Word[] words;
    public int currentIndex = 0;
    public List<bool> correctAnswers;

    [Header("Score Panel")]
    public RectTransform scorePanel;
    public TMPro.TextMeshProUGUI scoreLabel;
    public GameObject[] stars;

    [Header("Player Data")]
    public PlayerData playerData;

    private void Awake()
    {
        if (instance == null )
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            return;
        }
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnWordGameSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnWordGameSceneLoaded;
    }

    public void OnWordGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Word Games"))
        {
            this.currentIndex = 0;
            this.correctAnswers = new List<bool>();

            this.wordContainer = GameObject.Find("Answered").GetComponent<RectTransform>();
            this.shuffledContainer = GameObject.Find("Shuffled Letters").GetComponent<RectTransform>();
            this.questionLabel = GameObject.Find("Question").GetComponent<TMPro.TextMeshProUGUI>();
            this.confirmButton = GameObject.Find("Confirm Button").GetComponent<Button>();

            this.letter = Resources.Load<Button>("Prefabs/Letter");

            // Add Events to 
            this.confirmButton.onClick.AddListener(SetNextWord);
            this.confirmButton.gameObject.SetActive(false); // Hide the confirm button.

            scorePanel = GameObject.Find("Score Panel").GetComponent<RectTransform>();
            scoreLabel = GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
            stars = GameObject.FindGameObjectsWithTag("Score Star");

            scorePanel.gameObject.SetActive(false);

            foreach (GameObject star in stars)
            {
                star.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Empty Star");
            }

            this.words = FisherYates.shuffle(this.words);

            // Initialize
            this.SetWord();
        }
    }

    public void StartWordGames(Word[] words)
    {
        this.words = words;
    }

    /**
     * <summary>
     *  Ang function na ito ay i-seset niya ang score ng category
     *  based sa current region.
     * </summary>
     */
    public void SetRegionCategoriesScores(int noOfCorrectAnswers)
    {
        // Get all the regions.
        foreach (RegionData regionData in playerData.regionsData)
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

    private int CountNoOfStarsToShow(int noOfCorrectAnswers)
    {
        int passingScore = this.words.Length / 2 + 1;

        if (noOfCorrectAnswers == this.words.Length)
        {
            return 3;
        }
        else if (noOfCorrectAnswers >= passingScore)
        {
            return 2;
        }

        return 1;
    }


    public void ShowStars(int noOfCorrectAnswers)
    {
        if (noOfCorrectAnswers == 0)
        {
            return;
        }

        int noOfStars = this.CountNoOfStarsToShow(noOfCorrectAnswers);

        for (int i = 0; i < noOfStars; i++)
        {
            this.stars[i].GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Fill Star");
        }

        foreach (RegionData regionData in this.playerData.regionsData)
        {
            if (regionData.regionName.ToUpper() == this.regionName.ToUpper())
            {
                foreach (Category category in regionData.categories)
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

    public void CollectAllRewards()
    {
        if (AllCategoriesCompleted() != true)
            return;

        foreach (Collectible collectible in playerData.notebook.collectibles)
        {
            if (collectible.regionName.ToUpper() == this.regionName.ToUpper())
            {
                collectible.isCollected = true;
            }
        }
    }


    public void CheckAnswer()
    {
        // Get all the child buttons of the answered container.
        Transform buttons = this.wordContainer.transform;
        string answer = ""; // Variable for storing the content/text of each button.

        // Traverse and concatenate the characters.
        foreach (Transform button in buttons)
        {
            answer += button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;
        }

        /** Since some words have spaces, we need to replace all that spaces so that we can evaluate
         if it is correct or not based on the 'answer' variable from all the concatenated characters from buttons. */
        bool isCorrect = this.words[this.currentIndex].word.Replace(" ", "").ToUpper() == answer;

        this.correctAnswers.Add(isCorrect);
    }

    /**
     * <summary>
     *  This function is registered to the listener
     *  of confirm button as an event.
     * </summary>
     */
    public void SetNextWord()
    {
        this.CheckAnswer();
        this.currentIndex++;

        if (this.currentIndex >= this.words.Length)
        {
            int noOfCorrectAnswers = this.CountCorrectAnswers();

            this.scorePanel.gameObject.SetActive(true);
            this.scoreLabel.text = noOfCorrectAnswers + "/" + this.words.Length;

            this.SetRegionCategoriesScores(noOfCorrectAnswers);
            this.ShowStars(noOfCorrectAnswers);
            this.CheckIfNextRegionIsReadyToOpen();
            this.CollectAllRewards();

            DataPersistenceManager.instance.SaveGame();

            return;
        }
        else
        {
            if (this.currentIndex < this.words.Length)
            {
                this.SetWord();
            }

            foreach (Transform child in wordContainer.transform)
            {
                Destroy(child.gameObject);
            }

            this.confirmButton.gameObject.SetActive(false);
        }
    }

    public int CountCorrectAnswers()
    {
        int count = 0;

        foreach (bool isCorrect in this.correctAnswers)
        {
            if (isCorrect) count++;
        }

        return count;
    }

   

    public void SetWord()
    {
        Word word = this.words[this.currentIndex];
        this.questionLabel.text = word.question;

        /** <summary>
         *  Converts the word to array of characters and shuffles it.
         * </summary> */
        char[] shuffledWord = FisherYates.shuffle(word.word.ToCharArray());

        for (int i = 0; i < shuffledWord.Length; i++)
        {
            Button btn = null; // A Button to instantiate in the shuffled container.

            // We only tolerate the character that is not null.
            if (shuffledWord[i] != ' ')
            {
                btn = Instantiate(letter, shuffledContainer, false);

                // Set the current character to the textmeshpro of the button.
                btn.gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = shuffledWord[i].ToString().ToUpper();

                // Add an event to the current button.
                btn.onClick.AddListener(() =>
                {
                    /** 
                     <summary>
                        If the button is inside the word or answered container,
                        it must go to the shuffled container again while if it is inside
                        shuffled container, it must go to the answered / word container.
                    </summary>
                     */
                    if (btn.gameObject.transform.parent == wordContainer)
                    {
                        btn.gameObject.transform.SetParent(shuffledContainer);
                    }
                    else
                    {
                        btn.gameObject.transform.SetParent(wordContainer);
                    }

                    /** 
                        <summary>
                            If the child count of the shuffled container is 0, we
                            need to show the confirm button to go to the next question
                            or word.
                        </summary>
                     */
                    if (shuffledContainer.childCount == 0)
                    {
                        this.confirmButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        this.confirmButton.gameObject.SetActive(false);
                    }
                });
            }
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

[System.Serializable] 
public class Word
{
    public string question;
    public string word;
}
