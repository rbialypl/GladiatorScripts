using UnityEngine;
using System;
using System.Collections;
using Assets.Code.Scripts.AI.ENemies.Default;
public class RunOutState : EnemyStateBase {
	
	private EnemyStateManager manager;
	private GameObject waypoint;
	public RunOutState(EnemyStateManager manager){
		this.manager = manager;
		manager.fieldOfView = 360;
	}
	
	public void StateUpdate () {
		if (manager.friendly.Length > 1) {
			manager.SwitchState (new RetreatingState (manager));
		} else {
			manager.target = manager.isTargetVisible (); //look for target
			if (manager.target != null) {//if target isn't null then attack!
					manager.SwitchState (new AttackingState (manager));
			}
			if (waypoint == null) {
					waypoint = GameObject.Find ("Waypoint");
			} else {
					if (manager.navMeshAgent.isOnOffMeshLink) {
							try {
									manager.navMeshAgent.SetDestination (waypoint.transform.position);
							} catch (Exception e) {
			
							}
					}	
			}
		}
	}

}
