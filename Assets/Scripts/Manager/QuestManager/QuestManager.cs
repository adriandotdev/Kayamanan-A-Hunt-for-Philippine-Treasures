using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> quests;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void FindTalkQuestGoal(string npcName)
    {
        foreach(Quest quest in quests)
        {
            if (quest.questGoal.GetNPCName() == npcName)
            {
                quest.isCompleted = true;
                quest.questGoal.Finish();
            }
        }
    }
}
