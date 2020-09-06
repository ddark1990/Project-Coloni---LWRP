using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ProjectColoni
{
    public class BuildPart : MonoBehaviour
    {
        public BuildingPart buildingPart;
        
        [HideInInspector] public LayerMask buildOnLayers;

        
        public void InitializeBuildingPart()
        {
            buildOnLayers = buildingPart.buildOn;
        }
        
    }
}
