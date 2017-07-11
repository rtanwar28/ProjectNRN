using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// A meta-tag to link this script to the Tile Map game object. This meta-tag will allow it to associate to a custom editor.
[CustomEditor(typeof (TileMap))]

// The class extension 'Editor' will allow you to create a custom inspector.
public class TileMapEditor : Editor	
{
	// A reference to the TileMap class instance.
	public TileMap tMap;

	// A property to store the reference to the tile Brush.
	TileBrush brush;

	Vector3 mouseHitPosition;

	bool mouseOnMap
	{
		// Using a getter to make sure that the tile map brush stays within the boundaries of the map.
		get{return mouseHitPosition.x > 0 && mouseHitPosition.x < tMap.gridSize.x && mouseHitPosition.y < 0 && mouseHitPosition.y > -tMap.gridSize.y;}
	}

	// Overriding the method, "OnInspectorGUI". Why? because we are using the 'Editor' extension
	public override void OnInspectorGUI ()
	{
		// Creating a label so as to be able to view the custom inspector.
		// EditorGUILayout.LabelField("Our customr editor");
		EditorGUILayout.BeginVertical ();

		// Variable to store the old map size
		Vector2 oldMapSize = tMap.mapSize;

		/*Debug.Log ("Old Map Size: " + oldMapSize);
		Debug.Log ("Map Size: " + tMap.gridSize);*/


		// Creating a Vector2 Field for the map size.
		tMap.mapSize = EditorGUILayout.Vector2Field ("Map Size:", tMap.mapSize);

		// Checking if the old map size is not equal to the current map size entered.
		if(tMap.mapSize != oldMapSize)
		{
			UpdateCalculations ();
			tMap.tileID = 1;
			CreateBrush (); 
		}

		Texture2D oldTexture2D = tMap.texture2D;

		// Creating a Object Field for the texture 2D.
		tMap.texture2D = (Texture2D)EditorGUILayout.ObjectField ("Texture2D:", tMap.texture2D, typeof(Texture2D), false);

		if (oldTexture2D != tMap.texture2D)
		{
			UpdateCalculations ();
		}

		// If no texture is selected then display a warning in the Inspector.
		if(tMap.texture2D == null)
		{
			EditorGUILayout.HelpBox ("You have not selected a Texture.", MessageType.Warning);
		}
		// If a texture is selected then display the Tile Size as a Label Field.
		else
		{
			EditorGUILayout.LabelField ("Tile Size: " ,tMap.tileSize.x + "x" + tMap.tileSize.y);
			EditorGUILayout.LabelField ("Grid Size: " ,tMap.gridSize.x + "x" + tMap.gridSize.y);
			EditorGUILayout.LabelField ("Pixels To Unit: " ,tMap.pixelsToUnit.ToString());
			UpdateBrush (tMap.currentTileBrush);

			//Creating the button for ClearMap()
			if(GUILayout.Button("Clear Tiles"))
			{
				if (EditorUtility.DisplayDialog ("Clear Map Tiles?", "Are you sure?", "Clear", "Do not clear"))
				{
					ClearMap (); 
				}
			}
		}

		EditorGUILayout.EndVertical();
	}

	// Method to check on the currently selected item in the inspector.
	// This method is called when the new inspector is created and displayed in the UnityEditor.
	void OnEnable()
	{
		// Getting the reference to the target field (provided by the Editor extension). Hence, we can get the instance of the item we are selecting.
		tMap = target as TileMap; // target is: the object being inspected. Here, we set the value of the TileMap reference as a target type and recast it as a TileMap.

		// Setting the current tool view whenever the "Tile Map" gameobject is selected to 'View'.
		Tools.current = Tool.View;

		if(tMap.tiles == null)
		{
			GameObject tilesGO = new GameObject ("Tiles");
			tilesGO.transform.SetParent (tMap.transform);
			tilesGO.transform.position = Vector3.zero;

			tMap.tiles = tilesGO;
		}

		// Getting and setting the correxct values for the tile size from the asset.
		if (tMap.texture2D != null) 
		{
			UpdateCalculations ();
			NewBrush ();
		}
	}

	void OnDisable()
	{
		DestroyBrush ();
	}

	void OnSceneGUI()
	{
		if(brush!=null)
		{
			UpdateHitPosition ();
			MoveBrush ();
		}

		if (tMap.texture2D != null && mouseOnMap)
		{
			Event current = Event.current;
			if(current.shift)
			{
				Draw ();
			}

			else if(current.alt)
			{
				RemoveTile ();
			}
		}
	}

	void UpdateCalculations()
	{
		// Getting the path where the texture is located in the asset database.
		string assetPath = AssetDatabase.GetAssetPath (tMap.texture2D);
		//Debug.Log ("Asset Path: " + assetPath);
		// Setting what has been loaded from the Asset Database.
		tMap.spriteRef = AssetDatabase.LoadAllAssetsAtPath (assetPath);

		/*for(int i =0;i<tMap.spriteRef.Length-1;i++)
			{
				Debug.Log (tMap.spriteRef [i] + " ");
			}*/

		// Calculating the height and width of the sprite in the spriteRef array.
		// Casting an item from the array as a sprite since it is stored as a generic object.

		Sprite sprite = (Sprite)tMap.spriteRef [1];
		float width = sprite.textureRect.width;
		float height = sprite.textureRect.height;

		//Debug.Log ("Width: " + width + " Height:" + height);

		tMap.tileSize = new Vector2 (width, height);

		//Accurate size of pixels to unit.
		tMap.pixelsToUnit = (int)(sprite.rect.width / sprite.bounds.size.x);
		/*Debug.Log ("sprite.rect.width: " + (sprite.rect.width));
			Debug.Log ("sprite.bounds.size.x: " + (sprite.bounds.size.x));
			Debug.Log ("width/bounds.size of x: " + (sprite.rect.width / sprite.bounds.size.x));*/
		tMap.gridSize = new Vector2 ((width / tMap.pixelsToUnit) * tMap.mapSize.x, (height / tMap.pixelsToUnit) * tMap.mapSize.y);
	}

	void CreateBrush()
	{
		// Reference to the Sprite from the TileMap Script
		Sprite sprite = tMap.currentTileBrush;

		// Check if the sprite exists before it is set to the GameObject.
		if(sprite != null)
		{
			// Creating the brush instance
			GameObject brushGO = new GameObject ("Brush");

			// Now the brush will be nested inside the Tile Map within the Hierarchy View.
			brushGO.transform.SetParent (tMap.transform);

			// Reference to the brush
			brush = brushGO.AddComponent<TileBrush>();

			// Set the reference to renderer2D (in TileBrush Script) as SpriteRenderer.
			brush.renderer2D=brushGO.AddComponent<SpriteRenderer>();

			brush.renderer2D.sortingOrder = 1000;

			int brushPixelToUnits = tMap.pixelsToUnit;

			// Calculating the brush's size based on the pixels to units and the sprites own size.
			brush.brushSize = new Vector2 (sprite.textureRect.width / brushPixelToUnits, sprite.textureRect.height / brushPixelToUnits); 

			brush.UpdateBrush (sprite);
		}
	}

	// Checking if the brush exists
	void NewBrush()
	{
		if(brush == null)
		{
			CreateBrush ();
		}
	}

	// Destroying the brush
	void DestroyBrush()
	{
		if (brush != null)
			// Since the script is an Editor script and it is required to destory the brush immidiately, we use the DestroyImmediate method.
			// DestoryImmediate: Allows to destroy the gameobject, but, works only inside the Editor class
		DestroyImmediate (brush.gameObject);
	}

	public void UpdateBrush(Sprite sprite)
	{
		// Checking if the brush is not null
		if (brush != null)
			brush.UpdateBrush (sprite);
	} 

	void UpdateHitPosition()
	{
		// Creating a new plane
		Plane plane = new Plane (tMap.transform.TransformDirection (Vector3.forward), Vector3.zero);

		// Creating a ray for the raycast
		Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
		//Debug.Log ("THE RAY: " + ray);

		// To keep track of the hit position.
		Vector3 hit = Vector3.zero;

		// To know the distance
		float distance = 0f;

		// Checking the collision between the mouse point and the plane inside the scene view.
		if (plane.Raycast (ray, out distance)) 
		{
			// If a collision is detected then we set the hit value.
			hit = ray.origin + ray.direction.normalized * distance;

			mouseHitPosition = tMap.transform.InverseTransformPoint (hit);	
		}
	}


	// This method calculates the tile size in roder to snap the brush to the grid.
	void MoveBrush()
	{
		float tileSize = tMap.tileSize.x / tMap.pixelsToUnit; 

		float x = Mathf.Floor (mouseHitPosition.x / tileSize) * tileSize;
		float y = Mathf.Floor (mouseHitPosition.y / tileSize) * tileSize;

		float row = x / tileSize;
		float column = Mathf.Abs (y / tileSize) - 1;


		if (!mouseOnMap)
			return;
		
		int id = (int)((column * tMap.mapSize.x) + row);
		brush.tileID = id;

		x += tMap.transform.position.x + tileSize / 2;

		y += tMap.transform.position.y + tileSize / 2;

		brush.transform.position = new Vector3 (x, y, tMap.transform.position.z);
	}

	void Draw()
	{
		string drawTileID = brush.tileID.ToString ();
		//Debug.Log ("Draw Tile ID: " + drawTileID);

		float posX = brush.transform.position.x;
		float posY = brush.transform.position.y;

		GameObject tile = GameObject.Find (tMap.name + "/Tiles/tile_" + drawTileID);

		// Checking if tile exists
		if(tile == null)
		{
			tile = new GameObject ("tile_" + drawTileID);
			tile.transform.SetParent (tMap.tiles.transform);

			tile.transform.position = new Vector3 (posX, posY, 0);

			tile.AddComponent <SpriteRenderer>();
		}

		tile.GetComponent<SpriteRenderer> ().sprite = brush.renderer2D.sprite;
	}

	// To erase tiles
	void RemoveTile()
	{
		string removeTileID = brush.tileID.ToString ();

		// Getting the tile based on the id
		GameObject tile = GameObject.Find (tMap.name + "/Tiles/tile_" + removeTileID);

		if(tile != null)
		{
			// Destroying the tile based on the tile id retrieved.
			DestroyImmediate (tile);
		}
	}

	// A method to remove all the tiles in the map at once.
	void ClearMap()
	{
		for(int i =0;i<tMap.tiles.transform.childCount;i++)
		{
			// Reference to the child inside the tile's gameobject.
			Transform t = tMap.tiles.transform.GetChild (i);
			DestroyImmediate (t.gameObject);
			i--;
		}
	}
	
}

