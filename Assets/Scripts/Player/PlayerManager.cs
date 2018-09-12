using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public float health = 3f;

	
	// Update is called once per frame
	void FixedUpdate () {
		if(health <= 0)
        {
            Destroy(gameObject);
        }
	}

    public void takeDamage(float damage)
    {
        health -= damage;
    }
}
