using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The class is used to control the gameplay
 * 
 * Currently, this class keeps track of the players, whose turn it is and
 * the GameState of the turn, (ATTACK or MOVEMENT).
 *  
 */
public class GameClass
{
	public static readonly int ATTACK = 0;
	public static readonly int MOVEMENT = 1;
	public static int GameState = ATTACK;

	private static List<PlayerClass> players = new List<PlayerClass>();
	private static int currentPlayer = 0;

	private static List<Color> colours = new List<Color>(new Color[]{
		new Color(0.7f, 0.3f, 0.3f, 1),new Color(1.0f, 1.0f, 0.4f, 1),
		new Color(0.5f, 1.0f, 0.5f, 1),new Color(0.4f, 1.0f, 1.0f, 1),
		new Color(0.4f, 0.4f, 0.9f, 1),new Color(0.9f, 0.4f, 0.9f, 1),
		new Color(0.9f, 0.6f, 0.4f, 1),new Color(0.0f, 0.5f, 0.0f, 1),
		new Color(0.3f, 0.3f, 0.3f, 1),new Color(0.7f, 0.8f, 0.7f, 1)});

	public static void init(){
		for (int i = 1; i <= 3; i++) 
		{
			PlayerClass player = new PlayerClass ("Plr" + i, generateColour ());
			players.Add (player);
		}

	}

	public static Color generateColour()
	{
		int index = Random.Range (0, colours.Count);
		Color colour = colours [index];
		colours.Remove (colour);
		return colour;
	}

	public static PlayerClass CurrentPlayer
	{
		get{ return players[currentPlayer]; }
	}

	public static void changeTurn()
	{
		currentPlayer++;
		if (currentPlayer == players.Count) 
		{
			currentPlayer = 0;
		}
	}

	public static List<PlayerClass> Players {
		get { return players; }
	}
}

