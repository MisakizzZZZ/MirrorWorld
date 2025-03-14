using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public float interactDistance = 3f;  // 交互距离

    protected float interactDistanceSqr;
    protected bool isLookingAt = false;    // 是否被主摄像机注视
    protected bool isInteractable = false; // 是否在交互范围内


    public virtual void Start()
    {
        interactDistanceSqr = interactDistance* interactDistance;
    }


    public virtual void Update()
    {
        CheckInteractable();
        HandleInteraction();
    }

    public virtual void Interact()
    {
        Debug.Log("与 " + gameObject.name + " 进行了交互！");
    }

    protected void CheckInteractable()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        // 计算物体与摄像机的距离
        Vector3 temp = transform.position - mainCamera.transform.position;
        temp.y = 0;
        float distanceSqr = Vector3.SqrMagnitude(temp);
        isInteractable = distanceSqr <= interactDistanceSqr;

        // 使用射线检测摄像机是否正对物体
        if(isInteractable)
        {
            Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
            RaycastHit hit;
            isLookingAt = Physics.Raycast(ray, out hit, interactDistance) && hit.transform == transform;
        }
        else
        {
            isLookingAt = false;
        }
    }

    private void HandleInteraction()
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

        //TODO 暂时写死交互键为E
        if (isInteractable && isLookingAt && Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    protected void SetHighlight(bool highlight)
    {
        if(GetComponent<HighlightComponent>())
        {
            GetComponent<HighlightComponent>().SetActivateHighlight(highlight);
        }
    }

}
