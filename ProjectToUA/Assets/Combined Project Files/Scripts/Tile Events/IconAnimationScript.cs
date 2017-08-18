using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconAnimationScript : MonoBehaviour
{
    // Transform values of the icon
	Transform icon;
    // Declaring the variables for the icon animation
	float min, max;
	public float distMin, distMax, pingPongSpeed, rotationSpeed;
	public bool rotateRight;

	// Setting the minimum and maximum distance and setting the rotation of the icon.
	void Start ()
	{
		icon = this.transform;
		min = icon.position.y - distMin;
		max = icon.position.y + distMax;

		if (rotateRight)
			rotationSpeed = -1 * rotationSpeed;
	}
	
	// Updating the rotation and position of the icon every frame.
	void Update ()
	{

		icon.Rotate (new Vector3 (0, Time.deltaTime * rotationSpeed, 0));
		icon.position = new Vector3 (icon.position.x, Mathf.PingPong (Time.time * pingPongSpeed, max - min) + min, icon.position.z);
		
	}
}
