using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_ColonistButton : MonoBehaviour
    {
        [HideInInspector] public AiController aiReference;

        public Button selfButton;
        public Image icon;
        public Text nickName;
        

        public void InitializeButton()
        {
            //SelectionManager.Instance.SelectObjectOnUi(aiReference);
        }
    }
}
