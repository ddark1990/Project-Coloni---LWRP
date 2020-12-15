using System;
using UnityEditor;
using UnityEngine;

namespace ProjectColoni
{
    public class Node : SmartObject
    {
        [Header("Node")]
        public int amount;
        public NodeType nodeData;

        private void Start()
        {
            InitializeSelectable();
            //InitializeSmartActions();
            
            smartActions.InitializeSmartActions(this);
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
