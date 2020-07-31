#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;

namespace ProjectColoni
{
    [CustomEditor(typeof(GlobalObjectDictionary))]
    public class GlobalObjectDictionaryInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (!GUILayout.Button("Debug Item Database") || !Application.isPlaying) return;
            
            Debug.Log("There is " + GlobalObjectDictionary.Instance.dictionary.Count + " items in the database.");

            foreach (var obj in GlobalObjectDictionary.Instance.dictionary)
            {
                Debug.Log("Object Id: " + obj.Key + " | Object Name: " + obj.Value);
            }
        }
    }
}
#endif