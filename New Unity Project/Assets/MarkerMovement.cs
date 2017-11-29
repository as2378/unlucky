using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class MarkerMovement : MonoBehaviour
{

    public float movementSpeed = 50;
    public float limit = 10;
    public float MaxSpeed = 15;
    public float SpeedRange = 10;
    public bool stop = false;
    public float Position;
    public float score;

    //	Mathf.Abs( 50 * Mathf.Sin((transform.position.y/8.5)* Mathf.PI *2) +1 ) ;
    public bool up = true;

    public void Start()
    {
        GameObject.Find("StopButton").SetActive(false);
        GameObject.Find("MainCamera").SetActive(true);
        GameObject.Find("SliderCam").SetActive(false);
    }

    public void StartSlider()
    {
        GameObject.Find("StopButton").SetActive(true);
        stop = false;
        transform.position = new Vector3(250, 0, 0);
        GameObject.Find("MainCamera").SetActive(false);
        GameObject.Find("SliderCam").SetActive(true);
        //return true;
    }
    // Update is called once per frame
    void Update()
    {
        score = 1 - Mathf.Abs(Position);
        Position = transform.position.y / limit;
        if (stop == false)
        {
            movementSpeed = MaxSpeed - (SpeedRange * Mathf.Sin(Mathf.PI * 0.5f * Mathf.Abs(Position)));
            if (up == true)
            {

                transform.Translate(Vector3.up * movementSpeed * Time.deltaTime);

                if (transform.position.y >= limit)
                {
                    up = false;
                }
            }
            else
            {
                transform.Translate(Vector3.down * movementSpeed * Time.deltaTime);

                if (transform.position.y <= -limit)
                {
                    up = true;
                }
            }
        }
    }


    public void StopButton()
    {
        stop = true;
        GameObject.Find("StopButton").SetActive(false);
        GameObject.Find("MainCamera").SetActive(true);
        GameObject.Find("SliderCam").SetActive(false);
    }

    

}
