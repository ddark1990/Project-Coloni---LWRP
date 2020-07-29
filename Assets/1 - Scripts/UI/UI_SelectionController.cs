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
        [SerializeField] private GameObject selectedBasePanel;
        [SerializeField] private GameObject selectedColonistPanel;
        [SerializeField] private GameObject selectedObjectPanel;
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
            canvas.SetActive(_selectionManager.currentlySelectedObject != null);
            
            SwitchActiveInfoWindow();
        }

        private void LateUpdate()
        {
            if(selectedColonistPanel.activeSelf) PopulateColonistVitalsUiData();
        }

        private void SetActivePanel(string activePanel)
        {
            selectedColonistPanel.SetActive(activePanel.Equals(selectedColonistPanel.name));
            selectedObjectPanel.SetActive(activePanel.Equals(selectedObjectPanel.name));
        }
        
        public void TogglePanelHolder()
        {
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
                case Harvestable harvestable:
                    SetActivePanel(selectedObjectPanel.name);

                    PopulateBaseData(harvestable);
                    break;
                case Item item:
                    SetActivePanel(selectedObjectPanel.name);

                    PopulateBaseData(item);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void PopulateColonistVitalsUiData() //allocates 84 bytes
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

        private void PopulateBaseData(Selectable selected)
        {
            var panelHolderRelay = selectedBasePanel.GetComponentInChildren<UI_PanelHolderRelay>();
            switch (selected)
            {
                case AiController aiController:
                    //panelHolderRelay.objectName = aiController.aiStats.obj
                    break;
                case Harvestable harvestable:
                    
                    break;
                case Item item:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private void SwitchActiveInfoWindow()
        {
            if(_skillWindowOpen) UIView.ShowView("Selected", "SkillInfoPanel");
            else UIView.HideView("Selected", "SkillInfoPanel");
            
            if(_healthWindowOpen) UIView.ShowView("Selected", "HealthInfoPanel");
            else UIView.HideView("Selected", "HealthInfoPanel");

            if(_inventoryWindowOpen) UIView.ShowView("Selected", "InventoryInfoPanel");
            else UIView.HideView("Selected", "InventoryInfoPanel");
        }
        
        public void ToggleSkillWindow()
        {
            ResetWindows(_skillWindowOpen);

            _skillWindowOpen = !_skillWindowOpen;
        }
        
        public void ToggleHealthWindow()
        {
            ResetWindows(_healthWindowOpen);
            
            _healthWindowOpen = !_healthWindowOpen;
        }
        
        public void ToggleInventoryWindow()
        {
            ResetWindows(_inventoryWindowOpen);
            
            _inventoryWindowOpen = !_inventoryWindowOpen;
        }

        
        
        private void ResetWindows(bool active)
        {
            if (active)
            {
                active = false;
                return;
            }
            
            _inventoryWindowOpen = false;
            _healthWindowOpen = false;
            _skillWindowOpen = false;
        }
    }
}
