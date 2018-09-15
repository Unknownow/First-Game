using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameBombController : MonoBehaviour
{

    public float bombSpeed = 4f;
    Transform player;
    Vector3 bombLocation;
    public GameObject flame;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        bombLocation = player.position;
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, bombLocation, bombSpeed * Time.deltaTime);
        if(bombLocation == transform.position)
        {
            Instantiate(flame, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Instantiate(flame, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Border"))
        {
            Instantiate(flame, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
