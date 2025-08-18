using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PlaySoundOnTrigger : MonoBehaviour
{
    [SerializeField] private string soundName; // Must match the name in AudioManager's sounds list
    [SerializeField] private bool playOnce = true;

    private bool hasPlayed = false;

    private void Reset()
    {
        // Ensure the collider is set to trigger
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Only react to player (tag your player "Player" in Unity)
        if (!other.CompareTag("Player")) return;

        if (playOnce && hasPlayed) return;

        AudioEvents.OnPlaySound?.Invoke(soundName);

        hasPlayed = true;
    }
}