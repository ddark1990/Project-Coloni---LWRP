using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace ProjectColoni
{
    [CreateAssetMenu(fileName = "(BuildingPart)", menuName = "ProjectColoni/BuildingSystem/New Part")]
    public class BuildingPart : ScriptableObject
    {
        public LayerMask buildOn;
        public List<SnappingParts> snappingParts;
        
        [Serializable]
        public struct SnappingParts
        {
            public enum PartName
            {
                Foundation, FoundationStair, Wall, WindowWall, Doorway, Celling, Stair, Railing
            }
            
            public PartName partName;
            public List<SnapPoints> snapPoints;
            
            [Serializable]
            public class SnapPoints
            {
                public string name;
                public Vector3 position;
                public Vector3 rotation;
            }
        }
    }
}
