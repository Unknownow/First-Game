using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileController : MonoBehaviour {

    public float projectileSpeed = 2f;
    public float damage = 1f;
    public float knockBackRange = 3f;

    
	
	void Update () {
        transform.Translate(Vector2.up * projectileSpeed * Time.deltaTime);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.GetComponent<EnemyManager>().takeDamage(damage);
            Vector2 temp = collision.transform.position - transform.position;
            float alpha = Mathf.Atan2(temp.y, temp.x);
            temp = new Vector2(-temp.x / Mathf.Abs(temp.x) * knockBackRange * Mathf.Cos(alpha) , -temp.y / Mathf.Abs(temp.y) * knockBackRange * Mathf.Sin(alpha));
            collision.attachedRigidbody.AddForce(temp,ForceMode2D.Impulse);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
        else if(collision.CompareTag("Boss"))
        {
            collision.GetComponent<BossManager>().takeDamage(damage);
            Destroy(gameObject);
        }
    }
}
