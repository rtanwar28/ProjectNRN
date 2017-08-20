using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CountdownTimer : MonoBehaviour
{
    // Time: Represent the time value the user wants
    public int time;

    // Reference to the UI Text for timer
    public Text timeText;

    // For calculating minute and seconds
    int min, sec;

    // To keep a check. Helpful during coroutines
    bool tCheck;

    // Default value for seconds to reset at.
    int sixtySec = 60;

    void Start()
    {
        // Passing the time value to minute variable.
        min = time;
        min--;
        Debug.Log("Start Minute: " + min);
        // Setting the value of seconds to 60;
        sec = sixtySec;

        timeText.text = min + " : 00";

        tCheck = false;
    }

    public void Update()
    {
        // Reduce the seconds, if its value is not equal to 0.
        if (sec != 0)
        {
            if (!tCheck)
            {
                // For when the sec variable store a 2-digit integer
                if (sec > 10)
                {
                    // Reduce the sec value by 1.
                    sec--;

                    timeText.text = min + " : " + sec;

                    tCheck = true;
                    // Start the coroutine
                    StartCoroutine(MyCoroutine());
                }

                // For when the sec variable store a 1-digit integer
                else
                {
                    sec--;

                    timeText.text = min + " : 0" + sec;

                    tCheck = true;

                    StartCoroutine(MyCoroutine());
                }
            }
        }

        // When the value of seconds becomes 0
        else if (min > 0)
        {
            timeText.text = min + " : 00";

            // Reduce the value of minute.
            min--;
            Debug.Log("Minute: " + min);
            // Reset the value of seconds.
            sec = sixtySec;
        }

        // When timer reaches 0.
        else if (min <= 0)
        {
            timeText.text = "0 : 00";
        }
    }

    IEnumerator MyCoroutine()
    {
        while (tCheck)
        {
            // Wait for 1 second and then proceed.
            yield return new WaitForSeconds(1);

            tCheck = false;
        }
    }

}