using UnityEngine;

namespace MatrixAI
{
    public class MatrixChaseRig : MonoBehaviour
    {
        [Tooltip("The transform driven by Cinemachine that we should follow.")]
        public Transform targetAnchor;

        [Tooltip("Smoothly lerp rotation to avoid jarring snaps?")]
        public bool smoothRotation = true;
        public float rotationLerpSpeed = 5f;

        private void LateUpdate()
        {
            if (targetAnchor == null) return;

            // Move the XR Origin root to match the anchor
            transform.position = targetAnchor.position;

            if (smoothRotation)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, targetAnchor.rotation, rotationLerpSpeed * Time.deltaTime);
            }
            else
            {
                transform.rotation = targetAnchor.rotation;
            }
        }
    }
}