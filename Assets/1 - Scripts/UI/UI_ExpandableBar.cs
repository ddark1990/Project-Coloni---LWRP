﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_ExpandableBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
    {
        [SerializeField] public float expandTo = 20;
        [SerializeField] public float expandSpeed = 10;
        public bool expand;
    
        [HideInInspector] public RectTransform rect;
        public Text fadeText;
        public Image iconImage; //manual

        public bool fadeInText;
        
        private float _expandFrom;
        private Vector2 _sizeDelta;

        private void Start()
        {
            rect = GetComponent<RectTransform>();

            if (iconImage == null)
            {
                //Debug.LogWarning("No Icon Image component has been found! Check gameObject.", this);
            }
        
            _sizeDelta = rect.sizeDelta;
            _expandFrom = _sizeDelta.y;
        }

        private void Update()
        {
            FadeInText();
            ExpandBar();
        }

        private void FadeInText()
        {
            if (!fadeInText) return;
            
            if (fadeText == null)
            {
                //Debug.LogWarning("No Text component has been found! Check gameObject.", this);
                return;
            }
        
            var color = fadeText.color;
            color = expand ? new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 1, Time.deltaTime * expandSpeed)) : new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0, Time.deltaTime * expandSpeed));
            fadeText.color = color;
        }

        private void ExpandBar()
        {
            if (rect == null)
            {
                //Debug.LogWarning("No Rect Transform has been found! Check gameObject.", this);
                return;
            }

            _sizeDelta = expand ? new Vector2(_sizeDelta.x, Mathf.Lerp(_sizeDelta.y, expandTo, Time.deltaTime * expandSpeed)) : new Vector2(_sizeDelta.x, Mathf.Lerp(_sizeDelta.y, _expandFrom, Time.deltaTime * expandSpeed));
            rect.sizeDelta = _sizeDelta;
        }
    
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            expand = true;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            expand = false;
        }
    }
}
