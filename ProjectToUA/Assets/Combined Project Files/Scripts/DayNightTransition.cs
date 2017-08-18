using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightTransition : MonoBehaviour
{

    bool isSunset;
    //bool moonRise;
    float sunRotation, moonRotation;

    public ParticleSystem pS;

    // Use this for initialization
    void Start()
    {
        isSunset = true;
        pS.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Setting the timer value.
        float timer = 10f * Time.deltaTime;

        // Rotate the directional light about the axis passing through world cordinates.
        transform.RotateAround(Vector3.zero, new Vector3(1f, 0.5f, 0f), timer);
        transform.LookAt(Vector3.zero);

        // Depending on the rotation value of the directional light for the sun, set the color of the particle system.
        if (gameObject.name == "Sun")
        {
            sunRotation = this.gameObject.transform.rotation.eulerAngles.x;

            if (Mathf.Ceil(sunRotation) == 350)
            {
                if (!isSunset)
                {
                    var psObj = pS.main;

                    Color particleColor = Color.Lerp(new Color(255, 255, 174, 255), new Color(255, 255, 174, 0), 20f);
                    psObj.startColor = particleColor;
                    // TODO: If nothing works, enable the comment below.
                    //pS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                    isSunset = true;
                }
                else if (isSunset)
                {
                    pS.Play();
                    var psObj = pS.main;

                    Color particleColor = Color.Lerp(new Color(255, 255, 174, 0), new Color(255, 255, 174, 255), 10f);
                    psObj.startColor = particleColor;
                    isSunset = false;
                }
            }
        }
    }
}