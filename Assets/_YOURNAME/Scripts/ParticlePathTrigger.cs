using UnityEngine;
using UnityEngine.Events;

public class ParticlePathTrigger : MonoBehaviour
{
    [Header("References")]
    public ParticleSystem particleEffect;  // Particle system to play on trigger
    public Collider triggerCollider;       // Assign any collider in the inspector
    public GameObject player;              // Player GameObject

    [Header("Settings")]
    public string playerTag = "Player";    // Tag used to identify the player

    [Header("Events")]
    public UnityEvent onPlayerEnter;       // Event invoked when player enters the trigger

    private void Start()
    {
        // Ensure particle effect is assigned
        if (particleEffect == null)
        {
            particleEffect = GetComponentInChildren<ParticleSystem>();
            if (particleEffect == null)
            {
                Debug.LogError("No Particle System assigned or found in children.");
            }
        }

        // Ensure collider is assigned
        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider>();
            if (triggerCollider == null)
            {
                Debug.LogError("No Collider assigned or found on the GameObject.");
                return;
            }
        }
        triggerCollider.isTrigger = true; // Ensure it is a trigger

        // Ensure player GameObject is assigned, fallback to finding by tag
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag(playerTag);
            if (player == null)
            {
                Debug.LogError("No player GameObject found with the specified tag.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that enters is the player
        if (other.gameObject == player)
        {
            // Play particle effect if assigned
            if (particleEffect != null)
            {
                particleEffect.Play();
            }

            // Invoke any other custom events tied to this trigger
            onPlayerEnter?.Invoke();
        }
    }
}
