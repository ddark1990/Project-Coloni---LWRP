using System;
using System.Collections;
using System.Collections.Generic;
using Doozy.Engine.UI;
using Doozy.Engine.Utils;
using Ludiq.PeekCore;
using UnityEngine;

namespace ProjectColoni
{
    public class UI_SelectionController : MonoBehaviour
    {
        //TODO: item data population inside inventory, equipment logic and its UI updates
        [Header("Panels")]
        [SerializeField] private GameObject selectedPanelHolder;
        [SerializeField] private GameObject selectedColonistPanel;
        [SerializeField] private GameObject selectedResourcePanel;
        [SerializeField] private GameObject selectedItemPanel;
        
        public static UI_ColonistPanelRelay ColonistPanelRelay;
        public static UI_ResourcePanelRelay ResourcePanelRelay;
        public static UI_ItemPanelRelay ItemPanelRelay;

        private AiController _aiController;

        [Header("Manual Cache")]
        [SerializeField] private UI_InventoryPanelController inventoryPanelController;
        
        #region Unity Functions
        
        private void OnEnable()
        {
            //update the ui of selected/deselected object
            EventRelay.OnObjectSelected += UpdateSelectionUI;
            EventRelay.OnInventoryUpdated += UpdateSelectionUI;
            
            EventRelay.OnObjectSelected += ToggleSelectedObjectUi;
            EventRelay.OnObjectDeselected += CloseSelectedPanel;

            UIView.OnUIViewAction += UIViewEvents;
            UIButton.OnUIButtonAction += UIButtonEvents;
        }
        
        private void LateUpdate()
        {
            if(selectedColonistPanel.activeInHierarchy) PopulateColonistVitalsData();
            if(selectedResourcePanel.activeInHierarchy) PopulateResourceData();
            if(selectedItemPanel.activeInHierarchy) PopulateItemData();
        }
        
        #endregion

        #region Panels
        
        private void ToggleSelectedObjectUi(Selectable selected)
        {
            OpenSelectedPanel();
            
            switch (selected)
            {
                case AiController aiController:
                    SetActivePanel(selectedColonistPanel.name);
                    
                    PopulateBaseData(aiController);

                    //get ai reference for the inventory updates
                    _aiController = aiController;
                    break;
                case Node harvestable:
                    SetActivePanel(selectedResourcePanel.name);
                    
                    PopulateBaseData(harvestable);
                    break;
                case Item item:
                    SetActivePanel(selectedItemPanel.name);

                    PopulateBaseData(item);
                    break;
                case PlaceableObject placeableObject:
                    SetActivePanel(selectedItemPanel.name);

                    PopulateBaseData(placeableObject);
                    break;
            }
        }
        
        private void OpenSelectedPanel()
        {
            if(SelectionManager.CurrentlySelectedObject != null) UIView.ShowView("Selected", "SelectedInfoPanel");
        }
        
        private void CloseSelectedPanel(Selectable deselected)
        {
            //hides the selection panel
            UIView.HideViewCategory("Selected");

            //reset any open colonist panels (inventory, skills, health, etc)
            ResetColonistActivePanelWindows();
        }
        
        private void SetActivePanel(string activePanel)
        {
            selectedColonistPanel.SetActive(activePanel.Equals(selectedColonistPanel.name));
            selectedResourcePanel.SetActive(activePanel.Equals(selectedResourcePanel.name));
            selectedItemPanel.SetActive(activePanel.Equals(selectedItemPanel.name));
        }
        #endregion
        
        #region Ui Data
        private void PopulateBaseData(Selectable selected)
        {
            var panelHolderRelay = selectedPanelHolder.GetComponentInChildren<UI_PanelHolderRelay>();
            
            panelHolderRelay.objectName.text = selected.baseObjectInfo.ObjectName;
            //panelHolderRelay.objectImage.sprite = selected.baseObjectInfo.Sprite;
            panelHolderRelay.objectImage.texture = selected.baseObjectInfo.SpriteTexture;

            //infoPanelRelay.descriptionText.text = selected.baseObjectInfo.Description;
        }
        
        
        private void PopulateColonistVitalsData() 
        {
            if (!SelectionManager.CurrentlySelectedObject) return;

            var aiController = SelectionManager.CurrentlySelectedObject.GetComponentInChildren<AiController>();

            //bars
            ColonistPanelRelay.healthBar.fillAmount = aiController.aiStats.stats.Health / 
                                                      aiController.aiStats.stats.MaxHealth;
            ColonistPanelRelay.staminaBar.fillAmount = aiController.aiStats.stats.Stamina / 
                                                       aiController.aiStats.stats.MaxStamina;
            ColonistPanelRelay.foodBar.fillAmount = aiController.aiStats.stats.Food / 100;
            ColonistPanelRelay.energyBar.fillAmount = aiController.aiStats.stats.Energy / 100;
            ColonistPanelRelay.comfortBar.fillAmount = aiController.aiStats.stats.Comfort / 100;
            ColonistPanelRelay.recreationBar.fillAmount = aiController.aiStats.stats.Recreation / 100;
                    
            //text
            ColonistPanelRelay.healthText.text = CachedIntToString[(int)aiController.aiStats.stats.Health];
            ColonistPanelRelay.staminaText.text = CachedIntToString[(int)aiController.aiStats.stats.Stamina];
            ColonistPanelRelay.foodText.text = CachedIntToString[(int)aiController.aiStats.stats.Food ];
            ColonistPanelRelay.energyText.text = CachedIntToString[(int) aiController.aiStats.stats.Energy];
            ColonistPanelRelay.comfortText.text = CachedIntToString[(int)aiController.aiStats.stats.Comfort];
            ColonistPanelRelay.recreationText.text = CachedIntToString[(int)aiController.aiStats.stats.Recreation ];
            
            //status notifications
            
            
        }
        private void PopulateResourceData()
        {
            if (!SelectionManager.CurrentlySelectedObject) return;

            var resource = SelectionManager.CurrentlySelectedObject.GetComponentInChildren<Node>();

            ResourcePanelRelay.amountText.text = CachedIntToString[resource.amount];
        }

        
        private void PopulateItemData()
        {
            if (!SelectionManager.CurrentlySelectedObject) return;
            
            var item = SelectionManager.CurrentlySelectedObject.GetComponentInChildren<Item>();

            ItemPanelRelay.amountText.text = CachedIntToString[item.ItemCount];
        }

        #endregion
        
        #region Button Events

        //on button clicks
        private void TriggerButtonEvent(string buttonName)
        {
            switch (buttonName)
            {
                //colonist panel buttons
                case "InventoryButton":
                    SwitchActivePanel("ColonistUIPanels", "InventoryPanel");

                    break;
                case "SkillsButton":
                    SwitchActivePanel("ColonistUIPanels", "SkillPanel");
                    
                    break;
                case "HealthButton":
                    SwitchActivePanel("ColonistUIPanels", "HealthPanel");
                    
                    break;
                //
                
                //
                case "InfoButton":
                    Debug.Log(buttonName);

                    break;
                case "WalkFasterButton":
                    OnToggleWalkFasterPressed();

                    break;
                case "DraftColonistButton":
                    OnDraftedColonistButtonPressed();

                    break;
                case "FightModeButton":
                    OnFightModeButtonPress();

                    break;
                //
                
            }
        }
        
        private void OnToggleWalkFasterPressed()
        {
            if (SelectionManager.CurrentlySelectedObject == null) return;
            
            SelectionManager.CurrentlySelectedObject.gameObject.GetComponent<AiController>().moveFaster =
                !SelectionManager.CurrentlySelectedObject.gameObject.GetComponent<AiController>().moveFaster;
        }
        private void OnDraftedColonistButtonPressed()
        {
            var colonist = SelectionManager.CurrentlySelectedObject as AiController;

            if (colonist == null) return;
            colonist.combatController.ToggleDraftState();

            //update button visually somehow
            var color = colonist.stateController.Drafted ? new Color(1, 1, 1, 1) : new Color(1, 1, 1, .15f);
            ColonistPanelRelay.draftButton.image.color = color;
        }
        private void OnFightModeButtonPress()
        {
            var colonist = SelectionManager.CurrentlySelectedObject as AiController;
            if (colonist == null) return;

            colonist.combatController.ToggleCombatMode();
        }
        //TODO: update UI panels with one method
        /*private void ToggleInventoryWindow()
        {
            ResetActivePanelWindows(_inventoryWindowOpen);
            
            _inventoryWindowOpen = !_inventoryWindowOpen;
            
            SwitchToWindowView(_inventoryWindowOpen, "ColonistUIPanels", "InventoryPanel");
        }
        private void ToggleSkillWindow()
        {
            ResetActivePanelWindows(_skillWindowOpen);
            
            _skillWindowOpen = !_skillWindowOpen;
            
            SwitchToWindowView(_skillWindowOpen, "ColonistUIPanels", "SkillPanel");
        }*/
        
        private void ResetColonistActivePanelWindows()
        {
            UIView.HideViewCategory("ColonistUIPanels");
            
            inventoryPanelController.ClearInventoryUISlots();
        }
        
        private string _previousCategoryName;
        private string _previousPanelName;
        private void SwitchActivePanel(string categoryName, string panelName) 
        {
            if (UIView.IsViewVisible(categoryName, panelName))
                UIView.HideView(categoryName, panelName);
            else
            {
                //hides previously opened view if opened
                if (UIView.IsViewVisible(_previousCategoryName, _previousPanelName))
                    UIView.HideView(_previousCategoryName, _previousPanelName);
                
                UIView.ShowView(categoryName, panelName);
                
                _previousCategoryName = categoryName;
                _previousPanelName = panelName;
            }
        }
        #endregion
        
        #region Events
        
        private void UIButtonEvents(UIButton button, UIButtonBehaviorType behaviourType)
        {
            switch (behaviourType)
            {
                case UIButtonBehaviorType.OnClick:
                    TriggerButtonEvent(button.ButtonName);
                    break;
                case UIButtonBehaviorType.OnDoubleClick:
                    break;
                case UIButtonBehaviorType.OnLongClick:
                    break;
                case UIButtonBehaviorType.OnPointerEnter:
                    break;
                case UIButtonBehaviorType.OnPointerExit:
                    break;
                case UIButtonBehaviorType.OnPointerDown:
                    break;
                case UIButtonBehaviorType.OnPointerUp:
                    break;
                case UIButtonBehaviorType.OnSelected:
                    break;
                case UIButtonBehaviorType.OnDeselected:
                    break;
                case UIButtonBehaviorType.OnRightClick:
                    break;
            }
        }
        
        //updates the ui when a selection happens from EventRelay.OnObjectSelected
        private void UpdateSelectionUI(Selectable selectedObject)
        {
            switch (selectedObject)
            {
                case AiController aiController:
                    inventoryPanelController.PopulateInventoryData(aiController);
                    break;
                case Item item:
                    break;
                case Node node:
                    break;
            }
        }
        
        //might not need
        private void UIViewEvents(UIView view, UIViewBehaviorType behaviourType)
        {
            switch (behaviourType)
            {
                case UIViewBehaviorType.Unknown:
                    break;
                case UIViewBehaviorType.Show:
                    break;
                case UIViewBehaviorType.Hide:
                    break;
                case UIViewBehaviorType.Loop:
                    break;
            }
        }
        #endregion
        
        #region Statics
        
        public static readonly CacheIntString CachedIntToString = new CacheIntString(
            (values)=>values , //describe how seconds (key) are translated to useful value (hash)
            (value)=>value.ToString("0") //you describe how string is built based on value (hash)
            , 0 , 150 , 1 //initialization range and step, so cache will be warmed up and ready
        );
        
        #endregion
    }
}
