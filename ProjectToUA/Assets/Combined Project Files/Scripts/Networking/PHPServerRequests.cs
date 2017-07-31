using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PHPServerRequests : MonoBehaviour {

    public Text usernameText, passwordText, regNameText, regUsernameText, regPassText, regConfPassText, regEmailText, regDOBText, regSAText;
    public Text feedbackMessage;
    public GameObject ServerLauncher;

    private string username, password, regName, regUsername, regPass, regConfPass, regEmail, regDOB, regSA;

    public void Awake()
    {
        usernameText = GameObject.Find("LUNText").GetComponent<Text>();
        passwordText = GameObject.Find("LPText").GetComponent<Text>();
        regNameText = GameObject.Find("NText").GetComponent<Text>();
        regUsernameText = GameObject.Find("UNText").GetComponent<Text>();
        regPassText = GameObject.Find("PText").GetComponent<Text>();
        regConfPassText = GameObject.Find("CPText").GetComponent<Text>();
        regEmailText = GameObject.Find("EText").GetComponent<Text>();
        regDOBText = GameObject.Find("DOBText").GetComponent<Text>();
        regSAText = GameObject.Find("SAText").GetComponent<Text>();
        feedbackMessage = GameObject.Find("FbText").GetComponent<Text>();
        ServerLauncher = GameObject.Find("Launcher");

    }

    //FUNCTION TO BE CALLED VIA THE UI BUTTON
    public void LogIn()
    {
        feedbackMessage.text = ""; //CLEAR ANY DISPLAYED MESSAGES TO THE PLAYER

        username = usernameText.text;    //POPULATE THE PRIVATE username VARIABLE WITH THE TEXT THE PLAYER ENTERED INTO THE usernameText INPUT FIELD
        password = passwordText.text;    //POPULATE THE PRIVATE password VARIABLE WITH THE TEXT THE PLAYER ENTERED INTO THE passwordText INPUT FIELD

        if (username == "" || password == "") //IF THE PLAYER HASN'T ENTERED THE REQUIRED INFORMATION...TELL THEM TO
            feedbackMessage.text = "Please complete all fields.";
        else    //IF ALL INFORMATION IS ENTERED, BUILD A WWWForm AND SEND IT TO THE SERVER
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("password", password);
            WWW w = new WWW("http://tales-of-untrustworthy-adventurers.000webhostapp.com/Login.php", form);    //REPLACE ?????????? WITH YOUR AWARD SPACE DOMAIN
            StartCoroutine(LogIn(w));
        }
    }
    //FUNCTION TO BE CALLED VIA THE UI BUTTON
    public void Register()
    {
        feedbackMessage.text = ""; //CLEAR ANY DISPLAYED MESSAGES TO THE PLAYER

        regUsername = regUsernameText.text;    //POPULATE THE PRIVATE regUsername VARIABLE WITH THE TEXT THE PLAYER ENTERED INTO THE regUsernameText INPUT FIELD
        regPass = regPassText.text;    //POPULATE THE PRIVATE USERNAME regPass WITH THE TEXT THE PLAYER ENTERED INTO THE pregPassText INPUT FIELD
        regConfPass = regConfPassText.text;    //POPULATE THE PRIVATE regConfPass VARIABLE WITH THE TEXT THE PLAYER ENTERED INTO THE regConfPassText INPUT FIELD
        regEmail = regEmailText.text;    //POPULATE THE PRIVATE regEmail VARIABLE WITH THE TEXT THE PLAYER ENTERED INTO THE regEmailText INPUT FIELD
        regName = regNameText.text;    //POPULATE THE PRIVATE regName VARIABLE WITH THE TEXT THE PLAYER ENTERED INTO THE regNameText INPUT FIELD
        regDOB = regDOBText.text;
        regSA = regSAText.text;

        if (regName == "" || regUsername == "" || regPass == "" || regConfPass == "" || regEmail == "" || regDOB == "" || regSA == "") //IF THE PLAYER HASN'T ENTERED THE REQUIRED INFORMATION...TELL THEM TO
            feedbackMessage.text = "Please complete all fields.";
        else    //IF ALL INFORMATION IS ENTERED......
        {
            if (regPass == regConfPass)    //......AND THE PASSWORDS MATCH, BUILD A WWWForm AND SEND IT TO THE SERVER
            {
                WWWForm form = new WWWForm();
                form.AddField("username", regUsername);
                form.AddField("password", regPass);
                form.AddField("email", regEmail);
                form.AddField("name", regName);
                form.AddField("date_of_birth", regDOB);
                form.AddField("secret_answer", regSA);

                WWW w = new WWW("http://tales-of-untrustworthy-adventurers.000webhostapp.com/Register.php", form);    //REPLACE ?????????? WITH YOUR AWARD SPACE DOMAIN
                StartCoroutine(Register(w));
            }
            else    //......AND THE PASSWORDS DON'T MATCH, TELL THE PLAYER
                feedbackMessage.text = "Your passwords do not match.";
        }
    }

    //WE USE COROUTINES TO SEND INFORMATION TO THE SERVER, SO THAT WE CAN WAIT FOR A RESPONSE
    private IEnumerator LogIn(WWW _w)
    {
        yield return _w;    //WAIT FOR A RESPONSE FROM THE SERVER

        if (_w.error == null)    //IF THE SERVER DOESN'T SEND BACK AN ERROR
        {
            Debug.Log("Null Error.");
            Debug.Log(_w.text);
            if (_w.text == "Log In Successful.")    //THE PHP SCRIPT SUPPLIED WILL SEND THIS MESSAGE BACK IF THE LOGIN WAS SUCCESSFUL
            {
                Debug.Log("Log in If Triggered.");
                // WHAT HAPPENS WHEN THE PLAYER LOGS IN
                ServerLauncher.GetComponent<NRN.Tales.Launcher> (). Connect(usernameText.text);
            }
            else
                feedbackMessage.text = _w.text;    //THE PHP SCRIPT SUPPLIED WILL TELL THE PLAYER IF THEIR PASSWORD IS INCORRECT, OR IF THEIR USERNAME DOESN'T EXIST
        }
        else
            feedbackMessage.text = "ERROR: " + _w.error;    //IF THERE IS AN ERROR (SUCH AS THE SERVER BEING DOWN) THE PHP SCRIPT SUPPLIED WILL TELL THE PLAYER
    }
    private IEnumerator Register(WWW _w)
    {
        yield return _w;    //WAIT FOR A RESPONSE FROM THE SERVER

        if (_w.error == null)    //IF THE SERVER DOESN"T SEND BACK AN ERROR
            feedbackMessage.text = _w.text;        //THE PHP SCRIPT SUPPLIED WILL SEND A MESSAGE BACK TO THE PLAYER SAYING REGISTRATION WAS COMPLETED
        else
            feedbackMessage.text = "ERROR: " + _w.error;    //IF THERE IS AN ERROR (SUCH AS THE SERVER BEING DOWN) THE PHP SCRIPT SUPPLIED WILL TELL THE PLAYER
    }
}
