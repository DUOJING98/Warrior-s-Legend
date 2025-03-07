using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D coll;
    [Header("チェックパラメ�`タ")]
    public bool manual;
    public Vector2 bottomOffset;
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public float checkRaduis;
    public LayerMask groundLayer;
    [Header("彜�B")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coll = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            rightOffset = new Vector2((coll.bounds.size.x + coll.offset.x) / 2, coll.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    public void Check()
    {
        //仇中チェック
        isGround = Physics2D.OverlapCircle((Vector2)transform.position +
            new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y),
            checkRaduis, groundLayer);

        //謁チェック
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position +
            new Vector2(leftOffset.x * transform.localScale.x, leftOffset.y),
            checkRaduis, groundLayer);
        //touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position +
        //    new Vector2(rightOffset.x, rightOffset.y),
        //    checkRaduis, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        //仇中
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere((Vector2)transform.position +
            new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis);
        //謁
        Gizmos.DrawWireSphere((Vector2)transform.position +
            new Vector2(leftOffset.x * transform.localScale.x, leftOffset.y), checkRaduis);
        //Gizmos.DrawWireSphere((Vector2)transform.position +
        //    new Vector2(rightOffset.x, rightOffset.y), checkRaduis);

    }
}
