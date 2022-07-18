using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
public class GatherGoal : QuestGoal
{

}

public enum QuestType
{
    TALK_GOAL
}