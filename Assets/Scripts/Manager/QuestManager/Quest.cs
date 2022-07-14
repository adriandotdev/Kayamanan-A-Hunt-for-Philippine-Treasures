using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public bool isCompleted;

    public string title;
    public string description;
    public int dunongPointsRewards;

    public TalkGoal questGoal;
}
