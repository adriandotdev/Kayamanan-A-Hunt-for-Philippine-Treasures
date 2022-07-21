using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryGoalReceiver : MonoBehaviour
{
    private Button giveBtn;
    public Quest quest; // reason ay para ma set ang quest as completed by questID

    private void Start()
    {
        this.giveBtn = transform.GetChild(0).GetChild(3).GetComponent<Button>();

        this.giveBtn.onClick.AddListener(this.GiveItem);
    }

    void GiveItem()
    {
        foreach (Quest quest in DataPersistenceManager.instance.playerData.currentQuests)
        {
            if (quest.questID == this.quest.questID)
            {
                quest.isCompleted = true;
                this.giveBtn.gameObject.SetActive(false);
                // Find the Delivery Quest with a specific ID.
                QuestManager.instance.FindDeliveryQuestGoal(quest.questID);

                // Reset all gameobjects holding a DeliveryGoal Giver and Receiver.
                QuestManager.instance.SetupScriptsForDeliveryQuestToNPCs();
                return;
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        /** All NPC ay pwede maging Giver and Receiver ng item,
         * so if the quest is null, it means na hindi kabilang ang
         NPC gameobject na ito sa kahit anong quest na makikita sa pending
        quest ni player. */
        if (this.quest == null) return;

        if (collision.gameObject.CompareTag("Player") 
            && this.quest.deliveryGoal.itemReceivedFromGiver 
            && !this.quest.isCompleted)
        {
            this.giveBtn.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.giveBtn.gameObject.SetActive(false);
        }
    }
}
