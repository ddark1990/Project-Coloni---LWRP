using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class Selectable : MonoBehaviour
    {
        [Header("Selectable")]
        public bool selected;
        
        private OutlineObject _outline;
        private SelectionManager _selectionManager;

        [HideInInspector] public Collider objectCollider;

        public Dictionary<Sprite, UnityAction<AiController>> rightClickActions; //dont rly need name cuz can get name from the actual action method
        

        protected void InitializeSelectable()
        {
            rightClickActions = new Dictionary<Sprite, UnityAction<AiController>>();
            
            _selectionManager = SelectionManager.Instance;
            _outline = GetComponentInChildren<OutlineObject>();
            objectCollider = GetComponent<Collider>();
            
            _outline.OutlineWidth = _selectionManager.outlineWidth;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            OutlineHighlight(); //if inheriting, must update from top class
        }

        protected void OutlineHighlight()
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

        private void OnMouseDown()
        {
            _selectionManager.SelectObject(this);
        }

        private void OnMouseExit()
        {
            if(_selectionManager.hoveringObject.Equals(this))
                _selectionManager.hoveringObject = null;
        }

        protected void AddActionToCollection(Sprite actionSprite, UnityAction<AiController> action)
        {
            rightClickActions.Add(actionSprite, action);
        }
    }
}