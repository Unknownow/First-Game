using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : MonoBehaviour
{
    EnemyManager thisEnemy;
    float pushedBackCountdown;


    public float moveSpeed = 2f;
    public float minDistance = 20f;
    public float retreatDistance = 10f;

    
    public float shootRate = 1f;
    public float shootCooldown;

    public Transform frontFace;

    public GameObject projectile;

    public Transform player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisEnemy = gameObject.GetComponent<EnemyManager>();
        frontFace = transform.GetChild(0).transform;
        shootCooldown = shootRate;
        pushedBackCountdown = thisEnemy.pushedBackDuration;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!thisEnemy.isPushedBack)
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

            if (shootCooldown <= 0)
            {
                Instantiate(projectile, frontFace.position, transform.rotation);
                shootCooldown = shootRate;
            }
            else
            {
                shootCooldown -= Time.deltaTime;
            }
        }

        else
        {
            if(pushedBackCountdown > 0)
            {
                pushedBackCountdown -= Time.deltaTime;
            }
            else
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                pushedBackCountdown = thisEnemy.pushedBackDuration;
                thisEnemy.isPushedBack = false;
            }
        }
    }
}
