using UnityEngine;
using TMPro;

// Optional: Use this if you want a "Press E to Pick Up" prompt when near ingredients
public class IngredientPrompt : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject promptPanel;
    [SerializeField] private TextMeshProUGUI promptText;

    [Header("Settings")]
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private string promptMessage = "Press E to pick up {0}";

    private Transform playerTransform;
    private Ingredient nearestIngredient;
    private float nearestDistance = float.MaxValue;

    void Start()
    {
        // Find player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Hide prompt initially
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Find all ingredients in scene
        Ingredient[] ingredients = FindObjectsByType<Ingredient>(FindObjectsSortMode.None);

        nearestIngredient = null;
        nearestDistance = float.MaxValue;

        // Find nearest ingredient within interaction distance
        foreach (Ingredient ingredient in ingredients)
        {
            float distance = Vector3.Distance(playerTransform.position, ingredient.transform.position);

            if (distance < interactionDistance && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestIngredient = ingredient;
            }
        }

        // Show/hide prompt based on nearest ingredient
        if (nearestIngredient != null)
        {
            ShowPrompt(nearestIngredient.GetIngredientName());

            // Handle pickup input
            if (Input.GetKeyDown(KeyCode.E))
            {
                nearestIngredient.PickUp();
                HidePrompt();
            }
        }
        else
        {
            HidePrompt();
        }
    }

    void ShowPrompt(string ingredientName)
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(true);

            if (promptText != null)
            {
                promptText.text = string.Format(promptMessage, ingredientName);
            }
        }
    }

    void HidePrompt()
    {
        if (promptPanel != null)
        {
            promptPanel.SetActive(false);
        }
    }
}
