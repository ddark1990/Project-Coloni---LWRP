using System;
using System.Globalization;
using System.Text;
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
        private bool _infoWindowOpen;
    
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

        private void LateUpdate()
        {
            if(selectedColonistPanel.activeInHierarchy) PopulateColonistVitalsData();
            if(selectedResourcePanel.activeInHierarchy) PopulateResourceData();
            if(selectedItemPanel.activeInHierarchy) PopulateItemData();
        }

        public void TogglePanelHolder() 
        {
            OpenSelectedPanel();

            if (_selectionManager.currentlySelectedObject == null) return;

            switch (_selectionManager.currentlySelectedObject)
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

        [SerializeField] private UI_InfoPanelRelay infoPanelRelay;
        
        private void PopulateBaseData(Selectable selected)
        {
            var panelHolderRelay = selectedPanelHolder.GetComponentInChildren<UI_PanelHolderRelay>();
            
            switch (selected)
            {
                case AiController aiController:
                    panelHolderRelay.objectName.text = aiController.aiStats.baseObjectInfo.ObjectName;
                    panelHolderRelay.objectImage.sprite = aiController.aiStats.baseObjectInfo.Sprite;

                    infoPanelRelay.descriptionText.text = aiController.aiStats.baseObjectInfo.Description;
                    
                    break;
                case ResourceNode harvestable:
                    panelHolderRelay.objectName.text = harvestable.baseObjectInfo.ObjectName;
                    panelHolderRelay.objectImage.sprite = harvestable.baseObjectInfo.Sprite;

                    infoPanelRelay.descriptionText.text = harvestable.baseObjectInfo.Description;
                        
                    break;
                case Item item:
                    panelHolderRelay.objectName.text = item.baseObjectInfo.ObjectName;
                    panelHolderRelay.objectImage.sprite = item.baseObjectInfo.Sprite;

                    infoPanelRelay.descriptionText.text = item.baseObjectInfo.Description;
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HideInInspector] public UI_ColonistPanelRelay colonistPanelRelay;
        
        private void PopulateColonistVitalsData() 
        {
            var aiController = _selectionManager.selectedTemp.GetComponentInChildren<AiController>();

            //bars
            colonistPanelRelay.healthBar.fillAmount = aiController.aiStats.stats.Health / aiController.aiStats.stats.MaxHealth;
            colonistPanelRelay.staminaBar.fillAmount = aiController.aiStats.stats.Stamina / aiController.aiStats.stats.MaxStamina;
            colonistPanelRelay.foodBar.fillAmount = aiController.aiStats.stats.Food / 100;
            colonistPanelRelay.energyBar.fillAmount = aiController.aiStats.stats.Energy / 100;
            colonistPanelRelay.comfortBar.fillAmount = aiController.aiStats.stats.Comfort / 100;
            colonistPanelRelay.recreationBar.fillAmount = aiController.aiStats.stats.Recreation / 100;
                    
            //text
            colonistPanelRelay.healthText.text = _cachedIntToString[(int)aiController.aiStats.stats.Health];
            colonistPanelRelay.staminaText.text = _cachedIntToString[(int)aiController.aiStats.stats.Stamina];
            colonistPanelRelay.foodText.text = _cachedIntToString[(int)aiController.aiStats.stats.Food ];
            colonistPanelRelay.energyText.text = _cachedIntToString[(int) aiController.aiStats.stats.Energy];
            colonistPanelRelay.comfortText.text = _cachedIntToString[(int)aiController.aiStats.stats.Comfort];
            colonistPanelRelay.recreationText.text = _cachedIntToString[(int)aiController.aiStats.stats.Recreation ];
            
            //status notifications
            
            
        }

        [HideInInspector] public UI_ResourcePanelRelay resourcePanelRelay;
        
        private void PopulateResourceData()
        {
            var resource = _selectionManager.selectedTemp.GetComponentInChildren<ResourceNode>();

            resourcePanelRelay.amountText.text = _cachedIntToString[resource.amount];
        }

        [HideInInspector] public UI_ItemPanelRelay itemPanelRelay;
        
        private void PopulateItemData()
        {
            var item = _selectionManager.selectedTemp.GetComponentInChildren<Item>();

            itemPanelRelay.amountText.text = _cachedIntToString[item.itemCount];
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
        
        public void ToggleInfoWindow()
        {
            ResetColonistWindows(_infoWindowOpen);
            _infoWindowOpen = !_infoWindowOpen;
            
            ToggleWindowView(_infoWindowOpen, "General", "InfoPanel");
        }
        //
        
        private void OpenSelectedPanel()
        {
            if(_selectionManager.currentlySelectedObject != null) UIView.ShowView("Selected", "SelectedInfoPanel");
        }

        private string _tempCategoryName;
        private string _tempPanelName;
        
        private void ToggleWindowView(bool active, string categoryName, string panelName) //toggles views while caching old view and category names
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
            _infoWindowOpen = false;
        }

        public void ResetWindows()
        {
            _inventoryWindowOpen = false;
            _healthWindowOpen = false;
            _skillWindowOpen = false;
            _infoWindowOpen = false;
            
            CloseSelectedPanel();
        }
        
        private void CloseSelectedPanel()
        {
            if(_selectionManager.hoveringObject == null) UIView.HideViewCategory("General");

            UIView.HideViewCategory("Selected");
        }
        
        private readonly CacheIntString _cachedIntToString = new CacheIntString(
            (values)=>values , //describe how seconds (key) are translated to useful value (hash)
            (value)=>value.ToString("0") //you describe how string is built based on value (hash)
            , 0 , 150 , 1 //initialization range and step, so cache will be warmed up and ready
        );
    }
}
