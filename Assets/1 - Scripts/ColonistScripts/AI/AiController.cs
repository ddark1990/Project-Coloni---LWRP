using System;
using System.Collections;
using System.Collections.Generic;
using ProjectColoni;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    public class AiController : AiBase
    {
        private NavMeshAgent _navMeshAgent;
        private Selectable _selectable;
        private Camera _camera;
        private LineRenderer _destinationLineRenderer;


        private void Start()
        {
            GetComponents();
            
            OnStartInitializeComponents();
            InitializeSelectable();
        }

        private void GetComponents()
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _selectable = GetComponent<Selectable>();
            _destinationLineRenderer = GetComponent<LineRenderer>();
            _camera = SelectionManager.Instance.cam;
        }
        
        private void Update()
        {
            DrawLineRendererPaths(_navMeshAgent, _destinationLineRenderer);
            OutlineHighlight();
            
            if (Input.GetMouseButtonDown(1) && _selectable.selected)
            {
                SetDestinationToMousePosition();
            }
        }
        
        private void SetDestinationToMousePosition()
        {
            if (_camera == null) return;
            
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit)) return;
                
            _navMeshAgent.SetDestination(hit.point);
            OnGroundClickSpawn(hit.point);
        }
        
        private void OnGroundClickSpawn(Vector3 posClicked)
        {
            var obj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), posClicked, Quaternion.identity);
            
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            Destroy(obj, 3);
        }
        
        private void DrawLineRendererPaths(NavMeshAgent agent, LineRenderer lineRenderer) //navigation path render line for in game *currently allocates 88 bytes per line drawn
        {
            
            if (agent.remainingDistance <= 0.1f || !_selectable.selected) //should only be visible when that unit is selected
            {
                lineRenderer.enabled = false;
                return;
            }

            var agentPath = agent.path;
            
            lineRenderer.positionCount = agentPath.corners.Length;
            lineRenderer.SetPositions(agentPath.corners);
            lineRenderer.enabled = true;
        }
    }
}
