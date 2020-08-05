using System;
using UnityEngine;
using UnityEngine.EventSystems;

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

        private void InitializeRightClickActions()
        {
            AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["PickUp"], PickUp);
 
            switch (itemData)
            {
                case Consumable consumable:
                    AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Eat"], consumable.Eat);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(itemData));
            }
            
            AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Drop"], Drop);
            AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["HaulAway"], HaulAway);
        }
        
        private void Start()
        {
            InitializeSelectable();
            InitializeRightClickActions();

            GlobalObjectDictionary.Instance.AddToGlobalDictionary(baseObjectInfo.Id, this);
        }
        
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null);
        }

        private void PickUp()
        {
            Debug.Log("Picking Up " + this);
        }
        private void Drop()
        {
            Debug.Log("Dropping " + this);
        }
        private void HaulAway()
        {
            Debug.Log("Hauling Away " + this);
        }
    }
}
