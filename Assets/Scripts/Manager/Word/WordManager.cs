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

    public int currentIndex = 0;

    public Word[] words;

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
            this.wordContainer = GameObject.Find("Answered").GetComponent<RectTransform>();
            this.shuffledContainer = GameObject.Find("Shuffled Letters").GetComponent<RectTransform>();
            this.questionLabel = GameObject.Find("Question").GetComponent<TMPro.TextMeshProUGUI>();
            this.confirmButton = GameObject.Find("Confirm Button").GetComponent<Button>();

            this.letter = Resources.Load<Button>("Prefabs/Letter");

            // Add Events to 
            this.confirmButton.onClick.AddListener(SetNextWord);
            this.confirmButton.gameObject.SetActive(false); // Hide the confirm button.

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
        this.currentIndex++;

        if (this.currentIndex < this.words.Length)
        {
            this.SetWord();
        }
        
        foreach(Transform child in wordContainer.transform)
        {
            Destroy(child.gameObject);
        }

        this.confirmButton.gameObject.SetActive(false);
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

                print(shuffledWord[i] + " : " + originalPos);

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
