using UnityEngine;
using System.Collections;

public class ShowTheWay : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer; // Assign the MeshRenderer in the inspector
    [SerializeField] private string materialPropertyName = "_CutOffHeight"; // Name of the material property
    [SerializeField] private float transitionDuration = 2.0f; // Duration for the transition
    [SerializeField] private float startValue = -3f; // Starting value of the property
    [SerializeField] private float targetValue = 3f; // Target value of the property
    [SerializeField] private string triggeringTag = "Player"; // Tag that triggers the change

    private Material material; // Cached material reference

    private void Start()
    {
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer is not assigned!");
            return;
        }

        // Cache the material reference and initialize the property to the start value
        material = meshRenderer.material;
        material.SetFloat(materialPropertyName, startValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(triggeringTag))
        {
            StartCoroutine(ChangeCutoffHeight());
        }
    }

    private IEnumerator ChangeCutoffHeight()
    {
        float elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            // Lerp the property value over time
            float newValue = Mathf.Lerp(startValue, targetValue, elapsedTime / transitionDuration);
            material.SetFloat(materialPropertyName, newValue);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the final value is exactly the target
        material.SetFloat(materialPropertyName, targetValue);
    }
}
