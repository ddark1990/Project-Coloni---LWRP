using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace ProjectColoni
{
    public class ColonistController : MonoBehaviour
    {
        //ai
        private NavMeshAgent _myNavMeshAgent;
        
        //
        private Selectable _selectable;

        //rendering 
        private LineRenderer _destinationLineRenderer;
        private SkinnedMeshRenderer _skinnedMeshRenderer;


        private void GetComponents()
        {
            //root components
            _myNavMeshAgent = GetComponent<NavMeshAgent>();
            _selectable = GetComponent<Selectable>();
            _destinationLineRenderer = GetComponent<LineRenderer>();
            
            //children components
            _skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        private void Awake()
        {
            GetComponents(); //must come before anything
        }

        private void Start()
        {
            RandomColorOnStart();
        }

        private void Update()
        {
            DrawLineRendererPaths(_myNavMeshAgent, _destinationLineRenderer);
            
            if (Input.GetMouseButtonDown(1) && _selectable.selected)
            {
                SetDestinationToMousePosition();
                //Debug.Log("NEW PATH");
            }

            /*
            if (!_selectable.selected)
            {
                //Wander(_myNavMeshAgent, 1, 10);
            }

            if (_myNavMeshAgent.path.corners.Length <= 0) return;
            
            DrawCornerLines(_myNavMeshAgent.path.corners);
        */
        }

        private void SetDestinationToMousePosition()
        {
            if (Camera.main != null)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out var hit)) return;
                
                _myNavMeshAgent.SetDestination(hit.point);
                OnGroundClickSpawn(hit.point);
            }
        }

        private void OnGroundClickSpawn(Vector3 posClicked)
        {
            var obj = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Sphere), posClicked, Quaternion.identity);
            
            obj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            
            Destroy(obj, 3);
        }

        private void RandomColorOnStart() //will be replaced with a modular creation system for each colonist, or presets might be used
        {
            var newColor = new Color(Random.Range(0, 1f),Random.Range(0, 1f),Random.Range(0, 1f));

            _skinnedMeshRenderer.material.color = newColor;
        }
        
        private void DrawLineRendererPaths(NavMeshAgent agent, LineRenderer lineRenderer) //navigation path render line for in game *currently allocates 88 bytes per line drawn
        {
            
            if (agent.remainingDistance <= 0.1f || !_selectable.selected) //should only be visible when that unit is selected
            {
                //Debug.Log(agent.remainingDistance);

                lineRenderer.enabled = false;
                return;
            }

            var agentPath = agent.path;
            
            lineRenderer.positionCount = agentPath.corners.Length;
            lineRenderer.SetPositions(agentPath.corners);
            lineRenderer.enabled = true;
        }
        
        private void DrawCornerLines(IReadOnlyList<Vector3> corners) //debug lines for editor
        {
            for (int i = 0; i < corners.Count; i++)
            {
                var corner = corners[i];

                if ((i + 1) < corners.Count)
                {
                    Debug.DrawLine(corner, corners[i + 1]);
                }
            }
        }
        
        float elapsedTime = 0f;
        private NavMeshPath path;
        public void Wander(NavMeshAgent agent, float waitTime, float wanderRadius)
        {
            if (agent.hasPath) return;

            path = new NavMeshPath();
            
            elapsedTime += Time.deltaTime;
            Debug.Log(elapsedTime);

            if (elapsedTime > waitTime)
            {
                elapsedTime -= waitTime;
                NavMesh.CalculatePath(agent.transform.position, GetRandomRadialPos(wanderRadius), NavMesh.AllAreas, path);
                Debug.Log("Path Calculated for " + agent.name);

                agent.SetPath(path);
                Debug.Log("Path Set for " + agent.name);
            }
        }
        private Vector3 GetRandomRadialPos(float radiusSize)
        {
            return transform.position + OnUnitSphere() * radiusSize;
        }

        private static Vector3 OnUnitSphere() //move into job
        {
            return new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y);
        }
    }

}