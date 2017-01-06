using UnityEngine;
using System.Collections;
using Assets.Code.Scripts.AI.ENemies.Default;
public class WanderingState : MonoBehaviour, EnemyStateBase {
	
	private EnemyStateManager manager;
	
	public WanderingState(EnemyStateManager manager){
		this.manager = manager;
	}
	// Update is called once per frame
	public void StateUpdate () {
	
	}
}
