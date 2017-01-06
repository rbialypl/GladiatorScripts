/* Created by Rob Bialy and Dan Harasymiw
 * Simple GUI that displays the player's health, and the enemy's health
 */

using UnityEngine;
using System.Collections;
//using Assets.Code.Interfaces;

public class HUD : MonoBehaviour {

	public GUIText statusBar;
	public GameObject player;
	Character stats;
	
	// Use this for initialization
	void Start () {
		stats = player.GetComponent<Character>();
	}
	// Update is called once per frame
	void Update () {
		if (stats != null){
			statusBar.text = player.name + ": " + Mathf.Round(stats.health).ToString() + " Energy: " + Mathf.Round (stats.energy).ToString();
		}
		else {
			Destroy(this.gameObject);
		}
	}
}
