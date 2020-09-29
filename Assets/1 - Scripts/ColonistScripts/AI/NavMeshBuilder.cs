using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    public static class NavMeshBuilder
    {
        public static GameObject CreateNavSurface(AiController aiController, Vector3 navMeshSize) //should parent ai somehow
        {
            var navMeshSurface = new GameObject("NavMeshSurface: " + aiController.name);
            var surface = navMeshSurface.AddComponent<NavMeshSurface>();
            
            navMeshSurface.transform.SetParent(GameManager.Instance.transform);
            
            surface.collectObjects = CollectObjects.Volume;
            surface.agentTypeID = aiController.navMeshAgent.agentTypeID;
            surface.size = navMeshSize;
            
            return navMeshSurface;
        }

        public static GameObject CreateNavSurface(Vector3 position)
        {
            var navMeshSurface = new GameObject("NavMeshSurface");
            var surface = navMeshSurface.AddComponent<NavMeshSurface>();

            navMeshSurface.transform.SetParent(GameManager.Instance.transform);
            surface.collectObjects = CollectObjects.Volume;
            surface.transform.position = position;

            return navMeshSurface;
        }
        
        public static void UpdateNavMesh(NavMeshSurface surface, Vector3 aiPosition, float updateDistance)
        {
            if (!(Vector3.Distance(aiPosition, surface.transform.position) >= updateDistance)) return;
            
            surface.RemoveData();
            surface.transform.position = aiPosition;
            surface.BuildNavMesh();
        }
        
        public static void UpdateNavMesh(NavMeshSurface surface)
        {
            surface.RemoveData();
            surface.BuildNavMesh();
        }
    }
}
