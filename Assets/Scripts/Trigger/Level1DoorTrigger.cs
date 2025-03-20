using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1DoorTrigger : AutoTrigger
{
    public MirrorDoor mirrorDoor1;
    public MirrorDoor mirrorDoor2;


    public override void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            mirrorDoor1.SetSolved();
            mirrorDoor2.SetSolved();
        }

    }

}
