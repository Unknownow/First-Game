using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    public float maxHealth = 3f;
    float currentHealth;
	public int mobNo;
	public GameObject boundary;

    [Space]
    [Header("Push Back")]
    public bool isPushedBack = false;
    public float pushedBackDuration = .7f;


    [Space]
    [Header("Slow")]
    public float slowDuration;
    public float slowPercentage;
    public bool isSlow = false;

    [Space]
    [Header("Stun")]
    public float stunDuration;
    public bool isStun = false;

  
    

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
			boundary.GetComponent<MapManager> ().mobDeath (mobNo);
            Destroy(gameObject);
        }
    }

    public void healing(float hp)
    {
        if (currentHealth + hp < maxHealth)
            currentHealth += hp;
        else
            currentHealth = maxHealth;
    }

    public void slow(float slowDuration, float slowPercentage)
    {
        if(!isSlow)
        {
            isSlow = true;
            this.slowPercentage = slowPercentage;
        }
        this.slowDuration = slowDuration;
        
    }

    public void stun(float stunDuration)
    {
        if (isStun) {
            this.stunDuration = stunDuration;
            isStun = true;
        }
    }
}
