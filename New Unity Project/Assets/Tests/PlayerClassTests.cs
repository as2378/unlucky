using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayerClassTests {
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

	//*****************************************************************************************\\
	[Test]
	/**
	 * PlayerClass_instantiation:
	 * Tests if the constructor for PlayerClass works correctly.
	 */
	public void PlayerClass_instantiation() 
	{
		string playerName = "TestPlayer";
		Color playerColour = new Color (0.5f, 0.5f, 0.5f);

		PlayerClass newPlayer = new PlayerClass (playerName, playerColour);

		Assert.AreEqual (playerName, newPlayer.Name);
		Assert.AreEqual (playerColour, newPlayer.Colour);
	}

	[Test]
	/**
	 * PlayerClass_allocated_initialised_as_false:
	 * Tests if allocated is set to false on initialisation.
	 */
	public void PlayerClass_allocated_initialised_as_false()
	{
		string playerName = "TestPlayer";
		Color playerColour = new Color (0.5f, 0.5f, 0.5f);
		PlayerClass newPlayer = new PlayerClass (playerName, playerColour);

		Assert.IsFalse (newPlayer.Allocated);
	}

	[UnityTest]
	/**
	 * PlayerClass_allocates_correct_number_of_units:
	 * Tests that the players are assigned the correct number of gang members.
	 */
	public IEnumerator PlayerClass_allocates_correct_number_of_units()
	{
		this.load_game ();
		yield return null;

		PlayerClass aPlayer = GameClass.CurrentPlayer;
		MapClass map = GameObject.Find ("Map").GetComponent<MapClass> ();

		int numberOfOwnedSectors = 0;

		foreach (Transform child in map.gameObject.transform) 
		{
			if (child.name.Substring (0, 8) == "Sector #") 
			{
				if (child.GetComponent<Sector> ().Owner == aPlayer) {
					numberOfOwnedSectors++;
				}
			}
		}
		Assert.AreEqual (numberOfOwnedSectors * 10, aPlayer.GangMembersLeft);
	}
}
