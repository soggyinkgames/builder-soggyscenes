using UnityEngine;


[RequireComponent(typeof(Collider))]
public class RevealPath : MonoBehaviour
{
    [Header("Material Settings")]
    [Tooltip("The material whose float variable will be modified.")]
    public Material targetMaterial;

    [Tooltip("The name of the float variable in the shader.")]
    public string floatVariableName = "_MyFloat";

    [Header("Animation Settings")]
    [Tooltip("Value to start animating from.")]
    public float startValue = -10f;

    [Tooltip("Value to animate to.")]
    public float endValue = 10f;

    [Tooltip("Duration of the animation in seconds.")]
    public float animationDuration = 4f;

    private bool isAnimating = false;

    private void OnTriggerEnter(Collider other) //todo: IAN- change this code to implement a different event trigger that calls AnimateFloatChange()
    {
        // Ensure only the player triggers the effect and avoid multiple triggers
        if (isAnimating || !other.CompareTag("Player") || targetMaterial == null) return;

        // todo: IAN- add a conditional that checks for another event eg, accurately signing their name (event --- unlockReveal())
        StartCoroutine(AnimateFloatChange()); 
    }

    private System.Collections.IEnumerator AnimateFloatChange()
    {
        isAnimating = true;

        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            // Calculate the normalized time (0 to 1) and interpolate the float value
            float t = elapsedTime / animationDuration;
            float currentValue = Mathf.Lerp(startValue, endValue, t);
            targetMaterial.SetFloat(floatVariableName, currentValue);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final value is set
        targetMaterial.SetFloat(floatVariableName, endValue);

        isAnimating = false;
    }
}
