using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour {

    public float maxHp = 100;
    public float currentHp;



	void Start () {
        currentHp = maxHp;
	}

	void Update () {
		
	}


    public void takeDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            Destroy(gameObject);
        }
    }
    public bool checkPhase2()
    {
        if(currentHp <= maxHp / 3)
        {
            return true;
        }
        return false;
    }

}
