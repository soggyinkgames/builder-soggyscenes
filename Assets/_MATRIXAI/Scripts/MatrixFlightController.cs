using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace MatrixAI
{
    [RequireComponent(typeof(Rigidbody))]
    public class MatrixFlightController : MonoBehaviour
    {
        public enum FlightMode { Autopilot, Manual }

        [Header("Settings")]
        public FlightMode mode = FlightMode.Autopilot;
        public float cruiseSpeed = 50f;
        public float maxSpeed = 100f;
        public float turnSpeed = 2f;
        public float pitchSpeed = 1.5f;
        public float rollSpeed = 45f;
        public float bankAmount = 30f;
        public float altitudeOffset = 50f;

        [Header("Input Actions")]
        public InputActionReference toggleModeAction;
        public InputActionReference pitchAction; // Right Stick Y
        public InputActionReference rollAction;  // Right Stick X
        public InputActionReference throttleAction; // Left Stick Y

        [Header("Autopilot")]
        public List<Vector3> waypoints = new List<Vector3>();
        public int currentWaypointIndex = 0;
        public float waypointRadius = 50f;

        private Rigidbody rb;
        private float currentThrottle = 0.5f;
        private float targetRoll = 0f;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.useGravity = false;

            // Initialize waypoints from plan if empty
            if (waypoints.Count == 0)
            {
                waypoints.Add(new Vector3(4533, 80, 6359));
                waypoints.Add(new Vector3(6333, 100, 8159));
                waypoints.Add(new Vector3(8133, 200, 9959));
                waypoints.Add(new Vector3(8133, 70, 11759));
                waypoints.Add(new Vector3(9933, 106, 13559));
                waypoints.Add(new Vector3(9933, 116, 15359));
                waypoints.Add(new Vector3(9933, 125, 17159));
            }
        }

        private void OnEnable()
        {
            if (toggleModeAction != null) toggleModeAction.action.performed += OnToggleMode;
        }

        private void OnDisable()
        {
            if (toggleModeAction != null) toggleModeAction.action.performed -= OnToggleMode;
        }

        private void OnToggleMode(InputAction.CallbackContext context)
        {
            mode = (mode == FlightMode.Autopilot) ? FlightMode.Manual : FlightMode.Autopilot;
            Debug.Log($"Flight Mode Toggled: {mode}");
        }

        private void FixedUpdate()
        {
            if (mode == FlightMode.Manual)
            {
                HandleManualFlight();
            }
            else
            {
                HandleAutopilot();
            }

            ApplyFlightPhysics();
        }

        private void HandleManualFlight()
        {
            float pitchInput = pitchAction != null ? pitchAction.action.ReadValue<Vector2>().y : 0f;
            float rollInput = rollAction != null ? rollAction.action.ReadValue<Vector2>().x : 0f;
            float throttleInput = throttleAction != null ? throttleAction.action.ReadValue<Vector2>().y : 0f;

            // Update throttle
            currentThrottle = Mathf.Clamp01(currentThrottle + throttleInput * Time.fixedDeltaTime);
            
            // Rotation
            float pitch = pitchInput * pitchSpeed;
            float roll = -rollInput * rollSpeed;
            float yaw = rollInput * turnSpeed;

            rb.MoveRotation(rb.rotation * Quaternion.Euler(pitch, yaw, roll));
            targetRoll = -rollInput * bankAmount;
        }

        private void HandleAutopilot()
        {
            if (waypoints.Count == 0) return;

            Vector3 targetPos = waypoints[currentWaypointIndex];
            
            // Terrain height adjustment
            if (Terrain.activeTerrain != null)
            {
                float terrainHeight = Terrain.activeTerrain.SampleHeight(transform.position);
                targetPos.y = Mathf.Max(targetPos.y, terrainHeight + altitudeOffset);
            }

            Vector3 direction = (targetPos - transform.position).normalized;
            if (direction == Vector3.zero) direction = transform.forward;

            Quaternion targetRotation = Quaternion.LookRotation(direction);
            
            // Calculate bank based on turn
            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
            targetRoll = Mathf.Clamp(angle, -bankAmount, bankAmount);
            targetRotation *= Quaternion.Euler(0, 0, -targetRoll);

            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));

            // Move to next waypoint
            if (Vector3.Distance(transform.position, targetPos) < waypointRadius)
            {
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Count;
            }

            currentThrottle = 0.5f; // Constant cruise
        }

        private void ApplyFlightPhysics()
        {
            float speed = Mathf.Lerp(0, maxSpeed, currentThrottle);
            rb.linearVelocity = transform.forward * speed;

            // Visual bank smoothing
            Vector3 angles = transform.eulerAngles;
            float currentRoll = angles.z;
            if (currentRoll > 180) currentRoll -= 360;
            float lerpedRoll = Mathf.Lerp(currentRoll, targetRoll, 5f * Time.fixedDeltaTime);
            transform.eulerAngles = new Vector3(angles.x, angles.y, lerpedRoll);
        }
    }
}