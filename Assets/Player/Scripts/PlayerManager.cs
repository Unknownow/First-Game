using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public float health = 3f;
    bool isInvul = false;

    public float invulTime;
    float invulCooldown;

    private void Start()
    {
        isInvul = false;
        invulCooldown = invulTime;
    }

    // Update is called once per frame
    void FixedUpdate () {
		if(health <= 0)
        {
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
