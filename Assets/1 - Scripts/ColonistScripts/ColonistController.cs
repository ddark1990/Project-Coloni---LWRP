using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ProjectColoni
{
    public class ColonistController : MonoBehaviour
    {
        public NavMeshAgent _myNavMeshAgent;
        void Start()
        {
            _myNavMeshAgent = GetComponent<NavMeshAgent>();
        }

        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                SetDestinationToMousePosition();
            }
        }

        void SetDestinationToMousePosition()
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                _myNavMeshAgent.SetDestination(hit.point);
            }
        }
    }

}