using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/* Created by Daniel Harasymiw
 * This script does the actual work of joinning a game
 */ 
public class JoinGame : Photon.MonoBehaviour {
	public string roomName;
	Menu menu;
	void Start(){
		menu = GameObject.Find ("Canvas").GetComponent<Menu> ();

	}


	public void joinGame(){
		Debug.Log (GameObject.Find("NameInputField"));
		string name = GameObject.Find ("NameInputField").GetComponent<InputField>().text;
		if (name != "") {
			PhotonNetwork.playerName = name;
			PhotonNetwork.JoinRoom(roomName);
		}

	}

	void OnJoinedRoom(){
		menu.switchToClientLobby ();
	}

}
