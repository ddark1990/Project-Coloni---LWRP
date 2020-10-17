using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class UpdateActiveSlotOrder : MonoBehaviour
    {
        public List<Slot> _slotsToOrder;


        private void Start()
        {
            foreach (Slot child in transform)
            {
                if (_slotsToOrder.Contains(child)) return;
                
                _slotsToOrder.Add(child);
            }
        }

        private void Update()
        {
            if (_slotsToOrder.Count == 0) return;

            for (int i = 0; i < _slotsToOrder.Count; i++)
            {
                var slot = _slotsToOrder[i];

                
            }
        }
    }
}
