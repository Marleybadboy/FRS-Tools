using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;

#region InfoBox
[TypeInfoBox("Don't touch \n" + "Don't Move \n" + "Don't Delete\n" +
    "It's only handler do detect objects")]
    #endregion
public class MainQuestUIHandle : MonoBehaviour
{

    #region Variabels
    [SerializeField] private Image _CircleImage;
    #endregion
    #region Properties
    public Sprite m_AssignCricle {  set { _CircleImage.sprite = value; } }
    public string m_KeyName { get => GetComponentInChildren<KeyLangugageItem>().key; set { GetComponentInChildren<KeyLangugageItem>().key = value; } }
    #endregion
    #region Functions
    #endregion

    #region Methods

    #endregion
}
