using Doozy.Engine.UI;
using ProjectColoni;
using UnityEngine;

namespace ProjectColoni
{
    public class UI_SelectionController : MonoBehaviour
    {
        private SelectionManager _selectionManager;

        [SerializeField] private GameObject canvas;

        public bool _skillWindowOpen;
        public bool _healthWindowOpen;
        public bool _inventoryWindowOpen;
    
        private void Start()
        {
            _selectionManager = SelectionManager.Instance;
        }

        private void Update()
        {
            canvas.SetActive(_selectionManager.currentlySelectedObject != null);
            
            SwitchActiveInfoWindow();
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
