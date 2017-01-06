using UnityEngine;
using System.Collections;
using Assets.Code.Interfaces;
public class ClientArenaState : StateBase {
	bool isPlayerSpawned = false;
	StateManager manager;
	
	public ClientArenaState(StateManager manager){
		this.manager = manager;
	}
	
	// Use this for initialization
	// Update is called once per frame
	public void StateUpdate () {
	
	}
	
	public void ShowGUI(){
		if (!isPlayerSpawned){//if the player hasn't spawned yet show a spawn button
			if (GUI.Button (new Rect(50, 50, 100, 50), "Click to Spawn")){
				isPlayerSpawned = true;
//				PhotonNetwork.Instantiate(manager.playerObject, manager.spawnPoint.transform.position, Quaternion.identity, 0);
			}	
			
		}
	}
	
}
