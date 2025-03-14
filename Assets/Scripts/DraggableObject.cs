using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DraggableObject : InteractableObject
{

    public Transform player;
    public float followSpeed = 5f;
    public float dragDistanceFromPlayer = 0.5f;
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
        base.Update();
        // Press E to switch between dragging / not dragging
        if (isInteractable && isLookingAt && Input.GetKeyDown(KeyCode.E))
        {
            isDragging = !isDragging;
            if (isDragging)
            {
                Debug.Log("Dragging" + gameObject.name);
                StartDragging();
            }
            else
            {
                Debug.Log("Not dragging" + gameObject.name);
                EndDragging();
            }
        }

        // Move object
        if (isDragging)
        {
            MoveObject();
        }

        // If player is out of range after moving, stop dragging
        if (isDragging && player != null)
        {
            float distanceToPlayerSqr = Vector3.SqrMagnitude(transform.position - player.position);
            if (distanceToPlayerSqr > interactDistanceSqr)
            {
                EndDragging();
                Debug.Log("Out of range. Not dragging" + gameObject.name);
            }
        }
    }
    
    private void MoveObject()
    {
        if (player == null) return;

        Vector3 targetPosition = player.position + player.forward * dragDistanceFromPlayer;
        Vector3 direction = targetPosition - transform.position;

        Vector3 desiredVelocity = direction.normalized * followSpeed;
        rb.velocity = desiredVelocity;
    }

    private void StartDragging()
    {
        rb.isKinematic = false;
    }

    private void EndDragging()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
        isDragging = false;
    }
}
