using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {
	private string name;
	private Color colour;

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
}
