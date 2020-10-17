using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening.Plugins.Core.PathCore;
using Pathfinding;
using UnityEngine;

namespace ProjectColoni
{
    public class testpather : MonoBehaviour
    {
        public LayerMask groundLayer;

        private AIPath _aiPath;
        
        private void Start()
        {
            _aiPath = GetComponent<AIPath>();
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Mouse1)) return;
            
            var ray = SelectionManager.Instance.cam.ViewportPointToRay(SelectionManager.Instance.cam.ScreenToViewportPoint(Input.mousePosition));

            var rayCastHit = Physics.Raycast(ray, out var hit, Mathf.Infinity, groundLayer);
            Debug.Log(rayCastHit);
            
            if (!rayCastHit) return;
            
            _aiPath.destination = hit.point;
            
            //_aiPath.SearchPath();

        }
    }
}
