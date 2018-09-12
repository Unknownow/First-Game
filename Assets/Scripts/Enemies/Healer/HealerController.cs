﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealerController : MonoBehaviour
{
    public float moveSpeed = 2f;

    public Transform player;


    Collider2D healingCircle;
    public LayerMask whatIsEnemy;


    public float minDistance = 3f;
    public float retreatDistance = 2f;
    public float healingRange = 4f;
    public float HpHealed = 1f;
    public float timeBtwHealing;
    public float startTimeBtwHealing = 1;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        healingCircle = GetComponent<CircleCollider2D>();
        timeBtwHealing = startTimeBtwHealing;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 difference = player.position - transform.position;
        float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);

        float temp = Vector2.Distance(player.position, transform.position);

        if (temp >= minDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }
        else if (temp > retreatDistance && temp < minDistance)
        {
            transform.position = this.transform.position;
        }
        else if (temp <= retreatDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
        }

        if (timeBtwHealing <= 0)
        {
            Collider2D[] alliesHealed = Physics2D.OverlapCircleAll(transform.position, healingRange, whatIsEnemy);
            foreach (Collider2D i in alliesHealed)
            {
                i.GetComponent<EnemyManager>().healing(HpHealed);
            }
            timeBtwHealing = startTimeBtwHealing;
        }
        else
            timeBtwHealing -= Time.deltaTime;
        

    }



    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, healingRange);
    }
}
