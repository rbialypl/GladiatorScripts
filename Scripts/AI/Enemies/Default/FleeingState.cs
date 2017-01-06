using UnityEngine;
using System.Collections;
using Assets.Code.Scripts.AI.ENemies.Default;
public class FleeingState : EnemyStateBase {

	private EnemyStateManager manager;
	public float deathTimer = 5.0f;
	private GameObject safepoint;
	private Vector3 runAwayDirection;
	public FleeingState(EnemyStateManager manager){
		this.manager = manager;
	}
	
	// Update is called once per frame
	public void StateUpdate () {
		safepoint = GameObject.Find ("Safepoint");
		manager.navMeshAgent.SetDestination (safepoint.transform.position);
		if (Vector3.Distance (manager.gameObject.transform.position, safepoint.transform.position) < 20.0f) {
			manager.gameObject.GetComponent<Animation>().Play ("dance");
			deathTimer -= Time.deltaTime;
			if (deathTimer < 0){
				manager.myStats.health = -1;
			}
		}
		/*
		if (ai becomes grouped up){
			manager.SwitchState(new RetreatingState());
		}
		*/
	}
}
