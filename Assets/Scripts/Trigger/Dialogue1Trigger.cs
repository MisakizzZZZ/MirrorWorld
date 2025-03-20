using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue1Trigger : AutoTrigger
{
    private bool triggered = false;

    public override void OnTriggerEnter(Collider other)
    {
        if(!triggered)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                triggered = true;
                UIManager.Instance.ShowSubtitle("Huh...? The fan is spinning in the mirror...? No way...");
            }
        }
    }

}
