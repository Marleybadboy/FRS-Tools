using FRS.DOTweenAnimation;
using FRS.MainQuest.UI;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FRS.MainQuest
{
    public class MainQuestSystem : MonoBehaviour
    {
        #region Static Class
        public static MainQuestSystem instance; 
        #endregion
        #region Variabels
        [Title("UI ", TitleAlignment = TitleAlignments.Centered)]
        [TabGroup("UI Scene")][SerializeField] public Transform _ParentTopMenu;
        [TabGroup("UI Scene")][SerializeField] public Transform _ParentStartQuest;
        [TabGroup("UI Scene")][SerializeField] public GameObject _QuestInfoPrefab;
        [TabGroup("UI Scene")][SerializeField] public GameObject _QuestRewardPrefab;
        [Title("Data Base", TitleAlignment = TitleAlignments.Centered)]
        [TabGroup("Main Quest System Data")][SerializeReference] public MainQuest[] _MainQuestDB;
        [Title("Panel", TitleAlignment = TitleAlignments.Centered)]
        [TabGroup("Animation Panel")][SerializeReference] public DOTAnimationPanel _AnimPanel = new DOTAnimationPanel();
        #endregion
        #region Events
        public Action _CheckQuestCondition;
        #endregion
        #region Properties
        public GameManager.State m_MainState { get => GameManager.instance.GameState; set {} }
        #endregion
        #region Functions
        private void Awake()
        {
            instance = this;
        }
        // Start is called before the first frame update
      /// <summary>
      /// The Start function initializes all conditions and subscribes to the onNewGame event, while the
      /// OnDestroy function unsubscribes from the onNewGame event.
      /// </summary>
        void Start()
        {
            InitializeAllCondition();
            StaticEvents.onNewGame += StaticEvent_OnNewGame;
        }
        private void OnDestroy()
        {
            StaticEvents.onNewGame -= StaticEvent_OnNewGame;
        }

        #endregion
        #region Methods
       /// <summary>
       /// This code contains functions related to starting, completing, resetting, and initializing
       /// main quests in a game.
       /// </summary>
       /// <param name="questIndex">The questIndex parameter is an integer that represents the index of
       /// the quest in the _MainQuestDB list that you want to start.</param>
        private void QuestStart(int questIndex) 
        {
            
            _MainQuestDB[questIndex].InitializeQuest();
            _CheckQuestCondition += _MainQuestDB[questIndex].CheckQuest;
            Debug.LogError("Quest Start");
        }

        public void MainQuestComplete(MainQuest main) 
        { 
            _CheckQuestCondition -= main.CheckQuest; 
            
        }
        
        public void StartMainQuest(int index) 
        { 
            StartCoroutine(WaitToActiveQuest(index));
        }
        IEnumerator WaitToActiveQuest(int index) 
        { 
            while(m_MainState != GameManager.State.WALKING) 
            {
                yield return new WaitForSeconds(3);
            
            }
            yield return new WaitForEndOfFrame();
            QuestStart(index);
        
        }
        public void StaticEvent_OnNewGame(object o, EventArgs args) 
        {
            ResetQuests();
            MainQuestUIManager.Instance?.ResetAll();
            StartMainQuest(0);
            

        }

        private void InitializeAllCondition() 
        { 
            _MainQuestDB.ForEach(x => x._QuestCondtion.ConditionInitializeStart());
        }
        public void ResetQuests() 
        {
            _MainQuestDB.ForEach(quest =>
            {
                quest.ResetQuest();
                _CheckQuestCondition -= quest.CheckQuest;
            });
        }

        public MainQuest GetMainQuest(int id) 
        { 
           return _MainQuestDB.Where(quest => quest._QuestID.Equals(id)).First();
        }

        public void ResetToStartState() 
        {
            ResetQuests();
            MainQuestUIManager.Instance?.ResetAll();
            StartMainQuest(0);
        }
        #endregion
        [Button("Initialize Quest")]
        public void IniQuest(int questIndex) 
        {

            QuestStart(questIndex);
        }

    }
}
