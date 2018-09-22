using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    public float health = 3f;
    bool isInvul = false;

    public float invulTime;
    float invulCooldown;
    public Slider healthBar;

    private void Start()
    {
        isInvul = false;
        invulCooldown = invulTime;
        healthBar.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate () {
        healthBar.value = health;
        if (health <= 0)
        {
            healthBar.gameObject.SetActive(false);
            Destroy(gameObject);
        }
        else if(isInvul)
        {
            invulCooldown -= Time.deltaTime;
            if(invulCooldown <= 0)
            {
                invulCooldown = invulTime;
                isInvul = false;
            }
        }
	}

    public void takeDamage(float damage)
    {
        if(!isInvul)
        {
            health -= damage;
            isInvul = true;
        } 
    }
}
