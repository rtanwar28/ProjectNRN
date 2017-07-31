using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconAnimationScript : MonoBehaviour
{

	Transform icon;
	float min, max;
	public float distMin, distMax, pingPongSpeed, rotationSpeed;
	public bool rotateRight;

	// Use this for initialization
	void Start ()
	{
		icon = this.transform;
		min = icon.position.y - distMin;
		max = icon.position.y + distMax;

		if (rotateRight)
			rotationSpeed = -1 * rotationSpeed;
	}
	
	// Update is called once per frame
	void Update ()
	{

		icon.Rotate (new Vector3 (0, Time.deltaTime * rotationSpeed, 0));
		icon.position = new Vector3 (icon.position.x, Mathf.PingPong (Time.time * pingPongSpeed, max - min) + min, icon.position.z);
		
	}
}
