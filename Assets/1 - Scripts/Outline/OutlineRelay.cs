using System.Linq;
using UnityEngine;

namespace ProjectColoni
{
    public class OutlineRelay : MonoBehaviour
    {
        public Color OutlineColor {
            get { return outlineColor; }
            set {
                outlineColor = value;
                _needsUpdate = true;
            }
        }

        public float OutlineWidth {
            get { return outlineWidth; }
            set {
                outlineWidth = value;
                _needsUpdate = true;
            }
        }

        [SerializeField] private Color outlineColor;
        [SerializeField] private float outlineWidth = 2;
    
        [HideInInspector] public Renderer meshRenderer;
        [HideInInspector] public Material[] cachedMaterials;
        [HideInInspector] public Material[] outlineMaterials;
        private Texture _cachedTexture;
        private Color _cachedColor;

        private bool _needsUpdate;
        private Material _outlineMaskMaterial;
        private Material _outlineMaterial;
    
        private static readonly int Color = Shader.PropertyToID("_Color");
        private static readonly int Width = Shader.PropertyToID("_Border");
        private static readonly int BaseMap = Shader.PropertyToID("_BaseMap");

        private SelectionManager _selectionManager;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");

        private void Awake()
        {
            meshRenderer = GetComponentInChildren<Renderer>();
            
            _needsUpdate = true;
        }
    
        private void Start()
        {
            _selectionManager = SelectionManager.Instance;

            CacheMaterialData(); 
            CreateOutlineMaterials();
        }

        private void OnValidate()
        {
            _needsUpdate = true;
        }

        private void Update()
        {
            if (!_needsUpdate) return;
        
            UpdateMaterials();
            _needsUpdate = false;
        }

        private void CreateOutlineMaterials() 
        {
            _outlineMaskMaterial = Instantiate(_selectionManager.outlineMaskMaterial);
            _outlineMaterial = Instantiate(_selectionManager.outlineMaterial);

            SetMaterialData();
            
            if(outlineMaterials.Length == 0)
                outlineMaterials = new Material[2];
            
            outlineMaterials[0] = _outlineMaskMaterial;
            outlineMaterials[1] = _outlineMaterial;
            
            /*
            var materials = meshRenderer.sharedMaterials.ToList();

            materials.Add(_outlineMaskMaterial);
            materials.Add(_outlineMaterial);

            meshRenderer.materials = materials.ToArray();
        */
        }
    
        private void CacheMaterialData()
        {
            cachedMaterials = new Material[meshRenderer.sharedMaterials.Length];

            var materials = meshRenderer.materials;
            
            cachedMaterials = materials; //cache original materials

            foreach (var material in cachedMaterials) //cache each of the materials main textures
            {
                _cachedTexture = material.mainTexture;
                _cachedColor = material.color;
            }
        }
        
        private void SetMaterialData()
        {
            _outlineMaskMaterial.SetTexture(BaseMap, _cachedTexture);
            _outlineMaskMaterial.SetColor(BaseColor, _cachedColor);

            _outlineMaskMaterial.name = "OutlineMask (Instance)";
            _outlineMaterial.name = "OutlineFill (Instance)";
        }
        
        private void UpdateMaterials()
        {
            _outlineMaterial.SetColor(Color, outlineColor);
            _outlineMaterial.SetFloat(Width, outlineWidth);
        }
    }
}
