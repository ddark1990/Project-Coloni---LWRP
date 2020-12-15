using UnityEngine;

namespace ProjectColoni
{
    public class Item : SmartObject
    {
        public ItemType itemTypeData;

        [Header("Item")] 
        public bool equipped;
        
        public int ItemCount
        {
            get => _itemCount;
            private set
            {
                _itemCount = value;
                EventRelay.OnItemCountChange(value);
            }
        }
        private int _itemCount = 1;
        
        [Header("Debug")]
        public bool ignore;

        private MeshFilter _meshFilter;
        private MeshCollider _meshCollider;
        private Mesh _meshRef; //grab the first reference to swap back to later
        
        private Resource _resourceReference;

        
        #region Unity Functions
        private void OnEnable()
        {
            EventRelay.OnItemCountChange += UpdateItemOnCountChange;
        }
        
        private void Start()
        {
            InitializeSelectable();

            GlobalObjectDictionary.Instance.AddToGlobalDictionary(baseObjectInfo.Id, this);
            
            smartActions.InitializeSmartActions(this);
            
            //get required comp
            GrabAllRequiredComponents();
        }

        private void Update()
        {
            OutlineHighlight();

            if (Input.GetKeyDown(KeyCode.U)) ItemCount++;
            if (Input.GetKeyDown(KeyCode.I)) ItemCount--;
        }

        #endregion
        
        // Updates the items model based on ItemCount change event.
        private void UpdateItemOnCountChange(int var)
        {
            if (_resourceReference != null && _resourceReference.stackLevels.Length.Equals(0)) return;
            
            //update stack model based on itemCount
            if (var > 29)//large mesh
            {
                _meshFilter.mesh = _resourceReference.stackLevels[1].skin;
                _meshCollider.sharedMesh = _resourceReference.stackLevels[1].skin;

            }
            else if (var > 9) //medium mesh
            {
                _meshFilter.mesh = _resourceReference.stackLevels[0].skin;
                _meshCollider.sharedMesh = _resourceReference.stackLevels[0].skin;
            }
            else //small mesh
            {
                _meshFilter.mesh = _meshRef;
                _meshCollider.sharedMesh = _meshRef;
            }
        }

        private void GrabAllRequiredComponents()
        {
            _meshFilter = GetComponent<MeshFilter>();
            _meshCollider = GetComponent<MeshCollider>();
            _meshRef = _meshFilter.mesh;

            _resourceReference = itemTypeData as Resource;
            
        }
    }
}
