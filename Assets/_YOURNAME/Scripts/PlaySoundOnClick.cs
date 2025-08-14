using UnityEngine;

public class PlaySoundOnClick : MonoBehaviour
{
    [SerializeField] private string soundName = "gesture-recognised"; // Must match AudioManager's sound list

    public void PlaySound()
    {
        AudioEvents.OnPlaySound?.Invoke(soundName);
    }
}