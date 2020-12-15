using System;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_ResourcePanelRelay : MonoBehaviour
    {
        public Text amountText;

        private void OnEnable()
        {
            UI_SelectionController.ResourcePanelRelay = this;
        }
    }
}
