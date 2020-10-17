using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class Item : SmartObject
    {
        [Header("Item")] 
        public bool equipped;
        public int itemCount;
        [SerializeField] private BaseScriptableData baseData; 
        
        [Header("Debug")]
        public bool ignore;
        
        public ItemType itemTypeData;
        public BaseObjectData baseObjectInfo;

        
        private void Awake()
        {
            InitializeBaseObjectData();
        }

        private void Start()
        {
            InitializeSelectable();

            GlobalObjectDictionary.Instance.AddToGlobalDictionary(baseObjectInfo.Id, this);
            
            smartActions.InitializeSmartActions(this);
        }
        
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.spriteTexture) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null);
        }
    }
}
