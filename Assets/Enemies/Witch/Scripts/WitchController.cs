using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchController : MonoBehaviour
{

    public float moveSpeed = 2f;
    public float minDistance = 3f;

    public Transform weapon;
    public Transform player;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsBorder;


    bool isMad = false;
    bool isHit = false;
    public float dangerRange;
    public float attackRange;
    public int damage;

    float timeBetweenAttack;
    public float startTimeBetweenAttack = .5f;

    Animator witchAnimator;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weapon = transform.GetChild(0).transform;
        float temp = Vector2.Distance(player.position, transform.position);
        if (temp > dangerRange)
            isMad = false;
        else
            isMad = true;
        witchAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        float temp = Vector2.Distance(player.position, transform.position);
        if ( temp < dangerRange && isMad == false)
        {
            isMad = true;
        }

       
        if (!isMad)
        {
            Vector2 difference = player.position - transform.position;
            float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);
            
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            witchAnimator.SetTrigger("isRunning");
        }
        else
        {
            if(!isHit)
            {
                transform.Translate(Vector2.up * moveSpeed * 2 * Time.deltaTime);
                Collider2D hit = Physics2D.OverlapCircle(weapon.position, attackRange, whatIsPlayer);
                witchAnimator.SetTrigger("isMad");
                if (hit != null)
                {
                    isHit = true;
                    return;
                }
                Collider2D borderCheck = Physics2D.OverlapCircle(weapon.position, attackRange, whatIsBorder);
                if(borderCheck != null)
                {
                    Destroy(gameObject);
                }

            }
            else
            {
                witchAnimator.SetTrigger("isSlap");
                if (temp > minDistance)
                {
                    Vector2 difference = player.position - transform.position;
                    float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);
                    transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * 2 * Time.deltaTime);
                }
                else
                {
                    transform.position = this.transform.position;
                }
                
            }
            
            
            
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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(weapon.position, attackRange);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, dangerRange);
    }
}
