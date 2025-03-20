using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class KeyInteract : InteractableObject
{
    public static bool hasGainedKey = false;

    public override void Start()
    {
        base.Start();
        gameObject.SetActive(false);
    }
    public override void Interact()
    {
        Debug.Log("��Կ�׽����˻���");
        hasGainedKey = true;
        UIManager.Instance.ShowSubtitle("Item \"Door Key\" earned!");
        gameObject.SetActive(false);
    }

}
