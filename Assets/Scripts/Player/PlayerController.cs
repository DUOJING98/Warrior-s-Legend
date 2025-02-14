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

    [Header("基本")]
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
        //元のサイズを取得
        originalOffset = capsuleCollider.offset;
        originalSize = capsuleCollider.size;
        //元のスビートを取得
        runspeed = Speed;

        //歩く
        #region
        action.Player.Jump.started += Jump;
        //ｃｔｘ＝CallbackContext
        action.Player.Sprint.performed += ctx =>
        {
            //ボタンを押している間、歩く
            if (physicsCheck.isGround)
                Speed = walkspeed;
        };
        action.Player.Sprint.canceled += ctx =>
        {
            //ボタンが離れたとき、走る
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
        //移動
        if (!isCrouch)
            rb.linearVelocity = new Vector2(inputDirection.x * Speed * Time.deltaTime, rb.linearVelocity.y);
        //反転
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
            faceDir = 1;
        if (inputDirection.x < 0)
            faceDir = -1;
        transform.localScale = new Vector3(faceDir, 1, 1);

        //しゃがむ
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //コライダーのサイズを小さくする
            capsuleCollider.offset = new Vector2(-0.05f, 0.85f);
            capsuleCollider.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //コライダーのサイズを戻る
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
