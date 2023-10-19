
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    #region Variabels
    public static LanguageManager instance;
    [SerializeField][Tooltip("Add Language Asset with keys!")] public List<TextAsset> _TextAssets = new List<TextAsset>();
    public List<Dictionary<string,string>> _LangDictionaries = new List<Dictionary<string,string>>();
    #endregion
    #region Functions
    private void Awake()
    {
        instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        StaticEvents.onLanguageChanged += Event_OnLanguageChange;
        HandleLanguage();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Dictionary<string, string>>GetLangDictionaries(List<TextAsset> list, int getline) 
    {
        List<Dictionary<string, string>> listdictionaries = new List<Dictionary<string, string>>();

        list.ForEach(t => listdictionaries.Add(CsvReader.ReadCSV(t, getline+1, 0)));
        return listdictionaries;


    }
    public string GetLanguage(string key) 
    {
        string language;
        var query = _LangDictionaries.Where(t => t.ContainsKey(key));
        if (query.Any()) 
        {
            language = query.First()[key];
            return language;
        }
        else {return null; }
        
    }
    #endregion
    #region HandleLanguage
    public void HandleLanguage()
    {

        try
        {
            switch (GlobalSettings.instance.set.generic.lang)
            {
                case ForestSettings.Generic.Lang.POLISH:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.POLISH);
                    break;
                case ForestSettings.Generic.Lang.ENGLISH:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.ENGLISH);
                    break;
                case ForestSettings.Generic.Lang.French:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.French);
                    break;
                case ForestSettings.Generic.Lang.German:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.German);
                    break;
                case ForestSettings.Generic.Lang.Spanish:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.Spanish);
                    break;
                case ForestSettings.Generic.Lang.Italian:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.Italian);
                    break;
                case ForestSettings.Generic.Lang.Russian:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.Russian);
                    break;
                case ForestSettings.Generic.Lang.Simplified_Chinese:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.Simplified_Chinese);
                    break;
                case ForestSettings.Generic.Lang.TÜRKÇE:
                    _LangDictionaries = GetLangDictionaries(_TextAssets, (int)ForestSettings.Generic.Lang.TÜRKÇE);
                    break;
                case ForestSettings.Generic.Lang.NUM_LANGS:
                    break;
                default:
                    break;

            }
        }
        catch (System.Exception)
        {

        }
    }

    public void Event_OnLanguageChange(object o, EventArgs e) 
    {
        HandleLanguage();
    }

    private void OnDestroy()
    {
        StaticEvents.onLanguageChanged += Event_OnLanguageChange;
    }
    #endregion
}
