
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using System.Linq;
using Sirenix.Utilities;
using System.Collections.Generic;

namespace FRS.KeyBinding
{
    public class KeyBindingInputManager : MonoBehaviour
    {

        #region Variabels
        [Header("UI Reset Default element")]
        [SerializeField] private GameObject UIResetInformation;
        [SerializeField] private RectTransform UIResetParent;
        [SerializeField] private int IndexParentButton;
        [SerializeField] private int IndexNoButton;
        [SerializeField] private int IndexYesButton;
        [SerializeField] private string TempNameDefault = "Default save";
        private GameObject SavedInformation;
        #endregion
        #region Properties
        private RectTransform m_ParentButton { get { return SavedInformation.transform.GetChild(IndexParentButton).GetComponent<RectTransform>(); } }
        private Button NoButton { get { return m_ParentButton.GetChild(IndexNoButton).GetComponent<Button>(); } }
        private Button YesButton { get { return m_ParentButton.GetChild(IndexYesButton).GetComponent<Button>(); } }
        private ForestSettings.BindingDefault BindingData { get { return FindObjectOfType<SettingsUtility>().m_ForestSettings._BindingDefault; } set { FindObjectOfType<SettingsUtility>().m_ForestSettings._BindingDefault = value; } }
        #endregion
        #region Functions
        void Start()
        {

        }
        #endregion

        #region Methods
        public void OpenWindow()
        {
            if (SavedInformation != null)
            {
                Destroy(SavedInformation);
            }
            SavedInformation = Instantiate(UIResetInformation, UIResetParent);
            NoButton.onClick.AddListener(CloseWindow);
            YesButton.onClick.AddListener(ResetRebinding);
        }

        public void CloseWindow()
        {
            if (SavedInformation != null)
            {
                NoButton.onClick.RemoveAllListeners();
                YesButton.onClick.RemoveAllListeners();
                Destroy(SavedInformation);
            }

        }
        // Reset to default binding 
        public void ResetRebinding()
        {
            KeyBindingElement[] keybinding = GetComponentsInChildren<KeyBindingElement>().ToArray();
            CheckBinding(ref keybinding, GetBindingDefaultData());
            CloseWindow();


        }

        // Changing binding key if was changed and delete saved data from playerpref
        private void CheckBinding(ref KeyBindingElement[] keybinding, Dictionary<string, string> bindingdata)
        {
            keybinding.ForEach(binding =>
            {
                if (PlayerPrefs.HasKey(binding.m_KeyManager))
                {
                    string key = binding.m_KeyManager;
                    string jsonread = JsonUtility.ToJson(bindingdata[key]);
                    binding.LoadDefaultBinding(jsonread);
                    PlayerPrefs.DeleteKey(binding.m_KeyManager);
                }
            });

        }
        // Creating Dictionary to save default binding
        private Dictionary<string, string> GetBindingDefaultData()
        {
            Dictionary<string, string> defaultdata = new Dictionary<string, string>();
            for (int i = 0; i < BindingData.bindingdatakeys.Count; i++)
            {
                defaultdata.Add(BindingData.bindingdatakeys[i], BindingData.bindingdatavalue[i]);

            }
            if (defaultdata.Count != BindingData.bindingdatakeys.Count)
            {
                Debug.LogError($"Wrong binding data in dictionary {defaultdata.Count} is diffrent {BindingData.bindingdatakeys.Count}");
            }
            defaultdata.ForEach(x => Debug.Log($"{x.Key} + {x.Value}"));
            return defaultdata;
        }
        #endregion
        #region Editor Method
#if UNITY_EDITOR
        //Method to save the default Keybindings in ForrestSetting
        [Button("Save Default Data")]
        public void SaveDefaultBinding()
        {
            ForestSettings.BindingDefault binding = new ForestSettings.BindingDefault();
            KeyBindingElement[] keybinding = GetComponentsInChildren<KeyBindingElement>().ToArray();
            keybinding.ForEach(key =>
            {
                binding.bindingdatakeys.Add(key.m_KeyManager);
                binding.bindingdatavalue.Add(key.m_BindKey);


            });
            binding.BindingName = TempNameDefault;
            BindingData = binding;

        }
#endif
        #endregion
    }
}

