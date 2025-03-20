using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : InteractableObject
{
    public override void Interact()
    {
        if (KeyInteract.hasGainedKey)
        {
            Debug.Log("与门进行了互动");
            UIManager.Instance.ShowSubtitle("Door opened!");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("没有钥匙，无法打开门");
            UIManager.Instance.ShowSubtitle("You need a key to open the door!");
        }
    }
}
