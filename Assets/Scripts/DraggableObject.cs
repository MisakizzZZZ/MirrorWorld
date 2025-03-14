using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : InteractableObject
{

    public Transform player;
    public float followSpeed = 5f;
    public float dragDistanceFromPlayer = 1f;
    public float rotationSpeed = 90f;
    public LayerMask collisionLayers;
    
    private Rigidbody rb;
    private bool isDragging = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        rb.isKinematic = true;
    }

    // Update is called once per frame
    public override void Update()
    {
        CheckInteractable();

        // Press E to switch between dragging / not dragging
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            if (isDragging)
            {
                Debug.Log("Not dragging" + gameObject.name);
                EndDragging();
            }
            else
            {
                if (isLookingAt)
                {
                    Debug.Log("Dragging" + gameObject.name);
                    StartDragging();
                }
            }
        }

        // Move object / Look at object
        Interact();
    }
    
    public override void Interact()
    {
        if (isDragging)
        {
            if (player == null)
                return;

            Vector3 targetPosition = player.position + player.forward * dragDistanceFromPlayer;
            Vector3 direction = targetPosition - transform.position;

            Vector3 desiredVelocity = direction.normalized * followSpeed;
            rb.velocity = desiredVelocity;

            // Rotate object
            // Left mouse button = clockwise
            if (Input.GetMouseButton(0))
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
            }
            // Right mouse button = counter-clockwise
            if (Input.GetMouseButton(1))
            {
                transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
            }

            // If player is out of range after moving, stop dragging
            if (player != null)
            {
                Vector3 temp = transform.position - player.position;
                temp.y = 0;
                float distanceToPlayerSqr = Vector3.SqrMagnitude(temp);
                if (distanceToPlayerSqr > interactDistanceSqr)
                {
                    EndDragging();
                    Debug.Log("Out of range. Not dragging" + gameObject.name);
                }
            }
        }
        else
        {
            // 当物体在交互范围内且被看着，则进入高亮状态
            if (isInteractable && isLookingAt)
            {
                SetHighlight(true);
            }
            else
            {
                SetHighlight(false);
            }
        }
        

    }

    private void StartDragging()
    {
        SetHighlight(true);
        rb.isKinematic = false;
        isDragging = true;
    }

    private void EndDragging()
    {
        SetHighlight(false);
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        isDragging = false;
    }
}
