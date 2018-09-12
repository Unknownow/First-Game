using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Transform player;
    float moveX = 0, moveY = 0;
    public float moveSpeed = 2f;

	// Use this for initialization
	void Awake () {
        player = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");
        player.position += new Vector3(moveX * moveSpeed * Time.deltaTime, moveY * moveSpeed * Time.deltaTime,0);
	}

}
