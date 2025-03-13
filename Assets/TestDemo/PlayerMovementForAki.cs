using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementForAki : MonoBehaviour
{
    [Header("移动速度")]
    public float speed = 1f;

    [Header("重力加速度")]
    public float gravity = -9.81f;

    [Header("地面检测")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask GroundMask;

    [Header("跳跃高度")]
    public float jumpHeight = 3f;

    private CharacterController controller;

    private Vector3 velocity;
    private bool isGround;
    private Animator playerAni;

    float RotationY = 0f;
    float RotationX = 0f;

    public float minmouseY = -45f;
    public float maxmouseY = 45f;
    public Transform agretctCamera;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerAni = GetComponent<Animator>();
        
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

        playerAni.SetFloat("Speed", move.magnitude);
        

        //Vector3 move =new  Vector3(x, 0f, z); //错误的赋值。
        /*这里赋值的的 vector3 是面对整个世界坐标系的，无论角色面朝任何方向，都只会在世界坐标系的x轴跟z轴上移动。*/

        controller.Move(move * speed * Time.deltaTime);


        RotationX += agretctCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 1;

        RotationY -= Input.GetAxis("Mouse Y") * 1;
        RotationY = Mathf.Clamp(RotationY, minmouseY, maxmouseY);

        this.transform.eulerAngles = new Vector3(0, RotationX, 0);

        agretctCamera.transform.eulerAngles = new Vector3(RotationY, RotationX, 0);

        /*
        //考虑重力的y轴移动：
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, GroundMask);

        if (isGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //跳跃
        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(-2 * jumpHeight * gravity); //v=sqrt（2gh）；
        }
        */
    }

}