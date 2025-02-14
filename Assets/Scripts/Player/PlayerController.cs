using System;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputSystem_Actions action;
    private PhysicsCheck physicsCheck;
    public Vector2 inputDirection;

    [Header("����")]
    public float Speed;
    public float jumpForce;
    private float runspeed;
    private float walkspeed => Speed / 2.5f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        action = new InputSystem_Actions();
        physicsCheck = GetComponent<PhysicsCheck>();
        runspeed = Speed;
        //�i��
        #region
        action.Player.Jump.started += Jump;
        //�������CallbackContext
        action.Player.Sprint.performed += ctx =>
        {
            //�ܥ����Ѻ���Ƥ����g���i��
            if (physicsCheck.isGround)
                Speed = walkspeed;
        };
        action.Player.Sprint.canceled += ctx =>
        {
            //�ܥ����x�줿�Ȥ����ߤ�
            if (physicsCheck.isGround)
                Speed = runspeed;
        };
        #endregion
    }



    private void OnEnable()
    {
        action.Enable();
    }

    private void OnDisable()
    {
        action.Disable();
    }

    private void Update()
    {
        inputDirection = action.Player.Move.ReadValue<Vector2>();
    }
    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //�Ƅ�
        rb.linearVelocity = new Vector2(inputDirection.x * Speed * Time.deltaTime, rb.linearVelocity.y);
        //��ܞ
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        transform.localScale = new Vector3(faceDir, 1, 1);
    }
    private void Jump(InputAction.CallbackContext context)
    {
        //Debug.Log("JUMP");
        if (physicsCheck.isGround)
        {
            rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);

        }
    }



}
