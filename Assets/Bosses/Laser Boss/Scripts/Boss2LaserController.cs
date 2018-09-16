using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2LaserController : MonoBehaviour {

	public float damage = 1f;
	public float laserFireDuration = 0.1f;

	void Update () {
		if (laserFireDuration < 0)
			Destroy(gameObject);
		else
			laserFireDuration -= Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            collision.GetComponent<PlayerManager>().takeDamage(damage);
    }
}
