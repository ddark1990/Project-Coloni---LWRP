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

        public Selectable currentlySelectedObject;
        public Selectable hoveringObject;

        public LayerMask selectableMask;
        
        public Color hoverOverColor;
        public Color selectedColor;
        public float fadeSpeed = 1;
        public float outlineWidth = 2;

        public Camera cam;
        private RaycastHit _hit;
        [HideInInspector] public Selectable selectedTemp;
        
        
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

        public void SelectObject(Selectable selectedObject)
        {
            if (selectedTemp != null)
            {
                selectedTemp.selected = false;
                
                //ui
                UI_SelectionController.Instance.ResetWindows();
            }
            
            currentlySelectedObject = selectedObject;
            currentlySelectedObject.selected = true;
            
            //Let ui know what we selected
            UI_SelectionController.Instance.TogglePanelHolder(); //rewrite how this works
        }
        
        private void DeselectObject()
        {
            if (EventSystem.current.IsPointerOverGameObject()) return;
            
            if (currentlySelectedObject != null)
            {
                selectedTemp = currentlySelectedObject;

                if (hoveringObject != null) return;
                
                currentlySelectedObject.selected = false;
                currentlySelectedObject = null;
                
                //ui
                UI_SelectionController.Instance.ResetWindows();
            }
        }
    }
}
