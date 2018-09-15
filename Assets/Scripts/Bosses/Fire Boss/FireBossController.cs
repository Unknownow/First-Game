using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FireBossController : MonoBehaviour {

    [Header("Others")]
    Transform player;
    Transform bossWeapon;
    BossManager bossManager;
    public float bossRadius;
    public LayerMask whatToStop;
    public LayerMask whatIsPlayer;
    bool isPhase2 = false;

    [Space]
    [Header("Dashing")]
    public float dashSpeed;
    bool isDashing = false;
    public int dashRate = 5;
    public int dashRateIncrease;
    public float chargingTime = 1f;
    float chargingCountdown;

    [Space]
    [Header("Moving")]
    public float moveSpeed;
    Vector3 movePosition;
    public float minMoveDistance;
    public float movingRate = 5f;
    float movingCooldown;
    bool isMoving;

    [Space]
    [Header("Shotgun Config")]
    public float fireRate;
    public float fireRateCooldown;
    public GameObject shotgunProjectile;
    public float spreadRate;
    

    [Space]
    [Header("Flame Circle")]
    public float flameRadius;
    public float duration;
    float durationCountdown;
    public float flameDamage;
    public float circleRate;
    float circleCountdown;
    
    [Space]
    [Header("Flame Bomb")]
    public int bombRate;
    public GameObject flameBomb;
    float bombCountdown;
    float bombRateIncrease;
    float maxDistanceToPlayer;
    [HideInInspector]
    public Vector3 bombLocation;


    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossManager = gameObject.GetComponent<BossManager>();
        isDashing = false;
        dashRateIncrease = dashRate;
        movingCooldown = movingRate;
        bossWeapon = transform.GetChild(0).transform;
        chargingCountdown = chargingTime;
        durationCountdown = duration;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        

        if(!isPhase2)
        {
            phase2();
        }
        else
        {
            phase1();
        }
    }

    void phase1()
    {
        // check boss phase 2
        if (bossManager.checkPhase2() && !isPhase2)
        {
            isPhase2 = true;
            fireRate *= 2;
            dashSpeed *= 2;
            moveSpeed *= 2;
            spreadRate *= 2;
            return;
        }


        if (isDashing)
        {
            if(chargingCountdown > 0)
            {
                chargingCountdown -= Time.deltaTime;
                return;
            }
            dashing();
            return; 
        }


        lookAtObject(player.position);
        if(isMoving)
        {
            moving(); 
        }
        else
        {
            int rand;
            rand = Random.Range(0, dashRate);
            if (dashRateIncrease == 1)
            {
                rand = 1;
            }
            if (rand == 1)
            {
                isDashing = true;
                return;
            }
            dashRateIncrease -= 1;
        }
        
        // Moving 
        movingCooldown -= Time.deltaTime;
        if (movingCooldown <= 0 && isMoving == false)
        {
            isMoving = true;
            movePosition = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)), 0);
            float distance = Vector2.Distance(movePosition, transform.position);
            while (distance < minMoveDistance)
            {
                movePosition = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)), 0);
                distance = Vector2.Distance(movePosition, transform.position);
            }
        }

        // Shooting
        if(fireRateCooldown > 0)
        {
            fireRateCooldown -= Time.deltaTime;
        }
        else
        {
            lookAtObject(player.position);
            shot(spreadRate * 2);
            shot(spreadRate);
            shot(0);
            shot(-spreadRate);
            shot(-spreadRate * 2);
            fireRateCooldown = fireRate;
        }


        //Flame circle
        if(circleCountdown > 0)
        {
            circleCountdown -= Time.deltaTime;
        }
        else
        {
            if(durationCountdown > 0)
            {
                Collider2D playerHit = Physics2D.OverlapCircle(transform.position, flameRadius, whatIsPlayer);
                if (playerHit != null)
                {
                    playerHit.GetComponent<PlayerManager>().takeDamage(flameDamage);
                }
            }
            else
            {
                durationCountdown = duration;
                circleCountdown = circleRate;
            }
        }
    }

    void phase2()
    {
        if (isDashing)
        {
            if (chargingCountdown > 0)
            {
                chargingCountdown -= Time.deltaTime;
                return;
            }

            Collider2D playerHit = Physics2D.OverlapCircle(transform.position, flameRadius, whatIsPlayer);
            if (playerHit != null)
            {
                playerHit.GetComponent<PlayerManager>().takeDamage(flameDamage);
            }
            dashing();
            return;
        }


        lookAtObject(player.position);
        if (isMoving)
        {
            moving();
        }
        else
        {
            int rand;
            rand = Random.Range(0, dashRate);
            if (dashRateIncrease == 1)
            {
                rand = 1;
            }
            if (rand == 1)
            {
                isDashing = true;
                return;
            }
            dashRateIncrease -= 1;
        }

        // Moving 
        movingCooldown -= Time.deltaTime;
        if (movingCooldown <= 0 && isMoving == false)
        {
            isMoving = true;
            movePosition = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)), 0);
            float distance = Vector2.Distance(movePosition, transform.position);
            while (distance < minMoveDistance)
            {
                movePosition = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)), 0);
                distance = Vector2.Distance(movePosition, transform.position);
            }
        }

        // Shooting
        if (fireRateCooldown > 0)
        {
            fireRateCooldown -= Time.deltaTime;
        }
        else
        {
            lookAtObject(player.position);
            shot(spreadRate * 2);
            shot(spreadRate);
            shot(0);
            shot(-spreadRate);
            shot(-spreadRate * 2);

            shot(spreadRate * 2 + 45);
            shot(spreadRate + 45);
            shot(0 + 45);
            shot(-spreadRate + 45);
            shot(-spreadRate * 2 + 45);

            shot(spreadRate * 2 - 45);
            shot(spreadRate - 45);
            shot(0 - 45);
            shot(-spreadRate - 45);
            shot(-spreadRate * 2 - 45);


            fireRateCooldown = fireRate;
        }


        //Flame circle
        if (circleCountdown > 0)
        {
            circleCountdown -= Time.deltaTime;
        }
        else
        {
            if (durationCountdown > 0)
            {
                Collider2D playerHit = Physics2D.OverlapCircle(transform.position, flameRadius, whatIsPlayer);
                if (playerHit != null)
                {
                    playerHit.GetComponent<PlayerManager>().takeDamage(flameDamage);
                }
            }
            else
            {
                durationCountdown = duration;
                circleCountdown = circleRate;
            }
        }

        //Flame Bomb
        int bomb;
        bomb = Random.Range(0, bombRate);
        if (bombRateIncrease == 1)
        {
            bomb = 1;
        }
        if (bomb == 1)
        {
            Instantiate(flameBomb, transform.position, transform.rotation);
            bombRateIncrease = bombRate + 1;
        }
        bombRateIncrease -= 1;
    }

    void moving()
    {
        lookAtObject(player.position);
        transform.position = Vector2.MoveTowards(transform.position, movePosition, moveSpeed * Time.deltaTime);
        Collider2D collider = Physics2D.OverlapCircle(transform.position, bossRadius, whatToStop);
        if (collider != null || transform.position == movePosition)
        {
            movingCooldown = movingRate;
            isMoving = false;
        }
    }

    void dashing()
    {
        lookAtObject(player.position);
        transform.Translate(Vector2.up * dashSpeed * Time.deltaTime);
        Collider2D collider = Physics2D.OverlapCircle(transform.position, bossRadius, whatToStop);
        if (collider != null)
        {
            isDashing = false;
            dashRateIncrease = dashRate;
            chargingCountdown = chargingTime;
        }
    }

    void lookAtObject(Vector3 position)
    {
        Vector3 difference = position - transform.position;
        float rotationDegree = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, - rotationDegree - transform.rotation.z);
    }

    void shot(float angle)
    {
        Vector3 bullet = Quaternion.ToEulerAngles(transform.rotation);
        bullet.z += angle * Mathf.PI / 180;
        Quaternion temp = Quaternion.EulerAngles(bullet);
        Instantiate(shotgunProjectile, bossWeapon.transform.position, temp);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, bossRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, flameRadius);
    }
}
