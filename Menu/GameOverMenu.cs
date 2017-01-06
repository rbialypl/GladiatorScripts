using UnityEngine;
using System.Collections;

public class GameOverMenu : MonoBehaviour {
	StateManager manager;
	// Use this for initialization
	void Start () {
		manager = GameObject.Find ("GameManager").GetComponent<StateManager> ();
	}
	
	public void SwitchToMenu(){
		Destroy (manager);
		Application.LoadLevel ("Menu");
	}

	public void SwitchToArena(){
		Application.LoadLevel ("Arena");
	}

}
