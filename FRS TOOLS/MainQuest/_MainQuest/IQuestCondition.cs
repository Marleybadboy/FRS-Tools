using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRS.MainQuest
{
    public interface IQuestCondition
    {
/// <summary>
/// The above code defines several functions related to completing quests and checking conditions.
/// </summary>
        public int QuestObjectComplete();
        public int QuestObjectFinish();
        public bool CheckCondition();
        public void ConditionInitialize();
        public void ConditionInitializeStart();
        public void OnCompleteQuest();
    }
}
