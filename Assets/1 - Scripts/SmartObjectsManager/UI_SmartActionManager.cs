using System;
using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_SmartActionManager : MonoBehaviour
    {
        [SerializeField] private UIView rightClickPanel;

        private Transform[] _actionButtons;
        private SelectionManager _selectionManager;
        
        public float _panelOffset = .91f;


        private void OnEnable()
        {
            rightClickPanel.HideBehavior.OnFinished.Action += OnPanelHidden;
        }

        private void OnDisable()
        {
            //rightClickPanel.HideBehavior.OnFinished.Action -= OnPanelHidden;
        }

        private void Start()
        {
            _selectionManager = SelectionManager.Instance;

            GetActionButtons();
        }

        private void Update()
        {
            PanelDistanceCheck();

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                RightClickSelection();
            }
            
            if(!EventSystem.current.IsPointerOverGameObject() && Input.GetKey(KeyCode.Mouse0)) ClearRightClickButtons();

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

            if (_selectionManager.currentlySelectedObject == null) return;
                
            switch (_selectionManager.currentlySelectedObject)
            {
                case AiController aiController: //if colonist is selected, colonist related actions for that object is presented

                    if (aiController.stateController.Drafted) return;

                    //open & populate right click panel
                    ToggleRightClickPanel(aiController, _selectionManager.hoveringObject as SmartObject);
                    
                    break;
                case Item item:
                    break;
                case Node resourceNode:
                    break;
            }
        }
        
        private void ToggleRightClickPanel(AiController aiController, SmartObject smartObject)
        {
            ClearRightClickButtons();
            SetPanelPosToMouse(_panelOffset);
            
            rightClickPanel.Show();
            //rightClickPanel.gameObject.SetActive(true);
            
            PopulateActionButtonsData(aiController, smartObject);
        }
        
        private void PopulateActionButtonsData(AiController aiController, SmartObject smartObject)
        {
            var collectionOfActions = _selectionManager.hoveringObject.GetComponentInChildren<SmartObject>().smartActionDictionary;
            
            var collectionIndex = 0;
            foreach (var action in collectionOfActions)
            {
                var button = _actionButtons[collectionIndex].GetComponent<UI_SmartActionButton>();
                button.gameObject.SetActive(true);

                foreach (var sprites in GameManager.Instance.globalSpriteContainer.spriteCollection)
                {
                    if (sprites.Value == action.Key)
                        button.actionName.text = sprites.Key; //fix
                }
                
                button.actionButton.onClick.AddListener(delegate { action.Value(aiController, smartObject); });
                button.actionImage.sprite = action.Key;
                
                collectionIndex++;
            }
        }
        
        private void ClearRightClickButtons() //hides panel and uses its on finished action to disable and reset buttons
        {
            if (rightClickPanel.Visibility == VisibilityState.Hiding) return;
            
            rightClickPanel.Hide();
        }

        private void OnPanelHidden(GameObject go) //gives panel as gameObject
        {
            foreach (var button in _actionButtons)
            {
                var uiButton = button.GetComponent<Button>();
                
                button.gameObject.SetActive(false);
                uiButton.onClick.RemoveAllListeners();
            }
        }

        private void SetPanelPosToMouse(float offset)
        {
            rightClickPanel.transform.position = Input.mousePosition * offset;
        }
        
        private void PanelDistanceCheck()
        {
            if (!rightClickPanel.gameObject.activeSelf) return;
            
            var distBetween = Vector3.Distance(rightClickPanel.transform.position, Input.mousePosition);
            
            if (distBetween > 200)
            {
                rightClickPanel.Hide();
                //rightClickPanel.gameObject.SetActive(false);
            }
        }
    }
}
