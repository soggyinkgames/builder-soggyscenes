using UnityEngine;
using UnityEngine.Events;

public class HandsTouchingEvent : MonoBehaviour
{
    [Header("References")]
    public Transform leftHandArea; // First object
    public Transform rightHandArea; // Second object

    [Header("Distance Settings")]
    [Tooltip("Edge-to-edge gap allowed. Set 0 to trigger on overlap/touching. " +
             "Effective condition is: centerDistance <= (leftRadius + rightRadius + triggerDistance).")]
    [Min(0f)]
    [SerializeField] float triggerDistance = 0.002f;

    [Tooltip("If true, compute radii from colliders; otherwise use the manual radii below.")]
    [SerializeField] bool autoRadiiFromColliders = true;

    [Tooltip("Manual radius (meters) for left/right when autoRadiiFromColliders is false, or as fallback if no collider exists.")]
    [Min(0f)][SerializeField] float leftRadius = 0.003f;
    [Min(0f)][SerializeField] float rightRadius = 0.003f;

    [Header("Events")]
    public UnityEvent OnWithinDistance; // Editable in Inspector
    public UnityEvent OnOutsideDistance; // Called when outside distance


    private bool isInside = false;
    private bool active = false; // controlled externally - static hand gesture performed

    public void Activate()
    {
        active = true;
        isInside = false; // reset state
    }

    public void Deactivate()
    {
        active = false;
        isInside = false;
    }

    void Update()
    {
        if (!active) return;
        if (leftHandArea == null || rightHandArea == null) return;

        // Get effective radii (auto from colliders or manual)
        float rA = GetEffectiveRadius(leftHandArea, leftRadius);
        float rB = GetEffectiveRadius(rightHandArea, rightRadius);

        // Threshold is (rA + rB + triggerDistance); compare squared to avoid sqrt
        float threshold = rA + rB + triggerDistance;
        float thresholdSqr = threshold * threshold;

        Vector3 delta = leftHandArea.position - rightHandArea.position;
        float centerDistSqr = delta.sqrMagnitude;

        bool nowInside = centerDistSqr <= thresholdSqr;

        if (nowInside)
        {
            if (!isInside)
            {
                OnWithinDistance?.Invoke(); // Enter
                isInside = true;
            }
        }
        else
        {
            if (isInside)
            {
                OnOutsideDistance?.Invoke(); // Exit
                isInside = false;
            }
        }
    }


    float GetEffectiveRadius(Transform t, float fallback)
    {
        if (!autoRadiiFromColliders || t == null)
            return fallback;

        // Prefer SphereCollider if present (scale by the largest axis)
        var sphere = t.GetComponent<SphereCollider>();
        if (sphere != null)
        {
            float maxAxis = Mathf.Max(t.lossyScale.x, Mathf.Max(t.lossyScale.y, t.lossyScale.z));
            return Mathf.Max(0f, sphere.radius * maxAxis);
        }

        // Otherwise approximate using any collider's bounding sphere
        var col = t.GetComponent<Collider>();
        if (col != null)
        {
            // bounds.extents.magnitude ≈ radius of a sphere that encloses the collider
            return Mathf.Max(0f, col.bounds.extents.magnitude);
        }

        // Fallback to manual value
        return Mathf.Max(0f, fallback);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (leftHandArea == null || rightHandArea == null) return;

        float rA = GetEffectiveRadius(leftHandArea, leftRadius);
        float rB = GetEffectiveRadius(rightHandArea, rightRadius);

        // Draw the two "personal space" spheres
        Gizmos.matrix = Matrix4x4.identity;
        Gizmos.DrawWireSphere(leftHandArea.position, rA);
        Gizmos.DrawWireSphere(rightHandArea.position, rB);

        // For visualizing the additional allowed gap, draw a ring around left (optional)
        // This isn't a perfect ring, but helps reason about the threshold.
        Gizmos.DrawWireSphere(leftHandArea.position, rA + triggerDistance);
    }
#endif

}

