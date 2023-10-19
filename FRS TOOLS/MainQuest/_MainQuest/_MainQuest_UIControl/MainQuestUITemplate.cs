using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using FRS.MainQuest.UI;
using FRS.MainQuest;

public abstract class MainQuestUITemplate : MonoBehaviour
{

    #region Variabels
    [TitleGroup("Main Object")]
    public int _QuestID;
    public KeyLangugageItem _QuestName;
    public KeyLangugageItem _QuestDescription;
    #endregion
    #region Properties
    public MainQuestUIManager m_UIManager { get => MainQuestUIManager.Instance;}
   
    #endregion
    #region Functions
    #endregion

    #region Methods
    public virtual void InitializeObject(MainQuest quest) 
    {
        _QuestID = quest._QuestID;
        _QuestName.key = quest._QuestNameKey;
        _QuestDescription.key = quest._QuestDescriptionKey;
        m_UIManager.InvokeUIQuest(gameObject);
    
    }

    public virtual void UpdateInfo() { }
    #endregion
}
