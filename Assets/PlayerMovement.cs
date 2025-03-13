using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("移动速度")]
    public float speed = 12f;

    public Camera mainCamera;
    public CharacterController characterController;

    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        characterController= GetComponentInChildren<CharacterController>();
    }


    void Update()
    {
        movement();
    }
    void movement()
    {
        //x跟z轴移动：
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //根据角色的朝向进行基于x轴与z轴的移动   

   
        characterController.Move(move * speed * Time.deltaTime);
    }

}