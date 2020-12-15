using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class UI_ColonistPanel : MonoBehaviour
    {
        [SerializeField] private GameObject colonistButton;
        private List<UI_ColonistButton> _buttons = new List<UI_ColonistButton>();
        
        public void UpdateColonistPanel(AiController aiController)
        {
            var button = Instantiate(colonistButton, transform).GetComponent<UI_ColonistButton>();
            
            _buttons.Add(button);
            button.nickName.text = "fag";
            button.aiReference = aiController;
        }
    }
}
