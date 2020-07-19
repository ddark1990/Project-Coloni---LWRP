#if UNITY_EDITOR
using UnityEditor;
namespace ProjectColoni
{
    [CustomEditor(typeof(AiSensors))]
    public class AiSensorsCustomInspector : Editor
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

