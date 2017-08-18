using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this script to the Android Panel in the hierarchy
//to destroy the UI for Android if we arent in android mode
public class DestroyForNonMobile : MonoBehaviour
{
  
    // Use this for initialization
    void Start()
    {

#if UNITY_EDITOR || UNITY_STANDALONE

        Destroy(this.gameObject);

#endif

    }

}