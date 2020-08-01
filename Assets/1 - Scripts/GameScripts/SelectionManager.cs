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

        //rewrite selection manager to use all unity events 
        public Selectable currentlySelectedObject;
        public Selectable hoveringObject;

        public LayerMask selectableMask;
        
        public Color hoverOverColor;
        public Color selectedColor;
        public float fadeSpeed = 1;
        public float outlineWidth = 2;

        public Camera cam;
        private RaycastHit _hit;
        

        private void OnEnable()
        {
            EventRelay.ObjectSelected += OnSelectEvent;
            EventRelay.ObjectDeSelected += OnDeSelectEvent;
        }
        
        private void Awake()
        {
            InitializeSelectionManager();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse0)) return;

            DeselectObject();
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
            
            cam = RTSCamera.Instance.GetComponent<Camera>();

        }

        private void DeselectObject()
        {
            if(currentlySelectedObject != null && currentlySelectedObject != hoveringObject && !EventSystem.current.IsPointerOverGameObject())
                OnDeSelectEvent();
        }
        
        #region Events

        public void OnSelectEvent()
        {
            //Let ui know what we selected
            UI_SelectionController.Instance.TogglePanelHolder(); //rewrite how this works
        }
        private void OnDeSelectEvent()
        {
            currentlySelectedObject.selected = false;
            currentlySelectedObject = null;

            UI_SelectionController.Instance.ResetWindows();
        }
        
        #endregion

        
    }
}
