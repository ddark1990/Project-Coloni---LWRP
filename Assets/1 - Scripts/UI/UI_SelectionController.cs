using System;
using System.Globalization;
using System.Text;
using Doozy.Engine.UI;
using ProjectColoni;
using UnityEngine;
using UnityEngine.EventSystems;
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
        public RawImage selectedImage;
        public Text selectedName;
        [Header("Cache")] 
        public UI_InventoryPanelController inventoryPanelController;

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

        private void LateUpdate() //refactor into separate controllers
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

        [SerializeField] private UI_InfoPanelRelay infoPanelRelay;
        
        private void PopulateBaseData(Selectable selected)
        {
            var panelHolderRelay = selectedPanelHolder.GetComponentInChildren<UI_PanelHolderRelay>();
            
            switch (selected)
            {
                case AiController aiController:
                    panelHolderRelay.objectName.text = aiController.aiStats.baseObjectInfo.ObjectName;
                    //panelHolderRelay.objectImage.sprite = aiController.aiStats.baseObjectInfo.Sprite;
                    panelHolderRelay.objectImage.texture = aiController.aiStats.baseObjectInfo.SpriteTexture;

                    infoPanelRelay.descriptionText.text = aiController.aiStats.baseObjectInfo.Description;
                    
                    break;
                case Node harvestable:
                    panelHolderRelay.objectName.text = harvestable.baseObjectInfo.ObjectName;
                    //panelHolderRelay.objectImage.sprite = harvestable.baseObjectInfo.Sprite;
                    panelHolderRelay.objectImage.texture = harvestable.baseObjectInfo.SpriteTexture;

                    infoPanelRelay.descriptionText.text = harvestable.baseObjectInfo.Description;
                        
                    break;
                case Item item:
                    panelHolderRelay.objectName.text = item.baseObjectInfo.ObjectName;
                    //panelHolderRelay.objectImage.sprite = item.baseObjectInfo.Sprite;
                    panelHolderRelay.objectImage.texture = item.baseObjectInfo.SpriteTexture;

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
            colonistPanelRelay.healthText.text = CachedIntToString[(int)aiController.aiStats.stats.Health];
            colonistPanelRelay.staminaText.text = CachedIntToString[(int)aiController.aiStats.stats.Stamina];
            colonistPanelRelay.foodText.text = CachedIntToString[(int)aiController.aiStats.stats.Food ];
            colonistPanelRelay.energyText.text = CachedIntToString[(int) aiController.aiStats.stats.Energy];
            colonistPanelRelay.comfortText.text = CachedIntToString[(int)aiController.aiStats.stats.Comfort];
            colonistPanelRelay.recreationText.text = CachedIntToString[(int)aiController.aiStats.stats.Recreation ];
            
            //status notifications
            
            
        }

        [HideInInspector] public UI_ResourcePanelRelay resourcePanelRelay;
        
        private void PopulateResourceData()
        {
            var resource = _selectionManager.selectedTemp.GetComponentInChildren<Node>();

            resourcePanelRelay.amountText.text = CachedIntToString[resource.amount];
        }

        [HideInInspector] public UI_ItemPanelRelay itemPanelRelay;
        
        private void PopulateItemData()
        {
            var item = _selectionManager.selectedTemp.GetComponentInChildren<Item>();

            itemPanelRelay.amountText.text = CachedIntToString[item.itemCount];
        }
       
        //button events
        public void ToggleSkillWindow()
        {
            ResetColonistWindows(_skillWindowOpen);
            _skillWindowOpen = !_skillWindowOpen;

            SwitchToWindowView(_skillWindowOpen, "Selected", "SkillInfoPanel");
        }
        
        public void ToggleHealthWindow()
        {
            ResetColonistWindows(_healthWindowOpen);
            _healthWindowOpen = !_healthWindowOpen;
            
            SwitchToWindowView(_healthWindowOpen, "Selected", "HealthInfoPanel");
        }
        
        public void ToggleInventoryWindow()
        {
            ResetColonistWindows(_inventoryWindowOpen);
            _inventoryWindowOpen = !_inventoryWindowOpen;
            
            SwitchToWindowView(_inventoryWindowOpen, "Selected", "InventoryInfoPanel");
        }
        
        public void ToggleInfoWindow()
        {
            ResetColonistWindows(_infoWindowOpen);
            _infoWindowOpen = !_infoWindowOpen;
            
            SwitchToWindowView(_infoWindowOpen, "General", "InfoPanel");
        }

        public void ToggleWalkFaster()
        {
            if (_selectionManager.currentlySelectedObject == null) return;
            
            _selectionManager.currentlySelectedObject.gameObject.GetComponent<AiController>().moveFaster =
                !_selectionManager.currentlySelectedObject.gameObject.GetComponent<AiController>().moveFaster;
        }

        public void OnDraftedColonistButtonPressed()
        {
            var colonist = SelectionManager.Instance.currentlySelectedObject as AiController;
            if (colonist == null) return;
            
            colonist.combatController.ToggleDraftState();
            
            var color = colonist.stateController.Drafted ? new Color(1,1,1,1) : new Color(1,1,1,.15f);
            colonistPanelRelay.draftButton.image.color = color;
        }
        
        public void OnChangeCombatModeButtonPress()
        {
            var colonist = SelectionManager.Instance.currentlySelectedObject as AiController;
            if (colonist == null) return;

            colonist.combatController.ToggleCombatMode();
        }
        //
        
        private void OpenSelectedPanel()
        {
            if(_selectionManager.currentlySelectedObject != null) UIView.ShowView("Selected", "SelectedInfoPanel");
        }

        private static string _tempCategoryName;
        private static string _tempPanelName;
        
        public static void SwitchToWindowView(bool active, string categoryName, string panelName) //toggles views while caching old view and category names
        {
            if (_tempPanelName != string.Empty && _tempCategoryName != string.Empty)
            {
                UIView.HideView(_tempCategoryName, _tempPanelName);
            }
            
            _tempPanelName = panelName;
            _tempCategoryName = categoryName;
            
            if(active) UIView.ShowView(categoryName, panelName);
        }

        private static string _tempCategoryToggleReset;
        public static void ToggleWindowViewWithReset(bool active, string categoryName, string panelName)
        {
            if (_tempCategoryToggleReset != string.Empty)
            {
                UIView.HideViewCategory(_tempCategoryToggleReset);
            }

            _tempCategoryToggleReset = categoryName;
            
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
        
        public static readonly CacheIntString CachedIntToString = new CacheIntString(
            (values)=>values , //describe how seconds (key) are translated to useful value (hash)
            (value)=>value.ToString("0") //you describe how string is built based on value (hash)
            , 0 , 150 , 1 //initialization range and step, so cache will be warmed up and ready
        );
    }
}
