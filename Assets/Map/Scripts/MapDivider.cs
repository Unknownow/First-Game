using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDivider : MonoBehaviour {
	public GameObject player;
	int mapQuarter = 0;
	int mapPiece = 5;
	int nMapFree;
	int[] mapFree = new int[7];

	int Opposite (int i){
		if (i == 0)
			return 3;
		if (i == 1)
			return 2;
		if (i == 2)
			return 1;
		return 0;
	}

	int GetRanMap (){
		int temp, val;
		temp = Random.Range (0, nMapFree);
		val = mapFree [temp];
		for (int i = temp; i < nMapFree - 1; i++)
			mapFree [i] = mapFree [i + 1];
		nMapFree -= 1;
		return val;
	}

	public void MapSpawn(int mQ){
		int mP = GetRanMap();
		DestroyImmediate (transform.GetChild (mapQuarter + mQ).GetChild (2).gameObject);
		Instantiate (transform.GetChild (mapPiece + mP), transform.GetChild (mapQuarter + mQ));
		transform.GetChild (mapQuarter + mQ).GetChild (2).gameObject.GetComponent<MapPiece> ().mapQuarter = mQ;
		transform.GetChild (mapQuarter + mQ).GetChild (2).gameObject.SetActive (true);

		if (transform.GetChild (mapQuarter + Opposite (mQ)).GetChild (2).gameObject.GetComponent<MapPiece> ().weaponNumber != 0) 
		{
			transform.GetChild (mapQuarter + mQ).GetChild (0).gameObject.SetActive (true);
			transform.GetChild (mapQuarter + mQ).GetChild (1).gameObject.SetActive (false);
			transform.GetChild (mapQuarter + Opposite (mQ)).GetChild (0).gameObject.SetActive (true);
			transform.GetChild (mapQuarter + Opposite (mQ)).GetChild (1).gameObject.SetActive (false);
		}
	}

	public void MapDestroy(int mQ) {
		mapFree [nMapFree] = transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber - 1;
		nMapFree += 1;
		Instantiate (transform.GetChild (4), transform.GetChild (mapQuarter + mQ));
		transform.GetChild (mapQuarter + mQ).GetChild (3).gameObject.SetActive (true);
		transform.GetChild (mapQuarter + mQ).GetChild (0).gameObject.SetActive (false);
		transform.GetChild (mapQuarter + mQ).GetChild (1).gameObject.SetActive (true);
		transform.GetChild (mapQuarter + Opposite (mQ)).GetChild (0).gameObject.SetActive (false);
		transform.GetChild (mapQuarter + Opposite (mQ)).GetChild (1).gameObject.SetActive (true);
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
		nMapFree = 7;
		for (int i = 0; i < nMapFree; i++) 
			mapFree [i] = i;
		for (int i = 0; i < 4; i++)
			MapSpawn (i);
		int mQ = PlayerInQuarter ();
		transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().isPlayerStanding = true;
		player.GetComponent<PlayerController> ().currentMapQuarter = mQ;
		player.transform.GetChild (player.GetComponent<PlayerController> ().currentWeaponNumber).gameObject.SetActive (false);
		player.transform.GetChild (transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber).gameObject.SetActive (true);
		player.GetComponent<PlayerController> ().currentWeaponNumber = transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber;
	}
	
	// Update is called once per frame
	void Update () {
		int mQ = PlayerInQuarter ();
		if (transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber != player.GetComponent<PlayerController> ().currentWeaponNumber)
		{
			player.transform.GetChild (player.GetComponent<PlayerController> ().currentWeaponNumber).gameObject.SetActive (false);
			player.transform.GetChild (transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber).gameObject.SetActive (true);
			player.GetComponent<PlayerController> ().currentWeaponNumber = transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().weaponNumber;
			transform.GetChild ( mapQuarter + (player.GetComponent<PlayerController> ().currentMapQuarter)).GetChild (2).GetComponent<MapPiece> ().isPlayerStanding = false;
			transform.GetChild (mapQuarter + mQ).GetChild (2).GetComponent<MapPiece> ().isPlayerStanding = true;
			player.GetComponent<PlayerController> ().currentMapQuarter = mQ;
		}
		if (Input.GetButtonDown("Fire2"))
			{
				for (int i = 0; i < 4; i++)
				if (transform.GetChild (mapQuarter + i).GetChild (2).gameObject.GetComponent<MapPiece> ().weaponNumber == 0)
					{
						MapSpawn (i);
						break;
					}
			}
	}
}
