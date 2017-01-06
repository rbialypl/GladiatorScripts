using UnityEngine;
using System.Collections;

public class PlayerLabel : Photon.MonoBehaviour {
	string name; //the name of the player
	PhotonView view; 
	GameObject textMesh;
	// Use this for initialization
	void Start () {
		view = GetComponent <PhotonView> (); //get the player's photon view
		name = view.owner.name; //set the name to the owner of this object's photon view
		textMesh = gameObject.transform.FindChild ("TextMesh").gameObject;
		textMesh.GetComponent<TextMesh> ().text = name;

	}
	
	// Update is called once per frame
	void Update () {
		textMesh.transform.LookAt (Camera.main.transform.position);
		textMesh.transform.Rotate (0, 180, 0);
	}

	void OnGUI(){

	}
}
