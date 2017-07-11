	using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMap : MonoBehaviour 
{
	// Declaring a public vector2 value for the map size.
	public Vector2 mapSize = new Vector2 (20, 10);

	// Variable for the Texture
	public Texture2D texture2D;

	// Tile size x and y.
	public Vector2 tileSize = new Vector2 ();

	// An array to store the reference to the sprites of the texture.
	public Object[] spriteRef;

	// For the grid size of the map
	public Vector2 gridSize = new Vector2();

	public int pixelsToUnit = 100;

	// For the tile ID in the TilePickerWindow script
	public int tileID = 0;

	// Reference to the game object that contains all the tiles.
	public GameObject tiles;

	// Reference of the sprite that has been selected from the tilePicker
	public Sprite currentTileBrush
	{
		// The getter will return a sprite reference based on the Tile ID as a SPRITE.
		get{return spriteRef [tileID] as Sprite;}
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnDrawGizmosSelected()
	{
		// Getting the reference to the game object's transform position
		Vector2 pos = transform.position;

		// Checking if the texture2D is set, such that the player has selected a texture.
		if (texture2D != null) 
		{
			// Rendering logic for each of the cells.
			// Setting the color for the tiles.
			Gizmos.color = Color.green;

			int row = 0;
			float maxColumns = mapSize.x;
			float totalCells = mapSize.x * mapSize.y;
			Vector3 tile = new Vector3 (tileSize.x / pixelsToUnit, tileSize.y / pixelsToUnit, 0);
			Vector2 offset = new Vector2 (tile.x/2, tile.y/2);

			for(int i=0;i<totalCells;i++)
			{
				float col = i % maxColumns;

				float newX = (col * tile.x) + offset.x + pos.x;
				float newY = -(row * tile.y) - offset.y + pos.y;

				Gizmos.DrawWireCube (new Vector2 (newX, newY), tile);

				if(col == (maxColumns - 1))
				{
					row++;
				}
			}

			// If there is then draw a border around the full size of the tile map.
			Gizmos.color = Color.cyan;

			float centerX = pos.x + (gridSize.x / 2);
			float centerY = pos.y - (gridSize.y / 2);

			/*Debug.Log ("Center x: " + centerX);
			Debug.Log ("Center y: " + centerY);*/

			Vector2 mapCenter = new Vector2 (centerX, centerY);

			// Time to draw gizmos
			Gizmos.DrawWireCube (mapCenter, gridSize);
		}
	}
}
