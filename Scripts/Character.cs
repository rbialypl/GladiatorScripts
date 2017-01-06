/* Created by Dan Harasymiw and Rob Bialy
 * Tracks an object's stats such as health, energy, weapons they have equipped
 * 
 */

using UnityEngine;
using System.Collections;

public class Character : Photon.MonoBehaviour {
	
	public float health;
	public float energy;
	public float energyRegenRate;
	public float movementSpeedModifier;
	public float damageModifier;
	public bool hasShield;
	public MeleeWeapon equippedWeapon;
	public MeleeWeapon primaryWeapon;
	public MeleeWeapon secondaryWeapon;
	
	public bool isPlayer;
	
	public Transform spawnPoint;
	public GameObject spawnObject;
	
	public bool respawned;
	
	private bool  energyDelay;
	private bool isDead;
	private GameObject cam;
	
	public GUIContent healthbarTexture;
	public GUIContent energybarTexture;
	
	
	
	void Start(){
		energyDelay = false;
		respawned = false;
		isDead = false;
		cam = GameObject.Find ("Camera");
		
	}
	
	void Update(){
		if (energy <= 100) {
			energy += Time.deltaTime * energyRegenRate;
		}
		else {
			energy = 100;
		}
		if (health < 1){
			if (tag == "Person"){
				if (!isDead){
					isDead = true;
					GetComponent<Animation>().Play ("die");
				}
				else if (!gameObject.GetComponent<Animation>()["die"].enabled){
					PhotonNetwork.Destroy (gameObject);
				}
				
			}
			else {
				isDead = true;
				if (photonView.isMine){
					cam.transform.SetParent(null);
				}

				PhotonNetwork.Destroy (gameObject);
				StateManager stateManager = GameObject.Find ("GameManager").GetComponent<StateManager>();
				if (stateManager != null){
					if (photonView.isMine){
						stateManager.standbyCamera.SetActive (true);
						
					}
					
					if (!PhotonNetwork.isMasterClient && !PhotonNetwork.inRoom){
						stateManager.SwitchState (new LostState(stateManager));
						Application.LoadLevel ("GameOver");
					}
					else {
						stateManager.photonView.RPC ("removePlayer", PhotonTargets.MasterClient);
					}
					
				}
				
			}
			
			
		}
	}
	
	/*
	void OnGUI(){
		GUI.Label (new Rect(50, Screen.height - 50, health, 50), healthbarTexture);
		GUI.Label (new Rect(200, Screen.height - 50, energy, 50), energybarTexture);
	}
	*/
	
	
	public void Attack(int type) {
		equippedWeapon.Attack(type);
	}
	
	public void Block(){
		equippedWeapon.Block ();
	}
	
	public void EquipWeapon(MeleeWeapon wep){
		if (equippedWeapon != wep) {
			equippedWeapon = wep;
		}
	}
	
	//Allows the player to pick up a new weapon
	public void PickUpWeapon(MeleeWeapon wep){
		if (equippedWeapon == primaryWeapon) {
			primaryWeapon = wep;
		}
		else {
			secondaryWeapon = wep;
		}
		equippedWeapon = wep;
	}
	
	[RPC]
	public void takeDamage(float damage){
		health -= damage;
	}
	
}
