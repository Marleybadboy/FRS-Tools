
using System.Collections.Generic;
using System;
using FRS.MainQuest;
using Sirenix.Utilities;
using FRS.MainQuest.UI;

public class MainQuestSave : ISaveGameData
{

    #region Variabels
    public List<MainQuestData> MainQuestSaveData = new List<MainQuestData>();
    #endregion
    #region Properties
    MainQuest[] m_MainQuestDB { get => MainQuestSystem.instance._MainQuestDB; }
    MainQuestSystem m_MainQuestSystem { get => MainQuestSystem.instance; }
    MainQuestUIManager m_MainQuestUIManager { get => MainQuestUIManager.Instance; }
    #endregion
    #region Functions
    #endregion

    #region Methods
    void ISaveGameData.Serialize() 
    { 
        if(m_MainQuestDB != null) 
        {
            try 
            {
                m_MainQuestDB.ForEach(quest =>
                {
                    if (quest.m_QuestInitialized || quest.m_QuestComplete)
                    {
                        MainQuestSaveData.Add(new MainQuestData
                        {
                            QuestID = quest._QuestID,
                            Complete = quest.m_QuestComplete,
                            Initialized = quest.m_QuestInitialized,

                        });

                    }

                });
            
            
            }
            catch { }
        }
    }

    void ISaveGameData.Deserialize() 
    {
        if(MainQuestSaveData.Count > 0)
        {
            try 
            {
                m_MainQuestSystem.ResetQuests();
                m_MainQuestUIManager.ResetAll();
                MainQuestSaveData.ForEach(quest =>
                {
                    MainQuest mainQuest = m_MainQuestSystem.GetMainQuest(quest.QuestID);
                    mainQuest.m_QuestComplete = quest.Complete;
                    mainQuest.m_QuestInitialized = quest.Initialized;
                    CheckMainQuest(mainQuest);


                });
            
            }
            catch (Exception ex) 
            { }
        
        }
        else { m_MainQuestSystem.StartMainQuest(0); }
    }
    private void CheckMainQuest(MainQuest mainquest) 
    {
        if (mainquest.m_QuestInitialized) 
        {
            m_MainQuestSystem.StartMainQuest(mainquest._QuestID);
        }
        if (mainquest.m_QuestComplete) 
        {
            mainquest.InitializeComplete();
        }
    
    }
    #endregion

    #region Data
    [Serializable]
    public class MainQuestData 
    {
        public int QuestID;
        public bool Complete;
        public bool Initialized;
    }
    #endregion
}
