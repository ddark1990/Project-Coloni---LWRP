using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class Item : Selectable, IPointerClickHandler
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                Debug.Log("Right Mouse Button Clicked on: " + name);
            }
        }
    }
}
