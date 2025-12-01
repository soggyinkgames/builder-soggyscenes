using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "AudioScriptableObject", menuName = "Scriptable Objects/AudioScriptableObject")]
public class AudioScriptableObject : ScriptableObject
{
    public string id;
    public AudioClip clip;
    public AudioMixerGroup mixerGroup;
    [Range(0f, 1f)] public float volume = 1f;
    [Range(-3f, 3f)] public float pitch = 1f;
    public bool loop;
}
