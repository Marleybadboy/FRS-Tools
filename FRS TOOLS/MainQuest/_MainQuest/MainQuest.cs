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
        /* The `[SerializeReference]` attribute is used to serialize and store a reference to an object
        that implements the `IQuestCondition` interface. This allows the `MainQuest` class to have a
        flexible condition system, where different types of conditions can be assigned to quests. By
        using the `[SerializeReference]` attribute, the reference to the condition object will be
        saved and loaded correctly when the `MainQuest` object is serialized and deserialized. */
        [SerializeReference] public IQuestCondition _QuestCondtion;
        [TitleGroup("Reward", Alignment = TitleAlignments.Centered, HorizontalLine = true)]
        /* The `[SerializeReference]` attribute is used to serialize and store a reference to an object
        that implements the `IReward` interface. This allows the `MainQuest` class to have a
        flexible reward system, where different types of rewards can be assigned to quests. By using
        the `[SerializeReference]` attribute, the reference to the reward object will be saved and
        loaded correctly when the `MainQuest` object is serialized and deserialized. */
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
     /// <summary>
     /// The InitializeQuest function checks if the quest has already been initialized, and if not, it
     /// spawns a notification object and initializes the quest condition.
     /// </summary>
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
        /// <summary>
        /// The code contains functions for setting notification and reward information, spawning
        /// notification and reward objects, checking quest completion, completing a quest, resetting a
        /// quest, initializing quest completion, and checking rewards.
        /// </summary>
        /// <param name="GameObject">The "GameObject" parameter is used to pass a reference to a game
        /// object. In the code provided, it is used to instantiate and manipulate game objects during
        /// the execution of certain methods.</param>
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

     /// <summary>
     /// The function spawns a notification object, sets its information, instantiates it as a game
     /// object, sets its position, applies UI quest info animation, and destroys the object after the
     /// animation is complete.
     /// </summary>
        private void SpawnNotificationObject() 
        {
            SetNotificationInfo(out GameObject obj);
            GameObject myobject = GameObject.Instantiate(obj, m_NotificationParent);
            myobject.transform.localPosition = m_AnimationPanel.EndsPoints[2];
            UIQuestInfo(myobject.transform, m_AnimationPanel.ScalePoints[0], m_AnimationPanel.EndsPoints[0], m_AnimationPanel.EndsPoints[1], m_AnimationPanel.Duration,
                m_AnimationPanel.Delay).OnComplete(() => {GameObject.Destroy(myobject);});
        
        }
    /// <summary>
    /// The SpawnRewardObject function spawns a reward object, sets its information, animates it, and
    /// then destroys it while assigning the reward.
    /// </summary>
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
     /// <summary>
     /// The CompleteQuest function sets the quest as complete, initializes it as false, triggers the
     /// quest condition, notifies the main quest system and UI manager, and logs an error message.
     /// </summary>
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
