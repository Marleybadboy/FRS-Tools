
using UnityEngine;
using Sirenix.OdinInspector;
using System;

namespace FRS.Property
{
    [Serializable]
    public struct BuildingStructData
    {

        #region Variabels
        [ShowInInspector, GUIColor(0, 1, 0), Space(2)]
        private int _Cost;
        [ShowInInspector, GUIColor(0.8f, 0.4f, 0.3f), Space(2)]
        private int _ParkRank;
        [ShowInInspector, GUIColor(0.8f, 0.4f, 1f), Space(2)]
        private string _BuildingNameKey;
        #endregion

        #region Functions
        #endregion

        #region Methods

        #endregion
    }
}
