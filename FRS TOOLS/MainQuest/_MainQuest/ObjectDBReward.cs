using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectDBReward : MainQuestObjectDB
{
    #region Variabels
    public TextMeshProUGUI _RewardText;
    #endregion
    #region Properties
    public string m_RewardText { get => _RewardText.text; set { _RewardText.text = value;} }
    #endregion
}
