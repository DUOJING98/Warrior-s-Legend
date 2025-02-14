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

    [Header("基本")]
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
        rb.linearVelocity = new Vector2(inputDirection.x * Speed * Time.deltaTime, rb.linearVelocity.y);
        //反転
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
