using UnityEngine;
using TMPro; // Import for TMP_InputField

public class Again : MonoBehaviour
{
    [Tooltip("The Visual Effect prefab to instantiate when text is entered.")]
    public GameObject visualEffectPrefab; // Assign this in the Inspector with a visual effect prefab

    [Tooltip("Reference to the TMP Input Field to retrieve the input text.")]
    public TMP_InputField inputField; // Assign this in the Inspector with the TMP_InputField component

    // Start is called before the first frame update
    void Start()
    {
        if (inputField != null)
        {
            // Subscribe to the onEndEdit event to handle text input
            inputField.onEndEdit.AddListener(OnTextSubmitted);
        }
        else
        {
            Debug.LogError("TMP_InputField is not assigned!");
        }
    }

    // Called when text input is finished
    private void OnTextSubmitted(string finalText)
    {
        if (string.IsNullOrEmpty(finalText))
        {
            Debug.LogWarning("Input text is empty. No visual effect will be triggered.");
            return;
        }

        Debug.Log("Text Submitted: " + finalText);

        // Trigger visual effect for the submitted text
        TriggerVisualEffect(finalText);
    }

    // Instantiate and trigger a visual effect based on the input text
    private void TriggerVisualEffect(string text)
    {
        if (visualEffectPrefab != null)
        {
            // Example: Instantiate the visual effect at the position of the input field
            Vector3 effectPosition = inputField.transform.position + new Vector3(0, 0, -1); // Adjust as needed

            // Instantiate the visual effect prefab
            Instantiate(visualEffectPrefab, effectPosition, Quaternion.identity);

            // Additional logic to use the text value, if needed
            Debug.Log("Visual effect triggered for text: " + text);
        }
        else
        {
            Debug.LogError("No visual effect prefab assigned.");
        }
    }

    // Clean up listeners when the object is destroyed
    private void OnDestroy()
    {
        if (inputField != null)
        {
            inputField.onEndEdit.RemoveListener(OnTextSubmitted);
        }
    }
}
