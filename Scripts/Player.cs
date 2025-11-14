using UnityEngine;
using cakeslice;

public class Player : MonoBehaviour
{
    private float moveSpeed = 7f;
    private GameObject heldObject;
    public Transform playerCamera;
    public Transform holdPoint;

    private float verticalCameraAngle = 0f;
    private GameObject currentHighlight;
    private Outline currentOutline;

    void Start()
    {
        if (holdPoint == null)
        {
            GameObject holdPointObj = new GameObject("HoldPoint");
            holdPoint = holdPointObj.transform;
            holdPoint.SetParent(transform);
            holdPoint.localPosition = new Vector3(.12f, .3f, 2f);
        }
    }

    void Update()
    {
        handleMovement();
        UpdateHighlight();
        handleInteractions();
    }

    private void UpdateHighlight()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float interactDistance = 100f;

        if (Physics.Raycast(ray, out RaycastHit raycastHit, interactDistance))
        {
            float distanceToHit = Vector3.Distance(transform.position, raycastHit.point);
            float maxInteractDistance = 5f;

            if (distanceToHit <= maxInteractDistance)
            {
                GameObject hitObject = raycastHit.collider.gameObject;

                if (!hitObject.CompareTag("Item") && !hitObject.CompareTag("Wok"))
                {
                    if (raycastHit.collider.transform.parent != null)
                        hitObject = raycastHit.collider.transform.parent.gameObject;
                }

                if ((raycastHit.collider.CompareTag("Item") && heldObject == null) ||
                    (raycastHit.collider.CompareTag("Wok") && heldObject != null))
                {
                    if (currentHighlight != hitObject)
                    {
                        ClearHighlight();
                        HighlightObject(hitObject);
                    }
                    return;
                }
            }
        }

        ClearHighlight();
    }

    private void HighlightObject(GameObject obj)
    {
        currentHighlight = obj;
        currentOutline = obj.GetComponent<Outline>();

        if (currentOutline == null)
        {
            currentOutline = obj.AddComponent<Outline>();
        }

        currentOutline.color = 0;
        currentOutline.enabled = true;
    }

    private void ClearHighlight()
    {
        if (currentOutline != null)
        {
            currentOutline.enabled = false;
            currentOutline = null;
        }
        currentHighlight = null;
    }

    private void handleInteractions()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float interactDistance = 100f;

            if (Physics.Raycast(ray, out RaycastHit raycastHit, interactDistance))
            {
                Debug.Log("Mouse raycast hit: " + raycastHit.collider.name + " Tag: " + raycastHit.collider.tag);

                float distanceToHit = Vector3.Distance(transform.position, raycastHit.point);
                float maxInteractDistance = 5f;

                if (distanceToHit > maxInteractDistance)
                {
                    Debug.Log("Too far away distance is: " + distanceToHit);
                    return;
                }

                if (raycastHit.collider.CompareTag("Item") && heldObject == null)
                {
                    GetItem item = raycastHit.collider.GetComponent<GetItem>();

                    if (item == null)
                    {
                        item = raycastHit.collider.GetComponentInParent<GetItem>();
                    }

                    if (item == null)
                    {
                        item = raycastHit.collider.GetComponentInChildren<GetItem>();
                    }

                    if (item != null && item.HasObject())
                    {
                        FinishedDish dish = raycastHit.collider.GetComponent<FinishedDish>();
                        if (dish == null)
                        {
                            dish = raycastHit.collider.GetComponentInParent<FinishedDish>();
                        }

                        if (dish != null)
                        {
                            Debug.Log("Eating");
                            dish.TakeBite();
                            ClearHighlight();
                        }
                        else
                        {
                            PickupObject(item.GetObject());
                            ClearHighlight();
                        }
                    }
                }
                else if (raycastHit.collider.CompareTag("Wok") && heldObject != null)
                {
                    Debug.Log("Detected Wok");
                    Wok wok = raycastHit.collider.GetComponent<Wok>();

                    if (wok == null)
                    {
                        wok = raycastHit.collider.GetComponentInParent<Wok>();
                    }

                    if (wok == null)
                    {
                        wok = raycastHit.collider.GetComponentInChildren<Wok>();
                    }

                    if (wok != null)
                    {
                        Debug.Log("Adding to wok");
                        GameObject AddedIng = heldObject;
                        heldObject = null;
                        wok.AddIngredient(AddedIng);
                    }
                }
                else if (raycastHit.collider.CompareTag("Counter") && heldObject != null)
                {
                    Debug.Log("Hit Counter checking for wok");
                    Wok wok = raycastHit.collider.GetComponent<Wok>();

                    if (wok == null)
                    {
                        wok = raycastHit.collider.GetComponentInParent<Wok>();
                    }

                    if (wok == null)
                    {
                        wok = raycastHit.collider.GetComponentInChildren<Wok>();
                    }

                    if (wok != null)
                    {
                        Debug.Log("Found wok adding ingredient");
                        GameObject AddedIng = heldObject;
                        heldObject = null;
                        wok.AddIngredient(AddedIng);
                    }
                }
            }
        }
    }

    private void PickupObject(GameObject obj)
    {
        heldObject = obj;

        Rigidbody rb = heldObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        heldObject.transform.SetParent(holdPoint);
        heldObject.transform.localPosition = Vector3.zero;
        heldObject.transform.localRotation = Quaternion.identity;
    }

    private void handleMovement()
    {
        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = +1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = +1;
        }

        inputVector = inputVector.normalized;

        Vector3 moveDir = (playerCamera.forward * inputVector.y + playerCamera.right * inputVector.x);
        moveDir.y = 0f;

        float playerSize = .7f;
        bool canMove = !Physics.Raycast(transform.position, moveDir, playerSize);

        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        RotateWMouse();

        float rotateSpeed = 1.5f;
        if (inputVector.y > 0 || inputVector.x != 0)
        {
            Vector3 lookDir = new Vector3(moveDir.x, 0f, moveDir.z);
            transform.forward = Vector3.Slerp(transform.forward, lookDir, Time.deltaTime * rotateSpeed);
        }
    }

    private void RotateWMouse()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up * mouseX * 3f);

        verticalCameraAngle -= mouseY * 3f;
        verticalCameraAngle = Mathf.Clamp(verticalCameraAngle, -45f, 45f);

        playerCamera.localRotation = Quaternion.Euler(verticalCameraAngle, 0f, 0f);
    }
}