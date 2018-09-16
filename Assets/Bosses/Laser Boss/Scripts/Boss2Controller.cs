using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour {

	public GameObject boss2projectile;
	public GameObject boss2laser;

	public Transform frontFace;

	float shootCooldown;
	public float shootRate = 0.5f;

	float teleportCooldown;
	public float teleportRate = 8f;

	float laserCooldown;
	public float laserRate = 1f;

	bool isFreeze = false;
	bool fired = false;
	float freezeCooldown;
	public float freezeRate = 1f;

	public float safeTeleportDistance = 1f;

	public bool isPhase2 = false;

	public Transform player;

    BossManager boss;


	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player").transform;
		frontFace = transform.GetChild(0).transform;
		teleportCooldown = teleportRate;
		laserCooldown = laserRate;
		freezeCooldown = freezeRate;
        boss = gameObject.GetComponent<BossManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!isPhase2 && boss.checkPhase2())
        {
            isPhase2 = true;
        }

        gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

		if (!isFreeze) 
		{
			Vector2 difference = player.position - transform.position;
			float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);
		}

		if (teleportCooldown <= 0 && !isFreeze)
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
		
		if (shootCooldown <= 0)
		{
			Quaternion ranRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(-180, 180));
			Instantiate(boss2projectile, transform.position, ranRotation);
			shootCooldown = shootRate;
		}
		else
			shootCooldown -= Time.deltaTime;

        

		if (!isPhase2) 
		{
			if (laserCooldown <= 0) 
			{
				isFreeze = true;
				if (!fired) 
				{
					if (freezeCooldown <= 0) 
					{
						Instantiate (boss2laser, frontFace.position, transform.rotation);
						fired = true;
						freezeCooldown = freezeRate;
					}
					else
						freezeCooldown -= Time.deltaTime;
				}
				else 
				{
					isFreeze = false;
					laserCooldown = laserRate;
					fired = false;
				}
			} 
			else 
				laserCooldown -= Time.deltaTime;
		}
	}
}
