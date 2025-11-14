using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [Header("Ingredient Info")]
    [SerializeField] private string ingredientName;

    [Header("Pickup Settings")]
    [SerializeField] private bool autoPickupOnTouch = true;
    [SerializeField] private string playerTag = "Player";
    [SerializeField] private float pickupDelay = 0.1f;

    [Header("Effects")]
    [SerializeField] private AudioClip pickupSound;
    [SerializeField] private GameObject pickupParticles;

    private bool isPickedUp = false;

    void OnTriggerEnter(Collider other)
    {
        if (autoPickupOnTouch && !isPickedUp && other.CompareTag(playerTag))
        {
            PickUp();
        }
    }

    public void PickUp()
    {
        if (isPickedUp) return;

        isPickedUp = true;

        // Find the sticky note and update it
        StickyNote stickyNote = FindFirstObjectByType<StickyNote>();
        if (stickyNote != null)
        {
            stickyNote.MarkIngredientCollected(ingredientName);
        }
        else
        {
            Debug.LogWarning("No StickyNote3D found in scene!");
        }

        // Play pickup effects
        if (pickupSound != null)
        {
            AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        }

        if (pickupParticles != null)
        {
            Instantiate(pickupParticles, transform.position, Quaternion.identity);
        }

        // Destroy the ingredient after a short delay
        Destroy(gameObject, pickupDelay);
    }

    // Alternative: Use this if you want a key press to pick up instead of auto-pickup
    void OnTriggerStay(Collider other)
    {
        if (!autoPickupOnTouch && !isPickedUp && other.CompareTag(playerTag))
        {
            // Show prompt to player (you'd need UI for this)
            if (Input.GetKeyDown(KeyCode.E))
            {
                PickUp();
            }
        }
    }

    public string GetIngredientName()
    {
        return ingredientName;
    }
}