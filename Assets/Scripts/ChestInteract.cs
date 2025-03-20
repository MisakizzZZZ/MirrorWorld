using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteract : InteractableObject
{
    public override void Interact()
    {
        Debug.Log("与箱子进行了互动");
    }
}
