using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorDoor : MirrorInteratable
{

    private Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        originalRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        if(isVisibleInAnyMirror&&!isVisibleInPlayerCamera)
        {
            this.transform.rotation = objectInMirror.transform.rotation;
        }
        else
        {
            this.transform.rotation = originalRotation;
        }
    }
}
