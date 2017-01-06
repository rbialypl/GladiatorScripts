/* Created by Daniel Harasymiw, Robert Bialy
 * Created on June 3rd, 2014
 * Last Updated: June 3rd, 2014
 * 
 * This script controls the active state. By default the starting state is the main menu of the game.
 * The State Manager calls the StateUpdate and ShowGUI methods of the state that is currently active.
 */

using UnityEngine;
using System.Collections;
using Assets.Code.States;
using Assets.Code.Interfaces;
public class StateManager : Photon.MonoBehaviour {
	int levelLoaded = 0;
	public StateBase activeState; //public so that we can alter which state is active through unity?? 
	private StateManager instanceRef;
	public GameObject playerObject;//the player, might as well keep it here since other states will probably need it
	public bool foundPlayerObject;	 //states will use this to determine whether or not a player object has been found yet
									//This is required as players are equal to null when they die, and when they haven't been found
	public GameObject standbyCamera; //camera that the player sees through when their character is not alive
	public Transform spawnPoint; //the position of where players should spawn



	public int currentLevel; //the index of the level currently being played

	public PunNetworkManager networkManager;

	public int playersAlive = 0;
	
	bool alreadySpawned = false; //stops players from spawning multiple times while this is true



	public Spawner [] spawners; //keep track of all the spawners
	
	public GUIManager guiDisplayer;
	public bool guiCreated = false;
	public bool guiRpcCalled = false; //stops us from sending multiple rpc calls to display the gui if it is already being displayed
	
	public GUIStyle style; //font style for states, states can't get styles because they're not monobehaviour

	//Makes sure that the GameManager and the StateManager does not get deleted when a new scene is loaded
	void Awake(){
		if (instanceRef == null) {
			instanceRef = this;
			DontDestroyOnLoad (gameObject);
		} else {
			DestroyImmediate (gameObject);
		}
	}
	
	// Use this for initialization
	void Start () {
		activeState = new MenuState (this);
		networkManager = gameObject.GetComponent<PunNetworkManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (activeState != null) {
			activeState.StateUpdate ();
			activeState.ShowGUI();
		}
	}

	//calls the active states ShowGUI method
	void OnGUI(){
		if (activeState != null) {
			activeState.ShowGUI ();
		}
	}

	//Method that switches the active state to a new state
	public void SwitchState(StateBase newState){
		activeState = newState;
	}

	public void StartArena(){
		Application.LoadLevel("Arena");
		//WaitForLevelLoad ();
		activeState = new ArenaState (this);
	}
	
	public void StartArenaOfflineMode(){
		PhotonNetwork.Disconnect();
		PhotonNetwork.offlineMode = true;
		PhotonNetwork.CreateRoom ("Offline");
		StartArena ();
	}
	
	public IEnumerator WaitForLevelLoad(){
		yield return new WaitForSeconds(5);
	}

	//Finds the player and stores it in playerObject
	public IEnumerator FindPlayer(){
		float timeEnd = Time.time + 3.0f;//give 3 seconds to find the player
		while (Time.time < timeEnd) {
			playerObject = GameObject.FindGameObjectWithTag ("Player");
			yield return new WaitForEndOfFrame();
		}		
	}

	public IEnumerator FindSpawners(){
		float timeEnd = Time.time + 3.0f;
		while (Time.time < timeEnd){
			GameObject [] spawnerObject = GameObject.FindGameObjectsWithTag("Spawner"); //store all the spawners objects in an array
			spawners = new Spawner[spawnerObject.Length];
			for (int i = 0; i < spawnerObject.Length; i++){
				spawners[i] = spawnerObject[i].GetComponent<Spawner> (); //store all the spawner scripts in spawners array
			}
			yield return new WaitForEndOfFrame();
		}
		
	}
	
	public IEnumerator BreakDelay(){
		float timeEnd = Time.time + 10.0f;
		while (Time.time < timeEnd){
			yield return new WaitForEndOfFrame();
		}
		
		
		
	}



	void OnLevelWasLoaded(int level){
		Debug.Log ("ONLEVELWASLOADED");
		currentLevel = level;
		
		if (level == 1) {
			spawnPoint = GameObject.Find("SpawnPoint").transform;
			standbyCamera = GameObject.Find ("StandbyCamera");
			
			if (PhotonNetwork.offlineMode){
				Debug.Log ("############################### OFFLINE MODE ############################");
				standbyCamera = GameObject.Find ("StandbyCamera");
				Debug.Log ("Theres problems if you see this and you're in multiplayer...");
				standbyCamera.SetActive(false);
				Debug.Log ("Offline spawn");
				SpawnPlayer();
				guiDisplayer = PhotonNetwork.Instantiate ("GUIDisplayer", new Vector3(0, 0, 0), Quaternion.identity, 0).GetComponent<GUIManager>();
				//CharacterController characterController = playerPrefab.GetComponent<CharacterController> ();
				//characterController.enabled = true;
				
				StartCoroutine ("FindSpawners");		
							
				
			}
			else {//Playing Online
				Debug.Log ("####### NOT OFFLINE #######");
				if (PhotonNetwork.isMasterClient){
					photonView.RPC("LoadClientArena", PhotonTargets.Others);
					networkManager.StartCoroutine(WaitForLevelLoad ());
					Debug.Log ("Host spawn");
					SpawnPlayer ();
					playersAlive++;
					guiDisplayer = PhotonNetwork.Instantiate ("GUIDisplayer", new Vector3(0, 0, 0), Quaternion.identity, 0).GetComponent<GUIManager>();
					guiCreated = true;
					Debug.Log ("Finding Spawners!");
					StartCoroutine ("FindSpawners");	
							
									
				}
				else {
					Debug.Log ("Number of times level loaded: " + ++levelLoaded);
					if (!alreadySpawned){
						Debug.Log ("Client spawn");
						SpawnPlayer();
						alreadySpawned = true;
					}
					
				}
			}
			
		}
		
	}

	/* Spawns a player in the game
	 * and then enables their camera and controller
	 */ 
	[RPC]
	public void SpawnPlayer(){
		StartCoroutine (WaitForLoad());
		standbyCamera.SetActive(false);
		//spawn the player
		spawnPoint = GameObject.Find ("SpawnPoint").transform;
		GameObject playerGO = (GameObject)PhotonNetwork.Instantiate ("Player", spawnPoint.position, Quaternion.identity, 0);
		Debug.Log (playerGO + " spawned by client?");
		//enable the player's camera, and controllers so that only the person instantiating this object can control this player
		playerGO.transform.FindChild ("Camera").gameObject.SetActive (true);
		playerGO.GetComponent<PlayerController>().enabled = true;
		//disable the name above the player's head
		playerGO.transform.FindChild ("TextMesh").gameObject.SetActive (false);
		photonView.RPC ("addPlayer", PhotonTargets.MasterClient);
	}
	
	public IEnumerator WaitForLoad(){
		yield return new WaitForSeconds(3);
	}
	
	//Used to tell the clients to laod the arena level
	[RPC]
	public void LoadClientArena(){
		PhotonNetwork.LoadLevel (1);
	}
	
	
	[RPC]
	public void addPlayer(){
		playersAlive++;
	}
	[RPC]
	public void removePlayer(){
		playersAlive--;
	}

	[RPC]
	public void setGuiRpcCalled(bool called){
		guiRpcCalled = called;
	}
	



}
