/* Created by Daniel Harasymiw, Robert Bialy
 * Created on June 3rd, 2014
 * Last Updated: June 3rd, 2014
 * 
 * State that handles the main menu of the game
 * Menu will display the different game modes, "Options", and "Quit Game"
 */

using UnityEngine;
using Assets.Code.Interfaces;
namespace Assets.Code.States{
	public class MenuState : StateBase{

		private StateManager manager;

		public MenuState (StateManager manager){
			this.manager = manager;
		}

		public void StateUpdate(){


		}


		void StartArena(){
			Application.LoadLevel ("Arena");
			manager.SwitchState (new ArenaState (manager));
		}

		public void ShowGUI(){

		}

	}
}
                            