using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    public class NavMeshTest : MonoBehaviour
    {
        public NavMeshSurface surface;
        public float distanceThreshHold;
        NavMeshData m_NavMesh;

        private void OnEnable()
        {
            m_NavMesh = new NavMeshData();
            
        }

        private void Awake()
        {
            UpdateNavMesh();
        }

        private void Update()
        {
            if (Vector3.Distance(transform.position, surface.transform.position) >= distanceThreshHold)
                UpdateNavMesh();
        }

        private void UpdateNavMesh()
        {
            surface.RemoveData();
            surface.transform.position = transform.position;
            surface.BuildNavMesh();
            
        }
    }
}
