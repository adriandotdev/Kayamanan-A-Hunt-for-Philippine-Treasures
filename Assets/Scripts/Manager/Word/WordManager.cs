using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class WordManager : MainGame, IDataPersistence
{
    public static WordManager instance;

    [Header("UI")]
    public RectTransform layout;
    public RectTransform wordContainer;
    public RectTransform shuffledContainer;
    public TMPro.TextMeshProUGUI questionLabel;
    public Button confirmButton;

    [Header("Letter Button (Prefab)")]
    public Button letter;

    [Header("Word Games Properties")]
    public Word[] words;
    public int currentIndex = 0;

    [Header("Score Panel")]
    public RectTransform scorePanel;
    public TMPro.TextMeshProUGUI scoreLabel;
    public GameObject[] stars;

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

            try
            {
                this.currentIndex = 0;
                this.correctAnswers = new List<bool>();

                this.layout = GameObject.Find("Layout").GetComponent<RectTransform>();
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

                foreach (GameObject star in stars)
                {
                    star.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Empty Star");
                }

                //Array.Copy(FisherYates.shuffle(this.words), this.words, this.words.Length);

                FisherYates.Shuffle(this.shuffled);

                // Initialize
                this.SetWord();
            }
            catch (System.Exception e)
            {

            }
        }
    }

    public void StartWordGames(Word[] words)
    {
        this.words = words;
        this.shuffled = new Word[this.words.Length];
        Array.Copy(this.words, this.shuffled, this.words.Length);
    }


    public void ShowScorePanel(int noOfCorrectAnswers)
    {
        TweeningManager.instance.OpenScorePanel(noOfCorrectAnswers, () =>
        {
            this.CollectAllRewards();
        });
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
        bool isCorrect = ((Word)this.shuffled[this.currentIndex]).word.Replace(" ", "").ToUpper() == answer;

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

        if (this.currentIndex >= this.shuffled.Length)
        {
            int noOfCorrectAnswers = this.CountCorrectAnswers();

            this.layout.gameObject.SetActive(false); 
            this.scoreLabel.text = noOfCorrectAnswers + "/" + this.shuffled.Length;

            this.SetRegionCategoriesScores(noOfCorrectAnswers);
            this.ShowStars(noOfCorrectAnswers);
            this.ShowScorePanel(noOfCorrectAnswers);
            this.CheckIfNextRegionIsReadyToOpen();
            //this.CollectAllRewards();

            DataPersistenceManager.instance.SaveGame();

            return;
        }
        else
        {
            if (this.currentIndex < this.shuffled.Length)
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

    public void SetWord()
    {
        
        Word word = (Word) this.shuffled[this.currentIndex];
        this.questionLabel.text = word.question;

        /** <summary>
         *  Converts the word to array of characters and shuffles it.
         * </summary> */
        char[] shuffledWord = word.word.ToCharArray();

        FisherYates.Shuffle(shuffledWord);

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
