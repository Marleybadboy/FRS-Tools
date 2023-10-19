using DG.Tweening;
using FRS.DOTweenAnimation;
using FRS.MainQuest.Reward;
using FRS.MainQuest.UI;
using Sirenix.OdinInspector;
using System;
using System.Xml.Serialization;
using UnityEngine;
using static FRS.DOTweenAnimation.DOTweenSequences;

namespace FRS.MainQuest
{
    [Serializable]
    public class MainQuest
    {
        #region Variabels
        [TitleGroup("Quest Information", Alignment = TitleAlignments.Centered)]
        [SerializeField] private bool _QuestComplete;
        [SerializeField] private bool _QuestInitialized;
        [TitleGroup("Quest UI Object", Alignment = TitleAlignments.Centered)]
        [SerializeField][Tooltip("Which Object Display in Top Menu")] private GameObject _QuestUITemplate;
        [TitleGroup("Quest Text Keys", Alignment = TitleAlignments.Centered)]
        [SerializeField] public int _QuestID;
        [SerializeField] public string _QuestNameKey;
        [SerializeField] public string _QuestDescriptionKey;
        [SerializeField] public string _RewardDescriptionKey;
        [SerializeField] public string _RewardKey;
        [TitleGroup("Condition", Alignment = TitleAlignments.Centered, HorizontalLine = true)]
        [SerializeReference] public IQuestCondition _QuestCondtion;
        [TitleGroup("Reward", Alignment = TitleAlignments.Centered, HorizontalLine = true)]
        [SerializeReference] IReward _QuestReward;

        #endregion
        #region Properties
        public bool m_Condition { get => _QuestCondtion.CheckCondition();}
        DOTAnimationPanel m_AnimationPanel { get => MainQuestSystem.instance._AnimPanel;}
        GameObject m_NotificationObj { get => MainQuestSystem.instance._QuestInfoPrefab;}
        MainQuestObjectDB m_UIObjectData {get => m_NotificationObj.GetComponent<MainQuestObjectDB>();}
        Transform m_NotificationParent { get => MainQuestSystem.instance._ParentStartQuest;}
        MainQuestUITemplate m_QuestUITemplate {get => _QuestUITemplate.GetComponent<MainQuestUITemplate>();}
        GameObject m_RewardObject { get => MainQuestSystem.instance._QuestRewardPrefab;}
        ObjectDBReward m_RewardDB {get => m_RewardObject.GetComponent<ObjectDBReward>();}
        public bool m_QuestComplete { get => _QuestComplete; set { _QuestComplete = value; } }
        public bool m_QuestInitialized { get => _QuestInitialized; set { _QuestInitialized = value; } }
        public IReward m_QuestReward { get => _QuestReward;}
        #endregion
        #region Methods
        public void InitializeQuest() 
        { 
            switch(_QuestInitialized) 
            {
                case true:
                    _QuestCondtion.ConditionInitialize();
                    break;
                case false:
                    SpawnNotificationObject();
                    _QuestCondtion.ConditionInitialize();
                    break;
            
            }
            m_QuestUITemplate.InitializeObject(this);
            _QuestInitialized = true;
        
        }
        private void SetNotificationInfo(out GameObject obj) 
        {          
            m_UIObjectData._ObjectName.key = _QuestNameKey;
            m_UIObjectData._ObjecDescription.key = _QuestDescriptionKey;
            obj = m_NotificationObj; 
        }

        private void SetRewardInfo(out GameObject obj) 
        { 
            m_RewardDB._ObjectName.key = _QuestNameKey;
            m_RewardDB._ObjecDescription.key = _RewardDescriptionKey;
            string texttranslete = LanguageManager.instance?.GetLanguage(_RewardKey);
            string textall = string.Format(texttranslete, m_QuestReward.GetReward());
            m_RewardDB._RewardText.text = textall + ' ' + m_QuestReward.Name;
            obj = m_RewardObject;
        }

        private void SpawnNotificationObject() 
        {
            SetNotificationInfo(out GameObject obj);
            GameObject myobject = GameObject.Instantiate(obj, m_NotificationParent);
            myobject.transform.localPosition = m_AnimationPanel.EndsPoints[2];
            UIQuestInfo(myobject.transform, m_AnimationPanel.ScalePoints[0], m_AnimationPanel.EndsPoints[0], m_AnimationPanel.EndsPoints[1], m_AnimationPanel.Duration,
                m_AnimationPanel.Delay).OnComplete(() => {GameObject.Destroy(myobject);});
        
        }
        private void SpawnRewardObject() 
        { 
            SetRewardInfo(out GameObject obj);
            GameObject myobject = GameObject.Instantiate(obj, m_NotificationParent);
            myobject.transform.localPosition = m_AnimationPanel.EndsPoints[1];
            UIQuestInfo(myobject.transform, m_AnimationPanel.ScalePoints[0], m_AnimationPanel.EndsPoints[0], m_AnimationPanel.EndsPoints[2], m_AnimationPanel.Duration,
                m_AnimationPanel.Delay).OnComplete(() => { GameObject.Destroy(myobject); _QuestReward.AssignReward();});
        }
        public void CheckQuest() 
        {
            if (_QuestComplete) { return; }
            if (m_Condition) 
            {
                CompleteQuest();
                SpawnRewardObject();
            }
        }
        private void CompleteQuest() 
        {
            _QuestComplete = true;
            _QuestInitialized = false;
            _QuestCondtion.OnCompleteQuest();
            MainQuestSystem.instance?.MainQuestComplete(this);
            MainQuestUIManager.Instance?.CompleteQuest(_QuestID);
            Debug.LogError("COMPLETE");
        }

        public void ResetQuest() 
        {
            _QuestComplete = false;
            _QuestInitialized = false;

        }

        public void InitializeComplete() 
        {
            m_QuestUITemplate.InitializeObject(this);
            _QuestInitialized = true;
            CompleteQuest();
        }
        [Button("Reward Check")]
        public void Check() 
        { 
            SpawnRewardObject();
            string texttranslete = LanguageManager.instance?.GetLanguage(_RewardKey);
            string textall = string.Format(texttranslete, m_QuestReward.GetReward());
            Debug.LogError(textall);

        }
        #endregion
    }
}
