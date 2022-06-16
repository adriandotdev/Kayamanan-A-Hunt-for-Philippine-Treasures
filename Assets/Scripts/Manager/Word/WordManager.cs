using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordManager : MonoBehaviour
{
    [Header("UI")]
    public RectTransform wordContainer;
    public RectTransform shuffledContainer;
    public TMPro.TextMeshProUGUI questionLabel;
    public Button submitButton;

    [Header("Letter Button (Prefab)")]
    public Button letter;

    public int currentIndex = 0;

    public Word[] words;

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

        submitButton.gameObject.SetActive(false);
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
                        submitButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        submitButton.gameObject.SetActive(false);
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
