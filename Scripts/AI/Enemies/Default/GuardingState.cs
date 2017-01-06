using UnityEngine;
using System.Collections;
using Assets.Code.Scripts.AI.ENemies.Default;
public class GuardingState : EnemyStateBase {
	
	private EnemyStateManager manager;
	private float prevHealth;
	public GuardingState(EnemyStateManager manager){
		this.manager = manager;
		prevHealth = manager.myStats.health;
	}
	public void StateUpdate () {
		manager.target = manager.isTargetVisible (); //look for target
		if (prevHealth != manager.myStats.health){ //check if we got damaged
			manager.target = manager.playerObject[0].transform;
		}
		if (manager.target != null){//if target isn't null then attack!
			manager.SwitchState (new AttackingState(manager));
		}
	}
}
