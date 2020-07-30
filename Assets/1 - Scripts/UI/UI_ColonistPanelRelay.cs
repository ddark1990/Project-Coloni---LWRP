using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_ColonistPanelRelay : MonoBehaviour
    {
        [Header("Images")] 
        public Image healthBar;
        public Image staminaBar;
        public Image energyBar;
        public Image foodBar;
        public Image comfortBar;
        public Image recreationBar;
        [Header("Text")]
        public Text healthText;
        public Text staminaText;
        public Text energyText;
        public Text foodText;
        public Text comfortText;
        public Text recreationText;

        private void OnEnable()
        {
            UI_SelectionController.Instance.colonistPanelRelay = this;
        }
    }
}
