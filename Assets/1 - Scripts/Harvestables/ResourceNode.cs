using System;
using UnityEngine;

namespace ProjectColoni
{
    public class ResourceNode : Selectable
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
            InitializeRightClickActions();
        }

        /// <summary>
        /// Initialize base data from a scriptable object if one is available .
        /// </summary>
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.sprite) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null);
        }

        private void InitializeRightClickActions()
        {
            AddActionToCollection("Gather", Gather);
            AddActionToCollection("Inspect", Inspect);
        }
        
        private void Gather()
        {
            Debug.Log("Gathering " + this);
        }

        private void Inspect()
        {
            Debug.Log("Inspecting " + this);
        }
    }
}
