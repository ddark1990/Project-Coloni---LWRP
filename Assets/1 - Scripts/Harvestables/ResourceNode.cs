using System;
using UnityEngine;

namespace ProjectColoni
{
    public class ResourceNode : Selectable
    {
        [SerializeField] private BaseScriptableData baseData; //for manual creation from scriptableObjects 
        public int amount;

        public BaseObjectData baseObjectInfo;

        
        private void Start()
        {
            InitializeSelectable();
            InitializeBaseObjectData();
        }

        /// <summary>
        /// Initialize base data from a scriptable object if one is available .
        /// </summary>
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData("", "", null);
        }
    }
}
