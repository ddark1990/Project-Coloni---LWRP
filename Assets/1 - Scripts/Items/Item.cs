using System;
using UnityEngine;

namespace ProjectColoni
{
    public class Item : Selectable
    {
        [Header("Item")]
        public int itemCount;
        
        [SerializeField] private BaseScriptableData baseData; 
        public ItemType itemData;
        
        public BaseObjectData baseObjectInfo;

        private void Awake()
        {
            InitializeBaseObjectData();
        }

        private void Start()
        {
            InitializeSelectable();
            
            GlobalObjectDictionary.Instance.AddToGlobalDictionary(baseObjectInfo.Id, this);
        }
        
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null);
        }
    }
}
