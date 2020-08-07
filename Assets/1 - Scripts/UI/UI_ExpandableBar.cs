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
        [SerializeField] private float fadeSpeed = 10;

        [HideInInspector] public bool expand;
        
        private float _expandXFrom;
        private float _expandYFrom;
        private Vector2 _heightDelta;
        private Vector2 _widthDelta;

        private void Start()
        {
            if (expandHeight)
            {
                _heightDelta = heightRect.sizeDelta;
                _expandYFrom = _heightDelta.y;
            }

            if (expandWidth)
            {
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
            
            _iconColor = expand ? new Color(_iconColor.r, _iconColor.g, _iconColor.b, Mathf.Lerp(_iconColor.a, 1, Time.deltaTime * expandSpeed)) : new Color(_iconColor.r, _iconColor.g, _iconColor.b, Mathf.Lerp(_iconColor.a, 0, Time.deltaTime * expandSpeed));

            foreach (var icon in iconImagesArray)
            {
                icon.color = _iconColor;
            }
        }
        private Color GetUpdatedColor()
        {
            var color = Color.white;
            return expand ? new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 1, Time.deltaTime * expandSpeed)) : new Color(color.r, color.g, color.b, Mathf.Lerp(color.a, 0, Time.deltaTime * expandSpeed));
        }
        
        private void FadeInCanvas()
        {
            if (!fadeInCanvas) return;

            foreach (var canvasGroup in canvasGroupsArray)
            {
                canvasGroup.alpha = expand ? Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * fadeSpeed) :  Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime * fadeSpeed);
            }
        }

        private void ExpandHeight()
        {
            if (!expandHeight) return;
            
            _heightDelta = expand ? new Vector2(_heightDelta.x, Mathf.Lerp(_heightDelta.y, expandHeightTo, Time.deltaTime * expandSpeed)) : new Vector2(_heightDelta.x, Mathf.Lerp(_heightDelta.y, _expandYFrom, Time.deltaTime * expandSpeed));
            heightRect.sizeDelta = _heightDelta;
        }

        private void ExpandWidth()
        {
            if (!expandWidth) return;
            
            _widthDelta = expand ? new Vector2(Mathf.Lerp(_widthDelta.x, expandWidthTo, Time.deltaTime * expandSpeed), _widthDelta.y) : new Vector2(Mathf.Lerp(_widthDelta.x, _expandXFrom, Time.deltaTime * expandSpeed), _widthDelta.y);
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
