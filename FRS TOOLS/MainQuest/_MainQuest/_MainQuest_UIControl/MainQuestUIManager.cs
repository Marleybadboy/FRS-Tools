using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System;
using Sirenix.Utilities;
using UnityEngine.Rendering.HighDefinition;
using System.Linq;
using Unity.Services.Analytics;

namespace FRS.MainQuest.UI
{
    #region InfoBox
    [TypeInfoBox("Main Manager To control Main Quests Objects")]
    #endregion
    public class MainQuestUIManager : MonoBehaviour
    {
        #region Static Instance
        public static MainQuestUIManager Instance;
        #endregion
        #region Variabels
        [Title("UI GameObjects", TitleAlignment = TitleAlignments.Centered)]
        [BoxGroup("UI GameObjects")][SerializeField] private GameObject _QuestTitleTemplate;
        [BoxGroup("UI GameObjects")][SerializeField] private RectTransform _QuestActiveParent;
        [BoxGroup("UI GameObjects")][SerializeField] private RectMask2D _QuestActivatorObjects;
        [Title("Data Base", TitleAlignment = TitleAlignments.Centered)]
        [BoxGroup("ID data")][SerializeField] public List<MainQuestInfo> _MainQuestID = new List<MainQuestInfo>();
        #endregion
        #region Properties
        questActivator m_QuestActivator { get => questActivator.Instance;}
        MainQuestUIHandle[] m_ActiveHandleData { get => _QuestActiveParent.GetComponentsInChildren<MainQuestUIHandle>();}
        MainQuestUIHandle[] m_CompleteHandleData { get => m_CompleteQuestList.GetComponentsInChildren<MainQuestUIHandle>(); }
        Button[] m_CompleteButtonData { get => m_CompleteQuestList.GetComponentsInChildren<Button>(); }
        Button[] m_ActiveButtonData { get => m_ActiveQuestList.GetComponentsInChildren<Button>(); }
        RectTransform m_CompleteQuestList { get => m_QuestActivator.RectTransformInactive;}
        RectTransform m_ActiveQuestList { get => m_QuestActivator.RectTransformActive; }
        Sprite m_ActiveGreen { get => m_QuestActivator.Active;}
        Sprite m_Empty { get => m_QuestActivator.Empty; }
        Sprite m_GreenCircle { get => m_QuestActivator.GreenCircle;}
        #endregion
        #region Functions
        private void Awake()
        {
            Instance = this;
        }
        private void OnDestroy()
        {
            RemoveAllEvent();
        }
        #endregion

        #region Methods
        public void InvokeUIQuest(GameObject gameobject) 
        { 
            GameObject obj = Instantiate(gameobject,transform);
            GameObject main = MainQuestObject(obj.GetComponent<MainQuestUITemplate>()._QuestID);
            main.GetComponent<MainQuestUIHandle>().m_KeyName = obj.GetComponent<MainQuestUITemplate>()._QuestName.key;
            _MainQuestID.Add(QuestInfo(obj, main));
            AddEvent(obj);
            HideInfo();
        }

        public void CompleteQuest(int id) 
        {
            if (_MainQuestID.Exists(item => item._QuestID.Equals(id))) 
            { 
                MainQuestInfo info = _MainQuestID.Where(item => item._QuestID.Equals(id)).First();
                info._UIObjectMain.transform.SetParent(m_CompleteQuestList);
                info._UIObjectMain.GetComponent<MainQuestUIHandle>().m_AssignCricle = m_GreenCircle;
                RemoveEvent(info._UIObject);
            }
        
        }
        private bool ExistQuestList(int id) 
        {
            return _MainQuestID.Count > 0 && _MainQuestID.Exists(info => info._QuestID.Equals(id));
        
        }

        private MainQuestInfo QuestInfo(GameObject obj, GameObject main) 
        { 
            MainQuestInfo info = new MainQuestInfo();
            if(obj.TryGetComponent(out MainQuestUITemplate template)) 
            {
                info._QuestID = template._QuestID;
                info._UIObject = obj;
                info._UIObjectMain = main;
            }
            return info;
        
        }
        private void AddEvent(GameObject obj) 
        {
            MainQuestUITemplate template =  obj.GetComponent<MainQuestUITemplate>();
            MainQuestSystem.instance._CheckQuestCondition += template.UpdateInfo;
        }
        private void RemoveEvent(GameObject obj)
        {
            MainQuestUITemplate template = obj.GetComponent<MainQuestUITemplate>();
            MainQuestSystem.instance._CheckQuestCondition -= template.UpdateInfo;
        }
        private void RemoveAllEvent() 
        { 
            if(_MainQuestID.Count > 0) 
            {
                _MainQuestID.ForEach(quest => {RemoveEvent(quest._UIObject); });
            }
        }

        private GameObject MainQuestObject(int id) 
        {
            GameObject obj = Instantiate(_QuestTitleTemplate, _QuestActiveParent);
            Button btn = obj.GetComponentInChildren<Button>();
            btn.onClick.AddListener(() => { ShowInfo(id, btn.GetComponent<Image>());});
            return obj;
        }
        public void ShowInfo(int id, Image image) 
        {
            HideInfo();
            MainQuestInfo info = _MainQuestID.Find(quest => quest._QuestID.Equals(id));
            info._UIObject.SetActive(true);
            _QuestActivatorObjects.enabled = true;
            image.sprite = m_ActiveGreen;
        }
        public void HideInfo() 
        {
            _MainQuestID.ForEach(quest => { quest._UIObject.SetActive(false); });
            ChangeSprite(m_Empty, m_ActiveButtonData);
            ChangeSprite(m_Empty, m_CompleteButtonData);
            ChangeSprite(m_Empty, m_ActiveHandleData);
        }
        public void HideInfoMainQuest() 
        {
            _MainQuestID.ForEach(quest => { quest._UIObject.SetActive(false); });
            _QuestActivatorObjects.enabled = false;
            ChangeSprite(m_Empty, m_CompleteHandleData);
            ChangeSprite(m_Empty, m_ActiveHandleData);
        }
        private void ChangeSprite<T>(Sprite sprite, T[] handledata) where T : Component
        { 
            handledata.ForEach(handle => {handle.GetComponentInChildren<Image>().sprite = sprite;} );
        }

        public void ResetAll() 
        {
            if (_MainQuestID.Count > 0)
            {
                RemoveAllEvent();
                _MainQuestID.ForEach(quest => { Destroy(quest._UIObject); Destroy(quest._UIObjectMain); });
                _MainQuestID.Clear();
            }
        }
        #endregion
    }
    [Serializable]
    public struct MainQuestInfo 
    {
        public int _QuestID;
        public GameObject _UIObject;
        public GameObject _UIObjectMain;

    }
}