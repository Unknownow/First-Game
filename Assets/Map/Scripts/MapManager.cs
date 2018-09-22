using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{

    public GameObject[] boss = new GameObject[3];
    GameObject currentBoss;
    int nextBossIndex = 0;
    bool isBossing = false;

    public GameObject[] mob = new GameObject[5];
    public int[] numberOfMobAvail = new int[5];
    public GameObject[] mobQueue;
    public int mobQueueMax = 0;
    public int mobCounterMax = 50;
    int mobCounter = 0;
    public float spawnRate;
    float spawnCooldown;
    int currentMobOnMap = 0;

    void mobSpawn()
    {
        int temp;
        GameObject val;
        if (mobQueueMax == 0 || isBossing || mobCounter == mobCounterMax)
            return;
        temp = Random.Range(0, mobQueueMax);
        val = mobQueue[temp];
        for (int i = temp; i < mobQueueMax - 1; i++)
            mobQueue[i] = mobQueue[i + 1];
        mobQueueMax--;
        Vector3 ranPos = Camera.main.ScreenToWorldPoint(new Vector3(Random.Range(0, Screen.width), Random.Range(0, Screen.height), 0));
        if (ranPos.y >= 0)
            ranPos.y -= 1.5f;
        else
            ranPos.y += 1.5f;
        Instantiate(val, ranPos, transform.rotation);
        mobCounter++;
        currentMobOnMap++;
    }

    public void mobDeath(int mobNo)
    {
        mobQueue[mobQueueMax] = mob[mobNo];
        mobQueue[mobQueueMax].GetComponent<EnemyManager>().boundary = this.gameObject;
        mobQueueMax++;
        currentMobOnMap--;
    }

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < 5; i++)
            mobQueueMax += numberOfMobAvail[i];
        mobQueue = new GameObject[mobQueueMax];
        int temp = 0;
        for (int i = 0; i < 5; i++)
            for (int j = 0; j < numberOfMobAvail[i]; j++)
            {
                mobQueue[temp] = mob[i];
                mobQueue[temp].GetComponent<EnemyManager>().mobNo = i;
                mobQueue[temp].GetComponent<EnemyManager>().boundary = this.gameObject;
                temp++;
            }
        spawnCooldown = spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnCooldown <= 0)
        {
            mobSpawn();
            spawnCooldown = spawnRate;
        }
        else
            spawnCooldown -= Time.deltaTime;
        if (!isBossing && mobCounter == mobCounterMax && currentMobOnMap == 0)
        {
            isBossing = true;
            currentBoss = Instantiate(boss[nextBossIndex]);
            nextBossIndex++;
        }
        if (isBossing && currentBoss == null)
        {
            mobCounter = 0;
            isBossing = false;
        }
    }
}
