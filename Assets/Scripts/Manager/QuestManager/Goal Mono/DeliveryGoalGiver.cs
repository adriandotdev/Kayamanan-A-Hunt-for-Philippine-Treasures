using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * <summary>
 *  This script ay responsible para sa Delivery Quest
 *  which is specific sa Giver ng item kay player.
 * </summary> 
 */
public class DeliveryGoalGiver : MonoBehaviour
{
    public Quest quest;
    private Button helpBtn;
    [SerializeField]  private RectTransform fedexPanel;
    [SerializeField]  private TMPro.TextMeshProUGUI requestMessage;
    [SerializeField] private Button accept;
    [SerializeField] private Button cancel;

    private void OnEnable()
    {
        this.helpBtn = transform.GetChild(0).GetChild(2).GetComponent<Button>();

        this.helpBtn.onClick.AddListener(this.OpenDeliveryPanel);
        this.fedexPanel.gameObject.SetActive(false);
    }

    void OpenDeliveryPanel()
    {
        this.fedexPanel.gameObject.SetActive(true);
        this.accept.onClick.AddListener(this.AcceptDeliverableItem);
        this.cancel.onClick.AddListener(this.CloseDeliveryPanel);
        this.requestMessage.text = this.quest.deliveryGoal.deliveryMessage;
    }

    void CloseDeliveryPanel()
    {
        this.accept.onClick.RemoveAllListeners();
        this.cancel.onClick.RemoveAllListeners();
        this.fedexPanel.gameObject.SetActive(false);
    }

    void AcceptDeliverableItem()
    {
        this.SetItemAsReceived();
        this.CloseDeliveryPanel();
        this.helpBtn.gameObject.SetActive(false);
    }
    
    void SetItemAsReceived()
    {
        foreach (Quest quest in DataPersistenceManager.instance.playerData.currentQuests)
        {
            // check if ang quest ay Delivery Quest based sa pagcheck if existing ang deliverGoalId.
            if (quest.questID == this.quest.questID && quest.deliveryGoal.deliverGoalId.Length > 0)
            {
                quest.deliveryGoal.itemReceivedFromGiver = true; // set to true
                // reset ulit ang lahat ng gameobject na may handle ng DeliveryGoalGiver and DeliveryGoalReceiver.
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

        // Only show the help button if the quest item is not yet given.
        if (collision.gameObject.CompareTag("Player") 
             && !this.quest.deliveryGoal.itemReceivedFromGiver)
        {
            this.helpBtn.gameObject.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            this.helpBtn.gameObject.SetActive(false);
        }
    }
}
