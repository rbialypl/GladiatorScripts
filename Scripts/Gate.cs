using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {
	
	int playersInside = 0;
	int enemiesInside = 0;

	void OnTriggerEnter(Collider other){
		if ((other.tag == "Person" || other.tag == "Untagged") && enemiesInside == 0) {//AI triggered it open gate
			GetComponent<Animation>().Play ("MetalBars|Open");
			enemiesInside++;
		}
		else if (other.tag == "Player") {
			playersInside++;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == "Person") {
			enemiesInside--;
		}
		else if (other.tag == "Player") {
			playersInside--;
		}

		if (enemiesInside == 0 && playersInside == 0) {
			GetComponent<Animation>().Play ("MetalBars|Close");
		}

	}
	

}
