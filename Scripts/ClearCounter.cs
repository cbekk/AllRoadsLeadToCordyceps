using UnityEngine;

public class ClearCounter : MonoBehaviour //, IKitchenObjectParent
{
    public GameObject objectOnCounter;
    public bool HasObject()
    {
        return objectOnCounter != null;
    }
    public GameObject GetObject()
    {
        GameObject obj = objectOnCounter;
        objectOnCounter = null;
        return obj;
    }
}
