using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/* Created by Daniel Harasymiw
 * Last updated March 8, 2015
 * Handles the online multiplayer portion of the game
 */ 
public class PunNetworkManager : Photon.MonoBehaviour {
	StateManager manager;
	public PhotonView photonView;
	public GameObject playerPrefab;
	int currentLevel;
	// Use this for initialization
	void Start () {
		manager = gameObject.GetComponent<StateManager>();
		photonView = gameObject.GetComponent<PhotonView>();
		Connect ();
	}
	
	// Update is called once per frame
	void Connect(){
		PhotonNetwork.ConnectUsingSettings ("v1.0.0");
	}

	public void CreateRoom(){
		string name = GameObject.Find ("HostNameField").GetComponent<InputField> ().text;
		if (name != "") {
			Debug.Log ("NAME: " + name);
			PhotonNetwork.playerName = name;
			PhotonNetwork.CreateRoom (PhotonNetwork.playerName, new RoomOptions() {isOpen = true, isVisible = true, maxPlayers = 4}, null);
			GameObject.Find ("Canvas").GetComponent<Menu>().switchToHostLobby();
		}

	}

	public void JoinRoom(){
		//PhotonNetwork.JoinRandomRoom ();
	}

	void OnJoinedRoom(){
		Debug.Log ("Room joinned! " + PhotonNetwork.room.name);
	}

	void OnPhotonPlayerConnected(PhotonPlayer player){
		Debug.Log("Player connected!");
		if (manager.currentLevel != 0) {//game already started get client to load
			manager.activeState = new ClientArenaState(manager);

			//photonView.RPC ("LoadClientArena", player);
		}
	}
	
	//Disconnect player from the menu and return to the main menu if the host leaves the game
	void OnMasterClientSwitched(){
		Debug.Log ("Host disconnected");
		PhotonNetwork.LeaveRoom (); //disconnect
		Application.LoadLevel ("Menu"); //load menu
	}


	IEnumerator WaitForLoad(){
		yield return new WaitForSeconds (3);
	}




}
