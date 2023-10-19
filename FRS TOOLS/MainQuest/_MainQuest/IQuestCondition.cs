using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FRS.MainQuest
{
    public interface IQuestCondition
    {
        public int QuestObjectComplete();
        public int QuestObjectFinish();
        public bool CheckCondition();
        public void ConditionInitialize();
        public void ConditionInitializeStart();
        public void OnCompleteQuest();
    }
}
