using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapClass : MonoBehaviour {

	private List<List<GameObject>> sector_graph = new List<List<GameObject>>();

	// Use this for initialization
	void Start() {
		foreach (Transform child in transform) {
			if (child.name.Substring (0, 6) == "Sector") {
				List<GameObject> current_sector = new List<GameObject>();
                Sector sector = child.GetComponent<Sector>();
                List<GameObject> adjacent_sectors = sector.AdjacentSectors;

                current_sector.Add(child.gameObject);
				current_sector.AddRange(adjacent_sectors);
				this.sector_graph.Add(current_sector);
			}
		}

		//this.printSectorGraph();
	}
	
	// Update is called once per frame
	void Update() {
		
	}

	private void printSectorGraph() {
		//Printing sectors (for debug);
		foreach (List<GameObject> x in this.sector_graph) {
			string output = "";

            foreach (GameObject y in x) {
				output += y.name + " ";
			}

			print(output);
		}
	}
}
