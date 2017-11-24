using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	public float speed;
	public float boundarySize;

	void FixedUpdate () 
	{
		Bounds mapBounds = GameObject.Find ("Map").GetComponent<SpriteRenderer> ().bounds;
		Camera cam = GetComponent<Camera> ();

		float camHeight = cam.orthographicSize;
		float camWidth = cam.aspect * camHeight;

		List<string> directions = this.getMovementDirection (cam,camWidth,camHeight);

		if(directions.Contains("Right")){
			if (transform.position.x + camWidth + speed <= mapBounds.max.x) {
				transform.position += new Vector3 (speed, 0, 0);
			}
		}
		if (directions.Contains("Left")) {
			if (transform.position.x - camWidth - speed >= mapBounds.min.x) {
				transform.position += new Vector3 (-speed, 0, 0);
			}
		}
		if(directions.Contains("Up")){
			if (transform.position.y + camHeight + speed <= mapBounds.max.y) {
				transform.position += new Vector3 (0, speed, 0);
			}
		}
		if (directions.Contains("Down")) {
			if (transform.position.y - camHeight - speed >= mapBounds.min.y) {
				transform.position += new Vector3 (0, -speed, 0);
			}
		}
	}

	private List<string> getMovementDirection(Camera cam,float camWidth, float camHeight)
	{
		Vector2 mousePosition = new Vector2 (Input.mousePosition.x,Input.mousePosition.y);

		float screenWidth = Screen.width;
		float screenHeight = Screen.height;

		List<string> directions = new List<string> ();

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
