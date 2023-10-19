
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System;

public class KeyBindingElement : MonoBehaviour
{

    #region Variabels
    [SerializeField] private InputActionReference ActionReference;
    [SerializeField] private string KeyWaitBinding;
    [SerializeField] private TextMeshProUGUI BindKeyText;
    [SerializeField] private KeyLangugageItem BindingProcess;
    [SerializeField] private KeyLangugageItem KeyForSave;
    [SerializeField] private BindeType m_BindType;
    [SerializeField] private Image _ButtonImage;
    [SerializeField] private string SaveString { get { return KeyForSave.key; } }
    public string KeyManager { get { return SaveString; } }
    public string BindKey {get { return GetBindDefault(); } }
    [ShowIf("m_BindType", BindeType.COMPOSITE)][SerializeField] private int AxisIndex;
    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;
    public enum BindeType {BUTTON, COMPOSITE }
    #endregion
    #region Properties
    #endregion

    #region Functions
    // Start is called before the first frame update
    void Start()
    {
        StaticEvents.onLanguageChanged += StaticEvent_LangugeChange;
        BindingProcess.enabled = false;
        string rebinds = PlayerPrefs.GetString(SaveString, string.Empty);
        if(string.IsNullOrEmpty(rebinds))
        {
            InputBindingText(ActionReference.action);
            return; 
        }
        ActionReference.action.LoadBindingOverridesFromJson(rebinds);
        InputBindingText(ActionReference.action);
    }
    private void OnDestroy()
    {
        StaticEvents.onLanguageChanged -= StaticEvent_LangugeChange;
    }

    #endregion

    #region Methods
    public void StartRebaniding() 
    {
        
        BindingProcess.key = KeyWaitBinding;
        BindingProcess.enabled = true;
        BindingProcess.GetOnlyText();
        _ButtonImage.color = Color.yellow;
        if (m_BindType == BindeType.BUTTON)
        {

            rebindingOperation = ActionReference.action.PerformInteractiveRebinding().WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete())
                .Start();
        }
        if(m_BindType == BindeType.COMPOSITE) 
        {
            rebindingOperation = ActionReference.action.PerformInteractiveRebinding().WithTargetBinding(AxisIndex).WithControlsExcluding("Mouse")
                .OnMatchWaitForAnother(0.1f)
                .OnComplete(operation => RebindComplete())
                .Start();


        }
    
    }
    private void RebindComplete() 
    {
        InputBindingText(ActionReference.action);
        rebindingOperation.Dispose();
        Save();
        _ButtonImage.color = Color.white;
        BindingProcess.key = "";
        BindingProcess.enabled = false;


    }

    private void KeyText() 
    {
        int index = ActionReference.action.GetBindingIndexForControl(ActionReference.action.controls[0]);
        BindKeyText.text = InputControlPath.ToHumanReadableString(ActionReference.action.bindings[index].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    private void KeyTextAxisBinding() 
    {
        int index = ActionReference.action.GetBindingIndexForControl(ActionReference.action.controls[AxisIndex]);
        BindKeyText.text = InputControlPath.ToHumanReadableString(ActionReference.action.bindings[index].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    void InputBindingText(InputAction action) 
    {
        var BindIndexes = 0;
        if(m_BindType == BindeType.BUTTON) 
        {
           BindIndexes = action.GetBindingIndexForControl(action.controls[0]);
        }
        if(m_BindType == BindeType.COMPOSITE) 
        {
            BindIndexes = AxisIndex;
        
        }

        BindKeyText.text = InputControlPath.ToHumanReadableString(action.bindings[BindIndexes].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    void Save() 
    { 
    
        string rebind = ActionReference.action.SaveBindingOverridesAsJson();
        PlayerPrefs.SetString(SaveString, rebind);
    
    }
    private void StaticEvent_LangugeChange(object o, EventArgs e) 
    {
        /*BindingProcess.enabled = false;
        string rebinds = PlayerPrefs.GetString(SaveString, string.Empty);
        if (string.IsNullOrEmpty(rebinds))
        {
            InputBindingText(ActionReference.action);
            return;
        }
        ActionReference.action.LoadBindingOverridesFromJson(rebinds);
        InputBindingText(ActionReference.action);*/

    }

    private string GetBindDefault() 
    {

        var BindIndexes = 0;
        if (m_BindType == BindeType.BUTTON)
        {
            BindIndexes = ActionReference.action.GetBindingIndexForControl(ActionReference.action.controls[0]);
        }
        if (m_BindType == BindeType.COMPOSITE)
        {
            BindIndexes = AxisIndex;

        }

        return InputControlPath.ToHumanReadableString(ActionReference.action.bindings[BindIndexes].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
    }

    public void LoadDefaultBinding(string rebind) 
    {   
        ActionReference.action.LoadBindingOverridesFromJson(rebind);
        InputBindingText(ActionReference.action);
    }
}
    #endregion 

