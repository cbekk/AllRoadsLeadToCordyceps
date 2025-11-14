using UnityEngine;
using System.Collections.Generic;

public class RecipeManager : MonoBehaviour
{
    [Header("Recipe Database")]
    [SerializeField] private List<Recipe> availableRecipes = new List<Recipe>();

    [Header("Settings")]
    [SerializeField] private bool selectOnStart = true;
    [SerializeField] private bool avoidRepeats = true;

    private Recipe currentRecipe;
    private List<Recipe> usedRecipes = new List<Recipe>();

    // Singleton pattern (optional, but useful)
    public static RecipeManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (selectOnStart && availableRecipes.Count > 0)
        {
            SelectRandomRecipe();
        }
    }

    public Recipe SelectRandomRecipe()
    {
        if (availableRecipes.Count == 0)
        {
            Debug.LogWarning("No recipes available!");
            return null;
        }

        List<Recipe> selectableRecipes = new List<Recipe>(availableRecipes);

        // Remove used recipes if avoiding repeats
        if (avoidRepeats && usedRecipes.Count < availableRecipes.Count)
        {
            foreach (Recipe used in usedRecipes)
            {
                selectableRecipes.Remove(used);
            }
        }
        else if (avoidRepeats && usedRecipes.Count >= availableRecipes.Count)
        {
            // All recipes used, reset the list
            usedRecipes.Clear();
        }

        // Select random recipe
        int randomIndex = Random.Range(0, selectableRecipes.Count);
        currentRecipe = selectableRecipes[randomIndex];

        // Track used recipe
        if (avoidRepeats)
        {
            usedRecipes.Add(currentRecipe);
        }

        Debug.Log($"Selected recipe: {currentRecipe.recipeName}");

        // Notify 3D sticky note to update
        StickyNote stickyNote = FindFirstObjectByType<StickyNote>();
        if (stickyNote != null)
        {
            stickyNote.DisplayRecipe(currentRecipe);
        }

        return currentRecipe;
    }

    public Recipe GetCurrentRecipe()
    {
        return currentRecipe;
    }

    public Recipe SelectNewRecipe()
    {
        return SelectRandomRecipe();
    }

    // Optional: Select recipe by difficulty
    public Recipe SelectRecipeByDifficulty(int difficulty)
    {
        List<Recipe> matchingRecipes = availableRecipes.FindAll(r => r.difficultyLevel == difficulty);

        if (matchingRecipes.Count == 0)
        {
            Debug.LogWarning($"No recipes found with difficulty {difficulty}");
            return SelectRandomRecipe(); // Fallback to random
        }

        int randomIndex = Random.Range(0, matchingRecipes.Count);
        currentRecipe = matchingRecipes[randomIndex];

        // Notify 3D sticky note to update
        StickyNote stickyNote = FindFirstObjectByType<StickyNote>();
        if (stickyNote != null)
        {
            stickyNote.DisplayRecipe(currentRecipe);
        }

        return currentRecipe;
    }
}