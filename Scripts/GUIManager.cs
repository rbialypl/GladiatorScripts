using UnityEngine;
using System.Collections;

public class GUIManager : Photon.MonoBehaviour {
	StateManager manager;	
	public PhotonView photonView;
	int fontSize = 70;
	
	bool displayTimer = false;
	float timeLeft;
	
	
	
	void Start(){
		manager = GameObject.Find ("GameManager").GetComponent<StateManager>();
		photonView = gameObject.GetComponent<PhotonView>();
	}
	
	void Update(){
		if (displayTimer){
			timeLeft -= Time.deltaTime;
		}
	}
	
	[RPC]
	public void displayCountdownTimer(float timeLeft){
		this.timeLeft = timeLeft;
		displayTimer = true;
	}
	
	public void OnGUI(){
		if (displayTimer){
			float timeLeftDecimal = Mathf.Ceil (timeLeft) - timeLeft;
			
			manager.style.fontSize = 20;
			Color fadeColour = new Color(manager.style.normal.textColor.r, manager.style.normal.textColor.g, manager.style.normal.textColor.b, 100);
			manager.style.normal.textColor = fadeColour;
			GUI.Label (new Rect(Screen.width/2-250, Screen.height/2 - 100, 500, 50), "NEXT WAVE STARTS IN", manager.style);
			
			manager.style.fontSize = fontSize - (Mathf.RoundToInt (fontSize * timeLeftDecimal));
			if (timeLeft <= 3){
				manager.style.fontSize += 80;
			}
			else {
				manager.style.fontSize += 20;
			}
			fadeColour.a = 1 - (timeLeftDecimal) + 0.2f;
			manager.style.normal.textColor = fadeColour;
			
			
			GUI.Label (new Rect(Screen.width/2-50, Screen.height/2, 100, 50), Mathf.Ceil (timeLeft).ToString (), manager.style);
			
			if (timeLeft <= 0){
				displayTimer = false;
				manager.photonView.RPC ("setGuiRpcCalled", PhotonTargets.MasterClient, false);
			}
		}
		
	}
	

}
