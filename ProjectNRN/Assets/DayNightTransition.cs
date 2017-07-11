using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightTransition : MonoBehaviour {

	bool isSunset;
	float sunRotation, moonRotation;

	public ParticleSystem pS,pS1;

	// Use this for initialization
	void Start () 
	{
		isSunset = true;
		pS.Stop();
		pS1.Stop ();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		float timer = 10f * Time.deltaTime;
		transform.RotateAround (Vector3.zero, new Vector3(1f,0.5f,0f), timer);
		transform.LookAt (Vector3.zero);

		if (gameObject.name == "Sun") 
		{
			// Rotating the sun
			sunRotation = this.gameObject.transform.rotation.eulerAngles.x;

			if(Mathf.Ceil(sunRotation) == 350)
			{
				if(!isSunset)
				{
					var psObj = pS.main;
					var ps1Obj = pS1.main;

					// Lerping the particle color by setting the alpha to 0.
					Color particleColor = Color.Lerp(new Color(255, 255, 174, 255), new Color(255,255,174,0),20f);

					// Setting the color for the particle system
					psObj.startColor = particleColor;
					ps1Obj.startColor = particleColor;

					isSunset = true;
				}
				else if(isSunset)
				{
					pS.Play();
					pS1.Play();

					var psObj = pS.main;
					var ps1Obj = pS1.main;

					// Lerping the particle color by setting the alpha to 255.
					Color particleColor = Color.Lerp(new Color(255, 255, 174, 0), new Color(255,255,174,255),10f);

					// Setting the color for the particle system
					psObj.startColor = particleColor;
					ps1Obj.startColor = particleColor;

					isSunset = false;
				}
			}
		}
	}
}
