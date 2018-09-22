using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : MonoBehaviour
{

    public GameObject boss2projectile;
    public GameObject boss2laser;

    public Transform frontFace;
    Transform laserSnipe;

    float shootCooldown;
    public float shootRate = 1f;

    float teleportCooldown;
    public float teleportRate = 4f;

    float laserCooldown;
    public float laserRate = 1f;

    bool isFreeze = false;
    bool fired = false;
    float freezeCooldown;
    public float freezeRate = 1f;

    public float safeTeleportDistance = 1f;

    bool isPhase2 = false;

    public Transform player;


    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        frontFace = transform.GetChild(0).transform;
        laserSnipe = transform.GetChild(1);
        teleportCooldown = teleportRate;
        laserCooldown = laserRate;
        freezeCooldown = freezeRate;
        laserSnipe.GetComponent<Renderer>().enabled = false;
    }

    void Phase1()
    {
        isPhase2 = gameObject.GetComponent<BossManager>().checkPhase2();
        if (isPhase2)
        {
            laserSnipe.GetComponent<Renderer>().enabled = true;
            shootRate /= 2;
            teleportRate *= 2;
            return;
        }
        Vector2 difference = player.position - transform.position;
        float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);

        if (teleportCooldown <= 0)
        {
            Vector2 telePos = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
            if (telePos.y >= 0)
                telePos.y -= 1.5f;
            else
                telePos.y += 1.5f;
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
    }

    void Phase2()
    {
        if (!isFreeze)
        {
            Vector2 difference = player.position - transform.position;
            float rotationDegreeToPlayer = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToPlayer - transform.rotation.z);
        }

        if (teleportCooldown <= 0 && !isFreeze)
        {
            Vector2 telePos = Camera.main.ScreenToWorldPoint(new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height)));
            if (telePos.y >= 0)
                telePos.y -= 1.5f;
            else
                telePos.y += 1.5f;
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

        if (laserCooldown <= 0)
        {
            isFreeze = true;
            if (!fired)
            {
                if (freezeCooldown <= 0)
                {
                    Instantiate(boss2laser, frontFace.position, transform.rotation);
                    fired = true;
                    freezeCooldown = freezeRate;
                    laserSnipe.GetComponent<Renderer>().enabled = false;
                }
                else
                {
                    laserSnipe.GetComponent<Renderer>().enabled = true;
                    freezeCooldown -= Time.deltaTime;
                }

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

    // Update is called once per frame
    void Update()
    {
        if (!isPhase2)
            Phase1();
        else
            Phase2();
    }
}
