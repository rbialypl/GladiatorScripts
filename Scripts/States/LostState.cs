/* Created by Daniel Harasymiw, Robert Bialy
 * Created on June 3rd, 2014
 * Last Updated: June 3rd, 2014
 * 
 * State for when the player loses the game, this state gives the player the option to quit or to try again
 */


using UnityEngine;
using System.Collections;
using Assets.Code.States;
using Assets.Code.Interfaces;

public class LostState : StateBase {
	private StateManager manager;

	public LostState (StateManager manager){
		this.manager = manager;
		Debug.Log ("Lost State");
		
	}


	// Use this for initialization
	void Start () {
	
	}

	public void StateUpdate(){
		if (Input.GetKeyDown ("space")) {
			manager.SwitchState(new ArenaState(manager));	
		}
	}

	public void ShowGUI(){

	}



	

}
