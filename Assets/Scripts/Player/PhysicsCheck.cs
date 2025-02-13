using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    public bool isGround;
    public float checkRaduis;
    public LayerMask groundLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Check();
    }

    public void Check()
    {
        isGround = Physics2D.OverlapCircle(transform.position, checkRaduis, groundLayer);
    }
}
