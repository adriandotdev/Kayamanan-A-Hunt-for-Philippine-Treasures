using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class QuestGoal
{
    public bool isFinished;

    public virtual void Finish()
    {
        this.isFinished = true;
    }
}

[System.Serializable]
public class TalkGoal : QuestGoal
{
    [SerializeField] private string npcName;

    public TalkGoal(string npcName)
    {
        this.npcName = npcName;
    }

    public string GetNPCName()
    {
        return this.npcName;
    }

    public override void Finish()
    {
        base.Finish();
    }
}


[System.Serializable]
public class DeliveryGoal : QuestGoal
{
    public string deliverGoalId;
    public string deliveryMessage;
    public string giverName;
    public string receiverName;
    public bool itemReceivedFromGiver;
    public Item item;

    public DeliveryGoal(string giverName, string receiverName,  string deliveryMessage, Item item)
    {
        this.deliverGoalId = Guid.NewGuid().ToString();
        this.giverName = giverName;
        this.receiverName = receiverName;
        this.deliveryMessage = deliveryMessage;
        this.item = item;
    }

    public DeliveryGoal Copy()
    {
        DeliveryGoal dg =  new DeliveryGoal(this.giverName, this.receiverName, 
            this.deliveryMessage, new Item(this.item.itemName, 
            this.item.information, this.item.stackable));

        dg.itemReceivedFromGiver = this.itemReceivedFromGiver;

        return dg;
    }
}

[System.Serializable]
public class Item
{
    public string itemName;
    public string information;
    public int quantity;
    public bool stackable;

    public Item(string itemName, string information, bool stackable)
    {
        this.itemName = itemName;
        this.information = information;
        this.stackable = stackable;
    }

    public Item CopyItem()
    {
        Item clonedItem = new Item(this.itemName, this.information, this.stackable);

        clonedItem.quantity = this.quantity;

        return clonedItem;
    }
}