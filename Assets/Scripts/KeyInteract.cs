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
        hasGainedKey = true;
        UIManager.Instance.ShowSubtitle("I got the key!");
        gameObject.SetActive(false);
    }

}
