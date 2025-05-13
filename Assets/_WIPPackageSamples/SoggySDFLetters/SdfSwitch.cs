using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class SdfSwitch : MonoBehaviour
{
    public VisualEffect visualEffect;
    public List<Texture3D> sdfTextures; // A-Z, index 0 = A
    public string sdfPropertyName = "SDF";

    private Dictionary<char, Texture3D> sdfMap;

    void Awake()
    {
        sdfMap = new Dictionary<char, Texture3D>();

        if (sdfTextures.Count != 26)
        {
            Debug.LogError("sdfTextures must contain exactly 26 Texture3D entries, one for each letter A-Z.");
            return;
        }

        for (int i = 0; i < 26; i++)
        {
            sdfMap[(char)('A' + i)] = sdfTextures[i];
        }
    }

    public void SetSDFByLetter(char c)
    {
        char upper = char.ToUpper(c);
        if (sdfMap.TryGetValue(upper, out var tex))
        {
            visualEffect.SetTexture(sdfPropertyName, tex);
        }
        else
        {
            Debug.LogWarning($"No SDF mapped for character '{c}'");
        }
    }
}
