using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = System.Numerics.Quaternion;

namespace ProjectColoni
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance;

        public List<Part> buildingParts; //parts that are manually created with a ScriptableObject
        public Dictionary<string, BuildPart> cachedBuildParts = new Dictionary<string, BuildPart>(); //active parts to copy when building
        
        public bool inBuildingMode;

        [Header("Settings")]
        public Material transparentMaterial;
        public Color allowColor;
        public Color blockColor;
        
        private LayerMask _groundLayer;
        private GameObject _previewPart;

        private void Awake()
        {
            _groundLayer = LayerMask.GetMask("Ground");
            
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitializeParts();
        }

        private void Update()
        {
            if (!inBuildingMode) return;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                PlaceBuildPart();
            }

            if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape))
            {
                ClearPreview();
            }
            
            UpdatePreviewTransform();
        }

        private void InitializeParts() //create a copy of the object to use in the system
        {
            var cachedPartHolder = Instantiate(new GameObject("CachedBuildPieces"), transform);
            cachedPartHolder.name = cachedPartHolder.name.Replace("(Clone)", "");

            foreach (var buildingPart in buildingParts)
            {
                var cachedGameObjectPart = Instantiate(buildingPart.prefab, cachedPartHolder.transform);
                var cachedBuildPart = cachedGameObjectPart.GetComponentInChildren<BuildPart>();

                cachedGameObjectPart.name = cachedGameObjectPart.name.Replace("(Clone)", "");

                cachedGameObjectPart.SetActive(false);
                
                cachedBuildPart.InitializeBuildingPart();
                
                cachedBuildParts.Add(buildingPart.partName.ToString(), cachedBuildPart);
            }
        }

        private Material _cachedMaterial;
        
        private void CreatePreview(string partName)
        {
            _previewPart = Instantiate(cachedBuildParts[partName].gameObject);
            _previewPart.SetActive(true);

            _previewPart.GetComponentInChildren<Collider>().enabled = false;

            //material swap
            var rend = _previewPart.GetComponentInChildren<Renderer>();
            _cachedMaterial = rend.material;

            rend.material = transparentMaterial;
            rend.sharedMaterial.SetTexture("_BaseMap", _cachedMaterial.mainTexture);

            //
            
            
        }

        private void ClearPreview()
        {
            if (_previewPart == null) return;
            
            Destroy(_previewPart.gameObject);
            _previewPart = null;
            _cachedMaterial = null;
        }
        
        private void UpdatePreviewTransform()
        {
            if (_previewPart == null) return;
            
            var ray = SelectionManager.Instance.cam.ViewportPointToRay(SelectionManager.Instance.cam.ScreenToViewportPoint(Input.mousePosition));
            
            var rayCast = Physics.Raycast(ray, out var hit, 150, _groundLayer);

            if (rayCast)
            {
                _previewPart.transform.position = hit.point;
            }
        }

        private void PlaceBuildPart()
        {
            if (_previewPart == null) return;

            var placedObject = Instantiate(_previewPart, _previewPart.transform.position, _previewPart.transform.rotation);

            var rend = placedObject.GetComponentInChildren<Renderer>();
            rend.sharedMaterial = _cachedMaterial;
        }
        
        public void OnFoundationBuildPress()
        {
            CreatePreview("Foundation");
        }
        public void OnFoundationStairBuildPress()
        {
            CreatePreview("FoundationStair");
        }

        [Serializable]
        public struct Part
        {
            public enum PartName
            {
                Foundation, FoundationStair, Wall, WindowWall, Doorway, Celling, Stair, Railing
            }
            
            public PartName partName;
            public GameObject prefab;
        }
    }
}
