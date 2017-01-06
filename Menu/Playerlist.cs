using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/* Created by Daniel Harasymiw
 * Last updated March 11, 2015
 * Displays an updated list of players who are in the room
 * Updates whenever a player leaves or joins the room
 */
public class Playerlist : MonoBehaviour {
	public GameObject playerNameText;
	// Use this for initialization

	void OnJoinedRoom(){
		createList ();
	}

	void OnPhotonPlayerConnected(PhotonPlayer player){
		createList ();
	}
	
	void OnPhotonPlayerDisconnected(PhotonPlayer player){
		createList ();
	}
	
	void createList(){
		clearList ();
		for (int i = 0; i < PhotonNetwork.playerList.Length; i++){
			GameObject playerText = Instantiate (playerNameText);
			playerText.GetComponent<Text>().text = PhotonNetwork.playerList[i].name;
			playerText.transform.parent = gameObject.transform;				
		}
	}
	
	void clearList(){
		Transform parent = gameObject.transform;
		foreach (Transform child in parent){
			if (child.name != "StartButton"){
				Destroy (child.gameObject);
			}			
			
		}
	}

}
