using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadingMenu : Photon.MonoBehaviour {
	float time;
	float nextTime;
	float timeLength;
	int counter;
	public GameObject loadingTextGO;
	Text loadingText;
	public GameObject menu;

	// Use this for initialization
	void Start(){
		loadingText = loadingTextGO.GetComponent<Text> ();
		counter = 0;
		time = 0;
		timeLength = 0.1f;
		nextTime = timeLength;
	}

	void Update(){
		if (PhotonNetwork.connectedAndReady) {
			menu.GetComponent<Menu>().switchToMainMenu();
		}
		time += Time.deltaTime;

		if (time > nextTime){
			if (counter == 4){
				loadingText.text = "LOADING";
			}
			else {
				loadingText.text += ".";
			}
			
			counter++;
			nextTime = time + timeLength;
		}

	}
}
