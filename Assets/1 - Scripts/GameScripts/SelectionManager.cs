using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class SelectionManager : MonoBehaviour
    {
        private static SelectionManager Instance;

        public List<Selectable> currentlySelectedObjects;
        public Selectable currentlySelectedObject;

        public LayerMask selectableMask;
        
        private Camera _cam;
        private Selectable _selected;
        private RaycastHit _hit;

        private void Awake()
        {
            Instance = this;

            InitializeSelectionManager();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            SelectObject();
        }

        private void InitializeSelectionManager()
        {
            DontDestroyOnLoad(this);
            
            _cam = Camera.main;

        }

        private void SelectObject()
        {
            if (_selected != null) _selected.selected = false;
            
            currentlySelectedObjects.Clear();
            if (currentlySelectedObjects.Count == 0) currentlySelectedObject = null;
            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
                
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

            if (!Physics.Raycast(ray, out _hit, Mathf.Infinity, selectableMask)) return;
            
            _selected = _hit.collider.transform.root.gameObject.GetComponent<Selectable>();

            _selected.selected = true;
            currentlySelectedObject = _selected;
                    
            if (!currentlySelectedObjects.Contains(currentlySelectedObject))
            {
                currentlySelectedObjects.Add(_selected);
            }
        }
    }
}
