using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour {

	public float damage;
	public bool range=true;

	public float attackRange;
	public float attackRate = 1f;
	public float attackCooldown;

	public LayerMask whatIsEnemy;

	public Transform frontFace;

	public GameObject projectile;

	//public Animation swing;
	//public Animation fire;

	// Use this for initialization
	void Start () {
		frontFace = transform.GetChild(0).transform;
		attackCooldown = attackRate;
	}

	// Update is called once per frame
	void Update () {
		if ( Input.GetButtonDown ("Fire1") && attackCooldown <= 0 ) 
		{
			if (range) 
			{
				Instantiate (projectile, frontFace.position, transform.rotation);
				attackCooldown = attackRate;
			} 
			else 
			{
				Collider2D[] enemyHit = Physics2D.OverlapCircleAll(transform.position, attackRange, whatIsEnemy);
				foreach(Collider2D i in enemyHit)
				{
					i.GetComponent<EnemyManager>().takeDamage(damage);
				}
				attackCooldown = attackRate;
			}
		}
		attackCooldown -= Time.deltaTime;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
