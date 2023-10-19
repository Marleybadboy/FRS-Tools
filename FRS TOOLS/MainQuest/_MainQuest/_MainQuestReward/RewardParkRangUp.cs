
using FRS.MainQuest.Reward;
using Sirenix.OdinInspector;
using UnityEngine.UI;

public class RewardParkRangUp : IReward
{
    #region Variabels
    [Title("Reward Value", TitleAlignment = TitleAlignments.Centered)] public int _PointToAdd;

    public string RewardNameKey => "QUEST_REWARD_PARKPOINT";
    #endregion
    #region Properties
    #endregion
    [Button("Update Stats", ButtonSizes.Large)]
    public void AssignReward()
    {
        GeneralParkStatus.Instance?.UpdateParkStatus();
       
    }

    public object GetReward()
    {
        return _PointToAdd;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }
}
