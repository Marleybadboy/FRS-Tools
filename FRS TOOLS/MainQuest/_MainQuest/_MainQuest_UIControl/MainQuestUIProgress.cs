using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using FRS.MainQuest;
using UnityEngine.UI;
using TMPro;

public class MainQuestUIProgress : MainQuestUITemplate
{

    #region Variabels
    [BoxGroup("Progress Bar")][SerializeField] private Image _FillImage;
    [BoxGroup("Progress Bar")][SerializeField] private TextMeshProUGUI _CounterImage;
    private float _Ratio;
    [SerializeReference]private IQuestCondition _QuestCondition;
    #endregion
    #region Properties
    int m_AllObjectCounter { get => _QuestCondition.QuestObjectFinish();}
    int m_ObjectComplete {get => _QuestCondition.QuestObjectComplete();}
    string m_DisplayMessage { get => $"{m_ObjectComplete}/{m_AllObjectCounter}"; }
    float m_Ratio { get => 1f / _QuestCondition.QuestObjectFinish();}
    #endregion
    #region Functions
    private void OnDestroy()
    {
        MainQuestSystem.instance._CheckQuestCondition -= UpdateInfo;
    }
    #endregion

    #region Methods
    public override void InitializeObject(MainQuest quest)
    {
        _QuestCondition = quest._QuestCondtion;
        UpdateInfo();
        base.InitializeObject(quest);
        //MainQuestSystem.instance._CheckQuestCondition += UpdateInfo;
    }

    public override void UpdateInfo() 
    {
        _CounterImage.text = m_DisplayMessage;
        float fill = m_ObjectComplete * m_Ratio;
        _FillImage.fillAmount = fill;
    
    }
    #endregion 
}
