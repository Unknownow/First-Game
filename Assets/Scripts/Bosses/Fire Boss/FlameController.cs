using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameController : MonoBehaviour {


    public float duration;
    public LayerMask whatIsPlayer;
    public float flameRadius;
    public float flameDamage;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        duration -= Time.deltaTime;
        Collider2D playerHit = Physics2D.OverlapCircle(transform.position, flameRadius, whatIsPlayer);
        if(playerHit != null)
        {
            playerHit.GetComponent<PlayerManager>().takeDamage(flameDamage);
        }
        if (duration <= 0)
            Destroy(gameObject);
	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, flameRadius);
    }
}
