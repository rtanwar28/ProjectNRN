using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TilePickerWindow : EditorWindow
{
	// Creating an enum to store the size values
	public enum Scale
	{
		x1,
		x2,
		x3,
		x4,
		x5
	}

	// A variable to access the values stored in the Scale enum
	Scale scaleEnum;

	// Current selected tile.
	Vector2 currentTileSelected = Vector2.zero;

	// Creating a TilePicker option under Window in the Unity menu.
	[MenuItem("Window/TilePicker")]	
	public static void OpenTilePickerWindow()
	{
		// Creating a window for the tile picker.
		EditorWindow window = EditorWindow.GetWindow (typeof(TilePickerWindow), false, "Tile Picker");
	}

	// Making sure that the TilePicker window only displays the texture when the selected gameObject has a TileMap component attached to it.
	void OnGUI()
	{
		// Checking whether the current selection is not null.
		// Selection: Access to the selection in the editor.
		if (Selection.activeGameObject == null)
			return;

		// If the selection is not null. Check whether the current instance contains a tile map attached to it.
		TileMap currentSelection = Selection.activeGameObject.GetComponent<TileMap> ();

		// Check for the current selection not being null.
		if(currentSelection != null)
		{
			// Getting the reference to its texture2D of the current selection
			Texture2D currentSelTexture2D = currentSelection.texture2D;

			// To check if texture is not null before rendering it.
			if(currentSelTexture2D != null)
			{
				// Creating a drop down menu 
				scaleEnum = (Scale)EditorGUILayout.EnumPopup ("Zoom", scaleEnum);
				int newScale = ((int)scaleEnum) + 1;
				Vector2 newTextureSize = new Vector2 (currentSelTexture2D.width, currentSelTexture2D.height) * newScale;
				Vector2 offset = new Vector2 (10,25);

				// Call to the GUI class and draw the texture directly into the window.
				// public static void DrawTexture(Rect position, Texture image);
				GUI.DrawTexture (new Rect (offset.x, offset.y, newTextureSize.x,newTextureSize.y), currentSelTexture2D);

				Vector2 tilePickerTile = currentSelection.tileSize * newScale;
				Vector2 tilePickerGrid = new Vector2 (newTextureSize.x / tilePickerTile.x, newTextureSize.y / tilePickerTile.y);

				// Selecting the tile and then setting the offset value so that it does overlay on the drop-down box for the scale.
				Vector2 selectionPos = new Vector2 (tilePickerTile.x * currentTileSelected.x + offset.x, tilePickerTile.y * currentTileSelected.y + offset.y);

				// Creating a texture which would be the color of the box highlighting the tile.
				Texture2D boxTexture = new Texture2D (1, 1);

				// Setting the color of the pixel at x and y coordinates.
				boxTexture.SetPixel (0, 0, new Color (0, 0.5f, 1f, 0.4f));

				boxTexture.Apply ();
				GUIStyle style = new GUIStyle (GUI.skin.customStyles [0]);
				style.normal.background = boxTexture;

				// Creating a white box over the tile when it is selected.
				GUI.Box (new Rect (selectionPos.x, selectionPos.y, tilePickerTile.x, tilePickerTile.y), "", style);

				// Current state of the mouse cursor
				Event currentEvent = Event.current;
				//Debug.Log ("current event: " + currentEvent);

				// Setting the current mouse position based on the current event
				Vector2 mousePosition = new Vector2(currentEvent.mousePosition.x,currentEvent.mousePosition.y);

				// Checking if the mouse has been clicked
				if(currentEvent.type == EventType.mouseDown && currentEvent.button == 0)
				{
					// Flooring and setting the current tile selected x and y value.
					currentTileSelected.x = Mathf.Floor (mousePosition.x / tilePickerTile.x);
					currentTileSelected.y = Mathf.Floor (mousePosition.y / tilePickerTile.y);

					// To make sure that the selection stays within the boundaries.
					if(currentTileSelected.x > tilePickerGrid.x - 1)
					{
						currentTileSelected.x = tilePickerGrid.x - 1;
					}

					if(currentTileSelected.y > tilePickerGrid.y - 1)
					{
						currentTileSelected.y = tilePickerGrid.y - 1;
					}

					currentSelection.tileID = (int)(currentTileSelected.x + (currentTileSelected.y * tilePickerGrid.x) + 1);
					//Debug.Log ("Tile ID: " + currentSelection.tileID);

					// Forces the window to repaint immidiately.
					Repaint ();
				}
			}
		}


	}
}
