using UnityEngine;
using System.Collections;

public class Equip : MonoBehaviour {

	Character character;
	private GameObject cam;
	private Transform arm;
	private Transform weapon;
	private Transform storage;
	private Camera playerCamera;
	private Vector3 position;
	private Quaternion rotation;
	private bool showEquip;


	// Use this for initialization
	void Start () {
		character = gameObject.GetComponent<Character> ();
		cam = gameObject.transform.FindChild("Camera").gameObject;
		arm = gameObject.transform.FindChild ("Armature")
			.transform.FindChild ("Armature|waist").transform.FindChild ("Armature|chest")
			.transform.FindChild ("Armature|arm_upper.r").transform.FindChild ("Armature|arm_lower.r")
			.transform.FindChild ("Armature|hand.r");
		playerCamera = (Camera)(cam.GetComponent("Camera"));
		showEquip = false;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit;
		Ray ray = new Ray (playerCamera.transform.position, playerCamera.transform.forward); 
		Debug.DrawRay (playerCamera.transform.position, playerCamera.transform.forward);
		if (Physics.Raycast (ray, out hit, 60)) {
			if (hit.collider.tag == "Weapon") {
				showEquip = true;
				if (Input.GetKeyDown(KeyCode.E))
				{
					weapon = arm.gameObject.transform.GetChild(0).gameObject.transform;
					storage = hit.transform.parent.transform;
					position = new Vector3(weapon.transform.position.x,weapon.transform.position.y,weapon.transform.position.z);
					rotation = new Quaternion(weapon.transform.rotation.x,weapon.transform.rotation.y,weapon.transform.rotation.z,0);
					weapon.parent = null;
					weapon.parent = storage;

					weapon.position = new Vector3(hit.transform.position.x,hit.transform.position.y,hit.transform.position.z);
					weapon.rotation = new Quaternion(hit.transform.rotation.x,hit.transform.rotation.y,hit.transform.rotation.z,0);

					hit.transform.parent = null;
					hit.transform.parent = arm;
					hit.transform.position = position;
					hit.transform.rotation = rotation;
				}
			}
		} else {
			showEquip = false;
		}
		//Draw LINE along raycast Debug.DrawLine(playerCamera.transform.position,hit.point,Color.green);
	}
	public void OnGUI(){
		if (showEquip){
			GUI.Label (new Rect(Screen.width/2-50, Screen.height-(Screen.height/4), 100, 50), "Press E to Equip");
		}
	}
}
