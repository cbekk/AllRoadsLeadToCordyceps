using UnityEngine;

public class FinishedDish : MonoBehaviour
{
    int bitesTaken = 0;
    int bitesNeeded = 3;
    private Vector3 ogScale;
    private Wok parentWok;

    void Start()
    {
        ogScale = transform.localScale;
    }

    public void SetParentWok(Wok wok)
    {
        parentWok = wok;
    }

    public void TakeBite()
    {
        bitesTaken++;
        float scaleMultiplier = 1f - (bitesTaken / (float)(bitesNeeded + 1));
        transform.localScale = ogScale * scaleMultiplier;

        if (bitesTaken >= bitesNeeded)
        {
            if (parentWok != null)
                parentWok.RespawnIngredients();

            Destroy(gameObject);
        }
    }
}
