
using System.Collections.Generic;
using System;
using FRS.MainQuest;
using Sirenix.Utilities;
using FRS.MainQuest.UI;

/* The MainQuestSave class is responsible for serializing and deserializing main quest data for saving
and loading game progress. */
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
    /// <summary>
    /// The function serializes the data of the main quest database into a save data
    /// format.
    /// </summary>
    
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
/// <summary>
/// The function deserializes saved game data for main quests and updates the corresponding quest
/// objects and UI elements.
/// </summary>

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
 /// <summary>
 /// The CheckMainQuest function checks if a main quest is initialized and starts it if it is, and also
 /// initializes it as complete if it is marked as such.
 /// </summary>
 /// <param name="MainQuest">MainQuest is a class that represents a main quest in a game. It has
 /// properties such as m_QuestInitialized and m_QuestComplete, which indicate whether the quest has
 /// been initialized and whether it has been completed, respectively. The class also has a method
 /// called InitializeComplete() that is used to</param>
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

/* The MainQuestData class is a serializable class that contains information about a main quest,
including its ID, completion status, and initialization status. */
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
