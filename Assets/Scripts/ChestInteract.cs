using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestInteract : InteractableObject
{
    public ChestPasswordGUI passwordUI;
    public override void Interact()
    {
        Debug.Log("与箱子进行了互动");

        if (passwordUI != null)
        {
            passwordUI.ShowPanel();
        }
        else
        {
            Debug.LogWarning("未设置密码UI引用！");
        }
    }
}
