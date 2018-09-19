using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceBossController : MonoBehaviour {

    [Header("Others")]
    Transform player;
    Transform bossWeapon;
    BossManager bossManager;
    bool isPhase2 = false;
    public float bossRadius;
    public LayerMask whatToStop;
    public GameObject mainCamera;

    [Space]
    [Header("Moving")]
    public float moveSpeed;
    Vector3 movePosition;
    public float minMoveDistance;
    public float movingRate = 5f;
    float movingCooldown;
    bool isMoving;

    [Space]
    [Header("Teleportation")]
    float teleportCooldown;
    public float teleportRate = 4f;
    public float safeTeleportDistance = 1f;

    [Space]
    [Header("Shooting")]
    public GameObject iceBossProjectile;
    public float fireRate;
    float fireRateCooldown;

    [Space]
    [Header("Summon Pillar")]
    public float enterStateRate = 10f;
    float stateCountdown;
    public float standStillTime = 5f;
    float standStillCooldown;
    public float summonRate =.5f;
    float summonCooldown;
    Vector3 summonLocation;
    bool isStandingStill = false;
    public GameObject icePillar;


    [Space]
    [Header("Turret Mode")]
    public float turretFireRate;
    float fireCooldown;
    public float spreadRate = 0;
   
    // Use this for initialization
    void Start () {
        isMoving = false;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bossManager = gameObject.GetComponent<BossManager>();
        movingCooldown = movingRate;
        bossWeapon = transform.GetChild(0).transform;
        teleportCooldown = teleportRate;
        fireRateCooldown = fireRate;
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        standStillCooldown = standStillTime;
        summonCooldown = summonRate;
        stateCountdown = enterStateRate;

        fireCooldown = turretFireRate;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!isPhase2)
        {
            phase1();
        }
        else
        {
            phase2();
        }
    }


    void phase1()
    {

        //check phase 2
        if(gameObject.GetComponent<BossManager>().checkPhase2())
        {
            isPhase2 = true;
            teleportRate /= 2;
            standStillTime *= 1.5f;
            summonRate /= 2;
            enterStateRate /= 2;
            moveSpeed *= 1.5f;
            movingRate /= 2f;
            fireRate /= 2f;
        }

        if(!isStandingStill)
        {
            // Moving 
            lookAtObject(player.position);
            if (isMoving)
            {
                moving();
            }
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

            //teleporting
            if (teleportCooldown <= 0 && !isMoving)
            {
                Vector2 telePos = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
                float temp = Vector2.Distance(player.position, telePos);
                if (temp >= safeTeleportDistance)
                {
                    transform.position = telePos;
                    teleportCooldown = teleportRate;
                }
            }
            else
                teleportCooldown -= Time.deltaTime;


            //normal shooting
            if (fireRateCooldown > 0)
            {
                fireRateCooldown -= Time.deltaTime;
            }
            else
            {
                lookAtObject(player.position);
                shot(0, bossWeapon.transform.position);
                fireRateCooldown = fireRate;
            }
        }
        

        //enter pillar mode
        if(stateCountdown > 0)
        {
            stateCountdown -= Time.deltaTime;
        }
        else
        {
            if (!isStandingStill)
            {
                isStandingStill = true;
                transform.position = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);
                transform.rotation = Quaternion.identity;
            }
            if (standStillCooldown > 0 && isStandingStill)
            {
                if (summonCooldown > 0)
                {
                    summonCooldown -= Time.deltaTime;
                }
                else if (summonCooldown <= 0)
                {
                    summonLocation = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)), 0);
                    while(Vector2.Distance(summonLocation, transform.position) < bossRadius )
                    {
                        summonLocation = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)), 0);
                    }
                    Instantiate(icePillar, summonLocation, Quaternion.identity);
                    summonCooldown = summonRate;
                }
                standStillCooldown -= Time.deltaTime;
            }
            else
            {
                stateCountdown = Random.Range(enterStateRate - 2, enterStateRate + 2);
                standStillCooldown = standStillTime;
                isStandingStill = false;
            }
        }

    }



    void phase2()
    {
        if (!isStandingStill)
        {
            // Moving 
            lookAtObject(player.position);
            if (isMoving)
            {
                moving();
            }
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

            //teleporting
            if (teleportCooldown <= 0 && !isMoving)
            {
                Vector2 telePos = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
                float temp = Vector2.Distance(player.position, telePos);
                if (temp >= safeTeleportDistance)
                {
                    transform.position = telePos;
                    teleportCooldown = teleportRate;
                }
            }
            else
                teleportCooldown -= Time.deltaTime;


            //normal shooting
            if (fireRateCooldown > 0)
            {
                fireRateCooldown -= Time.deltaTime;
            }
            else
            {
                lookAtObject(player.position);
                shot(0, bossWeapon.transform.position);
                fireRateCooldown = fireRate;
            }
        }


        //enter pillar mode
        if (stateCountdown > 0)
        {
            stateCountdown -= Time.deltaTime;
        }
        else
        {
            if (!isStandingStill)
            {
                isStandingStill = true;
                transform.position = new Vector2(mainCamera.transform.position.x, mainCamera.transform.position.y);
                transform.rotation = Quaternion.identity;
            }
            if (standStillCooldown > 0 && isStandingStill)
            {
                if (summonCooldown > 0)
                {
                    summonCooldown -= Time.deltaTime;
                }
                else if (summonCooldown <= 0)
                {
                    summonLocation = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)), 0);
                    while (Vector2.Distance(summonLocation, transform.position) < bossRadius)
                    {
                        summonLocation = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)), 0);
                    }
                    Instantiate(icePillar, summonLocation, Quaternion.identity);
                    summonCooldown = summonRate;
                }
                standStillCooldown -= Time.deltaTime;

                if (fireCooldown > 0)
                {
                    fireCooldown -= Time.deltaTime;
                }
                else
                {
                    shot(0 + spreadRate, transform.position);
                    shot(90 + spreadRate, transform.position);
                    shot(180 + spreadRate, transform.position);
                    shot(270 + spreadRate, transform.position);
                    shot(45 + spreadRate, transform.position);
                    shot(135 + spreadRate, transform.position);
                    shot(225 + spreadRate, transform.position);
                    shot(315 + spreadRate, transform.position);
                    spreadRate += 5;
                    fireCooldown = turretFireRate;
                }

            }
            else
            {
                stateCountdown = Random.Range(enterStateRate - 2, enterStateRate + 2);
                standStillCooldown = standStillTime;
                isStandingStill = false;
            }

            
        }

    }



    //action function
    void lookAtObject(Vector3 position)
    {
        Vector3 difference = position - transform.position;
        float rotationDegree = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, - rotationDegree - transform.rotation.z);
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


    void shot(float angle, Vector3 location)
    {
        Vector3 bullet = Quaternion.ToEulerAngles(transform.rotation);
        bullet.z += angle * Mathf.PI / 180;
        Quaternion temp = Quaternion.EulerAngles(bullet);
        Instantiate(iceBossProjectile, location, temp);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, bossRadius);

    }
}
