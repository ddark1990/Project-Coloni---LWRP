using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace ProjectColoni
{
    [CustomEditor(typeof(BuildingManager))]
    public class BuildingManagerCustomInspector : Editor
    {
        private List<bool> _showPart;

        private SerializedProperty _buildPartsList;
        
        private bool _showBaseInspector;
        
        private float slider1;
        private float slider2;
        private float slider3;
        private float slider4;

        private void OnEnable()
        {
            _showPart = new List<bool>();
        }
        
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            
            var buildingManager = target as BuildingManager;
            _buildPartsList = serializedObject.FindProperty("buildingParts");
            
            while (_showPart.Count < _buildPartsList.arraySize)
            {
                _showPart.Add(false); 
            }
            
            EditorGUILayout.BeginVertical(GUI.skin.box);
            
            EditorGUILayout.LabelField("Building Parts", EditorStyles.boldLabel);
            
            EditorGUILayout.EndVertical();
            
            //EditorGUILayout.BeginVertical(GUI.skin.button);

            for (int i = 0; i < _buildPartsList.arraySize; i++)
            {
                var listRef = _buildPartsList.GetArrayElementAtIndex(i);
                var partName = listRef.FindPropertyRelative("partName");
                var prefab = listRef.FindPropertyRelative("prefab");
                
                var rect = EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.BeginVertical(GUI.skin.button, GUILayout.Height(23));
                _showPart[i] = EditorGUILayout.Foldout(_showPart[i], partName.enumNames[partName.enumValueIndex]);

                if (_showPart[i])
                {
                    /*
                    EditorGUI.PropertyField(new Rect(rect.x + slider1, rect.y + slider2, rect.width + slider3, rect.height + slider4), partName);
                    EditorGUI.PropertyField(new Rect(rect.x + slider1, rect.y + slider2, rect.width + slider3, rect.height + slider4), part);
                    */
                    
                    EditorGUILayout.PropertyField(partName);
                    EditorGUILayout.PropertyField(prefab);

                }

                EditorGUILayout.EndVertical();
                
                var removeRect = EditorGUILayout.BeginHorizontal(GUI.skin.box);
                var onRemoveButtonPressed = GUILayout.Button(" - ", GUILayout.Width(57), GUILayout.ExpandHeight(true));
                if (onRemoveButtonPressed) _buildPartsList.DeleteArrayElementAtIndex(i);

                if (_showPart[i])
                {
                    if (!onRemoveButtonPressed && prefab.objectReferenceValue != null)
                    {
                        var partObject = new SerializedObject(prefab.objectReferenceValue);
                
                        var icon = AssetPreview.GetAssetPreview(prefab.objectReferenceValue);

                        //GUI.DrawTexture(new Rect(removeRect.x + slider1, removeRect.y + slider2, removeRect.width + slider3, removeRect.height + slider4), icon, ScaleMode.ScaleToFit);
                        if(prefab.objectReferenceValue != null)
                            EditorGUI.DrawPreviewTexture(new Rect(removeRect.x + 7.5f, removeRect.y + 7.5f, removeRect.width + -15, removeRect.height + -15), icon);
                    }
                }
                

                EditorGUILayout.EndHorizontal();

                EditorGUILayout.EndHorizontal();
            }

            //EditorGUILayout.EndVertical();

                
            EditorGUILayout.BeginHorizontal(GUI.skin.button);
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            var onAddNewBuildPartPressed = GUILayout.Button("Add New Building Part", /*GUILayout.Width(200),*/ GUILayout.Height(25));

            var lastIndex = _buildPartsList.arraySize;

            if (onAddNewBuildPartPressed)
            {
                _buildPartsList.arraySize++;
                var lastPart = _buildPartsList.GetArrayElementAtIndex(lastIndex);
                var partName = lastPart.FindPropertyRelative("partName");
                var prefab = lastPart.FindPropertyRelative("prefab");
                
                prefab.objectReferenceValue = null;
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndHorizontal();


            /*
            EditorGUILayout.BeginHorizontal(GUI.skin.box); //for debugs 
            
            slider1 = EditorGUILayout.Slider(slider1, -350, 750);
            slider2 = EditorGUILayout.Slider(slider2, -350, 750);
            slider3 = EditorGUILayout.Slider(slider3, -350, 750);
            slider4 = EditorGUILayout.Slider(slider4, -350, 750);

            EditorGUILayout.EndHorizontal();
            */

            
            serializedObject.ApplyModifiedProperties();
            
            _showBaseInspector = EditorGUILayout.Toggle("Show Base Inspector" ,_showBaseInspector);
            
            if(_showBaseInspector)
                base.OnInspectorGUI();

        }

        private void OnValidate()
        {
            serializedObject.ApplyModifiedProperties();
        }
        
        private static Texture2D TextureField(Texture2D texture, float width, float height)
        {
            GUILayout.BeginVertical();
            var style = new GUIStyle(GUI.skin.label);
            style.alignment = TextAnchor.UpperCenter;
            style.fixedWidth = 70;
            var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(width), GUILayout.Height(height));
            GUILayout.EndVertical();
            return result;
        }
    }
}
