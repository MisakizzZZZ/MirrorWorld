using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AkiTest : MonoBehaviour
{

    public Animator playerAni;



    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //根据角色的朝向进行基于x轴与z轴的移动   

        playerAni.SetFloat("Speed", move.magnitude > 1f ? 1f : move.magnitude);
    }
}
