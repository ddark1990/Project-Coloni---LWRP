using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ProjectColoni
{
    public class UI_ExpandableBar : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
    {
        [Header("Height")]
        [SerializeField] private bool expandHeight;
        [SerializeField] private RectTransform heightRect;
        [SerializeField] private float expandHeightTo = 50;
        [Header("Width")]
        [SerializeField] private bool expandWidth;
        [SerializeField] private RectTransform widthRect;
        [SerializeField] private float expandWidthTo = 50;
        [Header("Fade Text")]
        public bool fadeInText;
        public Text[] fadeTextArray;
        [Header("Fade Icon")]
        public bool fadeInIcon;
        public Image[] iconImagesArray;
        [Header("Fade Canvas")]
        public bool fadeInCanvas;
        public CanvasGroup[] canvasGroupsArray; //manual
    
        [Header("Settings")]
        [SerializeField] private float expandSpeed = 10;
        [SerializeField] private float fadeInSpeed = 5;
        [SerializeField] private float fadeOutSpeed = 25;

        [HideInInspector] public bool expand;
        public bool fullyExpanded;
        
        private float _expandXFrom;
        private float _expandYFrom;
        private Vector2 _heightDelta;
        private Vector2 _widthDelta;

        private void Start()
        {
            if (expandHeight)
            {
                if (heightRect == null) heightRect = GetComponentInChildren<RectTransform>();

                _heightDelta = heightRect.sizeDelta;
                _expandYFrom = _heightDelta.y;
            }

            if (expandWidth)
            {
                if (widthRect == null) widthRect = GetComponentInChildren<RectTransform>();

                _widthDelta = widthRect.sizeDelta;
                _expandXFrom = _widthDelta.x;
            }
        }

        private void Update()
        {
            FadeInText();
            FadeInIcons();
            ExpandHeight();
            ExpandWidth();
            FadeInCanvas();

            if (_widthDelta.x >= (expandWidthTo - 1f) || _heightDelta.y >= (expandHeightTo - 1f)) fullyExpanded = true;
            else fullyExpanded = false;
        }

        private Color _textColor = Color.white;
        private void FadeInText()
        {
            if (!fadeInText) return;

            _textColor = expand ? new Color(_textColor.r, _textColor.g, _textColor.b, Mathf.Lerp(_textColor.a, 1, Time.deltaTime * expandSpeed)) : new Color(_textColor.r, _textColor.g, _textColor.b, Mathf.Lerp(_textColor.a, 0, Time.deltaTime * expandSpeed));
            
            foreach (var text in fadeTextArray)
            {
                text.color = _textColor;
            }
        }
        
        private Color _iconColor = Color.white;
        private void FadeInIcons()
        {
            if (!fadeInIcon) return;
            
            _iconColor = fullyExpanded ? new Color(_iconColor.r, _iconColor.g, _iconColor.b, Mathf.Lerp(_iconColor.a, 1, Time.deltaTime * fadeInSpeed)) : new Color(_iconColor.r, _iconColor.g, _iconColor.b, Mathf.Lerp(_iconColor.a, 0, Time.deltaTime * fadeOutSpeed));

            foreach (var icon in iconImagesArray)
            {
                icon.color = _iconColor;
            }
        }
        
        private void FadeInCanvas()
        {
            if (!fadeInCanvas) return;

            foreach (var canvasGroup in canvasGroupsArray)
            {
                canvasGroup.alpha = expand ? Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * fadeInSpeed) :  Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime * fadeOutSpeed);
            }
        }

        private void ExpandHeight()
        {
            if (!expandHeight) return;
            
            _heightDelta = expand ? new Vector2(heightRect.sizeDelta.x, Mathf.Lerp(_heightDelta.y, expandHeightTo, Time.deltaTime * expandSpeed)) : new Vector2(heightRect.sizeDelta.x, Mathf.Lerp(_heightDelta.y, _expandYFrom, Time.deltaTime * expandSpeed));
            heightRect.sizeDelta = _heightDelta;
        }

        private void ExpandWidth()
        {
            if (!expandWidth) return;
            
            _widthDelta = expand ? new Vector2(Mathf.Lerp(_widthDelta.x, expandWidthTo, Time.deltaTime * expandSpeed), widthRect.sizeDelta.y) : new Vector2(Mathf.Lerp(_widthDelta.x, _expandXFrom, Time.deltaTime * expandSpeed),widthRect.sizeDelta.y);
            widthRect.sizeDelta = _widthDelta;
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
