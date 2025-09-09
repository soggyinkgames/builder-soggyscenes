using UnityEngine;
using UnityEngine.VFX;

public class SDFManager : MonoBehaviour
{
    [SerializeField] private VisualEffect vfx;
    [SerializeField] private Texture3D[] sdfTextures; // or Texture2D if your SDFs are 2D slices
    [SerializeField] private string triggerEventName = "OnSDFSwitch";

    private int currentSDF;

    public void SwitchSDF(int sdfIndex)
    {
        if (sdfIndex < 0 || sdfIndex >= sdfTextures.Length) return;

        currentSDF = sdfIndex;
        vfx.SetTexture("SDF_Texture", sdfTextures[currentSDF]);
        // vfx.SetInt("SDF_ID", currentSDF); // optional, if you also want the graph to know which
        // Trigger the VFX event
        vfx.SendEvent(triggerEventName);
    }
}
