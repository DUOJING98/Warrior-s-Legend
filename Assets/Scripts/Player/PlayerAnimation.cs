using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private PhysicsCheck physicsCheck;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        physicsCheck = GetComponent<PhysicsCheck>();
        playerController = GetComponent<PlayerController>();
    }
    private void Update()
    {
        SetAnimation();
    }
    public void SetAnimation()
    {
        animator.SetFloat("velocityX",Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("velocityY",rb.linearVelocity.y);
        animator.SetBool("isGround",physicsCheck.isGround);
        animator.SetBool("isCrouch",playerController.isCrouch);
    }
}
