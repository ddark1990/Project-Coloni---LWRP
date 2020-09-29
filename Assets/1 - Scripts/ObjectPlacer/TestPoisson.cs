using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectColoni
{
    public class TestPoisson : MonoBehaviour
    {
        public float radius = 1;
        public Vector2 regionSize = Vector2.one;
        public int rejectionSamples = 30;
        public float displayRadius =1;

        private List<Vector2> _points;

        void OnValidate() {
            _points = PoissonDiscSampling.GeneratePoints(radius, regionSize, rejectionSamples);
        }

        private void OnDrawGizmos() {
            Gizmos.DrawWireCube(regionSize/2,regionSize);
            if (_points == null) return;
            
            foreach (var point in _points) {

                Gizmos.DrawWireSphere(point, displayRadius);
            }
        }
    }
}
