using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectColoni
{
    [CustomEditor(typeof(BuildingPart))]
    public class BuildPartCustomInspector : Editor
    {
        private bool _showBaseInspector;
        private SerializedProperty _snappingPartsList;
        private List<bool> _showPart;
        private List<bool> _showSnapPoint;

        private float slider1;
        private float slider2;
        private float slider3;
        private float slider4;

        private void OnEnable()
        {
            _showPart = new List<bool>();
            _showSnapPoint = new List<bool>();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            var buildOnLayers = serializedObject.FindProperty("buildOn");
            _snappingPartsList = serializedObject.FindProperty("snappingParts");
            
            while (_showPart.Count < _snappingPartsList.arraySize)
            {
                _showPart.Add(false); 
            }
            EditorGUILayout.PropertyField(buildOnLayers);
            
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
            EditorGUILayout.LabelField("Snapping Parts (What Snaps To This Part)", EditorStyles.boldLabel);
            var onAddSnappingPartPress = GUILayout.Button(" + ", EditorStyles.miniButton, GUILayout.Width(50));
            
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < _snappingPartsList.arraySize; i++)
            {
                var snappingPartsRef = _snappingPartsList.GetArrayElementAtIndex(i);
                var partName = snappingPartsRef.FindPropertyRelative("partName");
                var snapPoints = snappingPartsRef.FindPropertyRelative("snapPoints");
                
                while (_showSnapPoint.Count < snapPoints.arraySize)
                {
                    _showSnapPoint.Add(false); 
                }

                EditorGUILayout.BeginHorizontal(GUI.skin.button, GUILayout.Height(23));
                EditorGUILayout.BeginHorizontal(GUI.skin.box);

                _showPart[i] = EditorGUILayout.Foldout(_showPart[i], partName.enumNames[partName.enumValueIndex]);
                
                var onDeletePartPress = GUILayout.Button(" - ", EditorStyles.miniButton, GUILayout.Width(40));
                if (onDeletePartPress) _snappingPartsList.DeleteArrayElementAtIndex(i);

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndHorizontal();

                if (_showPart[i] && !onDeletePartPress)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                    
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
            
                    EditorGUILayout.LabelField("Snapping Points", EditorStyles.boldLabel);
                    var onAddSnappingPointPress = GUILayout.Button(" + ", EditorStyles.miniButton, GUILayout.Width(50));
            
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.PropertyField(partName);


                    for (int j = 0; j < snapPoints.arraySize; j++)
                    {
                        var snapPointsRef = snapPoints.GetArrayElementAtIndex(j);
                        var snapPointName = snapPointsRef.FindPropertyRelative("name");
                        var snapPosition = snapPointsRef.FindPropertyRelative("position");
                        var snapRotation = snapPointsRef.FindPropertyRelative("rotation");

                        EditorGUI.indentLevel++;
                        EditorGUILayout.BeginHorizontal(GUI.skin.button);

                        _showSnapPoint[j] = EditorGUILayout.Foldout(_showSnapPoint[j], snapPointName.stringValue);
                        var onDeleteSnapPointPress = GUILayout.Button(" - ", EditorStyles.miniButton, GUILayout.Width(35));
                        if (onDeleteSnapPointPress) snapPoints.DeleteArrayElementAtIndex(j);

                        EditorGUILayout.EndHorizontal();
                        EditorGUI.indentLevel--;

                        if (_showSnapPoint[j])
                        {

                            EditorGUILayout.BeginVertical(GUI.skin.box);
                        
                            EditorGUILayout.PropertyField(snapPointName);
                            EditorGUILayout.PropertyField(snapPosition);
                            EditorGUILayout.PropertyField(snapRotation);

                            EditorGUILayout.EndVertical();
                        }

                    }
                    
                    EditorGUILayout.EndVertical();
                    
                    var lastSnappingPointsIndex = snapPoints.arraySize;

                    if (onAddSnappingPointPress)
                    {
                        snapPoints.arraySize++;
                        
                        var lastSnapPoint = snapPoints.GetArrayElementAtIndex(lastSnappingPointsIndex);
                        var snapPointName = lastSnapPoint.FindPropertyRelative("name");
                        /*
                        var snapPoint = lastSnapPoint.FindPropertyRelative("point");
                        var snapPointRotation = lastSnapPoint.FindPropertyRelative("rotation");
                        */

                        snapPointName.stringValue = "New Snap Point";
                    }
                }
            }
            
            var lastSnappingPartsIndex = _snappingPartsList.arraySize;

            if (onAddSnappingPartPress)
            {
                _snappingPartsList.arraySize++;
                
                var lastPart = _snappingPartsList.GetArrayElementAtIndex(lastSnappingPartsIndex);
                var snapPoints = lastPart.FindPropertyRelative("snapPoints");

                snapPoints.arraySize = 0;
            }

            /*
            EditorGUILayout.BeginHorizontal(GUI.skin.box); //for debugs 
            
            slider1 = EditorGUILayout.Slider(slider1, -350, 750);
            slider2 = EditorGUILayout.Slider(slider2, -350, 750);
            slider3 = EditorGUILayout.Slider(slider3, -350, 750);
            //slider4 = EditorGUILayout.Slider(slider4, -350, 750);

            EditorGUILayout.EndHorizontal();
            */
            
            serializedObject.ApplyModifiedProperties();
            
            _showBaseInspector = EditorGUILayout.Toggle("Show Base Inspector" ,_showBaseInspector);
            
            if(_showBaseInspector)
                base.OnInspectorGUI();
        }
    }
}
