using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public float maxHealth = 3f;
    float currentHealth;
    public bool isPushedBack = false;
    public float pushedBackDuration = .7f;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void takeDamage(float damage)
    {
        isPushedBack = true;
        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void healing(float hp)
    {
        if(currentHealth < maxHealth)
            currentHealth += hp;
    }
}
