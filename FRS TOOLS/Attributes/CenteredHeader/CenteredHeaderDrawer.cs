#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(CenteredHeaderAttribute))]
public class CenteredHeaderDrawer : DecoratorDrawer
{

    #region Variabels

    #endregion

    #region Functions
    public override void OnGUI(Rect position)
    {
        CenteredHeaderAttribute headerAttribute = (CenteredHeaderAttribute)attribute;
        GUIStyle centeredStyle = new GUIStyle(GUI.skin.label);
        centeredStyle.alignment = TextAnchor.MiddleCenter;
        centeredStyle.fontSize = headerAttribute.FontSize;
        EditorGUI.LabelField(position, headerAttribute.headerText, centeredStyle);
    }
    #endregion

    #region Methods
    public override float GetHeight()
    {
        return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
    #endregion
}
#endif
