#if UNITY_EDITOR
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    public class ConvertGameObjects : EditorWindow
    {
        private string[] ConversionType { get; } = {"Ai", "Item", "Node"};
        private int _index;

        private bool _convertGameObjectsPressed;
        private bool _clearLastConvertedPressed;
        
        //debug
        [Range(0,100)]public float debugFloat1;
        private GameObject[] _tempSelectedObjects;
        
        private static readonly int Stencil = Shader.PropertyToID("_Stencil");
        private static readonly int WriteMask = Shader.PropertyToID("_WriteMask");
        private static readonly int Smoothness = Shader.PropertyToID("_Smoothness");


        [MenuItem("ProjectColoni/ConvertGameObjectsWizard")]
        public static void OpenWindow()
        {
            GetWindow<ConvertGameObjects>("ConvertWizard");
        }

        private void OnInspectorUpdate()
        {
            Repaint();
        }

        private void OnEnable()
        {
            _tempSelectedObjects = new GameObject[0];
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Height(10)); //START
            
            EditorGUILayout.BeginVertical(GUI.skin.box, GUILayout.Width(10));
            
            GUILayout.Label("Conversion Type");

            _index = EditorGUILayout.Popup(_index, ConversionType);
            
            EditorGUILayout.EndVertical();
            
            //convert buttons (lower)
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            _convertGameObjectsPressed = GUILayout.Button("Convert GameObjects: " + Selection.gameObjects.Length, GUILayout.Height(50));
            _clearLastConvertedPressed = GUILayout.Button("Clear Last Converted: " + _tempSelectedObjects.Length, GUILayout.Height(50));
            EditorGUILayout.EndHorizontal();

            if (_convertGameObjectsPressed) Convert();
            if (_clearLastConvertedPressed) ClearLastConverted();

            //debug
            /*EditorGUILayout.BeginVertical(GUI.skin.window, GUILayout.Height(100));

            //debugFloat1 = EditorGUILayout.FloatField(debugFloat1);
            debugFloat1 = EditorGUILayout.Slider(debugFloat1, 0, 500);
            
            EditorGUILayout.EndVertical();*/
            
            EditorGUILayout.EndVertical(); //END
        }

        private void Convert()
        {
            var selectedObjects = Selection.gameObjects;
            var tempListArray = _tempSelectedObjects.ToList(); //caching for clearing

            foreach (var selectedObject in selectedObjects)
            {
                AddBaseComponents(selectedObject);

                switch (_index)
                {
                    case 0: //ai
                        if (!selectedObject.GetComponentInChildren<AiController>())
                        {
                            selectedObject.AddComponent<Animator>();
                            selectedObject.AddComponent<NavMeshAgent>();
                            selectedObject.AddComponent<AiController>();
                        }

                        Debug.Log("Converted " + selectedObject + " to ai.", selectedObject);

                        break;
                    case 1: //item
                        if (!selectedObject.GetComponentInChildren<Item>()) selectedObject.AddComponent<Item>();

                        Debug.Log("Converted " + selectedObject + " to item.", selectedObject);

                        break;
                    case 2: //node
                        if (!selectedObject.GetComponentInChildren<Node>()) selectedObject.AddComponent<Node>();

                        Debug.Log("Converted " + selectedObject + " to node.", selectedObject);

                        break;
                }
                
                tempListArray.Add(selectedObject);
            }

            _tempSelectedObjects = tempListArray.ToArray();

            Debug.Log("Converted " + selectedObjects.Length + " objects.");
        }

        private void AddBaseComponents(GameObject selectedObject)
        {
            selectedObject.layer = LayerMask.NameToLayer("Selectable"); //set to selectable layer

            ClearConflictingComponents(selectedObject);
            
            var collider = selectedObject.AddComponent<MeshCollider>();
            collider.convex = true;
            
            var rb = selectedObject.AddComponent<Rigidbody>();
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
                        
            var outline = selectedObject.AddComponent<OutlineRelay>();

            var sharedMat = selectedObject.GetComponentInChildren<Renderer>().sharedMaterial; //will be dependant on the shader to exist
            
            sharedMat.shader = Shader.Find("Lux URP/Lit Extended");
            sharedMat.SetInt(Stencil, 1);
            sharedMat.SetInt(WriteMask, 0);
            sharedMat.SetFloat(Smoothness, 0);
            sharedMat.enableInstancing = true;
        }

        private void ClearConflictingComponents(GameObject selectedObject) //remove all base classes like item, node, aicontroller
        {
            //general
            if(selectedObject.GetComponentInChildren<Collider>())
                DestroyImmediate(selectedObject.GetComponentInChildren<Collider>());
            if(selectedObject.GetComponentInChildren<Rigidbody>())
                DestroyImmediate(selectedObject.GetComponentInChildren<Rigidbody>());
            
            //primary
            if(selectedObject.GetComponentInChildren<Node>())
                DestroyImmediate(selectedObject.GetComponentInChildren<Node>());
            if(selectedObject.GetComponentInChildren<Item>())
                DestroyImmediate(selectedObject.GetComponentInChildren<Item>());
            if (selectedObject.GetComponentInChildren<AiController>())
            {
                DestroyImmediate(selectedObject.GetComponentInChildren<AiController>());
                DestroyImmediate(selectedObject.GetComponentInChildren<Animator>());
                DestroyImmediate(selectedObject.GetComponentInChildren<NavMeshAgent>());
                DestroyImmediate(selectedObject.GetComponentInChildren<AiStatus>());
                DestroyImmediate(selectedObject.GetComponentInChildren<AiSensors>());
                DestroyImmediate(selectedObject.GetComponentInChildren<AiStats>());
            }
            
            if(selectedObject.GetComponentInChildren<OutlineRelay>()) //dependant on selectable
                DestroyImmediate(selectedObject.GetComponentInChildren<OutlineRelay>());

        }

        private void ClearLastConverted()
        {
            foreach (var selectedObject in _tempSelectedObjects)
            {
                ClearConflictingComponents(selectedObject);
            }
            
            _tempSelectedObjects = new GameObject[0];
        }
    }
}
#endif
