using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcePillarController : MonoBehaviour
{
    public float damage = 1f;
    public float slowPercentage = 0.5f;
    public float slowDuration = 2f;
    public float duration;
    public float delayTime;
    float originalDuration;

    private void Start()
    {
        originalDuration = duration;
    }

    void FixedUpdate()
    {
        if (duration <= 0)
            Destroy(gameObject);
        else
            duration -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(duration < originalDuration -delayTime)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<PlayerManager>().takeDamage(damage);
                collision.GetComponent<PlayerController>().slow(slowPercentage, slowDuration);
            }
            if (collision.CompareTag("Border"))
            {
                Destroy(gameObject);
            }
        }   
    }
}
