/* Created by Dan Harasymiw and Rob Bialy
 * A trap that deals damage when the player falls into it
 */ 

using UnityEngine;
using System.Collections;

public class Acid : MonoBehaviour {

	public float damage;
	
	void OnTriggerEnter(Collider other){
		if (other.tag == "Person" || other.tag == "Player"){
			Character stats = other.GetComponent<Character>();
			stats.health-=damage;
		}
	}	
	
	void OnTriggerStay(Collider other){
		if (other.tag == "Person" || other.tag == "Player"){
			Character stats = other.GetComponent<Character>();
			stats.health-=damage;
		}
		
	}
	
}
