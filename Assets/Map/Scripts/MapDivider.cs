using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDivider : MonoBehaviour {
	public GameObject player;
	int mapQuarter = 0;
	int mapPiece = 4;

	int Opposite (int i){
		if (i == 0)
			return 3;
		if (i == 1)
			return 2;
		if (i == 2)
			return 1;
		if (i == 3)
			return 0;
		return 0;
	}

	int SelectPos(int mQ) {
		for (int i = 0; i < 4; i++)
			if (i != mQ && transform.GetChild (mapQuarter + i).gameObject.GetComponent<Collider2D> ().isTrigger)
				return i;
		return -1;
	}

	void MapSpawn(int mQ, int mP){
		transform.GetChild(mapQuarter+mQ).gameObject.GetComponent<Collider2D>().isTrigger = true;
		Instantiate (transform.GetChild (mapPiece+mP), transform.GetChild (mapQuarter+mQ));

		if (transform.GetChild (mapQuarter + Opposite (mQ)).gameObject.GetComponent<Collider2D> ().isTrigger) {
			(transform.GetChild (mapQuarter + mQ)).GetChild (0).gameObject.SetActive (true);
			(transform.GetChild (mapQuarter + mQ)).GetChild (1).gameObject.SetActive (false);
			(transform.GetChild (mapQuarter + Opposite (mQ))).GetChild (0).gameObject.SetActive (true);
			(transform.GetChild (mapQuarter + Opposite (mQ))).GetChild (1).gameObject.SetActive (false);
		} 
		else 
		{
			(transform.GetChild (mapQuarter + mQ)).GetChild (0).gameObject.SetActive (false);
			(transform.GetChild (mapQuarter + mQ)).GetChild (1).gameObject.SetActive (true);
		}

		( transform.GetChild(mapQuarter+mQ) ).GetChild(2).gameObject.GetComponent<MapPiece>().mapQuarter = mQ;
		( transform.GetChild(mapQuarter+mQ) ).GetChild(2).gameObject.SetActive(true);

	}

	public void MapDestroy(int mQ) {
		(transform.GetChild (mapQuarter + mQ)).GetChild (0).gameObject.SetActive (false);
		(transform.GetChild (mapQuarter + mQ)).GetChild (1).gameObject.SetActive (false);
		if (transform.GetChild (mapQuarter + Opposite (mQ)).gameObject.GetComponent<Collider2D> ().isTrigger) {
			(transform.GetChild (mapQuarter + Opposite (mQ))).GetChild (0).gameObject.SetActive (false);
			(transform.GetChild (mapQuarter + Opposite (mQ))).GetChild (1).gameObject.SetActive (true);
		}
		transform.GetChild(mapQuarter+mQ).gameObject.GetComponent<Collider2D>().isTrigger = false;

		player.GetComponent<PlayerController> ().tele ( ( transform.GetChild ( SelectPos(mQ) ) ).GetChild (0).position );
	}

	int PlayerInQuarter() {
		float x = player.transform.position.x;
		float y = player.transform.position.y;
		if (x < 0 && y >= 0)
			return 1;
		if (x >= 0 && y < 0)
			return 2;
		if (x < 0 && y < 0)
			return 3;
		return 0;
	}


	// Use this for initialization
	void Start () {
		for (int i = 0; i < 4; i++) 
		{
			MapSpawn (i, i);
		}

		int mQ = PlayerInQuarter ();
		player.transform.GetChild (player.GetComponent<PlayerController> ().currentWeaponNumber).gameObject.SetActive (false);
		player.transform.GetChild (transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber).gameObject.SetActive (true);
		transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().isPlayerStanding = true;
		player.GetComponent<PlayerController> ().currentMapQuarter = mQ;
		player.GetComponent<PlayerController> ().currentWeaponNumber = transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber;

	}
	
	// Update is called once per frame
	void Update () {
		int mQ = PlayerInQuarter ();
		if (mQ != player.GetComponent<PlayerController> ().currentMapQuarter) 
		{
			player.transform.GetChild (player.GetComponent<PlayerController> ().currentWeaponNumber).gameObject.SetActive (false);
			if (transform.GetChild ( mapQuarter + (player.GetComponent<PlayerController> ().currentMapQuarter) ).gameObject.GetComponent<Collider2D> ().isTrigger)
				transform.GetChild ( mapQuarter + (player.GetComponent<PlayerController> ().currentMapQuarter) ).GetChild(2).GetComponent<MapPiece> ().isPlayerStanding = false;
			player.transform.GetChild (transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber).gameObject.SetActive (true);
			transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().isPlayerStanding = true;
			player.GetComponent<PlayerController> ().currentMapQuarter = mQ;
			player.GetComponent<PlayerController> ().currentWeaponNumber = transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber;
		}
		if (Input.GetButtonDown("Fire2"))
			{
				for (int i = 0; i < 4; i++)
					if (!transform.GetChild (mapQuarter + i).gameObject.GetComponent<Collider2D> ().isTrigger)
					{
						MapSpawn (i, i);
						break;
					}
			}
	}
}
