using UnityEngine;
using System.Collections;
using UnityEngine.UI;
/* Created by Daniel Harasymiw
 * Last updated March 8, 2015
 * Finds all of the rooms that the player can join and displays them as buttons.
 * When the player clicks on a button for the game it tries to connect them.
 */ 
public class GameBrowser : MonoBehaviour {
	RoomInfo[] rooms;

	public GameObject gameButton;
	
	public void GetServerList(){
		rooms = PhotonNetwork.GetRoomList ();

		//Destroy old buttons if they exist
		foreach (Transform child in gameObject.transform) {
			if (child.tag == "Join Button"){
				GameObject.Destroy(child.gameObject);
			}

		}


		foreach (RoomInfo room in rooms) {
			GameObject button = Instantiate (gameButton);
			JoinGame joinGame = button.GetComponent<JoinGame>();
			joinGame.roomName = room.name;
			button.transform.parent = gameObject.transform;
			Text text = button.transform.FindChild("Text").GetComponent<Text>();
			text.text = "NAME: " + room.name.ToUpper() + ", PLAYERS: " + room.playerCount + "/" + room.maxPlayers + 
				", ROOM AVAILABILITY: " + (room.open ? "OPEN" : "CLOSED");

		}

	}

	void OnReceivedRoomsListUpdate(){
		rooms = PhotonNetwork.GetRoomList ();
	}


}
