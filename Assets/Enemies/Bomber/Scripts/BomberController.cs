using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberController : MonoBehaviour
{

    EnemyManager thisEnemy;
    float pushedBackCountdown;

    public float moveSpeed = 2f;

    public Transform player;

    public LayerMask whatIsPlayer;


    public float bombTime = 1.5f;
    public bool isDetonated = false;
    public float detonateRange = 2f;
    public float damageRange = 3f;
    public int damage;


    bool isSlow = false;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisEnemy = gameObject.GetComponent<EnemyManager>();
        pushedBackCountdown = thisEnemy.pushedBackDuration;
        isSlow = false;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isSlow && thisEnemy.isSlow)
        {
            isSlow = true;
            moveSpeed *= (1 - thisEnemy.slowPercentage);
        }
        if (isSlow && thisEnemy.slowDuration > 0)
        {
            thisEnemy.slowDuration -= Time.deltaTime;
        }
        else if (isSlow && thisEnemy.slowDuration <= 0)
        {
            isSlow = false;
            thisEnemy.isSlow = false;
            moveSpeed /= (1 - thisEnemy.slowPercentage);
        }

        if (!thisEnemy.isPushedBack)
        {
            if (!isDetonated)
            {
                

                Vector2 difference = player.position - transform.position;
                float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);


                float temp = Vector2.Distance(player.position, transform.position);
                if (temp > detonateRange)
                {
                    transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
                }
                else if (temp <= detonateRange)
                {
                    isDetonated = true;
                }
            }

            else
            {
                if (bombTime <= 0)
                    detonate(damage);
                else
                    bombTime -= Time.deltaTime;
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

    void detonate(float damage)
    {
        Collider2D playerHit = Physics2D.OverlapCircle(transform.position, damageRange, whatIsPlayer);
        if (playerHit != null)
            playerHit.GetComponent<PlayerManager>().takeDamage(damage);
        gameObject.GetComponent<EnemyManager>().takeDamage(gameObject.GetComponent<EnemyManager>().maxHealth);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detonateRange);
    }

}
