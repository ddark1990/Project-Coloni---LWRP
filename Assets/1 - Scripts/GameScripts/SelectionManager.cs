using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class SelectionManager : MonoBehaviour
    {
        private static SelectionManager Instance { get; set; }

        public List<Selectable> currentlySelectedObjects;
        public Selectable currentlySelectedObject;

        public LayerMask selectableMask;
        
        private Camera _cam;
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
            
            _cam = Camera.main;

        }

        private void SelectObject()
        {
            //Debug.Log("FUCK");
            
            OnDeSelectEvent(_selectable); 

            currentlySelectedObjects.Clear();
            if (currentlySelectedObjects.Count == 0) currentlySelectedObject = null;
            
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
                
            var ray = _cam.ScreenPointToRay(Input.mousePosition);

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
