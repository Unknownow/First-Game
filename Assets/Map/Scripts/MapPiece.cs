using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiece : MonoBehaviour {

    [Header("Others")]
    public int weaponNumber = 1;
    public bool isPlayerStanding = false;
	public int mapQuarter;

    [Space]
    [Header("Map Health")]
    public int maxHealth = 20;
    public int currentHealth;
    public float healthRate = 1;
    float healthCountdown;
    public int healthPerSecond = 1;
    

    private void Start()
    {
        currentHealth = maxHealth;
        healthCountdown = healthRate;
    }


    private void FixedUpdate()
    {
		int d;
		if (isPlayerStanding)
			d = -1;
		else 
			d =1;
		if(healthCountdown < 0)
		{
			currentHealth += healthPerSecond * d;
			if (currentHealth > maxHealth)
				currentHealth = maxHealth;
			healthCountdown = healthRate;
		}
		else
		{
			healthCountdown -= Time.deltaTime;
		}
		if (currentHealth <= 0) 
		{
			transform.root.gameObject.GetComponent<MapDivider> ().MapDestroy (mapQuarter);
			Destroy (gameObject);
		}
    }
}
