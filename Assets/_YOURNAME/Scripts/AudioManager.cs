using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<AudioScriptableObject> sounds;
    private Dictionary<string, AudioScriptableObject> soundLookup;
    private List<AudioSource> activeSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            soundLookup = new Dictionary<string, AudioScriptableObject>();
            foreach (var s in sounds)
            {
                soundLookup[s.name] = s;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(string name)
    {
        if (!soundLookup.TryGetValue(name, out var s))
        {
            Debug.LogWarning($"Sound '{name}' not found!");
            return;
        }

        var src = gameObject.AddComponent<AudioSource>();
        src.clip = s.clip;
        src.volume = s.volume;
        src.pitch = s.pitch;
        src.loop = s.loop;
        src.outputAudioMixerGroup = s.mixerGroup;
        src.Play();

        if (!s.loop)
        {
            Destroy(src, s.clip.length);
        }
        activeSources.Add(src);
    }

    public void StopSound(string name)
    {
        foreach (var src in activeSources)
        {
            if (src != null && src.clip != null && src.clip.name == name)
            {
                Destroy(src);
                break;
            }
        }
    }

    private void OnEnable()
    {
        AudioEvents.OnPlaySound += PlaySound;
        AudioEvents.OnStopSound += StopSound;
    }

    private void OnDisable()
    {
        AudioEvents.OnPlaySound -= PlaySound;
        AudioEvents.OnStopSound -= StopSound;
    }
}
