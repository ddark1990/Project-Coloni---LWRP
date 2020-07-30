using System;
using UnityEngine;

namespace ProjectColoni
{
    public class Item : Selectable
    {
        [SerializeField] private BaseScriptableData baseData; //for manual creation from scriptableObjects 

        public int itemCount;
        public ItemType itemType;
        public BaseObjectData baseObjectInfo;

        private void Start()
        {
            InitializeSelectable();
            InitializeBaseObjectData();
        }
        
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData("", "", null);
        }
    }
}
