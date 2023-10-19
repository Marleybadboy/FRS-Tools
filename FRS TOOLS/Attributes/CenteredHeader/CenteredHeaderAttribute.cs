
using UnityEngine;



public class CenteredHeaderAttribute : PropertyAttribute
{

    #region Variabels
    public string headerText;
    public int FontSize;
     #endregion 

    #region Functions
    #endregion 

     #region Methods
    public CenteredHeaderAttribute(string text) 
    { 
        headerText = text;
        FontSize = 12;
    }
    public CenteredHeaderAttribute(string text, int fontsize)
    {
        headerText = text;
        FontSize = fontsize;
    }
    #endregion 
}
