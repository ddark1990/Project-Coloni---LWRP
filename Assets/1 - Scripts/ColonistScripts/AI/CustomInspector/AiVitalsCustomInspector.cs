#if UNITY_EDITOR
using UnityEditor;

namespace ProjectColoni
{
    [CustomEditor(typeof(AiStats))]
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
#endif
