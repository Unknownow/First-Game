using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public float maxHealth = 3f;
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
    }
    public void takeDamage(float damage)
    {
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
