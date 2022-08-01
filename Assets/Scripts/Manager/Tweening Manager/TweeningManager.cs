using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class TweeningManager : MonoBehaviour
{
    public static TweeningManager instance;
    
    [Header("Score Panel UIs")]
    CanvasGroup scorePanelGroup;
    GameObject scorePanel;
    GameObject panelTitle;
    GameObject starsContainer;
    GameObject[] starEmpty;
    TMPro.TextMeshProUGUI scoreText;
    TMPro.TextMeshProUGUI score;
    GameObject restartBtn;
    GameObject exitBtn;

    [Header("Tutorial UIs")]
    GameObject[] popups;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //private void Start()
    //{
    //    OpenScorePanel(2);
    //}
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnAssessmentOrWordGameSceneLoaded;
        SceneManager.sceneLoaded += OnHouseTutorialSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnAssessmentOrWordGameSceneLoaded;
        SceneManager.sceneLoaded -= OnHouseTutorialSceneLoaded;
    }

    void OnAssessmentOrWordGameSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Assessment")
            || SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Word Games"))
        {

            this.scorePanel = GameObject.Find("Score Panel");
            this.scorePanelGroup = GameObject.Find("Score Panel Group").GetComponent<CanvasGroup>();
            this.panelTitle = GameObject.Find("Panel Title");
            this.starsContainer = GameObject.Find("Stars");
            this.starEmpty = GameObject.FindGameObjectsWithTag("Score Star");
            this.scoreText = GameObject.Find("Your Score Text").GetComponent<TMPro.TextMeshProUGUI>();
            this.score = GameObject.Find("Score").GetComponent<TMPro.TextMeshProUGUI>();
            this.restartBtn = GameObject.Find("Replay Button");
            this.exitBtn = GameObject.Find("Exit Button");
        }
    }

    void OnHouseTutorialSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Add ('if isTutorialDone is False')
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("House"))
        {

            this.popups = GameObject.FindGameObjectsWithTag("Tutorial Popup");
        }
    }

    public void OpenScorePanel(int noOfStars, Action ShowRewardsScene)
    {
        /** 
         * Panel scale
         * 8.216969f, 8.216969f
         * 
         * Panel Title
         * -0.6f, 45.765f
            Score Text

            -0.4f, 1.4f

            Score

            0.5f, -14.365f

            Replay Button

            -14.894f, -33.8f

            Exit Button

            15.6f, -33.9f

        Animated Normal Scale of Star

        0.0341844 - Normal scale of star
        0.08146959 
         starEmpty[0].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Fill Star");
         */

        LeanTween.scale(this.scorePanel, new Vector2(8.216969f, 8.216969f), .3f)
            .setDelay(.3f).setOnComplete(() => this.scorePanelGroup.alpha = 1);
        LeanTween.moveLocal(this.panelTitle, new Vector2(-0.6f, 45.765f), .3f)
            .setDelay(.3f).setEaseSpring()
            .setOnComplete(() =>
            {


                LeanTween.moveLocal(starEmpty[0].gameObject, new Vector2(30.3f, 22.2f), .8f)
            .setEaseSpring();

                LeanTween.moveLocal(starEmpty[1].gameObject, new Vector2(0.3999996f, 26.3f), .8f)
                    .setEaseSpring();

                LeanTween.moveLocal(starEmpty[2].gameObject, new Vector2(-30.9f, 23.9f), .8f)
                    .setEaseSpring();

                // END OF FIRST PART

                LeanTween.scale(starEmpty[0].gameObject, new Vector2(0.08146959f, 0.08146959f), .3f)
                    .setDelay(.5f)
                    .setEaseOutElastic()
                    .setOnComplete(() =>
                    {
                        if (noOfStars >= 1)
                        starEmpty[0].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Fill Star");
                        LeanTween.scale(starEmpty[0].gameObject, new Vector2(0.0341844f, 0.0341844f), .3f);
                    });

                LeanTween.scale(starEmpty[1].gameObject, new Vector2(0.08146959f, 0.08146959f), .3f)
                    .setDelay(1f)
                    .setEaseOutElastic()
                    .setOnComplete(() =>
                    {
                        if (noOfStars >= 2)
                        starEmpty[1].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Fill Star");
                        LeanTween.scale(starEmpty[1].gameObject, new Vector2(0.0341844f, 0.0341844f), .3f);
                    });
                
                LeanTween.scale(starEmpty[2].gameObject, new Vector2(0.08146959f, 0.08146959f), .3f)
                    .setDelay(1.5f)
                    .setEaseOutElastic()
                    .setOnComplete(() =>
                    {
                        if (noOfStars >= 3)
                        starEmpty[2].gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI ELEMENTS/Fill Star");
                        LeanTween.scale(starEmpty[2].gameObject, new Vector2(0.0341844f, 0.0341844f), .3f)
                        .setOnComplete(() =>
                        {
                            LeanTween.moveLocal(this.scoreText.gameObject, new Vector2(-0.4f, 1.4f), .3f).setEaseSpring();
                            LeanTween.moveLocal(this.score.gameObject, new Vector2(0.5f, -14.365f), .3f).setDelay(.6f).setEaseSpring();
                            LeanTween.moveLocal(this.restartBtn, new Vector2(-14.894f, -33.8f), .3f).setDelay(.9f).setEaseSpring();
                            LeanTween.moveLocal(this.exitBtn, new Vector2(15.6f, -33.9f), .3f).setDelay(1f).setEaseSpring()
                                .setOnComplete(ShowRewardsScene);
                        });
                    });
                }
            );
    }

}
