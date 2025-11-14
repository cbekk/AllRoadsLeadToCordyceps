using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class IngredientData
{
    public GameObject prefab;
    public Vector3 originalPosition;
    public Quaternion originalRotation;
    public Transform originalParent;
}

public class Wok : MonoBehaviour
{
    public Transform ingredientPlacePt;
    private List<GameObject> ingredients = new List<GameObject>();
    private List<IngredientData> ingredientDataList = new List<IngredientData>();

    public int reqIngredients = 3;
    public GameObject finishedDishPrefab;

    void Start()
    {
        if (ingredientPlacePt == null)
        {
            GameObject placePtObj = new GameObject("IngredientPlacePt");
            ingredientPlacePt = placePtObj.transform;
            ingredientPlacePt.SetParent(transform);
            ingredientPlacePt.localPosition = new Vector3(0, .5f, 0);
        }
    }

    public void AddIngredient(GameObject ingredient)
    {
        Debug.Log("Added " + ingredient.name);

        IngredientData data = new IngredientData();
        data.prefab = ingredient;
        data.originalPosition = ingredient.transform.position;
        data.originalRotation = ingredient.transform.rotation;
        data.originalParent = ingredient.transform.parent;

        Debug.Log($"[AddIngredient] {ingredient.name} original position: {data.originalPosition}, rotation: {data.originalRotation}");

        ingredientDataList.Add(data);
        ingredients.Add(ingredient);

        ingredient.transform.SetParent(transform);
        float offset = ingredients.Count * .1f;
        ingredient.transform.position = ingredientPlacePt.position + new Vector3(offset, 0, 0);

        Rigidbody rb = ingredient.GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = true;

        if (ingredients.Count >= reqIngredients)
            CompleteRecipe();
    }

    private void CompleteRecipe()
    {
        Debug.Log("Recipe complete");

        foreach (GameObject ingredient in ingredients)
            ingredient.SetActive(false);

        if (finishedDishPrefab != null)
        {
            GameObject finishedDish = Instantiate(finishedDishPrefab, ingredientPlacePt.position, Quaternion.identity);
            finishedDish.transform.SetParent(transform);

            finishedDish.tag = "Item";

            if (finishedDish.GetComponent<GetItem>() == null)
                finishedDish.AddComponent<GetItem>();

            FinishedDish dishScript = finishedDish.GetComponent<FinishedDish>();
            if (dishScript == null)
                dishScript = finishedDish.AddComponent<FinishedDish>();

            dishScript.SetParentWok(this);
        }
        else
        {
            Debug.Log("No finished dish prefab");
        }
    }

    public void RespawnIngredients()
    {
        Debug.Log("Respawning ingredients to og positions");

        foreach (IngredientData data in ingredientDataList)
        {
            if (data.prefab != null)
            {
                data.prefab.transform.position = data.originalPosition;
                data.prefab.transform.rotation = data.originalRotation;
                data.prefab.SetActive(true);

                Rigidbody rb = data.prefab.GetComponent<Rigidbody>();
                if (rb != null)
                    rb.isKinematic = false;

                data.prefab.SetActive(true);
            }
            Debug.Log($"[RespawnIngredients] {data.prefab.name} respawning at: {data.prefab.transform.position}, rotation: {data.prefab.transform.rotation}");

        }


        ingredients.Clear();
        ingredientDataList.Clear();
    }

    public void RecordOriginalPosition(GameObject ingredient)
    {
        if (ingredientDataList.Exists(d => d.prefab == ingredient))
            return;

        IngredientData data = new IngredientData();
        data.prefab = ingredient;
        data.originalPosition = ingredient.transform.position;
        data.originalRotation = ingredient.transform.rotation;
        data.originalParent = ingredient.transform.parent;

        ingredientDataList.Add(data);

        Debug.Log($"[RecordOriginalPosition] {ingredient.name} position recorded at: {data.originalPosition}");
    }
}
