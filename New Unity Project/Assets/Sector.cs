using UnityEngine;
using System.Collections.Generic;

/*
 * The class is used as a component script for sectors
 * 
 * When attached to a sector, properties have to be set
 * in the inspector in Unity (unit spawn position, the names of
 * adjacent sectors, whether the sector is a college) 
 * */

public class Sector : MonoBehaviour
{
    private int attack_value = 0;
    private int defence_value = 0;
    private int units = 0;

    private List<GameObject> adjacent_sectors = new List<GameObject>();

    // Following fields are set via the inspector in Unity
    public List<int> adjacent_sector_ids = new List<int>();

    public bool is_college;

	private bool selected = false;
	private string playerName;

    void Start()
    {
        if (adjacent_sector_ids.Count > 0)
        {
            foreach (int sector_id in adjacent_sector_ids)
            {
                GameObject sector = GameObject.Find("/Map/Sector #" + sector_id);

                if (sector != null)
                {
                    if (sector.name != name)
                    {
                        if (!adjacent_sectors.Contains(sector))
                        {
                            adjacent_sectors.Add(sector);
                        }
                        else
                        {
                            throw new System.Exception("In '/Map/" + name + "': \"adjacent_sectors\" list cannot contain duplicates.");
                        }
                    }
                    else
                    {
                        throw new System.Exception("In '/Map/" + name + "': A sector cannot be adjacent to itself.");
                    }
                }
                else
                {
                    throw new System.Exception("In '/Map/" + name + "': /Map/Sector #" + sector_id + " cannot be found.");
                }
            }
        }
        else
        {
            throw new System.Exception("In '/Map/" + name + "': A sector must have at least one adjacent sector.");
        }
    }

	void OnMouseDown()
	{
		MapClass map = GameObject.Find ("Map").GetComponent<MapClass> ();
		GameObject originalSector = map.getSelectedSector ();
		if (originalSector != null) 
		{
			if (this.adjacent_sectors.Contains (originalSector)) 
			{
				Sector originalSectorClass = originalSector.GetComponent<Sector> ();
				if (this.playerName == originalSectorClass.PlayerName && GameClass.GameState == GameClass.MOVEMENT) 
				{
					//Move Gang members
				} 
				else if (this.playerName != originalSectorClass.PlayerName && GameClass.GameState == GameClass.ATTACK) 
				{
					//Attack from originalSector to this.
				}
			}
		} else {
			if (this.playerName == GameClass.CurrentPlayer) 
			{
				SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
				sprite.color = new Color (0, 0, 0, 1);
				if (GameClass.GameState == GameClass.MOVEMENT) 
				{
					foreach (GameObject adjSect in adjacent_sectors) 
					{
						if (adjSect.GetComponent<Sector> ().PlayerName == this.playerName) 
						{
							adjSect.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 100, 1);
						}
					}
				} 
				else 
				{
					foreach (GameObject adjSect in adjacent_sectors) 
					{
						if (adjSect.GetComponent<Sector> ().PlayerName != this.playerName) 
						{
							adjSect.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 100, 1);
						}
					}
				}
			}
		}
	}

    public int Attack
    {
        get { return attack_value; }
        set { attack_value = value; }
    }

    public int Defence
    {
        get { return defence_value; }
        set { defence_value = value; }
    }

    public int Units
    {
        get { return units; }
        set { units = value; }
    }

    public List<GameObject> AdjacentSectors
    {
		get { return adjacent_sectors; }
    }

	public bool Selected 
	{
		get { return selected; }
	}

	public string PlayerName
	{
		get { return playerName; }
		set { playerName = value; }
	}
}
