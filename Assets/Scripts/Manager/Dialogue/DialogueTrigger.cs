using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    public TextAsset ink;
    public TMPro.TextMeshProUGUI actorName;
    public Button talkButton;
    public string NPC_NAME;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            print("HELLO FROM SPACE BUTTON");
            DialogueManager._instance.ContinueDialogue();
        }
    }

    public void StartDialogue()
    {
        talkButton.gameObject.SetActive(false);
        DialogueManager._instance.StartDialogue(ink);
        DialogueManager._instance.actorField.text = this.NPC_NAME;

        QuestManager.instance.FindTalkQuestGoal(this.NPC_NAME);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //actorName.gameObject.SetActive(false);
            talkButton.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //actorName.gameObject.SetActive(true);
        talkButton.gameObject.SetActive(false);

    }
}
