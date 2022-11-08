using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    int myHP = 100;
    public HealthBar healthBar;
    int damageAmount = 25;
    public Action OnDie = delegate { };

    // getting attack from the enemies
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "projectile")
        {
            TakeDamage(damageAmount);
        }
    }

    void TakeDamage(int damage)
    {
        myHP -= damage;
        if (myHP <= 0)
            Die();
    }

    void Die()
    {
        myHP = 0;
        healthBar.health = myHP / 100f;
        OnDie();
    }

}
