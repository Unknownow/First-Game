using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour {

    EnemyManager thisEnemy;
    float pushedBackCountdown;

    public float moveSpeed = 2f;
    public float minDistance = 3f;

    public Transform weapon;
    public Transform player;
    public LayerMask whatIsPlayer;
    public float attackRange;
    public int damage;

    float timeBetweenAttack;
    public float startTimeBetweenAttack = 1f;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weapon = transform.GetChild(0).transform;
        thisEnemy = gameObject.GetComponent<EnemyManager>();
        pushedBackCountdown = thisEnemy.pushedBackDuration;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!thisEnemy.isPushedBack)
        {
            Vector2 difference = player.position - transform.position;
            float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);


            float temp = Vector2.Distance(player.position, transform.position);

            if (temp > minDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            else if (temp <= minDistance)
            {
                transform.position = this.transform.position;
            }

            if (timeBetweenAttack < 0 && temp <= minDistance)
            {
                Collider2D[] playerHit = Physics2D.OverlapCircleAll(weapon.position, attackRange, whatIsPlayer);
                foreach (Collider2D i in playerHit)
                {
                    i.GetComponent<PlayerManager>().takeDamage(damage);
                }
                timeBetweenAttack = startTimeBetweenAttack;
            }
            else if (timeBetweenAttack >= 0)
            {
                timeBetweenAttack -= Time.deltaTime;
            }
        }
        else
        {
            if (pushedBackCountdown > 0)
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weapon.position, attackRange);
    }
}
