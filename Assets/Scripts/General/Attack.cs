using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("»ùµA¥Ñ¥é¥á©`¥¿")]
    public int damage;
    public float attackRange;
    public float attackRate;

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.GetComponent<Character>()?.TakeDamage(this);
    }


}
