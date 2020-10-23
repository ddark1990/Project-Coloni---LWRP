using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.Events;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    [RequireComponent(typeof(OutlineRelay))]
    public class Selectable : MonoBehaviour
    {
        [Header("Selectable")]
        public bool selected;
        
        private OutlineRelay _outline;
        private SelectionManager _selectionManager;

        [HideInInspector] public Collider objectCollider;



        private void Start()
        {
            InitializeSelectable();
        }
        
        protected void InitializeSelectable()
        {
            
            _selectionManager = SelectionManager.Instance;
            objectCollider = GetComponent<Collider>();
            
            _outline = GetComponentInChildren<OutlineRelay>();
            _outline.OutlineWidth = _selectionManager.outlineWidth;
            _outline.OutlineColor = _selectionManager.selectedColor;
        }
        
        private void Update() //if inheriting, must update from top class
        {
            
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                OutlineHighlight();
            }

        }

        private Color _tempColor;
        
        protected void OutlineHighlight()
        {
            _outline.meshRenderer.sharedMaterials = _outline.outlineMaterials;
            _outline.OutlineColor = Color.Lerp(_outline.OutlineColor, _tempColor, _selectionManager.fadeSpeed * Time.deltaTime);

            if (selected)
            {
                _tempColor = _selectionManager.selectedColor;

                return;
            }
            
            /*if (_selectionManager.currentlySelectedObject != null && _selectionManager.hoveringObject.Equals(this) && !((AiController)_selectionManager.hoveringObject).playerOwned
                && ((AiController) _selectionManager.currentlySelectedObject).stateController.Drafted)
            {
                _tempColor = _selectionManager.attackHoverColor;

                return;
            }*/
            
            if (_selectionManager.hoveringObject != null && _selectionManager.hoveringObject.Equals(this))
            {
                _tempColor = _selectionManager.hoverOverColor;

                return;
            }

            if (_selectionManager.currentlySelectedObject == this && _selectionManager.hoveringObject == this) return;
            
            _tempColor = Color.clear;

            if (_outline.OutlineColor.a <= 0.5f)
            {
                _outline.meshRenderer.sharedMaterials = _outline.cachedMaterials;
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

        private void OnMouseOver()
        {
            
        }
    }
}