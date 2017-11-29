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
    static public float score;

    //	Mathf.Abs( 50 * Mathf.Sin((transform.position.y/8.5)* Mathf.PI *2) +1 ) ;
    public bool up = true;


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
        StartCoroutine(waiter()); ;
        SceneManager.LoadScene("MainGame");
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(400);
    }


}
