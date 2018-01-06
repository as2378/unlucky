using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MarkerMovement : MonoBehaviour
{
    public float movementSpeed = 50;
    public float limit = 10;
    public float MaxSpeed = 15;
    public float SpeedRange = 10;
    
	private float Position;
	private float score;
	private bool up = true;
	private bool active = false;
	private GameObject stopButton;
	private GameObject mainCamera;
	private GameObject sliderCamera;
	private GameObject attacker;
	private GameObject defender;
	private float attackMultiplier = -1;

	/**
	 * Start():
	 * Stores references to the cameras and the stop button, then hides the stop button, disables the slider camera
	 * and enables the main camera.
	 */
    public void Start()
    {
        stopButton = GameObject.Find("StopButton");
        mainCamera = GameObject.Find("MainCamera");
        sliderCamera = GameObject.Find("SliderCam");
		stopButton.SetActive (false);
		mainCamera.SetActive (true);
		sliderCamera.SetActive (false);
		stopButton.GetComponent<Button> ().interactable = false;
    }

	/**
	 * StartSlider(GameObject,GameObject):
	 * Resets marker position, activates stop button and slider camera, then disables the main camera.
	 */
	public void StartSlider(GameObject attacker, GameObject defender)
    {
		this.attacker = attacker;
		this.defender = defender;
		transform.position = new Vector3(250, 0, 0);
		stopButton.SetActive(true);
        mainCamera.SetActive(false);
        sliderCamera.SetActive(true);
		active = true;
    }

	/**
	 * Update():
	 * Update is called once per frame. It handles the movement of the marker on the slider and calculates the multiplier score.
	 */    
    void Update()
    {     
		if (active)
        {
			score = 1 - Mathf.Abs(Position);
			Position = transform.position.y / limit;
            movementSpeed = MaxSpeed - (SpeedRange * Mathf.Sin(Mathf.PI * 0.5f * Mathf.Abs(Position)));
            if (up == true)
            {
                transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);
                if (transform.position.y >= limit)
                {
                    up = false;
					stopButton.GetComponent<Button> ().interactable = true;
                }
            }
            else
            {
                transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);
                if (transform.position.y <= -limit)
                {
                    up = true;
					stopButton.GetComponent<Button> ().interactable = true;
                }
            }
        }
    }

	/**
	 * StopButton():
	 * Handles the click event for the stop button. If it is the attacker who clicked the button, then the slider is reset for the defender's turn.
	 * If the defender clicked the button, then the calculateCombatOutcome method is called and the stopbutton/slidercamera are disabled.
	 */
    public void StopButton()
    {
		active = false;
		stopButton.GetComponent<Button> ().interactable = false;

		MapClass mapClass = GameObject.Find ("Map").GetComponent<MapClass> ();
		if (attackMultiplier == -1)
		{
			attackMultiplier = score;
			StartSlider (attacker,defender);
		} 
		else 
		{
			stopButton.SetActive(false);
			mainCamera.SetActive(true);
			sliderCamera.SetActive(false);
			mapClass.calculateCombatOutcome (attacker, defender, attackMultiplier, score);
			attackMultiplier = -1;
		}
    }
}