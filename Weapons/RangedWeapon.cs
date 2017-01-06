/* Created by Daniel Harasymiw and Robert Bialy
 * 
 * Script for a ranged weapon that shoots a projectile
 * 
 * 
 */

using UnityEngine;
using System.Collections;

public class RangedWeapon : Weapon {

	//Stats for the Ranged Weapon
	public float damage; //how much damage the weapon deals
	public float timeBetweenAttacks; //time it takes to reload in ms
	public float meleeDamage; //how much melee damage the weapon will deal
	public float chargedAttackModifier;
	public float bulletVelocity;
	float currentDamage;
	GameObject projectile;

	//Sets up the stats of the weapon
	void Start () {
		damage = 40f;
		timeBetweenAttacks = 2000f;
		meleeDamage = 10;
		chargedAttackModifier = 2;
		bulletVelocity = 300;
	}

	void Update(){
		timeBetweenAttacks -= Time.deltaTime;
	}



	//Called by the player's Attack method, depending on the type of attack, this method will call the proper attack method
	public override void Attack(int type){

		if (timeBetweenAttacks < 0) {
			if (type == 0) {
				ShootWeapon();
			} else if (type == 1) {
				MeleeAttack ();
			} else {
				ChargedAttack ();
			}
		}

	}

	//Raises the weapon in an attempt to block incoming attacks
	public override void Block() {
		Debug.Log ("RangedWeapon - Block");
		//Start block animation
	}

	//Projectile attack that deals a moderate amount of damage
	void ShootWeapon(){
		Debug.Log ("RangedWeapon - Shoot");
		/* Start attack animation
		 * Create instance of bullet
		 * Set the damage of the bullet to damage
		 * Add velocity to bullet instance
		 */
	}

	//Attempt to hit the enemy with your weapon
	void MeleeAttack(){
		Debug.Log ("RangedWeapon - Melee");
		currentDamage = meleeDamage;
		//Do attack animation
		//Set the currentDamage back to 0 when the animation is complete
	}

	//A projectile attack that takes longer to perform but deals more damage and causes enemies to stagger?
	void ChargedAttack() {
		Debug.Log ("RangedWeapon - Charged Attack");
		/* Start attack animation
		 * Create instance of bullet
		 * Set the damage of the bullet equal to damage * chargedAttackModifier
		 * Add velocity to bullet instance
		 */

	}

	public override void dealDamage(GameObject target){
	}



}
