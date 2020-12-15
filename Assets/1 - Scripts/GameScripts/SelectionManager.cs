using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectColoni
{
    public class SelectionManager : MonoBehaviour
    {
        public static SelectionManager Instance { get; private set; }

        public static Selectable CurrentlySelectedObject { get; private set; }
        public static List<Selectable> CurrentlySelectedObjects { get; private set; }
        public static Selectable HoveringObject; //maybe fix
        
        [Header("Selectables")]
        
        public LayerMask selectableMask;
        
        public Color attackHoverColor; //when in draft mode the selection manager hover color should change to red instead of switching directly
        public Color hoverOverColor;
        public Color selectedColor;
        public float fadeSpeed = 1;
        public float outlineWidth = 2;

        public Camera cam;
        //private RaycastHit _hit;
        public static Selectable LastSelectedObject;

        [Header("Outline Materials")] 
        public Material outlineMaskMaterial;
        public Material outlineMaterial;

        private void OnEnable()
        {
            EventRelay.OnObjectSelected += SelectObject;
        }

        private void Start()
        {
            InitializeSelectionManager();
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject() || CurrentlySelectedObject == null) return;

            if (!Input.GetKey(KeyCode.LeftControl) && Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Escape))
            {
                DeselectOnKeyPress();
            }
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
        private void DebugSelectedRightClick(Selectable selectedObject, RaycastHit hit)
        {
            if(selectedObject.selected)
                Debug.Log("Right clicked on " + hit.collider.name + " while " + selectedObject.name + " is selected.");

        }

        /// <summary>
        /// Activated by an event which is called from the Selectable.cs objects. 
        /// </summary>
        /// <param name="selectingObject"></param>
        private void SelectObject(Selectable selectingObject)
        {
            //deselect CurrentlySelectedObject if selecting the same object again
            //trigger deselect event
            if (CurrentlySelectedObject != null && CurrentlySelectedObject.Equals(selectingObject))
            {
                DeselectObject();                
                return;
            }
            
            //multi selection
            if (Input.GetKey(KeyCode.LeftControl))
            {
                return;
            }
            
            //deselect previously selected object if the new one selectingObject != CurrentlySelectedObject
            //trigger deselect event
            if (CurrentlySelectedObject != null && CurrentlySelectedObject != selectingObject)
            {
                //reset object selection & invoke deselection event
                DeselectObject();                
            }
            
            //set newly selected object cache
            CurrentlySelectedObject = selectingObject;
            CurrentlySelectedObject.selected = true;

            //UpdateSelectionUI(CurrentlySelectedObject);
        }
        
        /// <summary>
        /// Deselect objects when clicking on anything but another selectable. Outputs OnObjectDeselected(obj) event
        /// which you can subscribe to.
        /// </summary>
        private void DeselectOnKeyPress()
        {
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, selectableMask)) return;
            
            if (HoveringObject != null) return;
                
            DeselectObject();
        }

        //deselects CurrentlySelectedObject and invokes an event 
        private void DeselectObject()
        {
            Debug.Log("Deselecting | " + CurrentlySelectedObject);
            
            EventRelay.OnObjectDeselected?.Invoke(CurrentlySelectedObject);
                
            CurrentlySelectedObject.selected = false;
            CurrentlySelectedObject = null;
        }
        
        
        
        public bool SelectionAndHoverClear()
        {
            return CurrentlySelectedObject == null && HoveringObject == null;
        }
    }
}
