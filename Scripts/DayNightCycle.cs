using UnityEngine;
using System.Collections;

public class DayNightCycle : MonoBehaviour {

	// Use this for initialization
	int colorAmt = 2;
	Color skyboxColor;
	public bool dayTime;
	void Start () {
		dayTime= true;
		skyboxColor = new Color(0,0,0);
		RenderSettings.skybox.color = skyboxColor;
		
	}
	
	// Update is called once per frame
	void Update () {
		RenderSettings.skybox.color = skyboxColor;
		if (dayTime == true) 
		{
			transform.Rotate(Vector3.right * Time.deltaTime * 2);
			if(transform.eulerAngles.x > 269)
			{
				dayTime = false;
			}
			else if (((transform.eulerAngles.x >= 0) && (transform.eulerAngles.x <= 80)) && (GetComponent<Light>().intensity < 0.5) && transform.eulerAngles.y < 91) 
			{
				GetComponent<Light>().intensity = (float)(GetComponent<Light>().intensity + Time.deltaTime * 0.02);
				skyboxColor = new Color(skyboxColor.r + colorAmt,skyboxColor.g + colorAmt,skyboxColor.b + colorAmt);
			} 
			else if ((transform.eulerAngles.x >= 0) && (transform.eulerAngles.x <= 60) && (GetComponent<Light>().intensity > 0) && transform.eulerAngles.y > 269) 
			{
				GetComponent<Light>().intensity = (float)(GetComponent<Light>().intensity - Time.deltaTime * 0.02);
				skyboxColor = new Color(skyboxColor.r - colorAmt,skyboxColor.g - colorAmt,skyboxColor.b - colorAmt);
			} 
		}
		else{
			transform.Rotate(Vector3.left * Time.deltaTime * 2);
			if(transform.eulerAngles.x >= 359)
			{
				dayTime = true;
			}
			else if ((transform.eulerAngles.x < 70) && (transform.eulerAngles.x > 0) && (transform.eulerAngles.y > 269) && GetComponent<Light>().intensity < 0.06) 
			{
				GetComponent<Light>().intensity = (float)(GetComponent<Light>().intensity + 0.00002);
			}
			else if ((transform.eulerAngles.x < 30) && (transform.eulerAngles.y < 91) && GetComponent<Light>().intensity > 0)
			{
				GetComponent<Light>().intensity = (float)(GetComponent<Light>().intensity - 0.00002);
			}

		}
	}
}
