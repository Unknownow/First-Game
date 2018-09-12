using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberController : MonoBehaviour
{


    public float moveSpeed = 2f;

    public Transform player;

    public LayerMask whatIsPlayer;


    public float bombTime = 1.5f;
    public bool isDetonated = false;
    public float detonateRange = 2f;
    public float bombRange = 3f;
    public int damage;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        

        if(!isDetonated)
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

    void detonate(float damage)
    {
        Collider2D playerHit = Physics2D.OverlapCircle(transform.position, bombRange, whatIsPlayer);
        playerHit.GetComponent<PlayerManager>().takeDamage(damage);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, bombRange);
    }

}
