using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 12f;
    public float mouseSensitivity = 100f;

    public Camera mainCamera;
    public CharacterController characterController;
    public Animator animator;

    private float animatorSpeed = 0f;

    //防止人物被顶飞
    private float originalYPosition;


    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; //在游戏中隐藏鼠标
        mainCamera = GetComponentInChildren<Camera>();
        characterController= GetComponentInChildren<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        originalYPosition = this.transform.position.y;
    }


    void Update()
    {
        Movement();
        CameraRotate();
        SetAnimator();
        
    }
    void Movement()
    {
        //x跟z轴移动：
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z; //根据角色的朝向进行基于x轴与z轴的移动   

        animatorSpeed = move.magnitude;
        characterController.Move(move * speed * Time.deltaTime);

        //防止人物被顶飞
        this.transform.position = new Vector3(this.transform.position.x, originalYPosition, this.transform.position.z);
    }

    private float xRotation = 0f;
    void CameraRotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        this.transform.Rotate(Vector3.up * mouseX);
    }


    private void SetAnimator()
    {
        animator.SetFloat("Speed", animatorSpeed);
    }

}