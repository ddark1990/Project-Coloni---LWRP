using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class Selectable : MonoBehaviour
    {
        public bool selected;

        private OutlineObject _outline;


        private void Awake()
        {
            _outline = GetComponentInChildren<OutlineObject>();

            _outline.enabled = false;
        }

        private void Update()
        {
            _outline.enabled = selected;
        }
    }
}