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

        private void InitializeSmartActions()
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
            InitializeSmartActions();

            GlobalObjectDictionary.Instance.AddToGlobalDictionary(baseObjectInfo.Id, this);
        }
        
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null);
        }

        //smart actions
        private void PickUp()
        {
            var aiController = SelectionManager.Instance.currentlySelectedObject as AiController;
            aiController.performingForcedAction = true;
            
            aiController.MoveAgent(objectCollider.ClosestPointOnBounds(aiController.transform.position));

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
