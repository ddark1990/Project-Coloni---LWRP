using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class Item : SmartObject
    {
        [Header("Item")]
        public int itemCount;
        [SerializeField] private BaseScriptableData baseData; 
        
        [Header("Debug")]
        public bool ignore;
        
        public ItemType itemData;
        public BaseObjectData baseObjectInfo;

        
        private void Awake()
        {
            InitializeBaseObjectData();
        }

        private void Start()
        {
            InitializeSelectable();
            InitializeSmartActions();

            GlobalObjectDictionary.Instance.AddToGlobalDictionary(baseObjectInfo.Id, this);
        }
        
        /// <summary>
        /// Adds all the required contextual right click actions for object.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void InitializeSmartActions()
        {
            AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["PickUp"], PickUp);
 
            switch (itemData)
            {
                case Consumable consumable:
                    AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Eat"], Eat);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemData));
            }
            
            AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Drop"], Drop);
            AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["HaulAway"], HaulAway);
        }
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null);
        }
    }
}
