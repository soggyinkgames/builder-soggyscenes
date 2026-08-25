using UnityEngine;

namespace MatrixAI
{
    /// <summary>
    /// Ensures the camera far-clip plane is set to the desired value at Awake.
    /// Used to override the XR Rig prefab's default far-clip (5000) so the full
    /// 18 000-unit terrain is visible without distant culling.
    /// </summary>
    [ExecuteAlways]
    [RequireComponent(typeof(Camera))]
    [DefaultExecutionOrder(-200)]
    public class CameraFarClipOverride : MonoBehaviour
    {
        [Header("Camera Clip Distances")]
        [Tooltip("Far clip plane to enforce on the attached Camera. Applied in both edit mode and Play mode.")]
        [SerializeField] private float _farClipPlane = 25000f;

        private void Awake()      => Apply();
        private void OnEnable()   => Apply();
        private void OnValidate() => Apply();

        private void Apply()
        {
            Camera cam = GetComponent<Camera>();
            if (cam != null)
                cam.farClipPlane = _farClipPlane;
        }
    }
}
