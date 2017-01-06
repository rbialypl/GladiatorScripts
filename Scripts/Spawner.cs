using UnityEngine;
using System.Collections;
using Assets.Code.Interfaces;
using System.Collections.Generic;


public class Spawner : MonoBehaviour {
	//private ArrayList enemies; //the enemies that are alive and out fighting
	private List<GameObject> enemies;
	
	
	public GameObject enemyToSpawn;
	int numberOfEnemiesToSpawn;
	public float timeBetweenSpawns;
	private float startingTime;
	private float nextSpawnTime;


	void Start(){
		enemies = new List<GameObject>();
		numberOfEnemiesToSpawn = 0;
		nextSpawnTime = Time.time + timeBetweenSpawns;
	}
	
	//Update method does the actual work of making sure enemies spawn on time
	void Update(){
		if (numberOfEnemiesToSpawn > 0){ //if there are enemies to spawn
			if (Time.time > nextSpawnTime){//if the time has surpassed the time at which an enemy should spawn
				GameObject enemy = (GameObject)PhotonNetwork.Instantiate("Skeleton", gameObject.transform.position, gameObject.transform.rotation, 0);
				enemy.GetComponent<EnemyStateManager>().setState (new RunOutState(enemy.GetComponent<EnemyStateManager>()));
				enemies.Add	(enemy); //spawn the enemy

				nextSpawnTime +=  timeBetweenSpawns; //determine the next spawn time
				numberOfEnemiesToSpawn--;
			}
		}
	
	}
	
	public void addEnemyToSpawnList(){
		numberOfEnemiesToSpawn++;
	}
	
	public bool areEnemiesDead(){
		//remove any enemies from the list that are dead
		for (int i = 0; i < enemies.Count; i++){
			if (enemies[i] == null){
				enemies.RemoveAt(i);
			}
		}
		//if the list is empty, then all the enemies are dead and return true
		if (numberOfEnemiesToSpawn == 0 && enemies.Count == 0){
			return true;
		}
		return false;
	}

}
