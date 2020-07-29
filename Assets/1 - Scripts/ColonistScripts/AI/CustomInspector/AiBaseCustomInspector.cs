/*
using ProjectColoni;
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace ColonistScripts.AI.CustomInspector
{
    [CustomEditor(typeof(AiController))]
    public class AiBaseCustomInspector : Editor
    {
        private bool _showBaseInspector;
        
        public override void OnInspectorGUI()
        {
            var controller = (AiController)target;
            

            EditorGUILayout.BeginVertical(GUI.skin.box);
            GUILayout.FlexibleSpace();
            GUILayout.Label("Vitals");    
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUI.skin.box);

            //EditorWindow.focusedWindow.Repaint();
            if(Application.isPlaying) Repaint();
            
            var healthFloat = Application.isPlaying ? EditorGUILayout.FloatField("Health", controller.stats.stats.Health) : EditorGUILayout.FloatField("Health", 0);
            var hungerFloat = Application.isPlaying ? EditorGUILayout.FloatField("Hunger", controller.stats.stats.Food) : EditorGUILayout.FloatField("Hunger", 0);
            var staminaFloat = Application.isPlaying ? EditorGUILayout.FloatField("Stamina", controller.stats.stats.Stamina) : EditorGUILayout.FloatField("Stamina", 0);
            var energyFloat = Application.isPlaying ? EditorGUILayout.FloatField("Energy", controller.stats.stats.Energy) : EditorGUILayout.FloatField("Energy", 0);

            EditorGUILayout.EndVertical();

            
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            GUILayout.Label("Sensors");   
            controller.enableSensors = EditorGUILayout.Toggle("Enable Sensors" , controller.enableSensors);
            EditorGUILayout.EndHorizontal(); 
            
            if (controller.enableSensors)
            {
                EditorGUILayout.BeginHorizontal(GUI.skin.box);

                //var enableOverlap = EditorGUILayout.Toggle("Enable OverlapSphereCast" , controller.sensors.EnableOverlapSphereCast);
            
                EditorGUILayout.EndHorizontal(); 
            }
            
            
            
            _showBaseInspector = EditorGUILayout.Toggle("Show Base Inspector" ,_showBaseInspector);

            if(_showBaseInspector)
                base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            //Repaint();
        }
    }
}
#endif
*/
