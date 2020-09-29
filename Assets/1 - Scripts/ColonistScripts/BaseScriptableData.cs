using UnityEngine;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(BaseData)", menuName = "ProjectColoni/Objects/BaseScriptableData", order = 0)]
    public class BaseScriptableData : ScriptableObject
    {
        public string objectName;
        [TextArea] public string description;
        public Texture spriteTexture;
    }
}
