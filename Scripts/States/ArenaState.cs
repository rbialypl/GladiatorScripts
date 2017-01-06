/* Created by Daniel Harasymiw, Robert Bialy
 * Created on June 3rd, 2014
 * Last Updated: June 3rd, 2014
 * 
 * State for when the player is playing "Arena Mode"
 * In this game mode, players will fight off waves of enemies which become more difficult as the waves progress.
 */
using UnityEngine;
using System.Collections;
using Assets.Code.States;
using Assets.Code.Interfaces;
public class ArenaState : StateBase {

	private StateManager manager;		
	
	
	//Arena state variable
	public int waveNumber;
	public float timeBetweenSpawns = 15.0f;
	public GameObject enemyToSpawn;
	public float timeUntilNextWave = 0.0f;
	public float timeBetweenWaves = 10.0f;
	public bool waveStarted = false;

	int fontSize = 70;
	
	//stops us from sending multiple rpc calls to display the gui if it is already being displayed
	
	public ArenaState (StateManager manager){
		this.manager = manager;
		manager.StartCoroutine ("BreakDelay");
		timeUntilNextWave = Time.time + timeBetweenWaves;
		waveNumber = 0;
		
		
	}
	
	

	public void StateUpdate(){
		//if the player objet isn't there
		if (manager.playersAlive <= 0){
			Debug.Log ("Player Object null!");
			
			//if player object has already been found then player died, so game over
			if (manager.foundPlayerObject){
				Debug.Log("You lose!");
				Application.LoadLevel ("Arena");
			}
		}
		else if (manager.spawners.Length == 0){ //player is alive to spawn waves
			manager.StartCoroutine ("FindSpawners");		
		}
		else {
			if (isWaveOver ()){
				if (waveStarted){ //if the wave is started calculate the next wave time the wave should start
					timeUntilNextWave = Time.time + timeBetweenWaves;
					waveStarted = false;
				}
				else {
					if (Time.time > timeUntilNextWave){//enough time has passed to start the next wave
						waveNumber++;
						Debug.Log ("Calculating wave #" + waveNumber);
						Debug.Log ("Time: " + Time.time + ", Wave start time: " + timeUntilNextWave);
						
						calculateNextWave(waveNumber);
						waveStarted = true;
					}
					
				}
				
			}
			
		}
	}
	
	public void calculateNextWave(int waveNumber){
	
		//Determine how many enemies to spawn based on wave
		//Enemies increase by 2 each wave
		int numberOfEnemiesToSpawn = 1 + (2 * (waveNumber - 1));
		bool bossWave = false;
		/*
		if (waveNumber % 10 == 0){ //every 10 waves do a boss wave?
			numberOfEnemiesToSpawn -= 2; //keep the number of enemies the same as the previous wave as players have to fight a boss as well
			bossWave = true;
		}
		*/
		
		for (int i = 0; i < numberOfEnemiesToSpawn; i++){
			manager.spawners[i%3].addEnemyToSpawnList();
		}
	}
	
	public bool isWaveOver (){
		int counter = 0;//when counter is 0 then all enemies are dead
		for (int i = 0; i < manager.spawners.Length; i++){
			if (manager.spawners[i].areEnemiesDead()){
				counter++;
			}
		}
	
		if (counter == manager.spawners.Length){
		 //all spawners have no enemies that are alive
			return true;
		}
		else
			return false;
	}
	
	
	

	public void ShowGUI(){
		//Countdown timer
		float timeLeft = timeUntilNextWave - Time.time;
		
		if (timeLeft <= 5 && timeLeft >=0 && !manager.guiRpcCalled /*&& manager.guiCreated*/){
			manager.guiDisplayer.photonView.RPC ("displayCountdownTimer", PhotonTargets.All, timeLeft);
			manager.guiRpcCalled = true;
		}
		
		
	}
	
	
}
