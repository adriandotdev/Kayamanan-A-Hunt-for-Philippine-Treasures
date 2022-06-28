using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Ink.Runtime;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager _instance;

    [Header("UI Elements")]
    [SerializeField] private RectTransform panel;
    [SerializeField] public TMPro.TextMeshProUGUI actorField;
    [SerializeField] private TMPro.TextMeshProUGUI dialogueField;
    [SerializeField] private Joystick joystick;

    [Header("Canvas Group")]
    public CanvasGroup houseGroup;

    private Story currentStory;
    private bool isDialogueRunning;

    public GameObject[] choicesBtn;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnHouseSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnHouseSceneLoaded;
    }

    private void Start()
    {
        if (_instance == null)
            _instance = this;

        isDialogueRunning = false;
    }

    // Run this function when house scene is loaded.
    public void OnHouseSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Outside"))
        {
            this.houseGroup = GameObject.Find("House Canvas Group").GetComponent<CanvasGroup>();
        }
    }

    public void StartDialogue(TextAsset ink)
    {
        currentStory = new Story(ink.text);
        panel.gameObject.SetActive(true);
        isDialogueRunning = true;
        this.houseGroup.interactable = false;
        this.houseGroup.blocksRaycasts = false;

        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();

            if (text == "")
            { 
                panel.gameObject.SetActive(false);
                return;
            }

            dialogueField.text = text;
            ShowChoices();
        }
        else
        {
            ExitDialogue();
        }
    }

    public void ContinueDialogue()
    {
        if (currentStory.canContinue)
        {
            string text = currentStory.Continue();

            if (text == "")
            {
                panel.gameObject.SetActive(false);
                this.houseGroup.interactable = true;
                this.houseGroup.blocksRaycasts = true;
                return;
            }

            dialogueField.text = text;
            ShowChoices();
        }
        else
        {
            ExitDialogue();
        }
    }


    public void ExitDialogue()
    {
        panel.gameObject.SetActive(false);
        isDialogueRunning = false;
        dialogueField.text = "";
    }

    public void ShowChoices()
    {
        List<Choice> choices = currentStory.currentChoices;

        if (choicesBtn.Length >= choices.Count)
        {
            int index = 0;

            foreach (Choice choice in choices)
            {
                choicesBtn[index].SetActive(true);
                choicesBtn[index].GetComponentInChildren<TMPro.TextMeshProUGUI>().text = choice.text;
                index++;
            }

            for (int i = index; i < choicesBtn.Length; i++)
            {
                choicesBtn[i].gameObject.SetActive(false);
            }
        }

    }

    public void MakeChoice(int choiceIndex)
    {
        currentStory.ChooseChoiceIndex(choiceIndex);

        this.ContinueDialogue();
        print(currentStory.currentText);
    }
}

