using UnityEngine;
using System.Collections;

using Assets.Code.Scripts.AI.ENemies.Default;
public class EnemyStateManager : Photon.MonoBehaviour {

	public EnemyStateBase activeState;
	public NavMeshAgent navMeshAgent;
	public bool regrouping;
	public Transform target; //AI target that they're going to
	public float fieldOfView; //the field of view of the AI
	public float sightDistance; //how far the ai can see
	public float attackDistance; //how close the AI has to be to attack
	public float timeBetweenAttacks; //how long it takes between attacks
	public float attackTimer; //increments over time, when it reaches timeBetweenAttacks the AI can attack, and the timer goes back to 0 
	public float scanTimer;
	public GameObject[] playerObject; //List of available targets
	public GameObject[] friendly; //List of fellow Allies
	public float followDistance;
	private bool stateSetElsewhere = false;

	//Animation Variables
	public Transform upperBody;
	Vector3 prevPosition;
	
	public DefendingState defState; //the original defense state that the AI was in
	public State startingState; //state that the AI will start in
	public Character myStats; //contains the stats of the enemy the ai is controlling
	
	public enum DefendingState{
		guarding,
		patrolling,
		wandering
	}
	
	public enum State{
		guarding,
		patrolling,
		wandering,
		attacking,
		retreating,
		fleeing,
		runOut
	}
	
	void Start(){
		playerObject = GameObject.FindGameObjectsWithTag ("Player");
		friendly = GameObject.FindGameObjectsWithTag ("Person");
		if (!PhotonNetwork.isMasterClient) {
			gameObject.GetComponent<EnemyStateManager>().enabled = false;
		}
		navMeshAgent = GetComponent<NavMeshAgent>();
		myStats = gameObject.GetComponent<Character>(); //get the stats
		
		attackTimer = timeBetweenAttacks; //so the AI can attack as soon as possible
		scanTimer = 10.0f;
		prevPosition = transform.position;

		if (!stateSetElsewhere){
			switch(startingState){
			case State.guarding:
				activeState = new GuardingState(this);
				Debug.Log ("State: " + activeState);
				break;
			case State.patrolling:
				activeState = new PatrollingState(this);
				break;
			case State.wandering:
				activeState = new WanderingState(this);
				break;
			case State.attacking:
				activeState = new AttackingState(this);
				break;
			case State.retreating:
				activeState = new RetreatingState(this);
				break;
			case State.fleeing:
				activeState = new FleeingState(this);
				break;
			case State.runOut:
				activeState = new RunOutState(this);
				break;
			default:
				activeState = new GuardingState(this);
				break;
			}
		}

		//Animation mixing
		//animation ["attack"].layer = 2;
		//animation ["attack"].AddMixingTransform (upperBody);
	}
	
	void Update () {
		friendly = GameObject.FindGameObjectsWithTag ("Person");
		playerObject = GameObject.FindGameObjectsWithTag ("Player");
		if (playerObject.Length == 0) {
			playerObject = null;
			gameObject.GetComponent<Animation>().Play ("dance");
		}
		//if the previous position is equal to the current position, AI is stnading still so do Idle animation
		if (prevPosition == transform.position) {
			photonView.RPC ("setIdleAnimation", PhotonTargets.All);
		} 
		else { //if the previous position is different than the current position than the AI must be moving so do the walking animation
			photonView.RPC ("setRunAnimation", PhotonTargets.All);
		}
		//Initialize the navMeshAgent if it hasn't been
		if (navMeshAgent == null){
			navMeshAgent = GetComponent<NavMeshAgent>();
		}
		
		activeState.StateUpdate(); //call the currently active state's update method
		prevPosition = transform.position; //update previous position
	}

	[RPC]
	public void setRunAnimation(){
		GetComponent<Animation>().Blend("run",1.0f,0.1f);
		GetComponent<Animation>().Blend("idle",0.0f,0.1f);
	}
	[RPC]
	public void setIdleAnimation(){
		GetComponent<Animation>().Blend ("idle",1.0f,0.1f);
		GetComponent<Animation>().Blend("run",0.0f,0.1f);
	}

	//Method that change's the AI's current active state
	public void SwitchState(EnemyStateBase newState){
		activeState = newState;
	}
	//Checks if the player is visible to the AI
	public Transform isTargetVisible(){
		if (playerObject!=null){ //check to see if there are possible targets
			RaycastHit hit; //stores the linecast's information
			Vector3[] direction = new Vector3[playerObject.Length]; //stores all possible target directions
			float[] angle = new float[playerObject.Length]; //stores all possible target angles

			//Populate all information related to possible targets
			for(int i = 0; i < playerObject.Length; i ++){
				direction[i] = playerObject[i].transform.position - transform.position;
				angle[i] = Vector3.Angle(direction[i], transform.forward); 
			}

			//If only one target present, check if the player is within the AI's field of view
			if(playerObject.Length == 1){
				if (angle[0] < fieldOfView / 2f){
					//check if the player is close enough to be seen
					if (Vector3.Distance (transform.position, playerObject[0].transform.position) < sightDistance){
						//do a linecast between the player and the AI to see if there are any objects between them
						if (Physics.Linecast (transform.position, playerObject[0].transform.position, out hit)){
							if (hit.collider.tag == "Player"){
								return hit.collider.transform;	
							}
						}
					}		
				}
			}
			//Following tests for best possible target
			else if(playerObject.Length > 1){
				int targetToAttack = -1;
				int closest = -1;
				int weakest = -1;
				if(Vector3.Distance(transform.position, playerObject[0].transform.position) < Vector3.Distance(transform.position,playerObject[1].transform.position)){
					closest = 0;
				}
				else {
					closest = 1;
				}
				if(playerObject[0].GetComponent<Character>().health <= (playerObject[1].GetComponent<Character>().health - 35)){
					weakest = 0;
				}else if ((playerObject[0].GetComponent<Character>().health - 35) >= playerObject[1].GetComponent<Character>().health){
					weakest = 1;
				}
				if((weakest == 1) && (closest == 1)){
					targetToAttack = 1;
				}
				else if ((weakest == 0) && (closest == 0)){
					targetToAttack = 0;
				}
				else{
					targetToAttack = Random.Range(0,2);
				}
				if(playerObject[0].GetComponent<Character>().health <= (playerObject[1].GetComponent<Character>().health - 55)){
					targetToAttack = 0;
				}else if ((playerObject[0].GetComponent<Character>().health - 55) >= playerObject[1].GetComponent<Character>().health){
					targetToAttack = 1;
				}
				Debug.Log("TargetToAttack " + targetToAttack);
				/*-- CURRENTLY SAME AS SINGLE TARGET --*/
				if (angle[targetToAttack] < fieldOfView / 2f){
					//check if the player is close enough to be seen
					if (Vector3.Distance (transform.position, playerObject[targetToAttack].transform.position) < sightDistance){
						//do a linecast between the player and the AI to see if there are any objects between them
						if (Physics.Linecast (transform.position, playerObject[targetToAttack].transform.position, out hit)){
							if (hit.collider.tag == "Player"){
								return hit.collider.transform;	
							}
						}
					}		
				}
				/*-- CURRENTLY SAME AS SINGLE TARGET --*/
			}
		}
		return null;
	}	
	
	public void setState(EnemyStateBase state){
		stateSetElsewhere = true;
		activeState = state;
	}
}
