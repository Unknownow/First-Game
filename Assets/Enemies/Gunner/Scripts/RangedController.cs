using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedController : MonoBehaviour
{
    [Header("Others")]
    EnemyManager thisEnemy;
    float pushedBackCountdown;
    Animator gunnerAnimator;

    [Space]
    [Header("Movement")]
    public float moveSpeed = 2f;
    public float minDistance = 20f;
    public float retreatDistance = 10f;

    
    [Space]
    [Header("Gun")]
    public float shootRate = 1f;
    public float shootCooldown;
    Transform gunPoint;
    public GameObject projectile;
    public Transform player;
    public Transform[] fuzzle;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisEnemy = gameObject.GetComponent<EnemyManager>();
        gunPoint = transform.GetChild(1).transform;
        shootCooldown = shootRate;
        pushedBackCountdown = thisEnemy.pushedBackDuration;
        gunnerAnimator = GetComponent<Animator>();

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
                gunnerAnimator.SetBool("isRunning", true);
            }
            else if (temp > retreatDistance && temp < minDistance)
            {
                transform.position = this.transform.position;
                gunnerAnimator.SetBool("isRunning", false);
            }
            else if (temp <= retreatDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, -moveSpeed * Time.deltaTime);
                gunnerAnimator.SetBool("isRunning", true);
            }

            if (shootCooldown <= 0)
            {
                Instantiate(projectile, gunPoint.position, transform.rotation);
                gunFuzzle();
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
                gunnerAnimator.SetBool("isRunning", false);
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                pushedBackCountdown = thisEnemy.pushedBackDuration;
                thisEnemy.isPushedBack = false;
            }
        }
    }


    void gunFuzzle()
    {
        int i = Random.Range(0, 3);
        fuzzleClone(fuzzle[i]);
        return;
    }

    void fuzzleClone(Transform fuzzle)
    {
        GameObject clone;
        clone = Instantiate(fuzzle.gameObject, gunPoint.position, transform.rotation);
        clone.transform.parent = transform;
        float size = Random.Range(0.8f, 1f);
        clone.transform.localScale = new Vector3(size, size, size);
        Destroy(clone, .1f);
        return;
    }
}
