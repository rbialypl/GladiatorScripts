using UnityEngine;
using System.Collections;

public class Spikes : MonoBehaviour {

	public float damage = 33f;
	private float spikeTimer = 0;
	public float cooldown = 3.0f;
	
	void OnTriggerEnter(Collider other){
		if ((other.gameObject.tag == "Person" || other.gameObject.tag == "Player") && (Time.time > spikeTimer)) {
			GetComponent<Animation>().Play ("Up Down");
			spikeTimer = Time.time + cooldown;
			other.GetComponent<Character>().health -= damage;

		}
	}




}

