using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass : MonoBehaviour {

	private List<List<GameObject>> sectorGraph = new List<List<GameObject>>();

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform){
			if (child.name.Substring (0, 6) == "Sector") {
				List<GameObject> currentSector = new List<GameObject> ();
				currentSector.Add (child.gameObject);
				SectorClass sect_cls = child.GetComponent<SectorClass> ();
				List<GameObject> adjacent_sectors = sect_cls.get_adjacent_sectors ();

				currentSector.AddRange (adjacent_sectors);
				this.sectorGraph.Add (currentSector);
			}
		}

		this.print_sectorGraph ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void print_sectorGraph(){
		//Printing sectors (for debug);
		foreach (List<GameObject> x in this.sectorGraph) {
			string output = "";
			foreach (GameObject y in x) {
				output += y.name + " ";
			}
			print (output);
		}
	}
}
