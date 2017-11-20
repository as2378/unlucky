using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {
	public float speed;

	void FixedUpdate () 
	{
		Bounds mapBounds = GameObject.Find ("Map").GetComponent<SpriteRenderer> ().bounds;

		Bounds cameraBounds = new Bounds ();
		Camera cam = GetComponent<Camera> ();

		float camHeight = cam.orthographicSize * 2;
		float camWidth = cam.aspect * camHeight;


		if(Input.GetKey(KeyCode.D)){
			if (transform.position.x + (camWidth/2) + speed <= mapBounds.max.x) {
				transform.position += new Vector3 (speed, 0, 0);
			}
		}
		if (Input.GetKey (KeyCode.A)) {
			if (transform.position.x - (camWidth/2) - speed >= mapBounds.min.x) {
				transform.position += new Vector3 (-speed, 0, 0);
			}
		}
		if(Input.GetKey(KeyCode.W)){
			if (transform.position.y + (camHeight / 2) + speed <= mapBounds.max.y) {
				transform.position += new Vector3 (0, speed, 0);
			}
		}
		if (Input.GetKey (KeyCode.S)) {
			if (transform.position.y - (camHeight / 2) - speed >= mapBounds.min.y) {
				transform.position += new Vector3 (0, -speed, 0);
			}
		}
	}
		
}
