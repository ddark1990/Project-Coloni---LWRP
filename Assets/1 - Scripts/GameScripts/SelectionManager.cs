using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance { get; private set; }

        public List<Selectable> currentlySelectedObjects;
        public Selectable currentlySelectedObject;
        public Selectable hoveringObject;

        public LayerMask selectableMask;
        
        public Color hoverOverColor;
        public Color selectedColor;
        public float fadeSpeed = 1;
        public float outlineWidth = 2;

        public Camera cam;
        private Selectable _selectable;
        private RaycastHit _hit;

        public delegate void ObjectSelected();


        private void OnEnable()
        {
            EventRelay.ObjectSelected += OnSelectEvent;
            EventRelay.ObjectDeSelected += OnDeSelectEvent;
        }

        private void OnDisable()
        {
            //EventRelay.ObjectSelected -= OnSelectEvent;
            //EventRelay.ObjectDeSelected -= OnDeSelectEvent;
        }

        private void Awake()
        {
            InitializeSelectionManager();
        }

        private void Update()
        {
            //hoveringObject = _hit.collider.transform.root.GetComponent<Selectable>();
            
            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            SelectObject();
        }

        private void InitializeSelectionManager()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(this);
            
            cam = Camera.main;

        }

        private void SelectObject()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            OnDeSelectEvent(_selectable); 

            currentlySelectedObjects.Clear();
            if (currentlySelectedObjects.Count == 0)
            {
                currentlySelectedObject = null;
            }
            
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if (!Physics.Raycast(ray, out _hit, Mathf.Infinity, selectableMask)) return;

            //Debug.Log(_hit.collider.name);

            _selectable = _hit.collider.transform.root.GetComponent<Selectable>();
                
            OnSelectEvent(_selectable);
        }
        
        #region Events

        private void OnSelectEvent(Selectable selectable)
        {
            selectable.selected = true;
            currentlySelectedObject = selectable;
                    
            if (!currentlySelectedObjects.Contains(currentlySelectedObject))
            {
                currentlySelectedObjects.Add(selectable);
            }
        }
        private void OnDeSelectEvent(Selectable selectable)
        {
            if(selectable != null) selectable.selected = false;
        }
        
        #endregion

        
    }
}
