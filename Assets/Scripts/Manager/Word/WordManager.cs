using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WordManager : MonoBehaviour
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
                star.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI and Fonts/UI Elements/UI ELEMENTS/Empty Star");
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

    public void SetNextWord()
    {
        this.CheckAnswer();
        this.currentIndex++;

        if (this.currentIndex >= this.words.Length)
        {
            this.scorePanel.gameObject.SetActive(true);
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

    public void CheckAnswer()
    {
        Transform buttons = this.wordContainer.transform;
        string answer = "";

        foreach (Transform button in buttons)
        {
           answer += button.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text;
        }


        print(this.words[this.currentIndex].word + " : " + answer);
    }

    public void SetWord()
    {
        Word word = this.words[this.currentIndex];
        this.questionLabel.text = word.question;

        char[] shuffledWord = FisherYates.shuffle(word.word.ToCharArray());

        for (int i = 0; i < shuffledWord.Length; i++)
        {
            Button btn = null;

            if (shuffledWord[i] != ' ')
            {
                btn = Instantiate(letter, shuffledContainer, false);
                Vector2 originalPos = btn.GetComponent<RectTransform>().anchoredPosition;

                btn.gameObject.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = shuffledWord[i].ToString().ToUpper();

                btn.onClick.AddListener(() =>
                {
                    if (btn.gameObject.transform.parent == wordContainer)
                    {
                        btn.gameObject.transform.SetParent(shuffledContainer);
                        btn.GetComponent<RectTransform>().anchoredPosition = originalPos;
                    }
                    else
                    {
                        btn.gameObject.transform.SetParent(wordContainer);
                    }

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
}

[System.Serializable] 
public class Word
{
    public string question;
    public string word;
}
