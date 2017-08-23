using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStuff : MonoBehaviour {

    public Text messageText, charnameText;

    private string charName, healthStr, dexStr, magStr, atkStr, cName;
    public string currentSelection;

    public GameObject[] character = new GameObject[4];
    GameObject currentGO, oldGO;

    int index;

    public  float healthFloat, dexFloat, magFloat, atkFloat;

    float oldValue, newValue;

    public Slider hltSlider, dexSlider, magSlider, atkSlider;

    bool isBtnClicked; //to check if player retrieves stats

    int valH, valD, valM, valA; //whole number values of the sliders 

    string[] stats = new string[4];

    public void Start()
    {
        //setting the initial values to 0

        oldValue = 0;
        hltSlider.value = oldValue;
        dexSlider.value = oldValue;
        magSlider.value = oldValue;
        atkSlider.value = oldValue;

        //checking if player is trying to retrieve stats
        isBtnClicked = false;

        index = 0;
        currentGO = character[index];
        oldGO = currentGO;

        currentGO.SetActive(true);
        RetrieveStats();
    }

    public void Update()
    {
        if (isBtnClicked)
        {            
            hltSlider.value = Mathf.Lerp(hltSlider.value, healthFloat, 0.05f); //lerping the float value of the sliders by 0.05f
            valH = Mathf.CeilToInt(hltSlider.value); //saving the float value to the closest integer

            dexSlider.value = Mathf.Lerp(dexSlider.value, dexFloat, 0.05f);
            valD = Mathf.CeilToInt(dexSlider.value);
        
            magSlider.value = Mathf.Lerp(magSlider.value, magFloat, 0.05f);
            valM = Mathf.CeilToInt(magSlider.value);

            atkSlider.value = Mathf.Lerp(atkSlider.value, atkFloat, 0.05f);
            valA = Mathf.CeilToInt(atkSlider.value);
        }

        //if player has retrieved stats using whole numbers
        if (valH == healthFloat && valD == dexFloat && valM == magFloat && valA == atkFloat)
        {
            //reset the GetStats button check
            isBtnClicked = false;
        }
    }

    public void PreviousBtn()
    {
        if (index == 0)
        {
            index = character.Length;
        }

        index--;

        currentGO = character[index];
        oldGO.SetActive(false);
        currentGO.SetActive(true);
        oldGO = currentGO;

        RetrieveStats();
    }

    public void NextBtn()
    {
        index++;

        if (index >= character.Length)
        {
            index = 0;
        }

        currentGO = character[index];
        oldGO.SetActive(false);
        currentGO.SetActive(true);
        oldGO = currentGO;

        RetrieveStats();
    }

    public void RetrieveStats()
    {
        //messageText.text = ""; //clear any displayed message to the player
        //charName = charnameText.text;

        for (int i = 0; i < character.Length; i++)
        {
            if (character[i].activeInHierarchy == true)
            {
                Debug.Log(character[i].name);
                cName = character[i].name;
            }
        }

        currentSelection = cName;
        WWWForm form = new WWWForm();
        form.AddField("charname", cName);
        WWW w = new WWW("https://atoua.000webhostapp.com/playerStats.php", form);
        StartCoroutine(Stats(w));

    }

    public string ReturnCurrentName()
    {
        return currentSelection;
    }


    /*
     * public void SelectPlayer()

	{

		// If a character is in scene

		if(charIsActive)

		{

			activeChar = GameObject.FindGameObjectWithTag ("Character");

			Destroy (activeChar);

			charIsActive = false;



			oldRotation = activeChar.transform.eulerAngles;

		}



		// Getting the name of the gameobject currently selected in the Event System.

		string btnName = EventSystem.current.currentSelectedGameObject.transform.name;



		// Checking if the button clicked, its string name exists in characterBtn array.

		for(int i=0;i<characterBtn.Length;i++)

		{

			// If the name match then pass the index value.

			if(btnName == characterBtn[i].name)

			{

				SpawnCharacter (i);

			}

		}

	}



	// To spawn the character on the platform

	public void SpawnCharacter(int indexValue)

	{

		index = indexValue;

		// Instantiate character and setting bool value to true.

		Instantiate (characterArray [indexValue], spawnPoint.position, Quaternion.Euler(oldRotation));

		charIsActive = true;

		playerSelected = characterArray [indexValue];

		passChar = ActiveCharScene [indexValue];

		playerSelected.transform.eulerAngles = oldRotation;

	}



	// To go to a scene.

	public void LoadScene(string sceneName)

	{

		if (charIsActive)

		{

			SceneManager.LoadScene (sceneName);

		}

	}
     */






























    //using CoRoutines to send information to the server, so that we can wait for a response
    private IEnumerator Stats(WWW _w)
    {
        yield return _w; //wait for a response from the server

                    if (_w.error == null) //if server doesn't send back an error
        {
            messageText.text = _w.text;
            string str = messageText.text;

            for (int i = 0; i < stats.Length; i++)
            {
                string[] statistics = str.Split(' ');

                //stores the Split values from statistics[] to stats[]
                stats[i] = statistics[i];
            }

            healthFloat = float.Parse(stats[0]);
            dexFloat = float.Parse(stats[1]);
            magFloat = float.Parse(stats[2]);
            atkFloat = float.Parse(stats[3]);

            isBtnClicked = true;
        }

        else
            messageText.text = "ERROR: " + _w.error;
    }
}
