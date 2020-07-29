using System;
using UnityEngine;

namespace ProjectColoni
{
    public class Harvestable : Selectable
    {
        public BaseScriptableData data;
        public int amount;

        
        private void Start()
        {
            InitializeSelectable();
        }
    }
}
