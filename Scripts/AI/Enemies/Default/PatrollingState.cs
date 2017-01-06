using UnityEngine;
using System.Collections;
using Assets.Code.Scripts.AI.ENemies.Default;


public class PatrollingState : EnemyStateBase {

	private EnemyStateManager manager;
	private Transform target;
	public PatrollingState(EnemyStateManager manager){
		this.manager = manager;
		manager.navMeshAgent.SetDestination (manager.target.transform.position);
	}
	public void StateUpdate () {
		target = manager.isTargetVisible();
		if (target != null){
			manager.SwitchState (new AttackingState(manager));
		}
	}
}
