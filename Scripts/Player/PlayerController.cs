using UnityEngine;
using System.Collections;


public class PlayerController : Photon.MonoBehaviour {

	public float movementSpeed = 5.0f;
	public float mouseSensitivity = 5.0f;
	public float jumpSpeed = 20.0f;
	float rotateVertical = 0;
	public float upDownRange = 60.0f;
	private float verticalVelocity = 0;
	CharacterController controller;
	GameObject camera;
	Character character;
	GameObject characterModel;
	

	//animation variables
	public Transform upperBody;
	
	void Start () {
		camera = GameObject.Find("Camera");
		Screen.lockCursor = true;
		controller = GetComponent<CharacterController> ();

		//Stats of the the player
		character = gameObject.GetComponent<Character>();
		character.movementSpeedModifier = 1;
		character.damageModifier = 1;
	
		//Add animations speeds
		GetComponent<Animation>() ["Armature|Stab"].speed = 1.5f;
		GetComponent<Animation>() ["Armature|Overhead"].speed = 1.7f;
		GetComponent<Animation>() ["Armature|SwordSwing"].speed = 2.0f;
		GetComponent<Animation>() ["Armature|Block"].speed = 2.0f;

		//Animation mixing to account for additional animations

		GetComponent<Animation>() ["Armature|Stab"].layer = 2;
		GetComponent<Animation>() ["Armature|Overhead"].layer = 2;
		GetComponent<Animation>() ["Armature|SwordSwing"].layer = 2;
		GetComponent<Animation>() ["Armature|Block"].layer = 2;

		GetComponent<Animation>() ["Armature|SwordSwing"].AddMixingTransform (upperBody);//walking and swordswing
		GetComponent<Animation>() ["Armature|Stab"].AddMixingTransform (upperBody);//walking and stab
		GetComponent<Animation>() ["Armature|Overhead"].AddMixingTransform (upperBody);//walking and overhead
		GetComponent<Animation>() ["Armature|Block"].AddMixingTransform (upperBody);//walking and blocking




	}
	
	void Update () {
		//Lets the player look around using the mouse
		float rotateHorizontal = Input.GetAxis ("Mouse X") * mouseSensitivity;
		transform.Rotate (0, rotateHorizontal, 0);

		rotateVertical -= Input.GetAxis ("Mouse Y") * mouseSensitivity;
		rotateVertical = Mathf.Clamp (rotateVertical, -upDownRange, upDownRange);
		camera.transform.localRotation = Quaternion.Euler (rotateVertical, 0, 0);

		//Lets the player move around using W, A, S, D
		float forwardSpeed = Input.GetAxis ("Vertical") * movementSpeed;
		float sideSpeed = Input.GetAxis ("Horizontal") * movementSpeed;

		verticalVelocity += Physics.gravity.y * Time.deltaTime;

		if (controller.isGrounded && Input.GetButton ("Jump")) {
			verticalVelocity = jumpSpeed;
		}

		Vector3 speed = new Vector3 (sideSpeed, verticalVelocity, forwardSpeed);
		speed = transform.rotation * speed;
		controller.Move (speed * Time.deltaTime);

		if (Input.GetKey ("w")) {
			photonView.RPC("setRunAnimation", PhotonTargets.All);

		} else {
			photonView.RPC ("setIdleAnimation", PhotonTargets.All);
		}



		if (Input.GetMouseButtonUp (0)) {
			character.Attack (0);
		} else if (Input.GetMouseButtonUp (1)) {
			character.Block ();
		}
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0){
			character.Attack(2);
		}
		else if (Input.GetAxis ("Mouse ScrollWheel") > 0){
			character.Attack (1);
		}
		//Swapping Weapons
		else if (Input.GetKeyDown ("1")) {
			character.EquipWeapon (character.primaryWeapon);
		}
		else if (Input.GetKeyDown ("2")){
			character.EquipWeapon (character.secondaryWeapon);
		}

	}

	[RPC]
	public void setRunAnimation(){
		GetComponent<Animation>().Blend("Armature|Walking",1.0f,0.1f);
		GetComponent<Animation>().Blend("Armature|Idle",0.0f,0.1f);
	}
	[RPC]
	public void setIdleAnimation(){
		GetComponent<Animation>().Blend ("Armature|Idle",1.0f,0.1f);
		GetComponent<Animation>().Blend("Armature|Walking",0.0f,0.1f);
	}

}
