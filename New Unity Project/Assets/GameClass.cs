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
public class GameClass : MonoBehaviour 
{
	public static readonly int ATTACK = 0;
	public static readonly int MOVEMENT = 1;
	public static int GameState = ATTACK;

	private static List<Player> players = new List<Player>();
	private static int currentPlayer = 0;

	private static List<Color> colours = new List<Color>(new Color[]{
		new Color(210,80,80),new Color(225,205,100),new Color(140,230,100),new Color(100,220,230),
		new Color(100,100,230),new Color(225,100,230),new Color(225,165,100),new Color(105,200,170),
		new Color(130,130,130),new Color(190,205,120)});

	public struct Player
	{
		string name;
		Color colour;
		public void setValues(string name, Color color){
			this.name = name;
			this.colour = color;
		}
		public string Name {
			get { return name; }
		}
		public Color Colour {
			get { return colour; }
		}
	}

	static GameClass(){
		Player player1 = new Player ();
		player1.setValues ("Plr1", generateColour ());
		players.Add (player1);

		Player player2 = new Player ();
		player2.setValues ("Plr2", generateColour ());
		players.Add (player2);

		Player player3 = new Player ();
		player3.setValues ("Plr3", generateColour ());
		players.Add (player3);
	}

	public static Color generateColour()
	{
		int index = Random.Range (0, colours.Length) as int;
		Color colour = colours [index];
		colours.Remove (colour);
		return colour;
	}

	public static Player CurrentPlayer
	{
		get{ return players[currentPlayer]; }
	}

	public static void changeTurn()
	{
		currentPlayer++;
		if (currentPlayer == players.Keys.Count) 
		{
			currentPlayer = 0;
		}
	}

	public static List<Player> PlayerList {
		get { return players; }
	}
}

