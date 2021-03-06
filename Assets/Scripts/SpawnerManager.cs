﻿using UnityEngine;
using System.Collections;

public class SpawnerManager : Photon.MonoBehaviour{

	private Master m;
	private ChkPointManager m_Chk;

	public GameObject[] RacerPrefabs;

	public Transform[] SpawnPoints; 

	private GameObject[] playerObjs;

	private int totalPlayers;

	public int RespawnSleepTime = 3;
	private WaitForSeconds respawnClock;

	private bool[] isNewToAssign;

	// Use this for initialization
	void Awake () {

		isNewToAssign = new bool[4];

		respawnClock = new WaitForSeconds (3);

		totalPlayers = 4;

		playerObjs = new GameObject[4];
	
		m = GameObject.FindGameObjectWithTag ("Master").GetComponent<Master> ();
		m_Chk = GameObject.FindGameObjectWithTag ("CheckpointManager").GetComponent<ChkPointManager> ();
	}


	public IEnumerator RespawnPlayer(int playerNum, GameObject oldChar){
		yield return respawnClock;
		PhotonNetwork.Instantiate(RacerPrefabs[(int)m.GetPlayerChar(playerNum)].name, m_Chk.GetRespawnCheckpoint(playerNum).position, m_Chk.GetRespawnCheckpoint(playerNum).rotation, 0);
		AssignPlayers ();
		if (playerObjs [playerNum].GetComponent<ShipControls>().myChar == Master.Character.DatBoi || playerObjs [playerNum].GetComponent<ShipControls>().myChar == Master.Character.Spaceship || 
			playerObjs [playerNum].GetComponent<ShipControls>().myChar == Master.Character.LittleGirl) {
			playerObjs [playerNum].transform.Rotate(new Vector3(0f, 180f, 0f));
		}

		playerObjs [playerNum].GetComponentInChildren<Camera>().rect = new Rect((playerNum == 0 || playerNum == 2)? 0.0f : 0.5f, 
															  (playerNum == 2 || playerNum == 3)? 0.0f : 0.5f, 
															  0.5f, 
															  0.5f);
		
		playerObjs [playerNum].name = "Player " + (playerNum + 1);

		isNewToAssign [playerNum] = true;
		PhotonNetwork.Destroy (oldChar);
	}

	public void OnJoinedRoom(){
		AssignPlayers ();
		if (PhotonNetwork.isMasterClient) {
			
		}
	}


	private void AssignPlayers(){
		GameObject[] tempObjs = GameObject.FindGameObjectsWithTag("Player");

		for (int x = 0; x < tempObjs.Length; x++) {
			playerObjs [x] = tempObjs [x];
		}
	}

	private void spawnPlayer(int playerNum){
		PhotonNetwork.Instantiate(RacerPrefabs[(int)m.GetPlayerChar(playerNum)].name, SpawnPoints[playerNum].position, Quaternion.Euler(Vector3.zero), 0);
		AssignPlayers ();
		playerObjs [playerNum].GetComponentInChildren<Camera>().rect = new Rect((playerNum == 0 || playerNum == 2)? 0.0f : 0.5f, 
			(playerNum == 2 || playerNum == 3)? 0.0f : 0.5f, 
			0.5f, 
			0.5f);

		playerObjs [playerNum].name = "Player " + (playerNum + 1);
		playerObjs [playerNum].transform.parent = SpawnPoints [playerNum];

		isNewToAssign[playerNum] = true;

	}
	
	// Update is called once per frame
	void Update () {
		for (int x = 0; x < isNewToAssign.Length; x++) {
			if (isNewToAssign [x]) {
				m_Chk.AssignNewTracking (playerObjs[x], x);
				isNewToAssign[x] = false;
			}
		}
	}
}
