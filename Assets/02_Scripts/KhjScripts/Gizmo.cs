using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace khj{
    public class Gizmo : MonoBehaviour
    {
        public Color color = Color.white;
        public float size = 0.1f;

        private void OnDrawGizmos()
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(transform.position, size);
        }
    }
}