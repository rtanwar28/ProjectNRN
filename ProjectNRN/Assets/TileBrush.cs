using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileBrush : MonoBehaviour 
{
	// Size of the brush size
	public Vector2 brushSize = Vector2.zero;

	// Reference to tileId
	public int tileID = 0;

	// A renderer for the sprite
	public SpriteRenderer renderer2D;

	void OnDrawGizmosSelected()
	{
		//set the color of the gizmo. Here: red
		Gizmos.color = Color.red;

		// Draw the wire of the cube which will represent the border of the brush.
		Gizmos.DrawWireCube (transform.position, brushSize);
	}

	// To change the sprite for the brush's renderer
	public void UpdateBrush(Sprite sprite)
	{
		// Basic logic foe the tile brush.
		renderer2D.sprite = sprite;
	}
}
