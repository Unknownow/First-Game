using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour {

    public float maxHp = 100;
    public float currentHp;
    public Slider bossHealthBar;



    void Start () {
        currentHp = maxHp;
        bossHealthBar.gameObject.SetActive(true);
	}
    private void Update()
    {
        bossHealthBar.value = currentHp;
    }

    public void takeDamage(float damage)
    {
        currentHp -= damage;
        if(currentHp <= 0)
        {
            bossHealthBar.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
    public bool checkPhase2()
    {
        if(currentHp <= maxHp / 2 )
        {
            return true;
        }
        return false;
    }

}
