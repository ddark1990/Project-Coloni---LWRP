﻿using System;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_RightClickManager : MonoBehaviour
    {
        public static UI_RightClickManager Instance { get; private set; }

        [SerializeField] private UIView rightClickPanel;

        private Transform[] _actionButtons;
        private SelectionManager _selectionManager;
        
        public float _panelOffset = .91f;


        private void Start()
        {
            _selectionManager = SelectionManager.Instance;

            GetActionButtons();

            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            SetPanelPosToMouse();
            PanelDistanceCheck();
            
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                RightClickSelection();
            }
        }

        private void GetActionButtons()
        {
            _actionButtons = new Transform[rightClickPanel.transform.childCount];
            
            for (int i = 0; i < rightClickPanel.transform.childCount; i++)
            {
                var child = rightClickPanel.transform.GetChild(i);

                _actionButtons[i] = child;
                _actionButtons[i].gameObject.SetActive(false);
            }
        }
        
        private void RightClickSelection() //right click actions based on what is selected, most likely only the ai would need this but doesnt hurt for all selectables to be able to receive 
        {
            var ray = _selectionManager.cam.ScreenPointToRay (Input.mousePosition);

            if (!Physics.Raycast(ray, out var hit, 200, _selectionManager.selectableMask)) return;
            
            //Debug.Log("Right Clicked On: " + _hit.collider);
            //DebugSelectedRightClick(currentlySelectedObject, hit);

            if (!_selectionManager.currentlySelectedObject.selected) return;
                
            switch (_selectionManager.currentlySelectedObject)
            {
                case AiController aiController: //if colonist is selected, colonist related actions for that object is presented

                    //open & populate right click panel
                    ToggleRightClickPanel();
                    
                    break;
                case Item item:
                    break;
                case ResourceNode resourceNode:
                    break;
            }
        }
        
        private void ToggleRightClickPanel()
        {
            ClearRightClickButtons();
            
            rightClickPanel.Show();
            //rightClickPanel.gameObject.SetActive(true);
            
            PopulateActionButtonsData();
        }

        private void PopulateActionButtonsData()
        {
            var collectionOfActions = _selectionManager.hoveringObject.GetComponent<Selectable>().rightClickActions;

            var collectionIndex = 0;

            foreach (var action in collectionOfActions)
            {
                collectionIndex++;
                
                var button = _actionButtons[collectionIndex].GetComponent<UI_RightClickActionButton>();
                button.gameObject.SetActive(true);

                button.actionName.text = action.Value.Method.Name;
                button.actionButton.onClick.AddListener(action.Value);
                button.actionImage.sprite = action.Key;
            }
        }
        
        private void ClearRightClickButtons()
        {
            foreach (var button in _actionButtons)
            {
                var uiButton = button.GetComponent<Button>();
                
                button.gameObject.SetActive(false);
                uiButton.onClick.RemoveAllListeners();
            }
        }

        private void SetPanelPosToMouse()
        {
            if (!rightClickPanel.gameObject.activeSelf)
            {
                rightClickPanel.transform.position = Input.mousePosition * _panelOffset;
            }
        }
        
        private void PanelDistanceCheck()
        {
            var distBetween = Vector3.Distance(rightClickPanel.transform.position, Input.mousePosition);
            
            if (distBetween > 200)
            {
                rightClickPanel.Hide();
                //rightClickPanel.gameObject.SetActive(false);
            }
        }
    }
}
