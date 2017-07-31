using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileIO : MonoBehaviour
{
	public GameObject greenGrass, darkGrass, blueWater, darkWater, yellowSand, brownMud, chanceTile, storeTile, enemyTile, treePrefab, playerPrefab;
    public Vector3 playerStartPos;


    Camera minimapCamera;

	StreamReader sr;
	public char[,] boardMap;

		int j, k, tileRot, treeK, treeJ, treeRot, minimapZPos;

	public List<Transform> prefabTransforms;

	// Use this for initialization
	void Start ()
	{
		minimapCamera = GameObject.Find ("Minimap Camera").GetComponent<Camera> ();

		prefabTransforms = new List<Transform> ();

		j = 0;
		k = 0;
		boardMap = new char[5,10];

		//Creating the file path
		string	filePath = Application.dataPath + Path.DirectorySeparatorChar + "Combined Project Files" + Path.DirectorySeparatorChar + "Resources" + Path.DirectorySeparatorChar;

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

		minimapZPos = j;

		for(int i=0;i<test.Length;i++)
		{
			if (test [i] != '\n') 
			{
				tileRot = Random.Range (0,3);

				// Instantiating the blue water prefab
				if (test [i] == 'b')
					Instantiate (blueWater, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f * tileRot, 0f));

				// Instantiating the dark blue water prefab
				else if (test [i] == 'w')
					Instantiate (darkWater, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f * tileRot, 0f));

				// Instantiating the green grass prefab
				else if (test [i] == 'g') {
					Instantiate (greenGrass, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f * tileRot, 0f));

					//Instantiate trees at random spots on tile with varying scale and rotation.
					int q = Random.Range (1, 4);
					int r = Random.Range (5, 15);
					for (int z = 1; z <= q; z++) {
						treeK = Random.Range (-8, 8);
						treeJ = Random.Range (-8, 8);
						treeRot = Random.Range (0, 3);
						GameObject treeObject = Instantiate (treePrefab, new Vector3 ((k * 2) + (treeK * 0.1f), 0.75f, (j * 2) + (treeJ * 0.1f)), Quaternion.Euler (0f, 90f * treeRot, 0f));
						treeObject.transform.localScale = new Vector3 (r * 0.1f, r * 0.1f, r * 0.1f); 
					}
				}

				// Instantiating the dark green grass prefab
				else if (test [i] == 'd') {
					Instantiate (darkGrass, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f * tileRot, 0f));

					//Instantiate trees at random spots on tile with varying scale and rotation.
					int q = Random.Range (1 , 4);
					int r = Random.Range (5 , 15);
					for (int z = 1; z <= q ; z++) {
						treeK = Random.Range (-8,8);
						treeJ = Random.Range (-8,8);
						treeRot = Random.Range (0,3);
						GameObject treeObject = Instantiate (treePrefab, new Vector3 ((k * 2)+(treeK * 0.1f), 0.75f, (j * 2)+(treeJ * 0.1f)), Quaternion.Euler (0f, 90f * treeRot, 0f));
						treeObject.transform.localScale = new Vector3 (r*0.1f, r*0.1f, r*0.1f); 
					}
				}

				// Instantiating the yellow path prefab
				else if (test [i] == 'y')
					prefabTransforms.Add(Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f*tileRot, 0f)).transform);

				// Instantiating the mud prefab
				else if (test [i] == 'm')
					Instantiate (brownMud, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f*tileRot, 0f));

				else if (test [i] == 'c')
				{
					prefabTransforms.Add(Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f)).transform);
					Instantiate (chanceTile, new Vector3 (k * 2, 1.26f, j * 2), Quaternion.Euler (0f, 90f*tileRot, 0f));
				} 

				else if (test [i] == 's')
				{
					prefabTransforms.Add(Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f)).transform);
					Instantiate (storeTile, new Vector3 (k * 2, 1.26f, j * 2), Quaternion.Euler (0f, 90f, 0f));
				} 

				else if (test [i] == 'e') 
				{
					prefabTransforms.Add(Instantiate (yellowSand, new Vector3 (k * 2, 0, j * 2), Quaternion.Euler (0f, 90f, 0f)).transform);
					Instantiate (enemyTile, new Vector3 (k * 2, 1.26f, j * 2), Quaternion.Euler (0f, 90f, 0f));
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

		minimapCamera.transform.position = new Vector3 (k-1 , 5f, minimapZPos);
		if (minimapZPos > k)
		    minimapCamera.orthographicSize = minimapZPos + 1;
		else 
			minimapCamera.orthographicSize = k + 1;

		Transform startTileTransform = prefabTransforms[94]; //Select the tile to instantiate player on from 'prefabTransform' list.
		playerStartPos = startTileTransform.position + new Vector3(0f,1.25f,0f); //To figure out later
		//Instantiate (playerPrefab, playerStartPos, Quaternion.identity);


    }

    public Vector3 GetPlayerSPostion()
    {
        return playerStartPos;
    }
}