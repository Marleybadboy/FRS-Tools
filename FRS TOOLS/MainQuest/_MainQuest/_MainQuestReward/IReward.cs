

namespace FRS.MainQuest.Reward
{
    public interface IReward
    {
        #region Variabels
        public string Name { get => LanguageManager.instance?.GetLanguage(RewardNameKey);}
        public string RewardNameKey { get;}
        #endregion
        #region Methods
        public void AssignReward();
        public object GetReward();
        #endregion
    }

}
