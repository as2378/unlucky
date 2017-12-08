using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * The class used to control the camera movements within the game.
 * 
 * CameraControl enables the camera to be moved by positioning the mouse cursor at the edge of the screen.
 * The camera moves until it reaches the edge of the map.
 */

public class CameraControl : MonoBehaviour {
	public float speed;
	public float boundarySize;

	private Bounds mapBounds;

	/*
	 * Start():
	 * When the game is initialised, the Map sprite's bounds are stored.
	 */
	void Start()
	{
		this.mapBounds = GameObject.Find ("Map").GetComponent<SpriteRenderer> ().bounds;
	}

	/*
	 * FixedUpdate(): Called every frame.
	 * 
	 * Moves the camera by amount 'speed' in the directions specified by getMovementDirection() 
	 * as long as the camera view stays within the Maps's bounds.
	 */ 

	void FixedUpdate () 
	{
		Camera cam = GetComponent<Camera> ();

		float camHeight = cam.orthographicSize;
		float camWidth = cam.aspect * camHeight;

		List<string> directions = this.getMovementDirection ();

		if(directions.Contains("Right"))
		{
			if (transform.position.x + camWidth + speed <= mapBounds.max.x) 
			{
				transform.position += new Vector3 (speed, 0, 0);
			}
		}
		if (directions.Contains("Left")) 
		{
			if (transform.position.x - camWidth - speed >= mapBounds.min.x) 
			{
				transform.position += new Vector3 (-speed, 0, 0);
			}
		}
		if(directions.Contains("Up"))
		{
			if (transform.position.y + camHeight + speed <= mapBounds.max.y) 
			{
				transform.position += new Vector3 (0, speed, 0);
			}
		}
		if (directions.Contains("Down")) 
		{
			if (transform.position.y - camHeight - speed >= mapBounds.min.y) 
			{
				transform.position += new Vector3 (0, -speed, 0);
			}
		}
	}


	/*
	 * getMovementDirection: called within FixedUpdate()
	 * Returns: List of strings containing textual representations of the directions the player want the camera to move in.
	 * 
	 * Finds the mouse position and compares it to the screen. If the mouse is closer to the edge than the distance 'boundarySize'
	 * then the direction is added to the directions list.
	 */
	private List<string> getMovementDirection()
	{
		List<string> directions = new List<string> ();
		Vector2 mousePosition = new Vector2 (Input.mousePosition.x,Input.mousePosition.y);

		float screenWidth = Screen.width;
		float screenHeight = Screen.height;

		if (mousePosition.x >= screenWidth - boundarySize)
		{
			directions.Add ("Right");
		}
		if (mousePosition.x <= boundarySize) 
		{
			directions.Add ("Left");
		}
		if (mousePosition.y <= boundarySize) 
		{
			directions.Add ("Down");
		}
		if (mousePosition.y >= screenHeight - boundarySize) 
		{
			directions.Add ("Up");
		}
		return directions;
	}		
}