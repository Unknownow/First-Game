using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectileController : MonoBehaviour {


    public float projectileSpeed = 2f;
    public float damage = 1f;
    public float slowPercentage = 0.5f;
    public float slowDuration = 2f;

    void Update()
    {
        transform.Translate(Vector2.up * projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerManager>().takeDamage(damage);
            collision.GetComponent<PlayerController>().slow(slowPercentage, slowDuration);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Border"))
        {
            Destroy(gameObject);
        }
    }
}
