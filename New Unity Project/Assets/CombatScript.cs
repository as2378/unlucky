using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour {
    public GameObject SelectedGo;
	public void Update() {

        foreach (GameObject i in Sectors)
        {
            
            if(i.GetComponent<Sector>().Selected == true)
            {
                SelectedGo = i;
            }

        }

        Sector sectorScript = SelectedGo.GetComponent<Sector>();
        foreach (GameObject j in sectorScript.AdjacentSectors)
        {
            j.GetComponent<SpriteRenderer>().material.color = Color.red;
        }
    }

    public GameObject[] Sectors;
    public void Start()
    {
        Sectors = GameObject.FindGameObjectsWithTag("Sector");


    }
}
