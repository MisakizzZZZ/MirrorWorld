using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorDoor : MirrorInteratable
{

    private Quaternion originalRotation;
    private bool hasSolved = false; //这关是否已经过了？如果过了、则不需要再检测门的是否可见、门将永远保持常开

    //设置已经通过的外部接口
    public void SetSolved()
    {
        hasSolved = true;
        this.transform.rotation = objectInMirror.transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        originalRotation = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //在没通过此谜题时才进行刷新
        if(!hasSolved)
        {
            base.Update();
            if (isVisibleInAnyMirror && !isVisibleInPlayerCamera)
            {
                this.transform.rotation = objectInMirror.transform.rotation;
            }
            else
            {
                this.transform.rotation = originalRotation;
            }
        }
    }
}
