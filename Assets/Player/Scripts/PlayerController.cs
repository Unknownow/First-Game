using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Transform player;
    float moveX = 0, moveY = 0;
    public float moveSpeed = 2f;

    [Space]
    [Header("Teleportation")]
    public float teleCooldown = 2f;
    public bool isTele = false;
    float teleCooldownTimer;
    public Vector3 target;


	// Use this for initialization
	void Awake () {
        player = GetComponent<Transform>();
        teleCooldownTimer = teleCooldown;
        isTele = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        player.position += new Vector3(moveX * moveSpeed * Time.deltaTime, moveY * moveSpeed * Time.deltaTime,0);


        Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        cursorPosition.z = 0;
        Vector2 difference = cursorPosition - transform.position;
        float rotationDegreeToCursor = Mathf.Atan2(difference.x, difference.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, -rotationDegreeToCursor - transform.rotation.z);
        
        if(isTele)
        {
            if(teleCooldownTimer == 0)
            {
                tele();
            }
            if (teleCooldownTimer < teleCooldown)
                teleCooldownTimer += Time.deltaTime;
            else
            {
                teleCooldownTimer = teleCooldown;
                isTele = false;
            }
        }
    }


    void tele()
    {
        transform.position = target;
    }

}
