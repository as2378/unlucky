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
	private static int currentPlayer = -1;

	private static List<Color> colours = new List<Color>(new Color[] {
		new Color(0.7f, 0.3f, 0.3f, 1), new Color(1.0f, 1.0f, 0.4f, 1),
		new Color(0.5f, 1.0f, 0.5f, 1), new Color(0.4f, 1.0f, 1.0f, 1),
		new Color(0.4f, 0.4f, 0.9f, 1), new Color(0.9f, 0.4f, 0.9f, 1),
		new Color(0.9f, 0.6f, 0.4f, 1), new Color(0.0f, 0.5f, 0.0f, 1),
		new Color(0.3f, 0.3f, 0.3f, 1), new Color(0.7f, 0.8f, 0.7f, 1)});

	/*
	 * generatePlayers(): Called by MapClass at the start of a game.
	 * 
	 * Generates three players and adds them to the players list.
	 * Currently used for testing.
	 */ 
	public static void generatePlayers() 
	{
		for (int i = 1; i <= 3; i++) 
		{
			PlayerClass player = new PlayerClass ("Plr" + i, generateColour ());
			players.Add (player);
		}

        changeTurn();
	}

	/*
	 * generateColour(): used by generatePlayers() to assign the players a colour.
	 * returns: a Color value chosen from the 'colours' list.
	 * 
	 * Picks a random colour from the colours list and pops it from the list.
	 */ 
	private static Color generateColour()
	{
		int index = Random.Range (0, colours.Count);
		Color colour = colours [index];
		colours.Remove (colour);
		return colour;
	}

	/*
	 * CurrentPlayer:
	 * get: returns the class for the player whos turn it is.
	 */ 
	public static PlayerClass CurrentPlayer
	{
		get{ return players[currentPlayer]; }
	}

	/*
	 * ChangeTurn():
	 * Increments the currentPlayer variable, and loops around to 0 if it reaches the end of the player list.
	 */
	public static void changeTurn()
	{
		currentPlayer++;
		if (currentPlayer == players.Count) 
		{
			currentPlayer = 0;
		}

        if(!players[currentPlayer].Allocated)
        {
            //Show the "gang members left" label
            GameObject.Find("UICanvas").GetComponent<GameUI>().showGangMembersLeftLabel(true);
        }

        //GameObject.Find("Map").GetComponent<MapClass>().printPlayerName();
    }

	public static List<PlayerClass> Players {
		get { return players; }
	}
}

