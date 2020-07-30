using System;
using System.Globalization;
using Doozy.Engine.UI;
using ProjectColoni;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_SelectionController : MonoBehaviour
    {
        public static UI_SelectionController Instance { get; private set; }

        [Header("Canvases")]
        [SerializeField] private GameObject canvas;
        [Header("Panels")]
        [SerializeField] private GameObject selectedPanelHolder;
        [SerializeField] private GameObject selectedColonistPanel;
        [SerializeField] private GameObject selectedResourcePanel;
        [SerializeField] private GameObject selectedItemPanel;
        [Header("PanelCache")]
        public Image selectedImage;
        public Text selectedName;

        private SelectionManager _selectionManager;
        private UI_SkillPanel _skillPanel;

        private bool _skillWindowOpen;
        private bool _healthWindowOpen;
        private bool _inventoryWindowOpen;
    
        private void Start()
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
            
            _selectionManager = SelectionManager.Instance;

            _skillPanel = GetComponentInChildren<UI_SkillPanel>();
        }

        private void Update()
        {
            //canvas.SetActive(_selectionManager.currentlySelectedObject != null);
            
            //SwitchActiveInfoWindow();
        }

        private void LateUpdate()
        {
            if(selectedColonistPanel.activeSelf) PopulateColonistVitalsData();
            if(selectedResourcePanel.activeSelf) PopulateResourceData();
            if(selectedItemPanel.activeSelf) PopulateItemData();
        }

        public void TogglePanelHolder() 
        {
            OpenSelectedPanel();

            if (_selectionManager.currentlySelectedObject == null) return;
            /*
            var harvestablePanelRelay = selectedColonistPanel.GetComponentInChildren<UI_ColonisPanelRelay>();
            var itemPanelRelay = selectedColonistPanel.GetComponentInChildren<UI_ColonisPanelRelay>();
            */

            switch (_selectionManager.selectedType)
            {
                case AiController aiController:
                    SetActivePanel(selectedColonistPanel.name);
                    
                    PopulateBaseData(aiController);
                    break;
                case ResourceNode harvestable:
                    SetActivePanel(selectedResourcePanel.name);
                    PopulateBaseData(harvestable);
                    
                    
                    break;
                case Item item:
                    SetActivePanel(selectedItemPanel.name);

                    PopulateBaseData(item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void PopulateBaseData(Selectable selected)
        {
            var panelHolderRelay = selectedPanelHolder.GetComponentInChildren<UI_PanelHolderRelay>();
            
            switch (selected)
            {
                case AiController aiController:
                    panelHolderRelay.objectName.text = aiController.aiStats.baseObjectInfo.ObjectName;
                    panelHolderRelay.objectImage.sprite = aiController.aiStats.baseObjectInfo.Sprite;
                    
                    break;
                case ResourceNode harvestable:
                    panelHolderRelay.objectName.text = harvestable.baseObjectInfo.ObjectName;
                    panelHolderRelay.objectImage.sprite = harvestable.baseObjectInfo.Sprite;

                    break;
                case Item item:
                    panelHolderRelay.objectName.text = item.itemType.itemData.itemName;
                    panelHolderRelay.objectImage.sprite = item.itemType.itemData.itemSprite;

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void PopulateColonistVitalsData() //allocates 84 bytes
        {
            var colonistPanelRelay = selectedColonistPanel.GetComponentInChildren<UI_ColonistPanelRelay>();
            var aiController = _selectionManager.selectedType.GetComponentInChildren<AiController>();

            //bars
            colonistPanelRelay.healthBar.fillAmount = aiController.aiStats.stats.Health / aiController.aiStats.stats.MaxHealth;
            colonistPanelRelay.staminaBar.fillAmount = aiController.aiStats.stats.Stamina / aiController.aiStats.stats.MaxStamina;
            colonistPanelRelay.foodBar.fillAmount = aiController.aiStats.stats.Food / 100;
            colonistPanelRelay.energyBar.fillAmount = aiController.aiStats.stats.Energy / 100;
            colonistPanelRelay.comfortBar.fillAmount = aiController.aiStats.stats.Comfort / 100;
            colonistPanelRelay.recreationBar.fillAmount = aiController.aiStats.stats.Recreation / 100;
                    
            //text
            colonistPanelRelay.healthText.text = (aiController.aiStats.stats.Health).ToString("#");
            colonistPanelRelay.staminaText.text = (aiController.aiStats.stats.Stamina).ToString("#");
            colonistPanelRelay.foodText.text = (aiController.aiStats.stats.Food).ToString("#");
            colonistPanelRelay.energyText.text = (aiController.aiStats.stats.Energy).ToString("#");
            colonistPanelRelay.comfortText.text = (aiController.aiStats.stats.Comfort).ToString("#");
            colonistPanelRelay.recreationText.text = (aiController.aiStats.stats.Recreation).ToString("#");
            
        }

        private void PopulateResourceData()
        {
            var resourcePanelRelay = selectedResourcePanel.GetComponentInChildren<UI_ResourcePanelRelay>();
            var resource = _selectionManager.selectedType.GetComponentInChildren<ResourceNode>();

            resourcePanelRelay.amountText.text = resource.amount.ToString();
        }
        
        private void PopulateItemData()
        {
            var itemPanelRelay = selectedItemPanel.GetComponentInChildren<UI_ItemPanelRelay>();
            var item = _selectionManager.selectedType.GetComponentInChildren<Item>();

            itemPanelRelay.amountText.text = item.itemCount.ToString();
        }

        
        
        //button events
        public void ToggleSkillWindow()
        {
            ResetColonistWindows(_skillWindowOpen);
            _skillWindowOpen = !_skillWindowOpen;

            ToggleWindowView(_skillWindowOpen, "Selected", "SkillInfoPanel");
        }
        
        public void ToggleHealthWindow()
        {
            ResetColonistWindows(_healthWindowOpen);
            _healthWindowOpen = !_healthWindowOpen;
            
            ToggleWindowView(_healthWindowOpen, "Selected", "HealthInfoPanel");
        }
        
        public void ToggleInventoryWindow()
        {
            ResetColonistWindows(_inventoryWindowOpen);
            _inventoryWindowOpen = !_inventoryWindowOpen;
            
            ToggleWindowView(_inventoryWindowOpen, "Selected", "InventoryInfoPanel");
        }
        //
        
        private void OpenSelectedPanel()
        {
            if(_selectionManager.currentlySelectedObject != null) UIView.ShowView("Selected", "SelectedInfoPanel");
        }
        
        private void CloseSelectedPanel()
        {
            UIView.HideViewCategory("Selected");
        }

        private string _tempCategoryName;
        private string _tempPanelName;
        
        private void ToggleWindowView(bool active, string categoryName, string panelName)
        {
            if (_tempPanelName != string.Empty && _tempCategoryName != string.Empty)
            {
                UIView.HideView(_tempCategoryName, _tempPanelName);
            }
            
            _tempPanelName = panelName;
            _tempCategoryName = categoryName;
            
            if(active) UIView.ShowView(categoryName, panelName);
        }
        
        private void SetActivePanel(string activePanel)
        {
            selectedColonistPanel.SetActive(activePanel.Equals(selectedColonistPanel.name));
            selectedResourcePanel.SetActive(activePanel.Equals(selectedResourcePanel.name));
            selectedItemPanel.SetActive(activePanel.Equals(selectedItemPanel.name));
        }

        private void ResetColonistWindows(bool active)
        {
            if (active)
            {
                return;
            }
            
            _inventoryWindowOpen = false;
            _healthWindowOpen = false;
            _skillWindowOpen = false;
        }

        public void ResetWindows()
        {
            _inventoryWindowOpen = false;
            _healthWindowOpen = false;
            _skillWindowOpen = false;
            CloseSelectedPanel();
        }
    }
}
