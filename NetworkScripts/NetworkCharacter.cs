using UnityEngine;
using System.Collections;

/* Created by Daniel Harasymiw
 * March 14, 2015
 * This script does the network smoothing of characters in the game
 * Should be attached to any game object that has movement in a networked game
 * This script sends the characters position if it owns that character
 * If the player does not own this character, than this script receives data about
 * this character and smoothes its movement
 */ 
public class NetworkCharacter : Photon.MonoBehaviour {
	Vector3 newTransform = Vector3.zero; //the object's actual position on the owner's game
	Quaternion newRotation = Quaternion.identity; //object's actual rotation on the owner's game
	Animation anim;
	string animation;
	public float lerpPercent = 0.1f; //the percentage of how much to update object's position


	// Update is called once per frame
	void Update () {
		if (!photonView.isMine) {//if we don't own this character, smooth its position
			transform.position = Vector3.Lerp (transform.position, newTransform, lerpPercent);
			transform.rotation = Quaternion.Lerp (transform.rotation, newRotation, lerpPercent);
		}
	}
	
	//updates the players positions by sending/receiving data to other players
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if (stream.isReading) {//we don't own this character so we are receiving data about this character
			newTransform = (Vector3) stream.ReceiveNext();
			newRotation = (Quaternion) stream.ReceiveNext ();

		}
		else { //we own this character, so send data
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		}
	}

}
