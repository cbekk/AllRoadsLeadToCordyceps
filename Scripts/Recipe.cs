using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Recipe", menuName = "Game/Recipe")]
public class Recipe : ScriptableObject
{
    [Header("Recipe Info")]
    public string recipeName;

    [TextArea(3, 6)]
    public string description;

    [Header("Ingredients")]
    public List<string> ingredients = new List<string>();

    [Header("Optional")]
    public Sprite recipeImage;
    public int difficultyLevel = 1;
    public float timeLimit = 300f; // Time in seconds to complete
}
