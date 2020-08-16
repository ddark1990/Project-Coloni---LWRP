using System;
using UnityEngine;

namespace ProjectColoni
{
    public class Node : SmartObject
    {
        [Header("Node")]
        public int amount;
        [SerializeField] private BaseScriptableData baseData; //for manual creation from scriptableObjects 
        public NodeType nodeData;
        
        public BaseObjectData baseObjectInfo;

        
        private void Awake()
        {
            InitializeBaseObjectData();
        }

        private void Start()
        {
            InitializeSelectable();
            //InitializeSmartActions();
            
            smartActions.InitializeSmartActions(this);
        }

        /// <summary>
        /// Initialize base data from a scriptable object if one is available .
        /// </summary>
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null);
        }

        /*/// <summary>
        /// Adds all the required contextual right click actions for object.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private void InitializeSmartActions()
        {
            AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Inspect"], Inspect);
            AddSmartActionToCollection(GameManager.Instance.globalSpriteContainer.spriteCollection["Gather"], Gather);
        }*/
    }
}
