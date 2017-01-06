using UnityEngine;
using System.Collections;

public class DayNightCycleOriginal : MonoBehaviour {

	// Use this for initialization
	public bool dayTime; //Time of day
	Light areaLight; //Used for sunset
	Color colorStart = new Color(0,0,0,1); //Starting Tint Color of Skybox (black)
	Color colorEnd = new Color(1,1,1,1); //Target end Tint Color of Skybox (white)
	float ratio = 0.0f;
	Light lt;

	void Start () {
		areaLight = transform.Find("Area Light").GetComponent<Light>();
		lt = GetComponent<Light> ();
		dayTime= true;
	} 
	
	// Update is called once per frame
	void Update () {
		//Daytime behaviour for Sun
		if (dayTime) 
		{
			transform.Rotate(Vector3.right * Time.deltaTime); //Rotate at a consistent pace
			if(transform.eulerAngles.x > 268)//If we have rotated 180 degrees its night time
			{
				//initalize starting night time values
				dayTime = false; 
				lt.intensity = 0.0f; 
				areaLight.intensity = 0.0f;
			}
			//While the sun is rising but is also lower the high noon increase light intensity and skybox color
			else if (((transform.eulerAngles.x >= 0) && (transform.eulerAngles.x <= 80)) && (lt.intensity < 1.3) && transform.eulerAngles.y < 91) 
			{
				lt.intensity = (float)(lt.intensity + Time.deltaTime * 0.04);
				//Changes the skybox tint color to reflect the time of day
				ratio += 0.0005f;
				RenderSettings.skybox.SetColor ("_Tint", Color.Lerp(colorStart,colorEnd, ratio ));
			} 
			//Once the sun passes high noon start reducing sun intensity.
			else if ((transform.eulerAngles.x >= 0) && (transform.eulerAngles.x <= 60) && transform.eulerAngles.y > 269) 
			{
				//set a base limit for how low light intensity can reach
				if(lt.intensity > 0.5){
					lt.intensity = (float)(lt.intensity - Time.deltaTime * 0.02);
				}
				//Once the sun reaches the sunset phase increase the intensity of the area light.
				//This produces a sunset landscape effect
				if((transform.eulerAngles.x >= 12.5) && (transform.eulerAngles.x < 26) && areaLight.intensity < .8){
					areaLight.intensity += 0.02f;
				}
				else if ((transform.eulerAngles.x >= 0) && (transform.eulerAngles.x < 12.5) && areaLight.intensity > .2){
					areaLight.intensity -= 0.005f;
				}
				//Changes the skybox tint color to reflect the time of day
				ratio -= 0.000231f;
				RenderSettings.skybox.SetColor ("_Tint", Color.Lerp(colorStart,colorEnd, ratio ));
			} 
		}
		//Night Time
		else{
			transform.Rotate(Vector3.left * Time.deltaTime);
			if(transform.eulerAngles.x >= 359) //We are back to daytime
			{
				dayTime = true;
			}
			//Slightly increase light intensity for moonlight
			else if ((transform.eulerAngles.x < 70) && (transform.eulerAngles.x > 0) && (transform.eulerAngles.y > 269) && lt.intensity < 0.11) 
			{
				lt.intensity = (float)(lt.intensity + 0.0001);
			}
			//Decrease light intensity until we go back to daytime
			else if ((transform.eulerAngles.x < 30) && (transform.eulerAngles.y < 91) && lt.intensity > 0)
			{
				lt.intensity = (float)(lt.intensity - 0.0001);
			}

		}
	}
}
