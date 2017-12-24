using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameClassTests {
	/**
	 * load_game:
	 * Loads MainGame if it is not already loaded.
	 */
	private void load_game()
	{
		if (SceneManager.GetActiveScene ().name != "MainGame") 
		{
			SceneManager.LoadScene ("MainGame");
		}
	}

	/**
	 * reset_turn_to_first_player():
	 * Used to return the current player to the inital current player (at index 0).
	 * This is necessary for esuring that the state of the game is reset before other tests are run.
	 */
	private void reset_turn_to_first_player()
	{
		List <PlayerClass> playersInGame = GameClass.Players;
		PlayerClass currentPlayer = GameClass.CurrentPlayer;
		int currentPlayerIndex = playersInGame.IndexOf (currentPlayer);

		if (currentPlayerIndex != 0) 
		{
			for (int i = 0; i < playersInGame.Count - currentPlayerIndex; i++) 
			{
				GameClass.changeTurn ();
			}
		}
	}

	/**
	 * test_players_are_same:
	 * Parameters: PlayerClass expectedPlr, PlayerClass actualPlr
	 * Tests if expectedPlr & actualPlr are the same, then resets the game state
	 * regardless of if the test passed or not.
	 */
	private void test_players_are_same(PlayerClass expectedPlr, PlayerClass actualPlr)
	{
		try
		{
			Assert.AreSame (expectedPlr, actualPlr);
		}
		catch(AssertionException e)
		{
			Debug.LogError (e.Message);
		}
		finally
		{
			this.reset_turn_to_first_player ();
		}
	}

	//*****************************************************************************************\\
	[UnityTest]
	/**
	 * changeTurn_moves_to_next_player_basic_case:
	 * Tests if changeTurn moves to the next player in the Player list.
	 * Tests the basic case where the next player is in the next position in the list.
	 */
	public IEnumerator changeTurn_moves_to_next_player_basic_case() 
	{
		this.load_game ();
		yield return null;

		List <PlayerClass> playersInGame = GameClass.Players;

		PlayerClass expectedNextPlayer = playersInGame [1];
		GameClass.changeTurn ();
		PlayerClass actualNextPlayer = GameClass.CurrentPlayer;

		test_players_are_same (expectedNextPlayer, actualNextPlayer);
	}

	[UnityTest]
	/**
	 * changeTurn_moves_to_next_player_loop_case():
	 * Tests if changeTurn loops back to the first player when it reaches the end of the player list.
	 */
	public IEnumerator changeTurn_moves_to_next_player_loop_case()
	{
		this.load_game ();
		yield return null;

		List <PlayerClass> playersInGame = GameClass.Players;
		for (int i = 0; i < playersInGame.Count; i++) 
		{
			GameClass.changeTurn ();
		}
		PlayerClass expectedNextPlayer = playersInGame [0];
		PlayerClass actualNextPlayer = GameClass.CurrentPlayer;

		test_players_are_same (expectedNextPlayer, actualNextPlayer);
	}

	[UnityTest]
	/**
	 * currentPlayer_on_start:
	 * Tests if the CurrentPlayer accessor returns the correct player when the game is initialised.
	 */ 
	public IEnumerator currentPlayer_on_start()
	{
		this.load_game ();
		yield return null;

		PlayerClass expectedPlayer = GameClass.Players [0];
		Assert.AreSame (expectedPlayer, GameClass.CurrentPlayer);
	}
}