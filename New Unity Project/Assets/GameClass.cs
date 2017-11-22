using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameClass : MonoBehaviour 
{
	public static readonly int ATTACK = 0;
	public static readonly int MOVEMENT = 1;

	public static int GameState = MOVEMENT;

	private static List<string> playerNames = new List<string>();

	/*static GameClass(){
		playerNames.Add ("Test");
	}*/

	private static int currentPlayer = 0;

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

