/* Created by Daniel Harasymiw and Rob Bialy
 * A script to be attached to a sword that allows the use of both attacks and blocks
 */

using UnityEngine;
using System.Collections;
public class Sword : MeleeWeapon {


	private string[] animations;
	//Initialize the weapons stats
	void Start(){
		damage = 15.0f;
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
		
		animations[0] = "Armature|SwordSwing";
		animations[1] = "Armature|Stab";
		animations[2] = "Armature|Overhead";
		
		
	}


	void Update(){
		if (attacking) {
			if (!owner.GetComponent<Animation>() ["Armature|SwordSwing"].enabled && !owner.GetComponent<Animation>()["Armature|Stab"].enabled && !owner.GetComponent<Animation>()["Armature|Overhead"].enabled) {
				//set attacking and all of the attack modifiers back to false
				attacking = false;
				stabbing = false;
				animationPlaying = "";
				enemiesHit.Clear (); //Empty the array list and forget about all of the people that we hit with the last swing
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
			photonView.RPC ("setAttackAnimation", PhotonTargets.All, new object[]{type});
		}
		return true;
	}

	[RPC]
	public void setAttackAnimation(int type){
		owner.GetComponent<Animation>().Play (animations[type]);
		animationPlaying = animations[type];
	}


	//Make the weapon do the block animation
	public override void Block(){
		photonView.RPC ("setBlockAnimation", PhotonTargets.All);
	}
	
	[RPC]
	public void setBlockAnimation(){
		owner.GetComponent<Animation>().Play ("Armature|Block");
		blocking = true;
	}
	
	
	
}
