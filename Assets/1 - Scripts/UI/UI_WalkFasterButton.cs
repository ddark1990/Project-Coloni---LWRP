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
            if (SelectionManager.CurrentlySelectedObject == null)
            {
                _image.color = Color.white; //set to default color
                return;
            }
            
            _image.color = SelectionManager.CurrentlySelectedObject.GetComponent<AiController>().moveFaster
                ? Color.green
                : Color.white;
        }
    }
}
