/* Created by Daniel Harasymiw and Robert Bialy
 * State for when the AI is trying to attack the player
 */ 
	
using UnityEngine;
using System.Collections;
using Assets.Code.Scripts.AI.ENemies.Default;

public class AttackingState : EnemyStateBase{

	private EnemyStateManager manager;

	public AttackingState(EnemyStateManager manager){
		this.manager = manager;
		this.manager.regrouping = false;
		Debug.Log ("ATTACKING NOW");
		if (manager.target == null) {
			manager.target = manager.isTargetVisible ();
		}
	}
	
	public void StateUpdate () {
		manager.scanTimer -= Time.deltaTime;
		if (manager.scanTimer < 0){
			manager.target = manager.isTargetVisible ();
			manager.scanTimer = Random.Range(10.0f,15.0f);
		}
		//Run towards enemy
		if ((manager.playerObject != null)){
			manager.attackTimer -= Time.deltaTime; //decrement the time until the next attack
			float distance = Vector3.Distance (manager.gameObject.transform.position, manager.target.transform.position); //calculates the distance between the AI and the player
			
			if ((distance < manager.attackDistance) && (manager.attackTimer < 0)){//if the AI is in range and enough time has passed for the AI to attack
				manager.myStats.equippedWeapon.Attack (0); //attack with a random attack
				manager.attackTimer = Random.Range (0.5f, 4f); //set a random time for the AI's next possible attack time
			}
			//stop trying to get to the player when close enough to attack
			if (isAttackable()){ //if the player is attackable stop moving
				manager.navMeshAgent.enabled = false;
			}
			else {//if the ai can't attack the player move towards the player
				manager.navMeshAgent.enabled = true;
				manager.navMeshAgent.SetDestination (manager.target.transform.position);
			}
			//if AI's health has dropped below 0, try to run away
			if (manager.myStats.health <= 15){
				manager.SwitchState(new FleeingState(manager));
			}
		}
		else {
			manager.gameObject.GetComponent<Animation>().Play ("dance");
		}
		//if the target has died
		/*if (manager.target == null){
			//go back to the defense state that the AI was in before
			Switch(manger.defState){
			case guarding: 
				manager.SwitchState(new GuardingState());
				break;
			case patrolling:
				manager.SwitchState(new PatrollingState());
				break;
				
			case wandering:
				manager.SwitchState(new WanderingState());
				break;
			}
		} */

	}
	//checks if the AI's target is close enough to attack and that the player is at an angle in which the AI's attacks would be able to hit the player
	bool isAttackable(){
		Vector3 direction = manager.target.transform.position - manager.transform.position;
		float angle = Vector3.Angle (direction, manager.transform.forward); 
		if (angle < 45){
			//Debug.Log ("Distance: " + Vector3.Distance (manager.transform.position,manager.playerObject.transform.position));
			if (Vector3.Distance (manager.transform.position, manager.target.transform.position) < manager.followDistance){
				return true;	
			}		
		}
		return false;
		
	}
	
}
