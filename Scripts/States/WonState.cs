/* Created by Daniel Harasymiw, Robert Bialy
 * Created on June 3rd, 2014
 * Last Updated: June 3rd, 2014
 * 
 * State for when the player wins the game that displays a Victory screen to the player
 */


using UnityEngine;
using System.Collections;
using Assets.Code.States;
using Assets.Code.Interfaces;
public class WonState : StateBase {
	private StateManager manager;

	public WonState (StateManager manager){
		this.manager = manager;
		Debug.Log ("Won State");
	}

	void Start () {
	
	}



	public void StateUpdate(){
		if (Input.GetKeyDown("space")) {
			manager.SwitchState(new LostState(manager));	
		}
	}

	public void ShowGUI(){
		
	}

}
