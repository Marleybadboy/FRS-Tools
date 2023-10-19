using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FRS.MainQuest.Condition
{
    public class QCRepairHouse : IQuestCondition
    {
        #region Properties
        HouseRepairComponent[] m_RepairComponents { get => ReapairHouseManager.Instance._RepairComponents;}
        #endregion
        #region Methods
        public bool CheckCondition()
        {
           foreach(HouseRepairComponent component in m_RepairComponents) 
           { 
                if(component.m_State != IHouseRepairComponent.ComponentState.NORMAL) 
                { 
                    return false;
                } 
           }
           return true;
        }

        public void ConditionInitializeStart()
        {
            ReapairHouseManager.Instance?.SetDestroyed();
        }

        public void ConditionInitialize()
        {
            ReapairHouseManager.Instance?.DefaultState();
        }

        public int QuestObjectComplete()
        {
           int counter = 0;
            foreach (HouseRepairComponent component in m_RepairComponents)
            {
                if (component.m_State == IHouseRepairComponent.ComponentState.NORMAL)
                {
                    counter++;
                }
                
            }
            return counter;

        }

        public int QuestObjectFinish()
        {
           return m_RepairComponents.Length;
        }

        public void OnCompleteQuest()
        {
            ReapairHouseManager.Instance.SetNormal();
        }
        #endregion
    }
}
