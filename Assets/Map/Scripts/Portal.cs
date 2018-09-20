using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {
	public GameObject nextPortal;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			collision.GetComponent<PlayerController> ().tele (nextPortal.transform.position);
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(collision.CompareTag("Player"))
		{
			collision.GetComponent<PlayerController> ().tele (nextPortal.transform.position);
		}
	}
}
