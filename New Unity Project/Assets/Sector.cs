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
	private bool selected = false;

	private PlayerClass owner;


    // Following fields are set via the inspector in Unity
    public List<int> adjacent_sector_ids = new List<int>();
    public bool is_college;

	/*
	 * This method is called at the start of the game.
	 * It takes the adjacent_sector_ids from the inspector and converts them into a list
	 * of adjacent sector GameObjects.
	 */
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

	/*
	 * OnMouseDown(): called when the sector is clicked.
	 * It handles the logic behind selecting which action to be taken (Attack, Movement or sector Highlighting)
	 * depending on if another sector is selected, whether the turn is on an Attacking or Movement phase,
	 * and the ownership of the current sector compared to the original.
	 */
	void OnMouseDown()
	{

		if (Input.GetMouseButtonDown (0)) 

		{
			MapClass map = GameObject.Find ("Map").GetComponent<MapClass> ();
			GameObject originalSector = map.getSelectedSector ();
			if (originalSector != null) 
			{
				if (this.adjacent_sectors.Contains (originalSector)) 
				{
					Sector originalSectorClass = originalSector.GetComponent<Sector> ();
					if (this.owner == originalSectorClass.Owner && GameClass.GameState == GameClass.MOVEMENT) 
					{
						//Move Gang members from originalSector to currentSector (this).
						print("Move gang members from " + originalSector.name + " to " + name);
						map.deselectAll ();
					} 
					else if (this.owner != originalSectorClass.Owner && GameClass.GameState == GameClass.ATTACK) 
					{
						//Attack from originalSector to currentSector (this).
						print("Attack " + name + " from " + originalSector.name);
						map.deselectAll();
					}
				}
			}
			else 
			{
				if (this.owner == GameClass.CurrentPlayer) 
				{
					int sectorsHighlighted = 0;
					foreach (GameObject adjSect in adjacent_sectors) 
					{
						if (GameClass.GameState == GameClass.MOVEMENT) 
						{
							if (adjSect.GetComponent<Sector> ().Owner == this.owner) 
							{
								adjSect.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1, 1);
								sectorsHighlighted++;
							}
						} 
						else 
						{
							if (adjSect.GetComponent<Sector> ().Owner != this.owner) 
							{
								adjSect.GetComponent<SpriteRenderer> ().color = new Color (1, 0, 0, 1);
								sectorsHighlighted++;
							}
						}
					}
					if (sectorsHighlighted > 0) 
					{
						SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
						sprite.color = new Color (0, 0, 0, 1);
						this.selected = true;
					} 
					else 
					{
						// No valid moves from clicked sector
						//print("No valid moves from clicked sector");
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
		set { selected = value; }
	}

	public PlayerClass Owner
	{
		get { return owner; }
		set { owner = value; }
	}
}
