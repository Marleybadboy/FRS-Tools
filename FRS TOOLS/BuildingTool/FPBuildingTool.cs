# if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;
using System;
using System.Linq;

namespace FRS.Property
{
    public class FPBuildingTool : OdinEditorWindow
    {

        #region Variabels
        [HorizontalGroup("Data", 200, Title = "Model & Data"), PreviewField(125), ShowInInspector, HideLabel]
        [ValidateInput("ColliderDetect","MISSING BOX COLLIDER IN MODEL" + " ASSIGN BOX COLLIDER TO PARENT", InfoMessageType.Error)]
        private GameObject _Model;

        [HorizontalGroup("Data"), ShowInInspector, GUIColor(0.2f, 1f, 0.5f)]
        [BoxGroup("Data/Data")]
        private string _BuildingName;

        [HorizontalGroup("Data"), ShowInInspector]
        [BoxGroup("Data/Data")]
        [HideLabel]
        private BuildingStructData _BuildingStructData;

        [HorizontalGroup("Data"), ShowInInspector]
        [BoxGroup("Data/Data")]
        [VerticalGroup("Data/Data/Material")]
        [ValueDropdown("GhostMaterial", IsUniqueList = true)]
        private Material _GhostMaterial;


        [Space(8)]
        [HorizontalGroup("Icon", MarginRight = 350, Title = "Icon"), PreviewField(80), ShowInInspector,HideLabel] private Sprite _Icon;
        #endregion
        #region Properties
        string m_ModelGreenPath { get => "Assets/Resources/ForestProperty.Resources/FP.BuildingsGreenPlan/"; }
        string m_ModelPath { get => "Assets/Resources/ForestProperty.Resources/FP.BuildingsPrefabs/"; }
        string m_ScriptablePath { get => "Assets/Resources/ForestProperty.Resources/FP.Buildings/"; }
        int m_GreenPlanLayer { get => LayerMask.GetMask("GreenPlanCrafting"); }

        #endregion

        #region Methods
        [MenuItem("Tools/FPBuildingTool/Building Window")]
        private static void OpenWindow()
        {
            FPBuildingTool window = GetWindow<FPBuildingTool>("Building Window");

            window.minSize = new Vector2(550f, 350f);
            window.maxSize = new Vector2(550f, 350f);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(270, 200);
            window.titleContent = new GUIContent("Building Window", EditorIcons.House.Active);

            window.Show();
        }

        public bool ColliderDetect(GameObject obj) 
        {
            return obj.GetComponent<BoxCollider>();
        }

        [GUIColor(0.3f, 0.5f,0)]
        [Button(ButtonSizes.Large,DirtyOnClick = true)]
        public void CreateBuilding() 
        { 
            if(_Model != null) 
            {
                FPBuilding building = CreateScriptableData();
                AssetDatabase.CreateAsset(building, m_ScriptablePath + _BuildingName + ".asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }

       private GameObject CreateGreenPlan() 
       {
                GameObject newob = Instantiate(_Model);
                string path = m_ModelGreenPath + _BuildingName + "_Greenplan" + ".prefab";
                CheckModelComponents<MeshCollider>(ref newob);
                CheckModelComponents<Rigidbody>(ref newob);
                CheckModelComponents<MonoBehaviour>(ref newob);
                ChangeLayer(ref newob);
                newob.AddComponent<FPBuilidngPlacment>();
                GameObject green = PrefabUtility.SaveAsPrefabAsset(newob, path);
                green.name = _BuildingName + "_Greenplan";
                var renderdata = green.GetComponentsInChildren<MeshRenderer>();
                renderdata.ForEach(x => x.material = _GhostMaterial);
                DestroyImmediate(newob);
                return green;
       } 
       private void ChangeLayer(ref GameObject obj) 
       {
            obj.GetComponentsInChildren<Transform>().ForEach(go => go.SetLayer(m_GreenPlanLayer));
       }
       private void CheckModelComponents<T>(ref GameObject obj) where T : Component 
       { 
         if(obj.GetComponentsInChildren<T>().Length > 0) 
         {
                obj.GetComponentsInChildren<T>().ForEach(comp => DestroyImmediate(comp));

         }
       }
      /* private BoxCollider CreateBoxCollider(ref GameObject obj) 
       { 
            BoxCollider box =  
        
       }*/

        private FPBuilding CreateScriptableData() 
        { 
            FPBuilding building = new FPBuilding 
            { 
                Name = _BuildingName,
                BuildingStructData = _BuildingStructData,
                Building = _Model,
                BuildingGreenPlan = CreateGreenPlan(),
                Icon = _Icon,
            
            };
            return building;
        
        }

        private IEnumerable GhostMaterial() 
        { 
            return AssetDatabase.FindAssets("t:Material").
                Select(x => AssetDatabase.GUIDToAssetPath(x)).
                Select( x => new ValueDropdownItem(x, AssetDatabase.LoadAssetAtPath<Material>(x)));
        }

        #endregion
        #region Class
        [Serializable]
        public class Building 
        {
            [SerializeField] private float _Example;
        
        }
        #endregion

    }
}
#endif
