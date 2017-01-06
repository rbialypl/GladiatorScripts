//Script for a basic melee weapon with three kinds of attacks, slash, stab, and overhead

using UnityEngine;
using System.Collections;

public class MeleeWeapon : Photon.MonoBehaviour{

	//Variable of the person/baddy/ w.e. is holding the weapon
	public Transform owner; 
	public Character stats;
	
	//damage stats for weapon
	public float damage; //the base damage of the weapon
	//damage modifies for the different attacks
	public float stabDamageModifier;
	public float bleedDamageModifier;
	public float overheadDamageModifier;

	//energystats for weapon
	public float energyCost;
	public float stabEnergyModifier;
	public float overheadEnergyModifier;
	public float blockingCost;

	public bool attacking; //whether or not the weapon is currently attacking
	public bool blocking; //whether or not the weapon is being used to block
	protected bool stabbing; //if the weapon is stabbing
	protected float currentAttackDamage; //what damage will be dealt to enemies if it makes contact
	
	public string animationPlaying;	//we need to keep track of what weapon animation is playing so that we can stop it if attack is blocked
									//There is no way to just get the animation that is playing since multiple animations can play at a time
	
	//the number of times the bleed will hurt the enemy
	int numberOfTicks = 10;
	//the total time in which the bleed will be on the player
	float totalBleedTime = 10f;
	
	public ArrayList enemiesHit = new ArrayList(); //This is used to keep track of who has been hit with an attack, otherwise an attack might hit multiple times for each time it collides

	void Start(){
		energyCost = 20f;
		stabEnergyModifier = 0.5f;
		overheadEnergyModifier = 1.5f;
		blockingCost = 10f;
		stats = owner.gameObject.GetComponent<Character> (); //get the stats of the user
	}


	//Called by the player script's Attack method
	//Performs an attack based on the type variable that is passed to this method
	public virtual bool Attack(int type){
		if (attacking == false) { //checks if not currently attacking, that way you can't do multiple attacks with one swing
			attacking = true; //set attacking to true so that damage can be dealt
			Debug.Log ("Attacking!!!");
			stats = owner.gameObject.GetComponent<Character> ();
			if (type == 0) {
				if (stats.energy - energyCost >= 0) {
					stats.energy -= energyCost;
					SlashAttack ();
					return true;
				}
			} else if (type == 1) {
				if (stats.energy - (energyCost * stabEnergyModifier) >= 0) {
					stats.energy -= energyCost * stabEnergyModifier;
					StabAttack ();
					return true;
				}
			} else {
				if (stats.energy - (energyCost * overheadEnergyModifier) >= 0) {
					stats.energy -= energyCost * overheadEnergyModifier;
					OverheadAttack ();	
					return true;
				}
			}
		}
		return false;
	}

	//Method called by the player script's Block method, causes the player to block incoming attacks with this weapon
	//Blocking slowly drains energy and causes the player to lose energy when they block an attack
	public virtual void Block(){
		if (!blocking){
			blocking = true;
			stats = owner.gameObject.GetComponent<Character>();
			stats.energy -= blockingCost;
		
		}
		
	}
	//Stabbing attack in front of the player
	//Deals little damage, causes a slight bleed (DoT) and causes the target's moves to cost more energy
	void StabAttack() {
		currentAttackDamage = damage * stabDamageModifier * stats.damageModifier;
		stabbing = true;
	}
	//A powerful vertical overhead attack
	//Takes longer for the attack to startup, but deals more damage than the other attacks
	void OverheadAttack() {
		currentAttackDamage = damage * overheadDamageModifier * stats.damageModifier;
	}
	//A horizontal swing in front of the player
	//Average attack, deals a normal amount of damage
	void SlashAttack() {
		currentAttackDamage = damage * stats.damageModifier;
	}
	//Remove health from the enemy that got hit
	public void dealDamage(GameObject target){
		Character targetStats = target.GetComponent<Character>(); //get the stats of the target

		if (targetStats.gameObject.tag != owner.tag) {
			targetStats.photonView.RPC ("takeDamage", PhotonTargets.All, currentAttackDamage);
			//targetStats.health -= currentAttackDamage; //reduce targets health
			if (stabbing) {
					StartCoroutine (ApplyBleedDamage (targetStats, totalBleedTime / numberOfTicks)); //apply the bleed effect to the target
					stabbing = false;
			}
		}
	}
	
	public virtual void stopAttack(){	
	}
	
	
	
	//applies bleed damage to the target
	//takes in the stats of the target so it can reduce its health
	//the number of ticks that will hurt the enemy
	//the total time in seconds that the bleed will be on the target for
	public IEnumerator ApplyBleedDamage(Character targetStats, float bleed){
		while (numberOfTicks > 0){
			yield return new WaitForSeconds(bleed);
			targetStats.health -= Mathf.Round(damage * bleedDamageModifier);
			numberOfTicks--;	
		}
	}
	//Method called when the weapon collides with a 'trigger', checks if the object it collided with is an enemy and deals damage accordingly
	void OnTriggerEnter(Collider coll){
		//if we are attacking
		if (attacking){
			if (!(coll.tag == "Player" && stats.isPlayer)){//dont let other players hurt players
				//if the object we colide with is a melee weapon and that weapon is blocking -- NOTE: MeleeWeapon might cause problems! maybe sword instead?
				if (coll.tag == "MeleeWeapon" && coll.gameObject.GetComponent<Sword>().blocking){
					owner.GetComponent<Animation>().Stop(animationPlaying);
					Debug.Log ("Block successfull");
				}
				
				//Check if the player is attacking, we're attacking something that is attackable, and if the person has bit been hit already with the current attack
				else if (coll.transform != owner && (coll.tag == "Person" || coll.tag == "Player") && !enemiesHit.Contains (coll)) {
					enemiesHit.Add (coll);
					dealDamage(coll.gameObject);
					Debug.Log ("ATTACK HIT");
				}
			}

		
		}
		
	}

}
