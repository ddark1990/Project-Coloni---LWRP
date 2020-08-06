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
        [Header("Fade")]
        public bool fadeInText;
        public Text fadeText;
        public bool fadeInIcon;
        public Image iconImage;
        public bool fadeInCanvas;
        public CanvasGroup canvasGroup; //manual
    
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
            heightRect = GetComponent<RectTransform>();

            if (iconImage == null)
            {
                //Debug.LogWarning("No Icon Image component has been found! Check gameObject.", this);
            }

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
            ExpandHeight();
            ExpandWidth();
            FadeInCanvas();
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
            
            if (!fadeInIcon) return;
            iconImage.color = color;
        }

        private void FadeInCanvas()
        {
            if (!fadeInCanvas) return;
            
            canvasGroup.alpha = expand ? Mathf.Lerp(canvasGroup.alpha, 1, Time.deltaTime * fadeSpeed) :  Mathf.Lerp(canvasGroup.alpha, 0, Time.deltaTime * fadeSpeed);
        }

        private void ExpandHeight()
        {
            if (!expandHeight) return;
            
            if (heightRect == null)
            {
                //Debug.LogWarning("No Rect Transform has been found! Check gameObject.", this);
                return;
            }

            _heightDelta = expand ? new Vector2(_heightDelta.x, Mathf.Lerp(_heightDelta.y, expandHeightTo, Time.deltaTime * expandSpeed)) : new Vector2(_heightDelta.x, Mathf.Lerp(_heightDelta.y, _expandYFrom, Time.deltaTime * expandSpeed));
            heightRect.sizeDelta = _heightDelta;
        }

        private void ExpandWidth()
        {
            if (!expandWidth) return;
            
            if (widthRect == null)
            {
                //Debug.LogWarning("No Rect Transform has been found! Check gameObject.", this);
                return;
            }
            
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
