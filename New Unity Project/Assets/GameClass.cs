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

	private static List<string> playerNames = new List<string>();
	private static int currentPlayer = 0;

	/*static GameClass(){
		playerNames.Add ("Plr1");
		playerNames.Add ("Plr2");
		playerNames.Add ("Plr3");
	}*/

	public static string CurrentPlayer
	{
		get{ return playerNames[currentPlayer]; }
	}

	public static void changeTurn()
	{
		currentPlayer++;
		if (currentPlayer == playerNames.Count) 
		{
			currentPlayer = 0;
		}
	}
}

