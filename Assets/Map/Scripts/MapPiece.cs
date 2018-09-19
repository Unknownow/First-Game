using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiece : MonoBehaviour {

    [Header("Others")]
    public int weaponNumber = 1;
    public bool isPlayerStanding = false;

    [Space]
    [Header("Map Health")]
    public int maxHealth = 20;
    public int currentHealth;
    public float healthRate = 1;
    float healthCountdown;
    public int healthPerSecond = 1;
    

    private void Start()
    {
        currentHealth = maxHealth;
        healthCountdown = healthRate;
    }


    private void FixedUpdate()
    {
        if(isPlayerStanding)
        {
            if(healthCountdown < 0)
            {
                currentHealth -= healthPerSecond;
                healthCountdown = healthRate;
            }
            else
            {
                healthCountdown -= Time.deltaTime;
            }
            if (currentHealth <= 0)
                Destroy(gameObject);
        }
        else
        {
            if (healthCountdown < 0)
            {
                if(currentHealth + healthPerSecond <= maxHealth)
                {
                    currentHealth += healthPerSecond;
                    healthCountdown = healthRate;
                }       
            }
            else
            {
                healthCountdown -= Time.deltaTime;
            }
        }
    }
}
