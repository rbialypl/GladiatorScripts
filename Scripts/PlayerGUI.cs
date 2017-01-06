using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerGUI : MonoBehaviour {
	public GameObject player;
	Character playerStats;
	public Slider healthSlider;
	public Slider energySlider;
	public GameObject healthbarFill;
	public GameObject energybarFill;
	float flashRate = 0.3f;
	
	Image healthbarColour;
	Image energybarColour;
	
	// Use this for initialization
	void Start(){
		playerStats = player.GetComponent<Character>();
		healthbarColour = healthbarFill.GetComponent<Image>();
		energybarColour = energybarFill.GetComponent<Image>();
	}
	void Update(){
		healthSlider.value = playerStats.health;
		if (playerStats.energy < 20){
			StartCoroutine (energybarFlash());
		}
		if (playerStats.health < 20){
			StartCoroutine (healthbarFlash());
		}
		energySlider.value = playerStats.energy;
	}
	
	IEnumerator healthbarFlash(){
		while (playerStats.health < 20){
			if (playerStats.health < 10){
				healthbarColour.color = Color.red;
				yield return new WaitForSeconds(flashRate/2);
				healthbarColour.color = Color.green;
				yield return new WaitForSeconds(flashRate/2);
			}
			else {
				healthbarColour.color = Color.red;
				yield return new WaitForSeconds(flashRate);
				healthbarColour.color = Color.yellow;
				yield return new WaitForSeconds(flashRate);
			}
		}
		
	}

	IEnumerator energybarFlash(){
		while (playerStats.energy < 20){
			if (playerStats.energy < 10){
				energybarColour.color = Color.red;
				yield return new WaitForSeconds(flashRate/2);
				energybarColour.color = Color.yellow;
				yield return new WaitForSeconds(flashRate/2);
			}
			else {
				energybarColour.color = Color.red;
				yield return new WaitForSeconds(flashRate);
				energybarColour.color = Color.yellow;
				yield return new WaitForSeconds(flashRate);
			}

		}
	}
	
}
