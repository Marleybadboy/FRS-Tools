
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

namespace FRS.KeyBinding
{
    public class KeyBindingElement : MonoBehaviour
    {

        #region Variabels
        public enum BindeType { BUTTON, COMPOSITE }
        [SerializeField] private InputActionReference _ActionReference;
        [SerializeField] private string _KeyWaitBinding;
        [SerializeField] private TextMeshProUGUI _BindKeyText;
        [SerializeField] private KeyLangugageItem _BindingProcess;
        [SerializeField] private KeyLangugageItem _KeyForSave;
        [SerializeField] private BindeType _BindType;
        [SerializeField] private Image _ButtonImage;
        [ShowIf("m_BindType", BindeType.COMPOSITE), SerializeField] private int m_AxisIndex;
        private InputActionRebindingExtensions.RebindingOperation _rebindingOperation;
        #endregion
        #region Properties
        [SerializeField] private string m_SaveString { get { return _KeyForSave.key; } }
        public string m_KeyManager { get { return m_SaveString; } }
        public string m_BindKey { get { return GetBindDefault(); } }
        #endregion

        #region Functions
        // Start is called before the first frame update
        void Start()
        {
            _BindingProcess.enabled = false;
            string rebinds = PlayerPrefs.GetString(m_SaveString, string.Empty); // Checking if in playerpref exist save for bindedkey
            if (string.IsNullOrEmpty(rebinds))
            {
                InputBindingText(_ActionReference.action);
                return;
            }
            _ActionReference.action.LoadBindingOverridesFromJson(rebinds);
            InputBindingText(_ActionReference.action);
        }
        #endregion

        #region Methods
        public void StartRebaniding()
        {

            _BindingProcess.key = _KeyWaitBinding;
            _BindingProcess.enabled = true;
            _BindingProcess.GetOnlyText();
            _ButtonImage.color = Color.yellow;
            if (_BindType == BindeType.BUTTON)
            {

                _rebindingOperation = _ActionReference.action.PerformInteractiveRebinding().WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(operation => RebindComplete())
                    .Start();
            }
            if (_BindType == BindeType.COMPOSITE)
            {
                _rebindingOperation = _ActionReference.action.PerformInteractiveRebinding().WithTargetBinding(m_AxisIndex).WithControlsExcluding("Mouse")
                    .OnMatchWaitForAnother(0.1f)
                    .OnComplete(operation => RebindComplete())
                    .Start();


            }

        }
        private void RebindComplete()
        {
            InputBindingText(_ActionReference.action);
            _rebindingOperation.Dispose();
            Save();
            _ButtonImage.color = Color.white;
            _BindingProcess.key = "";
            _BindingProcess.enabled = false;


        }

        void InputBindingText(InputAction action)
        {
            var BindIndexes = 0;
            if (_BindType == BindeType.BUTTON)
            {
                BindIndexes = action.GetBindingIndexForControl(action.controls[0]);
            }
            if (_BindType == BindeType.COMPOSITE)
            {
                BindIndexes = m_AxisIndex;

            }

            _BindKeyText.text = InputControlPath.ToHumanReadableString(action.bindings[BindIndexes].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }

        void Save()
        {

            string rebind = _ActionReference.action.SaveBindingOverridesAsJson();
            PlayerPrefs.SetString(m_SaveString, rebind);

        }

        private string GetBindDefault()
        {

            var BindIndexes = 0;
            if (_BindType == BindeType.BUTTON)
            {
                BindIndexes = _ActionReference.action.GetBindingIndexForControl(_ActionReference.action.controls[0]);
            }
            if (_BindType == BindeType.COMPOSITE)
            {
                BindIndexes = m_AxisIndex;

            }

            return InputControlPath.ToHumanReadableString(_ActionReference.action.bindings[BindIndexes].effectivePath, InputControlPath.HumanReadableStringOptions.OmitDevice);
        }

        public void LoadDefaultBinding(string rebind)
        {
            _ActionReference.action.LoadBindingOverridesFromJson(rebind);
            InputBindingText(_ActionReference.action);
        }
    }
}
    #endregion 

