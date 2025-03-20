using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : InteractableObject
{
    public float openYRotation = -90f; //门打开的角度是多少


    public override void Interact()
    {
        if (KeyInteract.hasGainedKey)
        {
            UIManager.Instance.ShowSubtitle("It's open!");
            gameObject.transform.eulerAngles = new Vector3(gameObject.transform.eulerAngles.x, openYRotation, gameObject.transform.eulerAngles.z);
            SetHighlight(false);
            this.enabled = false; //禁用脚本防止重复互动
        }
        else
        {
            UIManager.Instance.ShowSubtitle("I hope the key is nearby...");
        }
    }
}
