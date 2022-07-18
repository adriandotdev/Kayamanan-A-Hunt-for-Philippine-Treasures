using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public string questID;
    public bool isCompleted;

    public string title;
    public string description;
    public int dunongPointsRewards;
    public string region;

    public TalkGoal talkGoal;
    public GatherGoal gatherGoal;

    public Quest(string title, string description, int dunongPointsRewards, string region, TalkGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.dunongPointsRewards = dunongPointsRewards;
        this.talkGoal = goal;
        this.region = region;
    }

    public Quest(string title, string description, int dunongPointsRewards, string region, GatherGoal goal)
    {
        this.questID = Guid.NewGuid().ToString();
        this.title = title;
        this.description = description;
        this.dunongPointsRewards = dunongPointsRewards;
        this.gatherGoal = goal;
        this.region = region;
    }
}

