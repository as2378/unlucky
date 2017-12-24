using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is currently used to store information about a player within the game.
 */
public class PlayerClass {
	private string name;
	private Color colour;
	private bool has_allocated = false;
	private int gang_members_left = 0;

	public PlayerClass (string name, Color colour)
	{
		this.name = name;
		this.colour = colour;
	}

	/**
	 * giveNewGangMembers:
	 * This method increases the GangMembersLeft attribute, adding 10 to its value for each sector this player owns. 
	 */
	public void giveNewGangMembers()
	{
		foreach (Transform sector in GameObject.Find("Map").transform) 
		{
			if (sector.name.Substring (0, 8) == "Sector #") 
			{
				if (this == sector.GetComponent<Sector> ().Owner) 
				{
					this.gang_members_left += 10;
				}
			}
		}
	}

	public string Name
	{
		get { return this.name; }
	}

	public Color Colour
	{
		get { return this.colour; }
	}

	public bool Allocated
	{
		get { return has_allocated; }
		set { has_allocated = value; }
	}

	public int GangMembersLeft
	{
		get { return gang_members_left; }
		set { gang_members_left = value; }
	}
}
