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
        public LayerMask buildOnLayers;
        public GameObject parentObject;
        public int floorLevel;
        
        private List<Collider> _collidersHit = new List<Collider>(); //colliding objects must have a rigidbody

        private Renderer _renderer;
        
        public void InitializeBuildingPart()
        {
            buildOnLayers = buildingPart.buildOn;
            _renderer = GetComponentInChildren<Renderer>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer != 9)
            {
                _collidersHit.Add(other);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != 9)
            {
                _collidersHit.Remove(other);
            }
        }
        
        public bool IsPlaceable()
        {
            return _collidersHit.Count == 0;
        }
        
        public void UpdatePlaceableColor(Renderer rend)
        {
            rend.sharedMaterial.SetColor("_BaseColor",
                IsPlaceable() ? BuildingManager.Instance.allowColor : BuildingManager.Instance.blockColor);
        }
    }
}
