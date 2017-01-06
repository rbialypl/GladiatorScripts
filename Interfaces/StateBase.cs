/* Created by Daniel Harasymiw, Robert Bialy
 * Created on June 3rd, 2014
 * Last Updated: June 3rd, 2014
 * 
 * Interface for the States
 * Has two methods, StateUpdate that the StateManager can call to update the active state,
 * and ShowGUI which will display that active State's GUI to the screen
 */
namespace Assets.Code.Interfaces {
	public interface StateBase{
		void StateUpdate ();
		void ShowGUI ();
	}
}