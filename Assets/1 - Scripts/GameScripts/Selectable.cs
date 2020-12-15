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
        
        [HideInInspector] public OutlineRelay outline;
        private SelectionManager _selectionManager;

        [HideInInspector] public Collider objectCollider;
        
        [Header("BaseData")]
        [Tooltip("If null, will generate random values for the base data.")] 
        [SerializeField] private BaseScriptableData baseData;
        public BaseObjectData baseObjectInfo;

        
        private void Start()
        {
            InitializeSelectable();
        }

        protected void InitializeSelectable()
        {
            _selectionManager = SelectionManager.Instance;
            objectCollider = GetComponent<Collider>();
            
            outline = GetComponentInChildren<OutlineRelay>();
            outline.OutlineWidth = _selectionManager.outlineWidth;
            outline.OutlineColor = _selectionManager.hoverOverColor;
            
            InitializeBaseObjectData();
        }
        
        /// <summary>
        /// Initialize base data from a scriptable object if one is available .
        /// </summary>
        private void InitializeBaseObjectData()
        {
            baseObjectInfo = baseData != null ? new BaseObjectData(StaticUtility.GenerateUniqueHashId(), baseData.objectName, baseData.description, baseData.spriteTexture) 
                : new BaseObjectData(StaticUtility.GenerateUniqueHashId(), "", "", null); //should generate random base stats, maybe
        }
        private void Update() //if inheriting, must update from top class
        {
            OutlineHighlight();
        }

        private Color _tempColor;
        
        protected void OutlineHighlight()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            outline.meshRenderer.sharedMaterials = outline.outlineMaterials;
            outline.OutlineColor = Color.Lerp(outline.OutlineColor, _tempColor, _selectionManager.fadeSpeed * Time.deltaTime);

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
            
            if (SelectionManager.HoveringObject != null && SelectionManager.HoveringObject.Equals(this))
            {
                _tempColor = _selectionManager.hoverOverColor;

                return;
            }

            if (SelectionManager.CurrentlySelectedObject == this && SelectionManager.HoveringObject == this) return;
            
            _tempColor = Color.clear;

            if (outline.OutlineColor.a <= 0.5f)
            {
                outline.meshRenderer.sharedMaterials = outline.cachedMaterials;
            }
        }

        private void OnMouseEnter()
        {
            SelectionManager.HoveringObject = this;
        }

        private void OnMouseDown()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            EventRelay.OnObjectSelected(this);
            //SelectionManager.SelectObject(this);
        }

        private void OnMouseExit()
        {
            if(SelectionManager.HoveringObject.Equals(this))
                SelectionManager.HoveringObject = null;
        }
    }
}