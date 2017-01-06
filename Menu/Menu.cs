/* Created by Daniel Harasymiw 
 * Last updated: Jan 14, 2014
 * This script handles the switching of panels of the GUI by setting certain panels active based on the method being called
 * The methods in this script will be called by button clicks 
*/

using UnityEngine;
using System.Collections;

public class Menu : Photon.MonoBehaviour {

	// Use this for initialization
	public GameObject loadingMenuPanel;
	public GameObject mainMenuPanel;
	public GameObject hostGamePanel;
	public GameObject joinGamePanel;
	public GameObject clientLobbyPanel;
	public GameObject hostLobbyPanel;
	public Canvas canvas;
	public PunNetworkManager networkManager;
	
	private GameObject activePanel;
	private bool isServerListReturned;
	private bool serverButtonsCreated = false;
	
	void Start(){
		activePanel = loadingMenuPanel;
		mainMenuPanel.SetActive (false);
		hostGamePanel.SetActive(false);
		joinGamePanel.SetActive (false);
		clientLobbyPanel.SetActive (false);
		hostLobbyPanel.SetActive (false);
		networkManager = GameObject.Find ("GameManager").GetComponent<PunNetworkManager>();
		isServerListReturned = false;
	}
	
	public void switchToMainMenu(){
		activePanel.SetActive(false);
		activePanel = mainMenuPanel;
		activePanel.SetActive (true);

	}
	
	public void switchToHostGame(){
		activePanel.SetActive(false);
		activePanel = hostGamePanel;
		activePanel.SetActive (true);
	}
	
	public void switchToJoinGame(){
		activePanel.SetActive(false);
		activePanel = joinGamePanel;
		activePanel.SetActive (true);
	}	

	public void switchToClientLobby(){
		Debug.Log ("Switching to client lobby");
		activePanel.SetActive (false);
		activePanel = clientLobbyPanel;
		activePanel.SetActive (true);
	}

	public void switchToHostLobby(){
		activePanel.SetActive (false);
		activePanel = hostLobbyPanel;
		activePanel.SetActive (true);
	}
		

}
