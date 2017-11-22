using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass : MonoBehaviour {
	
	private Dictionary<GameObject,List<GameObject>> sector_graph = new Dictionary<GameObject,List<GameObject>>(); //Key = Sector, Data = list of sectors adjacent to key.

	// Use this for initialization
	void Start() {
       
		foreach (Transform child in transform) 
		{
			if (child.name.Substring (0, 6) == "Sector") 
			{
                Sector sector = child.GetComponent<Sector>();
				List<GameObject> adjacent_sectors = sector.AdjacentSectors;

				this.sector_graph.Add(child.gameObject,adjacent_sectors);
			}
		}
<<<<<<< HEAD
       
        //this.printSectorGraph();
    }
	
=======

		//this.printSectorGraph();
	}

	public GameObject getSelectedSector()
	{
		foreach (GameObject sector in sector_graph.Keys) {
			if (sector.GetComponent<Sector>().Selected == true) {
				return sector;
			}
		}
		return null;
	}




>>>>>>> 37872798b6be49487742209534a560355f0fc82c
	// Update is called once per frame
	void Update() {
		
	}

	private void printSectorGraph() {
		//Printing sectors (for debug);
		foreach (GameObject key in this.sector_graph.Keys) 
		{
			string output = key.name + ": ";
			foreach (GameObject adjSect in this.sector_graph[key]) 
			{
				output += adjSect.name + " ";
			}
			print (output);
		}
	}
}
