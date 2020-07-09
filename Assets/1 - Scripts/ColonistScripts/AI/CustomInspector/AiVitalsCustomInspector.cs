using ProjectColoni;
using UnityEditor;

namespace ColonistScripts.AI.CustomInspector
{
    [CustomEditor(typeof(AiVitals))]
    public class AiVitalsCustomInspector : Editor
    {
        private bool _enableDefaultInspector;
        
        public override void OnInspectorGUI()
        {
            _enableDefaultInspector = EditorGUILayout.Toggle("Enable Default Inspector" , _enableDefaultInspector);
            
            if(_enableDefaultInspector)
                base.OnInspectorGUI();
        }
    }
}
