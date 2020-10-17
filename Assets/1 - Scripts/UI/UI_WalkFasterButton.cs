using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_WalkFasterButton : MonoBehaviour
    {
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (SelectionManager.Instance.currentlySelectedObject == null) return;
            
            _image.color = SelectionManager.Instance.currentlySelectedObject.GetComponent<AiController>().moveFaster
                ? Color.green
                : Color.white;
        }
    }
}
