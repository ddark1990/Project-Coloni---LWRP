using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    public static class NavMeshBuilder
    {
        public static GameObject CreateNavSurfaceForAgent(AiController aiController, Vector3 navMeshSize, float updateDistance)
        {
            var navMeshSurface = new GameObject("NavMeshSurface: " + aiController.name);
            var surface = navMeshSurface.AddComponent<NavMeshSurface>();
            
            const int layerMask =~ (1<<2); //everything but ignore layer
            navMeshSurface.transform.SetParent(GameManager.Instance.transform);

            surface.collectObjects = CollectObjects.Volume;
            surface.agentTypeID = aiController.navMeshAgent.agentTypeID;
            surface.layerMask = layerMask;

            UpdateNavMesh(surface, navMeshSize, aiController, updateDistance);

            return navMeshSurface;
        }

        public static NavMeshSurface CreateNavSurfaceAtPosition(Vector3 position)
        {
            var navMeshSurface = new GameObject("NavMeshSurface");
            var surface = navMeshSurface.AddComponent<NavMeshSurface>();
            
            const int layerMask =~ (1<<2); 
            navMeshSurface.transform.SetParent(GameManager.Instance.transform);
            
            surface.collectObjects = CollectObjects.Volume;
            surface.layerMask = layerMask;
            surface.transform.position = position;

            UpdateNavMesh(surface);

            return surface;
        }
        
        public static void UpdateNavMesh(NavMeshSurface surface, Vector3 size, AiController aiController, float updateDistance)
        {
            if (!(Vector3.Distance(aiController.transform.position, surface.transform.position) >= updateDistance)) return;
            
            surface.RemoveData();

            surface.size = size;
            
            surface.transform.position = aiController.transform.position;
            surface.BuildNavMesh();
        }
        
        public static void UpdateNavMesh(NavMeshSurface surface)
        {
            surface.RemoveData();
            surface.BuildNavMesh();
        }
    }
}
