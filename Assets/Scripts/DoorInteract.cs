using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteract : InteractableObject
{
    public override void Interact()
    {
        if (KeyInteract.hasGainedKey)
        {
            Debug.Log("���Ž����˻���");
            UIManager.Instance.ShowSubtitle("Door opened!");
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("û��Կ�ף��޷�����");
            UIManager.Instance.ShowSubtitle("You need a key to open the door!");
        }
    }
}
