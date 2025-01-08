using UnityEngine;

public class RemoveObjectsOnTrigger : MonoBehaviour
{
    // Array to hold references to removable GameObjects, exposed in the Inspector
    [SerializeField] private GameObject[] removableObjects;

    // This method is called when another collider enters the trigger collider on this GameObject
    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider belongs to the Player
        if (other.CompareTag("Player"))
        {
            RemoveObjects();
        }
    }

    // Method to remove the specified GameObjects
    private void RemoveObjects()
    {
        foreach (GameObject obj in removableObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }

}
