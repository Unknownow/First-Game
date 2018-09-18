using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Transform player;
    float moveX = 0, moveY = 0;
    public float moveSpeed = 2f;
    public int currentWeaponNumber = 0;
    public GameObject currentMapPiece;

    [Space]
    [Header("Teleportation")]
    public float teleCooldown = 2f;
    public bool canTele = true;
    float teleCooldownTimer;
    public Vector3 target;

    [Space]
    [Header("Slow")]
    float duration;
    float percentage;
    bool isSlow = false;


	// Use this for initialization
	void Awake () {
        player = GetComponent<Transform>();
        teleCooldownTimer = teleCooldown;
        canTele = true;
        isSlow = false;
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
        
        if(!canTele)
        {
            if (teleCooldownTimer < teleCooldown)
                teleCooldownTimer += Time.deltaTime;
            else
            {
                teleCooldownTimer = 0;
                canTele = true;
            }
        }


        if(isSlow && duration > 0)
        {
            duration -= Time.deltaTime;
        }
        else if(isSlow && duration <= 0)
        {
            isSlow = false;
            moveSpeed = moveSpeed / percentage;
        }
    }


    public void tele(Vector3 target)
    {
        if(canTele)
        {
            canTele = false;
            transform.position = target;
        }
        
    }

    public void slow(float slowPercentage, float slowDuration)
    {
        if (!isSlow)
            moveSpeed = moveSpeed * slowPercentage;
        isSlow = true;
        duration = slowDuration;
        percentage = slowPercentage;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Map Piece"))
        {
            transform.GetChild(currentWeaponNumber).gameObject.SetActive(false);
            currentMapPiece.GetComponent<MapPiece>().isPlayerStanding = false;
            transform.GetChild(collision.GetComponent<MapPiece>().weaponNumber).gameObject.SetActive(true);
            collision.GetComponent<MapPiece>().isPlayerStanding = true;
            currentMapPiece = collision.gameObject;
            currentWeaponNumber = collision.GetComponent<MapPiece>().weaponNumber;
        }
    }
}
