using System;
using System.Collections.Generic;
using System.Linq;
using Ludiq.PeekCore;
using Unity.Mathematics;
using UnityEngine;

namespace ProjectColoni
{
    public class BuildingManager : MonoBehaviour
    {
        public static BuildingManager Instance;

        public List<Part> buildingParts; //parts that are manually created with a ScriptableObject
        public Dictionary<string, BuildPart> cachedBuildParts = new Dictionary<string, BuildPart>(); //active parts to copy when building
        
        [Header("Settings")]
        public float heightAdjustStep = 1;
        public Material transparentMaterial;
        public Color allowColor;
        public Color blockColor;
        
        [Header("Debug")]
        public bool inBuildingMode;
        
        private float _heightAdjustment;
        private LayerMask _groundLayer;
        private BuildPart _previewPart;
        [HideInInspector] public bool heightAdjusting;
        public bool inSnap;
        
        
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
            if (!inBuildingMode || _previewPart == null) return;
            
            AdjustHeightPlacement();
            RotatePreview();

            if (Input.GetKeyDown(KeyCode.Mouse1) || Input.GetKeyDown(KeyCode.Escape))
            {
                ClearPreview();
            }
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && _previewPart.IsPlaceable())
            {
                PlaceBuildPart();
            }

            
            UpdatePreviewTransform();
        }

        private GameObject _buildingsHolder;
        private BuildingsHolder _bHolder; //holds list info
        
        private void InitializeParts() //create a copy of the object to use in the system
        {
            var cachedPartHolder = new GameObject("CachedBuildPieces");
            cachedPartHolder.transform.SetParent(transform);
            //cachedPartHolder.name = cachedPartHolder.name.Replace("(Clone)", "");

            if (_buildingsHolder == null)
            {
                _buildingsHolder = new GameObject("BuildingsHolder");
                _buildingsHolder.transform.SetParent(transform);
                //_buildingsHolder.name = _buildingsHolder.name.Replace("(Clone)", "");

                _bHolder = _buildingsHolder.AddComponent<BuildingsHolder>();
            }
            
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
            _previewPart = Instantiate(cachedBuildParts[partName]/*, _buildingsHolder.transform*/);
            _previewPart.GameObject().SetActive(true);
            _previewPart.GetComponentInChildren<Collider>().enabled = false;

            //material swap
            var rend = _previewPart.GetComponentInChildren<Renderer>();
            _cachedMaterial = rend.material;
            
            var transMat = new Material(transparentMaterial);

            rend.material = transMat;
            rend.sharedMaterial.SetTexture("_BaseMap", _cachedMaterial.mainTexture);

            //
            
            
        }

        private void PlaceBuildPart()
        {
            if (inSnap && _hitPart != null)
            {
                var snappedObject = Instantiate(_previewPart, _previewPart.transform.position, _previewPart.transform.rotation, _hitPart.parentObject.transform);
                snappedObject.name = snappedObject.name.Replace("(Clone)", "");

                var snappedObjectRend = snappedObject.GetComponentInChildren<Renderer>();
                var snappedObjectCol = snappedObject.GetComponentInChildren<Collider>();
            
                snappedObjectCol.enabled = true;
                snappedObjectRend.sharedMaterial = _cachedMaterial;

                snappedObject.parentObject = _hitPart.parentObject;
                snappedObject.floorLevel = _hitPart.floorLevel;
                
                return;
            }

            //create new building and first floor gameObjects if the first foundation is placed without snapping to anything
            var building = new GameObject();
            building.transform.SetParent(_buildingsHolder.transform);
            
            var floor = new GameObject();
            floor.transform.SetParent(building.transform);
            floor.name = "Floor_0";

            var placedObject = Instantiate(_previewPart, _previewPart.transform.position, _previewPart.transform.rotation, floor.transform);
            placedObject.name = placedObject.name.Replace("(Clone)", "");

            CreateBuildPartData(placedObject, floor, building);

            //foundations always base level (floorLevel = 0)
            //placing an unsnapped foundation creates a new building & adds it into buildings list
            //gets named Building_(listIndex)
            //with Floor_(floorLevel) as a child
            //celling will ray cast down and will become that floorLevel + 1

            var rend = placedObject.GetComponentInChildren<Renderer>();
            var col = placedObject.GetComponentInChildren<Collider>();
            
            col.enabled = true;
            rend.sharedMaterial = _cachedMaterial;
        }

        private void CreateBuildPartData(BuildPart part, GameObject floor, GameObject building)
        {
            var floorData = new BuildingsHolder.FloorData
            {
                Object = floor,
                FloorIndex = 0
            };
            floorData.AddPart(part);
            
            var buildingData = new BuildingsHolder.BuildingData
            {
                BuildingGameObject = building
            };
            buildingData.Floors.Add(floorData);
            
            _bHolder.AddBuilding(buildingData);
        }
        
        private void ClearPreview()
        {
            Destroy(_previewPart.gameObject);
            _previewPart = null;
            _cachedMaterial = null;
            _resultRotation = Quaternion.Euler(0,0,0);
        }

        private BuildPart _hitPart;
        
        private void UpdatePreviewTransform()
        {
            var ray = SelectionManager.Instance.cam.ViewportPointToRay(SelectionManager.Instance.cam.ScreenToViewportPoint(Input.mousePosition));
            var rend = _previewPart.GetComponentInChildren<Renderer>();

            var rayCastHit = Physics.Raycast(ray, out var hit, 150, _previewPart.buildOnLayers);
            _hitPart = hit.collider.GetComponentInChildren<BuildPart>();

            if (rayCastHit && !heightAdjusting)
            {
                if (_hitPart != null && _previewPart.buildOnLayers.Includes(_hitPart.gameObject.layer))
                {
                    var pointList = new List<PointData>();
                    inSnap = true;
                    
                    //snap to point
                    foreach (var snap in _hitPart.buildingPart.snappingParts)
                    {
                        if (Enum.GetName(typeof(BuildingPart.SnappingParts.PartName), snap.partName) !=
                            Enum.GetName(typeof(BuildingPart.BuildName), _previewPart.buildingPart.buildName)) continue;
                        
                        foreach (var point in snap.snapPoints)
                        {
                            var pointDistance = _hitPart.transform.position + _hitPart.transform.TransformDirection(point.position);
                            var pointRotation = point.rotation + _hitPart.transform.eulerAngles;
                            
                            var pointData = new PointData
                            {
                                Position = pointDistance,
                                Rotation = pointRotation
                            };
                            
                            if (pointList.Contains(pointData)) break;
                            pointList.Add(pointData);
                            
                            Debug.DrawLine(_hitPart.transform.position + new Vector3(0,2,0), pointDistance + new Vector3(0,2,0));
                        }
                    }
                    
                    pointList.Sort((a, b) =>
                        Vector3.Distance(hit.point, a.Position).CompareTo(Vector3.Distance(hit.point, b.Position)));

                    _previewPart.transform.position = pointList[0].Position; //sorted so can use first in list
                    _previewPart.transform.eulerAngles = pointList[0].Rotation; //fix rotation

                    return;
                }

                inSnap = false;

                var offset = _previewPart.buildingPart.buildOffset + _heightAdjustment;
                
                _previewPart.transform.position = new Vector3(hit.point.x, hit.point.y + offset, hit.point.z);
            }

            _previewPart.UpdatePlaceableColor(rend);
        }
        
        private void AdjustHeightPlacement()
        {
            heightAdjusting = Input.GetKey(KeyCode.R);
            
            var scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            var position = _previewPart.transform.position;
            
            if (Input.GetKey(KeyCode.R) && scrollWheel != 0)
            {
                var heightAdjustment = 0f;
                
                heightAdjustment += scrollWheel * heightAdjustStep;
                heightAdjustment = Mathf.Clamp(heightAdjustment, -0.5f, 0.5f);

                position = new Vector3(position.x, position.y + heightAdjustment, position.z);
                position.y = Mathf.Clamp(position.y, -1.5f, -0.5f);

                _previewPart.transform.position = position;
                
                _heightAdjustment += heightAdjustment;
                _heightAdjustment = Mathf.Clamp(_heightAdjustment, -0.5f, 0.5f);
            }
        }
        
        public float rotateSpeed = 25;
        public float rotateFriction = 0.3f;
        public float rotateSmoothness = 25;
        public Quaternion _resultRotation;
        
        private void RotatePreview()
        {
            if (inSnap)
            {
                _resultRotation = Quaternion.identity;
                return;
            }
            var currentRotation = _previewPart.transform.rotation;

            if (Input.GetKey (KeyCode.E)) _resultRotation = Quaternion.Euler(0, 1 * rotateSpeed * rotateFriction, 0) * currentRotation;
            if (Input.GetKey (KeyCode.Q)) _resultRotation = Quaternion.Euler(0, -1 * rotateSpeed * rotateFriction, 0) * currentRotation;
            
            _previewPart.transform.rotation = Quaternion.Lerp(currentRotation, _resultRotation,
                Time.deltaTime * rotateSmoothness);
        }
        
        public void OnFoundationBuildPress()
        {
            CreatePreview("Foundation");
        }
        public void OnFoundationStairBuildPress()
        {
            CreatePreview("FoundationStair");
        }
        public void OnWallBuildPress()
        {
            CreatePreview("Wall");
        }
        public void OnWallDoorwayBuildPress()
        {
            CreatePreview("Doorway");
        }
        public void OnCellingBuildPress()
        {
            CreatePreview("Celling");
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

    public class PointData
    {
        public Vector3 Position;
        public Vector3 Rotation;
    }
}
