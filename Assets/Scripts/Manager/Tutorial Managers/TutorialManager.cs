using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    public GameObject[] popups;
    private int popupIndex;

    private int firstPopupIndex = 0;

    public Joystick joystick;

    private void Start()
    {
        if (DataPersistenceManager.instance.playerData.isTutorialDone == false)
        {
            GameObject.Find("Quest Panel").SetActive(false);
            GameObject.Find("Alert Box").SetActive(false);
            this.popups[this.popupIndex].SetActive(true);
            LeanTween.scale(this.popups[this.popupIndex].transform.GetChild(0).GetChild(0).GetChild(0).gameObject, Vector2.one, .2f)
                .setDelay(1f).setEaseSpring();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DataPersistenceManager.instance.playerData.isTutorialDone == true)
        {
            for (int i = 0; i < popups.Length; i++)
            {
                popups[i].SetActive(false);
            }
            return;
        }

        this.joystick.gameObject.SetActive(false);

        for (int i = 0; i < popups.Length; i++)
        {
            if (i == this.popupIndex)
            {
                popups[i].SetActive(true);
            }
            else
            {
                popups[i].SetActive(false);
            }
        }

        if (this.popupIndex < 1)
        {
            if (Input.touchCount == 1)
            {

                if (Input.GetMouseButtonDown(0))
                {
                    GameObject selected = EventSystem.current.currentSelectedGameObject;

                    if (selected != null &&  selected.name == "Next Step")
                    {
                        print(firstPopupIndex);

                        this.firstPopupIndex++;

                        if (firstPopupIndex == 1)
                        {
                            GameObject obj = this.popups[this.popupIndex].transform.GetChild(0).GetChild(0).GetChild(0).GetChild(1).gameObject;

                            TMPro.TextMeshProUGUI text = obj.GetComponent<TMPro.TextMeshProUGUI>();

                            text.text = "So now let's dive in the tutorial";
                        }

                        else if (firstPopupIndex == 2)
                        {
                            LeanTween.scale(this.popups[this.popupIndex].transform.GetChild(0).GetChild(0).GetChild(0).gameObject, Vector2.zero, .5f)
                                .setEaseSpring()
                                .setOnComplete(() => {

                                    this.popups[this.popupIndex].SetActive(false);

                                   
                                });
                            this.popupIndex++;
                            print("POPUP INDEX: " + this.popupIndex);
                        }
                    }
                    selected = null;
                }
            }
        }

        else if (this.popupIndex == 1)
        {
            
            LeanTween.scale(this.popups[this.popupIndex].transform.GetChild(0)
            .GetChild(0).GetChild(1).gameObject, Vector2.one, .5f)
            .setEaseSpring();

            if (Input.touchCount == 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject selected = EventSystem.current.currentSelectedGameObject;

                    if (selected != null && selected.name == "Next Step")
                    {
                        this.popups[this.popupIndex].SetActive(false);
                        this.popupIndex++;
                    }
                }
            }
        }
        else if (this.popupIndex == 2)
        {
            LeanTween.scale(this.popups[this.popupIndex].transform.GetChild(0)
            .GetChild(0).GetChild(1).gameObject, Vector2.one, .5f)
            .setEaseSpring();

            if (Input.touchCount == 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject selected = EventSystem.current.currentSelectedGameObject;

                    if (selected != null && selected.name == "Next Step")
                    {
                        this.popups[this.popupIndex].SetActive(false);
                        this.popupIndex++;
                        DataPersistenceManager.instance.playerData.isTutorialDone = true;
                        DataPersistenceManager.instance.SaveGame();
                        SceneManager.LoadScene("House");
                    }
                }
            }
            
        }
    }
}
