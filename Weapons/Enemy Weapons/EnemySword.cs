/* Created by Daniel Harasymiw and Rob Bialy
 * A script to be attached to a sword that allows the use of both attacks and blocks
 */

using UnityEngine;
using System.Collections;
public class EnemySword : MeleeWeapon {
	
	
	private string[] animations;
	//Initialize the weapons stats
	void Start(){
		damage = 30f;
		stabDamageModifier = 0.2f;
		bleedDamageModifier = 0.033f;
		overheadDamageModifier = 1.5f;
		
		energyCost = 20f;
		stabEnergyModifier = 0.5f;
		overheadEnergyModifier = 1.5f;
		
		attacking = false;
		currentAttackDamage = 0;
		
		//instead of checking what animation to do each time an attack is done, just set the animations now
		animations = new string[3];
		
		animations[0] = "attack";
		animations[1] = "attack";
		animations[2] = "attack";
		
	}
	
	
	void Update(){
		if (attacking) {
			
			if (!owner.GetComponent<Animation>()["attack"].enabled){
				attacking = false;
				stabbing = false;
				animationPlaying = "";
				enemiesHit.Clear ();
			}
			
		}
		if (blocking) { //if the person is blocking, do the block animation
			if(!owner.GetComponent<Animation>()["Armature|Block"].enabled){
				blocking = false;
			}
		}
	}
	
	//Gets the weapon to play the specific attack animation and call the melee weapon's attack method to deal damage
	public override bool Attack(int type){
		//call the attack method
		if (base.Attack (type)) {
			owner.GetComponent<Animation>().Play (animations[type]);
			animationPlaying = animations[type];
		}
		return true;
	}
	
	//Make the weapon do the block animation
	public override void Block(){
		owner.GetComponent<Animation>().Play ("Armature|Block");
		blocking = true;
	}
	
	
	
	
	
}