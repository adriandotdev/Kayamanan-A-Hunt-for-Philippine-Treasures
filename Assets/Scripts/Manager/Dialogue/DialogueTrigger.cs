using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset[] inks;
    public TMPro.TextMeshProUGUI actorName;
    public Button talkButton;
    public string NPC_NAME;

    private void Start()
    {
        this.NPC_NAME = transform.name;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            print("HELLO FROM SPACE BUTTON");
            DialogueManager._instance.ContinueDialogue();
        }
    }

    public int CurrentOpenRegion()
    {
        int regionsData = 0;

        foreach (RegionData regionData in DataPersistenceManager.instance.playerData.regionsData)
        {
            if (regionData.isOpen)
                regionsData = regionData.regionNumber - 1;
        }
        return regionsData;
    }

    public void StartDialogue()
    {
        talkButton.gameObject.SetActive(false);
        DialogueManager._instance.StartDialogue(inks[this.CurrentOpenRegion()]);
        DialogueManager._instance.actorField.text = this.NPC_NAME;

        QuestManager.instance.FindTalkQuestGoal(this.NPC_NAME);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //actorName.gameObject.SetActive(false);
            talkButton.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        talkButton.gameObject.SetActive(false);
    }
}
