using System;
using UnityEngine;

namespace ProjectColoni
{
    public class ResourceNode : SmartObject
    {
        public int amount;
        
        [SerializeField] private BaseScriptableData baseData; //for manual creation from scriptableObjects 
        public BaseObjectData baseObjectInfo;
        
        
        private void Awake()
        {
            InitializeBaseObjectData();
        }

        private void Start()
        {
            InitializeSelectable();
            InitializeSmartActions();
        }

        /// <summary>
        /// Initialize base data from a scriptable object if one is available .
        /// </summary>
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null);
        }

        private void InitializeSmartActions()
        {
            AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Inspect"], Inspect);
            AddActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Gather"], Gather);
        }
        
        //smart actions
        private void Gather()
        {
            Debug.Log("Gathering " + this);
        }

        private void Inspect()
        {
            var aiController = SelectionManager.Instance.currentlySelectedObject as AiController;
            aiController.performingForcedAction = true;
            
            aiController.MoveAgent(objectCollider.ClosestPointOnBounds(aiController.transform.position));

            Debug.Log("Inspecting " + this);
        }
    }
}
