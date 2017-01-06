using UnityEngine;
using System.Collections;
using Assets.Code.Scripts.AI.ENemies.Default;
public class RetreatingState : MonoBehaviour, EnemyStateBase {

	private EnemyStateManager manager;
	private float health;
	Vector3 regroupPoint;
	float distanceFromPoint;
	public RetreatingState(EnemyStateManager manager){
		this.manager = manager;
		health = manager.myStats.health;
	}
	
	// Update is called once per frame
	public void StateUpdate () {
		manager.regrouping = true;
		//Check if person is being attacked during regroup and change to closest target if need be
		if (health != manager.myStats.health) {
				//If only single player just set the target to the one player
				if(manager.playerObject.Length == 1){
					manager.target = manager.playerObject[0].transform;
				}
				//Otherwise multiple playerrs so choose the player closest to attack
				else{
				if(Vector3.Distance(manager.gameObject.transform.position, manager.playerObject[0].transform.position) < Vector3.Distance(manager.gameObject.transform.position, manager.playerObject[1].transform.position)){
						manager.target = manager.playerObject[0].transform;
					}
					else{
						manager.target = manager.playerObject[1].transform;
					}
				}
				manager.SwitchState (new AttackingState (manager));
		}
		else{
			//Calcaulte Average Distance between all fellow allies and run to this point
			regroupPoint = getRegroupPoint ();
			manager.navMeshAgent.enabled = true;
			manager.navMeshAgent.SetDestination (regroupPoint);
			distanceFromPoint = Vector3.Distance (manager.gameObject.transform.position, regroupPoint);
			if (distanceFromPoint < 20.0f) {
				int count = manager.friendly.Length;
				int friendliesReady = 0;
				manager.navMeshAgent.enabled = false;
				for(int i = 0; i < count; i++){
					if((Vector3.Distance (manager.friendly[i].transform.position, regroupPoint) < 24.0f) || manager.friendly[i].GetComponent<EnemyStateManager>().regrouping == false){
						friendliesReady ++;
					}
				}
				if(friendliesReady == manager.friendly.Length){
					manager.SwitchState (new AttackingState (manager));
				}
			}
			/*
			if (multiple AI has become regrouped and their morale is high enough){
				manager.SwitchState(new AttackingState());
			}
			*/
		}
	}
	public Vector3 getRegroupPoint(){
		Vector3 center = new Vector3 (0, 0, 0);
		int count = manager.friendly.Length;
		for (int i = 0; i < count; i++) {
			center += manager.friendly[i].transform.position;
		}
		center /= count;
		return center;
	}
}
