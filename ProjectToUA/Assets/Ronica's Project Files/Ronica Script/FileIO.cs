using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileIO : MonoBehaviour
{
	public GameObject greenGrass, darkGrass, blueWater, darkWater, yellowSand, brownMud, chanceTile, storeTile, enemyTile, 
	startTile, leftTile, rightTile, upTile, optionTile, playerPrefab;

	StreamReader sr;
	public char[,] boardMap;

	int j,k;

	// Use this for initialization
	void Start ()
	{
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

		for(int i=0;i<test.Length;i++)
		{
			if (test [i] != '\n') 
			{
				// Instantiating the blue water prefab
				if (test [i] == 'b')
					Instantiate (blueWater, new Vector3 (k * 2, 0, j * 2), Quaternion.identity);

				// Instantiating the dark blue water prefab
				else if (test [i] == 'w')
					Instantiate (darkWater, new Vector3 (k * 2, 0, j * 2), Quaternion.identity);

				// Instantiating the green grass prefab
				else if (test [i] == 'g')
					Instantiate (greenGrass, new Vector3 (k * 2, 0, j * 2), Quaternion.identity);

				// Instantiating the dark green grass prefab
				else if (test [i] == 'd')
					Instantiate (darkGrass, new Vector3 (k * 2, 0, j * 2), Quaternion.identity);

				// Instantiating the yellow path prefab
				else if (test [i] == 'y')
					Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f));

				// Instantiating the left path prefab
				else if (test [i] == 'l')
					Instantiate (leftTile, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f));

				// Instantiating the right path prefab
				else if (test [i] == 'r')
					Instantiate (rightTile, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f));

				// Instantiating the up path prefab
				else if (test [i] == 'u')
					Instantiate (upTile, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f));

				// Instantiating the option path prefab
				else if (test [i] == 'o')
					Instantiate (optionTile, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f));

				// Instantiating the mud prefab
				else if (test [i] == 'm')
					Instantiate (brownMud, new Vector3 (k * 2, 0, j * 2), Quaternion.identity);
				
				else if (test [i] == 'c')
				{
					Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f));
					Instantiate (chanceTile, new Vector3 (k * 2, 2f, j * 2), Quaternion.identity);
				} 

				else if (test [i] == 's')
				{
					Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f));
					Instantiate (storeTile, new Vector3 (k * 2, 2f, j * 2), Quaternion.Euler (0f, 90f, 0f));
				} 

				else if (test [i] == 'e') 
				{
					Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f));
					Instantiate (enemyTile, new Vector3 (k * 2, 2f, j * 2), Quaternion.Euler (0f, 90f, 0f));
				} 

				else if (test [i] == 'p') 
				{
					Instantiate (startTile, new Vector3 (k * 2, 0, j * 2), Quaternion.identity);
					Instantiate (playerPrefab, new Vector3 (k * 2, 1.75f, j * 2), Quaternion.identity);
				}
				
				j++;

			}
			else 
			{
				// Setting the value of k to the next row
				k++;
				// Resetting the value of j in order to start from the 0th column
				j = 0;
			}
		}
	}
}
