using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class Selectable : MonoBehaviour
    {
        public bool selected;
        
        private OutlineObject _outline;
        private SelectionManager _selectionManager;


        private void Start()
        {
            _selectionManager = SelectionManager.Instance;
            _outline = GetComponentInChildren<OutlineObject>();
            
            _outline.OutlineWidth = _selectionManager.outlineWidth;
        }

        private void Update()
        {
            OutlineHighlight();
        }

        private void OutlineHighlight()
        {
            if (selected)
            {
                _outline.OutlineColor = Color.Lerp(_outline.OutlineColor, _selectionManager.selectedColor, _selectionManager.fadeSpeed * Time.deltaTime);
                return;
            }
            
            if (_selectionManager.hoveringObject != null && _selectionManager.hoveringObject.Equals(this))
            {
                _outline.OutlineColor = Color.Lerp(_outline.OutlineColor, _selectionManager.hoverOverColor, _selectionManager.fadeSpeed * Time.deltaTime);
            }
            else
            {
                _outline.OutlineColor = Color.Lerp(_outline.OutlineColor, Color.clear, _selectionManager.fadeSpeed * Time.deltaTime);
            }
        }

        private void OnMouseEnter()
        {
            _selectionManager.hoveringObject = this;
        }

        private void OnMouseExit()
        {
            if(_selectionManager.hoveringObject.Equals(this))
                _selectionManager.hoveringObject = null;
        }
    }
}