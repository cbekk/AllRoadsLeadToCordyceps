using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class StickyNote : MonoBehaviour
{
    [Header("Text References")]
    [SerializeField] private TextMeshPro recipeNameText;
    [SerializeField] private TextMeshPro ingredientsText;

    [Header("Visual Settings")]
    [SerializeField] private Color completedColor = Color.green;
    [SerializeField] private Color incompleteColor = Color.white;
    [SerializeField] private string strikethroughFormat = "<s>{0}</s>"; // Strikethrough for completed items

    [Header("Optional")]
    [SerializeField] private Renderer stickyNoteRenderer; // For changing sticky note color
    [SerializeField] private Material completedMaterial;

    private Recipe currentRecipe;
    private HashSet<string> collectedIngredients = new HashSet<string>();

    void Start()
    {
        // Subscribe to recipe changes
        RecipeManager recipeManager = FindFirstObjectByType<RecipeManager>();
        if (recipeManager != null && recipeManager.GetCurrentRecipe() != null)
        {
            DisplayRecipe(recipeManager.GetCurrentRecipe());
        }
    }

    public void DisplayRecipe(Recipe recipe)
    {
        if (recipe == null)
        {
            Debug.LogWarning("Cannot display null recipe!");
            return;
        }

        currentRecipe = recipe;
        collectedIngredients.Clear();

        UpdateStickyNote();
    }

    public void MarkIngredientCollected(string ingredientName)
    {
        if (currentRecipe == null)
        {
            Debug.LogWarning("No recipe assigned to sticky note!");
            return;
        }

        // Check if ingredient is in current recipe
        if (currentRecipe.ingredients.Contains(ingredientName))
        {
            collectedIngredients.Add(ingredientName);
            Debug.Log($"Collected ingredient: {ingredientName}");

            UpdateStickyNote();

            // Check if recipe is complete
            if (IsRecipeComplete())
            {
                OnRecipeCompleted();
            }
        }
        else
        {
            Debug.LogWarning($"{ingredientName} is not part of the current recipe!");
        }
    }

    public void UpdateStickyNote()
    {
        if (currentRecipe == null) return;

        // Update recipe name
        if (recipeNameText != null)
        {
            recipeNameText.text = currentRecipe.recipeName;
        }

        // Update ingredients list with strikethrough for collected items
        if (ingredientsText != null)
        {
            string ingredientsList = "Ingredients:\n\n";

            foreach (string ingredient in currentRecipe.ingredients)
            {
                bool isCollected = collectedIngredients.Contains(ingredient);

                if (isCollected)
                {
                    // Strikethrough and color for collected ingredients
                    ingredientsList += string.Format(strikethroughFormat,
                        $"<color=#{ColorUtility.ToHtmlStringRGB(completedColor)}>✓ {ingredient}</color>") + "\n";
                }
                else
                {
                    // Normal text for uncollected ingredients
                    ingredientsList += $"○ {ingredient}\n";
                }
            }

            ingredientsText.text = ingredientsList;
        }
    }

    public bool IsRecipeComplete()
    {
        if (currentRecipe == null) return false;

        foreach (string ingredient in currentRecipe.ingredients)
        {
            if (!collectedIngredients.Contains(ingredient))
            {
                return false;
            }
        }

        return true;
    }

    public void OnRecipeCompleted()
    {
        Debug.Log($"Recipe '{currentRecipe.recipeName}' completed!");

        // Change sticky note appearance
        if (stickyNoteRenderer != null && completedMaterial != null)
        {
            stickyNoteRenderer.material = completedMaterial;
        }

        // You can add more effects here:
        // - Play a sound
        // - Spawn particles
        // - Trigger an event

        // Optionally load next recipe after a delay
        Invoke(nameof(LoadNextRecipe), 3f);
    }

    void LoadNextRecipe()
    {
        RecipeManager recipeManager = FindFirstObjectByType<RecipeManager>();
        if (recipeManager != null)
        {
            Recipe newRecipe = recipeManager.SelectNewRecipe();
            DisplayRecipe(newRecipe);

            // Reset sticky note appearance
            if (stickyNoteRenderer != null && stickyNoteRenderer.material != completedMaterial)
            {
                // Reset to original material (you'd need to store this)
            }
        }
    }

    public Recipe GetCurrentRecipe()
    {
        return currentRecipe;
    }

    public bool HasIngredient(string ingredientName)
    {
        return collectedIngredients.Contains(ingredientName);
    }

    public int GetCollectedCount()
    {
        return collectedIngredients.Count;
    }

    public int GetTotalIngredients()
    {
        return currentRecipe != null ? currentRecipe.ingredients.Count : 0;
    }
}