using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerUnityEvent : MonoBehaviour
{
    [SerializeField] public UnityEvent onTriggerEnter;
    [SerializeField] private bool playOnce = true;

    private bool hasTriggered = false;

    private void Reset()
    {
        // Ensure the collider is set as trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to player
        if (!other.CompareTag("Player")) return;

        if (playOnce && hasTriggered) return;

        onTriggerEnter?.Invoke();

        hasTriggered = true;
    }
}
