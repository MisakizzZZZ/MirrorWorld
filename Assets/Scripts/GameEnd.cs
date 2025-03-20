using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : InteractableObject
{
    public override void Interact()
    {
        UIManager.Instance.ShowEndGame();
    }
}
