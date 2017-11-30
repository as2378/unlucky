using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {
	private string name;
	private Color colour;
	private bool has_allocated = false;
	private int gang_members_left = 50;

	public PlayerClass (string name, Color colour)
	{
		this.name = name;
		this.colour = colour;
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
