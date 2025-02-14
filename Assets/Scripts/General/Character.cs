using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("»ùµA¥Ñ¥é¥á©`¥¿")]
    public float maxHealth;
    public float currentHealth;
    [Header("¥À¥á©`¥¸Ÿo„¿•rég")]
    public float invulnerableDuration;
    private float invulnerableCounter;
    private bool invulnerable;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    public void TakeDamage(Attack attack)
    {
        if (invulnerable)
            return;
        if (currentHealth - attack.damage > 0)
        {
            Debug.Log(attack.damage);
            currentHealth -= attack.damage;
            TriggerInvulnerable();
        }
        else
        {
            currentHealth = 0;
            //¤ä¤é¤ì¤¿
        }
    }
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
}
