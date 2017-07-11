// NOTE: This class does not extend Monobehaviour as this script will not be attached to any gameonbject. 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Classes to be imported
using UnityEditor;

public class NewTileMapMenu
{
	// A meta-tag that connects the methid below with the menu item in Unity.
	[MenuItem("GameObject/TileMap")]
	// This is the method that is called when you choose the option from the menu.
	public static void CreateTileMap()
	{
		//Debug.Log ("<color=blue>In Create Tile Map Method</color>");

		// A call to create a new GameObject.
		GameObject tilemapGO = new GameObject ("Tile Map");

		// Now add the TileMap Script component to the gameobject tilemapGO.
		tilemapGO.AddComponent<TileMap> ();
	}
}
