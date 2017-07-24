using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileIO : MonoBehaviour
{
	public GameObject greenGrass, darkGrass, blueWater, darkWater, yellowSand, brownMud, chanceTile, storeTile, enemyTile, playerPrefab;

	StreamReader sr;
	public char[,] boardMap;
    public Vector3 playerStartPos;


    int j, k, randomRotInt;

	public List<Transform> prefabTransforms;

	// Use this for initialization
	void Start ()
	{


		prefabTransforms = new List<Transform> ();

		j = 0;
		k = 0;
		boardMap = new char[5,10];

		//Creating the file path
		string	filePath = Application.dataPath + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar;

		//Creating a stream reader in order to read the contents in the text file
		sr = new StreamReader (filePath + "MapEditor.txt");

		// Read the context from the textfile
		string line = sr.ReadToEnd ();

		// Convert the string line into a character array
		char[] test = line.ToCharArray ();

		for (int x = 0; x < test.Length; x++) {
			if (test [x] == '\n')
				j++;
		}

		for(int i=0;i<test.Length;i++)
		{
			if (test [i] != '\n') 
			{
				randomRotInt = Random.Range (0,3);
				// Instantiating the blue water prefab
				if (test [i] == 'b')
					Instantiate (blueWater, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f*randomRotInt, 0f));

				// Instantiating the dark blue water prefab
				else if (test [i] == 'w')
					Instantiate (darkWater, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f*randomRotInt, 0f));

				// Instantiating the green grass prefab
				else if (test [i] == 'g')
					Instantiate (greenGrass, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f*randomRotInt, 0f));

				// Instantiating the dark green grass prefab
				else if (test [i] == 'd')
					Instantiate (darkGrass, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f*randomRotInt, 0f));

				// Instantiating the yellow path prefab
				else if (test [i] == 'y')
					prefabTransforms.Add(Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f*randomRotInt, 0f)).transform);

				// Instantiating the mud prefab
				else if (test [i] == 'm')
					Instantiate (brownMud, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f*randomRotInt, 0f));

				else if (test [i] == 'c')
				{
					prefabTransforms.Add(Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f)).transform);
					Instantiate (chanceTile, new Vector3 (k * 2, 2f, j * 2), Quaternion.Euler (0f, 90f*randomRotInt, 0f));
				} 

				else if (test [i] == 's')
				{
					prefabTransforms.Add(Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f)).transform);
					Instantiate (storeTile, new Vector3 (k * 2, 2f, j * 2), Quaternion.Euler (0f, 90f, 0f));
				} 

				else if (test [i] == 'e') 
				{
					prefabTransforms.Add(Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f)).transform);
					Instantiate (enemyTile, new Vector3 (k * 2, 2f, j * 2), Quaternion.Euler (0f, 90f, 0f));
				} 

				k++;

			}
			else 
			{
				// Setting the value of k to the next row
				j--;
				// Resetting the value of j in order to start from the 0th column
				k = 0;
			}
		}

		Transform startTileTransform = prefabTransforms[94]; //Select the tile to instantiate player on from 'prefabTransform' list.
		playerStartPos = startTileTransform.position + new Vector3(0f,1.75f,0f); //To figure out later
		//Instantiate (playerPrefab, playerStartPos, Quaternion.identity);


	}

    public Vector3 GetPlayerSPostion()
    {
        return playerStartPos;
    }
}