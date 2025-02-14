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
    private CapsuleCollider2D capsuleCollider;

    [Header("����")]
    public float Speed;
    public float jumpForce;
    private float runspeed;
    private float walkspeed => Speed / 2.5f;
    public bool isCrouch;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        action = new InputSystem_Actions();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        //Ԫ�Υ�������ȡ��
        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;
        //Ԫ�Υ��ө`�Ȥ�ȡ��
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
        if (!isCrouch)
            rb.linearVelocity = new Vector2(inputDirection.x * Speed * Time.deltaTime, rb.linearVelocity.y);
        //��ܞ
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        transform.localScale = new Vector3(faceDir, 1, 1);

        //���㤬��
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //���饤���`�Υ�������С��������
            capsuleCollider.offset = new Vector2(-0.05f, 0.85f);
            capsuleCollider.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //���饤���`�Υ����������
            capsuleCollider.size = originalSize;
            capsuleCollider.offset = originalOffset;
        }

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
