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

        protected void OutlineHighlight()
        {
            if (selected)
            {
                _outline.meshRenderer.sharedMaterials = _outline.outlineMaterials;
                _outline.OutlineColor = Color.Lerp(_outline.OutlineColor, _selectionManager.selectedColor, _selectionManager.fadeSpeed * Time.deltaTime);
                return;
            }
            
            if (_selectionManager.hoveringObject != null && _selectionManager.hoveringObject.Equals(this))
            {
                _outline.meshRenderer.sharedMaterials = _outline.outlineMaterials;
                _outline.OutlineColor = Color.Lerp(_outline.OutlineColor, _selectionManager.hoverOverColor, _selectionManager.fadeSpeed * Time.deltaTime);
                return;
            }
            
            _outline.OutlineColor = Color.Lerp(_outline.OutlineColor, Color.clear, _selectionManager.fadeSpeed * Time.deltaTime);

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
    }
}