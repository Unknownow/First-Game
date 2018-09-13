using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour {

    public float projectileSpeed = 2f;
    public float damage = 1f;
	
	void Update () {
        transform.Translate(Vector2.up * projectileSpeed * Time.deltaTime);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyManager>().takeDamage(damage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}
