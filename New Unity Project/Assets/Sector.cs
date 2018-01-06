using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

/**
 * The class is used as a component script for sectors.
 * 
 * When attached to a sector, properties have to be set
 * in the inspector in Unity (unit spawn position, the names of
 * adjacent sectors, whether the sector is a college) 
 */

public class Sector : MonoBehaviour
{
    private int attack_value = 1;
    private int defence_value = 1;
    private int units = 1;
    private List<GameObject> adjacent_sectors = new List<GameObject>();
	private bool selected = false;

	private PlayerClass owner;

    // Following fields are set via the inspector in Unity
    public List<int> adjacent_sector_ids = new List<int>();
    public bool is_college;

	/**
	 * Start(): This method is called at the start of the game.
	 * It takes the adjacent_sector_ids from the inspector and converts them into a list
	 * of adjacent sector GameObjects.
	 * Throws: System.Exception
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

	/**
	 * OnMouseDown(): called when the sector is clicked.
	 * Calls the click sector method, which handles the selection of sectors.
	 */
	void OnMouseDown()
	{
		if (Input.GetMouseButtonDown (0)) //has the left mouse button been pressed.
		{
			clickSector ();
		}
	}

	/*
	 * clickSector(): called when a sector is clicked by a player.
	 * It handles the logic behind selecting which action to be taken (Attack, Movement or sector Highlighting)
	 * depending on if another sector is selected, whether the turn is on an Attacking or Movement phase,
	 * and the ownership of the current sector compared to the original.
	 */
	public void clickSector()
	{
		MapClass map = GameObject.Find ("Map").GetComponent<MapClass> ();

		if (!GameClass.CurrentPlayer.Allocated) 
		{
			if (this.owner == GameClass.CurrentPlayer) 
			{
				startAllocation (map);
			}
		} 
		else 
		{
			GameObject originalSector = map.getSelectedSector ();
			if (originalSector != null) 
			{
				if (this.adjacent_sectors.Contains (originalSector)) 
				{
					makeMove (originalSector);
				}
			} 
			else 
			{
				if (this.owner == GameClass.CurrentPlayer) 
				{
					int numberOfHighlightedSectors = highlightAdjacentSectors ();
					if (numberOfHighlightedSectors > 0) //Are valid moves, select current sector.
					{ 
						SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
						sprite.color = new Color (0, 0, 0, 1);
						this.selected = true;
					} 
				}
			} 
		}
	}

	/**
	 * makeMove: used when the player clicks a valid sector after selecting one of their own first.
	 * param: GameObject originalSector - this is the gameobject of the previous sector selected in the turn. Moves will be from the originalSector to the current sector.
	 * This method decides which type of move (attack or movement) needs to be made based on the GameState.
	 * The necessary methods are then called to execute the move.
	 */
	private void makeMove(GameObject originalSector)
	{
		MapClass map = GameObject.Find ("Map").GetComponent<MapClass> ();
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
			map.Combat(originalSector, this.gameObject);
			map.deselectAll();
		}
	}

	/**
	 * startAllocation: used when the sector is clicked and the player hasn't allocated all their gang members.
     * Deselects anyother sectors, shows the allocation menu and selects the current sector.
     */
	private void startAllocation (MapClass map)
	{
		map.deselectAll(); //Deselecting all sectors at this point lets the player allocate members more easily by just left-clicking through the sectors

		GameObject.Find("UICanvas").GetComponent<GameUI>().showAllocationUIForm(true);
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		sprite.color = new Color (0, 0, 0, 1);
		this.selected = true;
	}

	/**
	 * highlightAdjacentSectors: used when a player clicks one of their sectors, without previously selecting one.
	 * returns: number of sectors highlighted. if this value is 0, there are no valid moves for the sector.
	 * 
	 * Highlights sectors adjacent to the current sector based on the turn phase (ATTACK/MOVEMENT) and the sector ownership.
	 */
	private int highlightAdjacentSectors()
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
		return sectorsHighlighted;
	}

	public void addUnits(int value)
	{
		this.attack_value += value;
		this.defence_value += value;
		this.units += value;
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
        set 
		{ 
			units = value;
			attack_value = value;
			defence_value = value;
		}
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
