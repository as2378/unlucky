using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectorClass : MonoBehaviour {
	private int attack_value;
	private int defence_value;

	public List<int> adjacentSectorIDs = new List<int>();
	private List<GameObject> adjacent_sectors = new List<GameObject> ();

	public bool is_college;

	// Use this for initialization
	void Start () {
		if (adjacentSectorIDs.Count == 0) {
			throw new System.Exception("In '/Map/"+name+"': A sector must have at least one adjacent sector.");
		} else {
			//Convert SectorIDs into a list of adjacent sectors
			foreach (int sectorID in adjacentSectorIDs) {
				GameObject sect = GameObject.Find ("/Map/Sector #" + sectorID);
				if (sect != null) {				//Finds the sector and checks if it exists
					if (sect.name != name) {
						if (adjacent_sectors.Contains (sect) == false) {
							adjacent_sectors.Add (sect);	//adjacent sector added to adjacent_sectors list
						} else {
							throw new System.Exception ("In '/Map/"+name+"':AdjacentSector list cannot contain duplicates.");
						}
					} else {
						throw new System.Exception ("In '/Map/"+name+"': A sector cannot be adjacent to itself.");
					}
				} else {
					throw new System.Exception ("In '/Map/"+name+"': /Map/Sector #" + sectorID + " cannot be found.");
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int get_attack_value(){
		return this.attack_value;
	}

	public int get_defence_value(){
		return this.defence_value;
	}

	public List<GameObject> get_adjacent_sectors(){
		return this.adjacent_sectors;
	}
}
