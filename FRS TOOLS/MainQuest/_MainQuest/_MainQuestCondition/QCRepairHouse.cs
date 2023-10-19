using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FRS.MainQuest.Condition
{
/* The QCRepairHouse class is an implementation of the IQuestCondition interface that checks if all
house repair components are in a normal state. */
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

   /// <summary>
   /// The function ConditionInitializeStart sets the destroyed state of the ReapairHouseManager
   /// instance if it exists.
   /// </summary>
        public void ConditionInitializeStart()
        {
            ReapairHouseManager.Instance?.SetDestroyed();
        }

  /// <summary>
  /// The function "ConditionInitialize" calls the "DefaultState" method of the "ReapairHouseManager"
  /// instance, if it exists.
  /// </summary>
        public void ConditionInitialize()
        {
            ReapairHouseManager.Instance?.DefaultState();
        }

    /// <summary>
    /// The function "QuestObjectComplete" counts the number of house repair components that are in a
    /// normal state.
    /// </summary>
    /// <returns>
    /// The method is returning the value of the counter variable, which represents the number of
    /// HouseRepairComponents in the m_RepairComponents list that have a state of NORMAL.
    /// </returns>
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
