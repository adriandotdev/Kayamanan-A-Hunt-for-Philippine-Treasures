using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questID;
    public bool isCompleted;
    public bool isClaimed;

    public string title;
    public string description;
    public int dunongPointsRewards;
    public string region;

    public TalkGoal talkGoal;
    public DeliveryGoal deliveryGoal;

    public Quest(string title, string description, int dunongPointsRewards, string region, TalkGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.dunongPointsRewards = dunongPointsRewards;
        this.talkGoal = goal;
        this.region = region;
    }

    public Quest(string title, string description, int dunongPointsRewards, string region, DeliveryGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.dunongPointsRewards = dunongPointsRewards;
        this.deliveryGoal = goal;
        this.region = region;
    }

    private void SetQuestID(string questID)
    {
        this.questID = questID;
    }

    public Quest CopyTalkQuestGoal()
    {
        Quest questCopy = new Quest(this.title, this.description, this.dunongPointsRewards, this.region, this.talkGoal.CopyTalkGoal());

        questCopy.SetQuestID(this.questID);

        return questCopy;
    }

    public Quest CopyQuestDeliveryGoal()
    {
        Quest questCopy = new Quest(this.title, this.description, this.dunongPointsRewards, this.region,
            this.deliveryGoal.Copy());

        questCopy.SetQuestID(this.questID);

        return questCopy;
    }
}

