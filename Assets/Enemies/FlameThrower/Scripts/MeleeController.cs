using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeController : MonoBehaviour {

    [Space]
    [Header("Others")]
    EnemyManager thisEnemy;
    float pushedBackCountdown;
    Animator flameThrowerAnimator;

    [Space]
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float minDistance = 3f;

    [Space]
    [Header("Attacking")]
    public Transform gunPoint;
    public Transform weapon;
    public Transform player;
    public LayerMask whatIsPlayer;
    public float attackRange;
    public int damage;
    float timeBetweenAttack;
    public float startTimeBetweenAttack = 1f;
    public Transform[] flame;


    //slow
    float slowDuration;
    float slowPercentage;


    float stunDuration;
    float stunPercentage;

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        weapon = transform.GetChild(0).transform;
        thisEnemy = gameObject.GetComponent<EnemyManager>();
        pushedBackCountdown = thisEnemy.pushedBackDuration;
        flameThrowerAnimator = GetComponent<Animator>();
        gunPoint = transform.GetChild(1).transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (thisEnemy.isStun && stunDuration > 0)
        {
            stunDuration -= Time.deltaTime;
            return;
        }
        else if (thisEnemy.isStun && stunDuration <= 0)
        {
            thisEnemy.isStun = false;
        }


        if (!thisEnemy.isPushedBack)
        {
            if (thisEnemy.isSlow && slowDuration > 0)
            {
                slowDuration -= Time.deltaTime;
                return;
            }
            else if (thisEnemy.isSlow && slowDuration <= 0)
            {
                thisEnemy.isSlow = false;
                moveSpeed /= slowPercentage;
            }


            Vector2 difference = player.position - transform.position;
            float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);


            float temp = Vector2.Distance(player.position, transform.position);

            if (temp > minDistance)
            {
                flameThrowerAnimator.SetBool("isRunning", true);
                transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
            }
            else if (temp <= minDistance)
            {
                flameThrowerAnimator.SetBool("isRunning", false);
                transform.position = this.transform.position;
            }

            if (timeBetweenAttack < 0 && temp <= minDistance)
            {
                flameGun();
                Collider2D[] playerHit = Physics2D.OverlapCircleAll(gunPoint.position, attackRange, whatIsPlayer);
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


    void flameGun()
    {
        int i = Random.Range(0, 3);
        flameClone(flame[i]);
        return;
    }

    void flameClone(Transform fuzzle)
    {
        GameObject clone;
        clone = Instantiate(fuzzle.gameObject, gunPoint.position, transform.rotation);
        clone.transform.parent = transform;
        float size = Random.Range(0.8f, 1f);
        clone.transform.localScale = new Vector3(size, size, size);
        Destroy(clone, .06f);
        return;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gunPoint.position, attackRange);
    }

    public void slow(float slowPercentage, float slowDuration)
    {
        if (!thisEnemy.isSlow)
        {
            moveSpeed *= slowPercentage;
        }
        thisEnemy.isSlow = true;
        this.slowDuration = slowDuration;
        this.slowPercentage = slowPercentage;
    }

    public void stun(float stunPercentage, float stunDuration)
    {
        thisEnemy.isStun = true;
        this.stunDuration = stunDuration;
        this.stunPercentage = stunPercentage;
    }
}
