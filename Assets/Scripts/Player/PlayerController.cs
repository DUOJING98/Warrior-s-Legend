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
    private CapsuleCollider2D coll;
    private PlayerAnimation playerAnim;

    [Header("����")]
    public float Speed;
    public float jumpForce;
    private float runspeed;
    private float walkspeed => Speed / 2.5f;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("�ޥƥꥢ��")]
    public PhysicsMaterial2D Normal;
    public PhysicsMaterial2D Wall;
    [Header("״�B")]
    public bool isCrouch;
    public bool isHurt;
    public float hurtForce;
    public bool isDead;
    public bool isAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        action = new InputSystem_Actions();
        physicsCheck = GetComponent<PhysicsCheck>();
        coll = GetComponent<CapsuleCollider2D>();
        playerAnim = GetComponent<PlayerAnimation>();
        //Ԫ�Υ�������ȡ��
        originalOffset = coll.offset;
        originalSize = coll.size;
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

        //����
        action.Player.Attack.started += PlayerAttack;
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
        CheckState();
    }

    private void FixedUpdate()
    {
        if (!isHurt && !isAttack)
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
            coll.offset = new Vector2(-0.05f, 0.85f);
            coll.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //���饤���`�Υ����������
            coll.size = originalSize;
            coll.offset = originalOffset;
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

    private void PlayerAttack(InputAction.CallbackContext context)
    {
        if(!physicsCheck.isGround)
            return;
        playerAnim.PlayerAttack();
        isAttack = true;
    }

    #region UnityEvent
    public void GetHurt(Transform attack)
    {
        isHurt = true;
        rb.linearVelocity = Vector2.zero;
        Vector2 dir = new Vector2((transform.position.x - attack.position.x), 0).normalized;

        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }

    public void PlayerDead()
    {
        isDead = true;
        action.Player.Disable();
    }
    #endregion

    private void CheckState()
    {
        if (isDead)
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        else
            gameObject.layer = LayerMask.NameToLayer("Player");

        //�ޥƥꥢ��
        coll.sharedMaterial = physicsCheck.isGround ? Normal : Wall;
    }
}
