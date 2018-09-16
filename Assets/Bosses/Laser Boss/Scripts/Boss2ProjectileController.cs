using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2ProjectileController : MonoBehaviour {

    public float projectileSpeed = 2f;
	public float disappearTime = 1.5f;
    public float damage = 1f;

	void Update () {
		if (projectileSpeed >= 0)
			transform.Translate (Vector2.up * projectileSpeed * Time.deltaTime);
		else 
		{
			if (disappearTime < 0)
				Destroy(gameObject);
			else
				disappearTime -= Time.deltaTime;
		}
		projectileSpeed -= Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().takeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}
