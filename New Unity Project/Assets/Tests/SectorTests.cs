using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class SectorTests 
{
	/**
	 * createScene:
	 * Creates a new empty scene named 'SectorTestScene' and sets it as the active scene.
	 * returns: the new scene.
	 */
	private Scene createScene()
	{
		Scene newScene = EditorSceneManager.CreateScene("SectorTestScene");
		SceneManager.SetActiveScene (newScene);
		return newScene;
	}

	/**
	 * createSector:
	 * Generates a new (deactivated) sector gameobject named 'name', adjacent to 'adjacentSectors'.
	 * The sector is placed within the 'parent' gameobject.
	 * returns: the new sector gameobject.
	 */
	private GameObject createSector(string name, int[] adjacentSectors, GameObject parent)
	{
		GameObject newSector = new GameObject (name);
		newSector.transform.SetParent (parent.transform);
		newSector.SetActive (false);
		newSector.AddComponent<Sector> ();
		newSector.GetComponent<Sector> ().adjacent_sector_ids.AddRange(adjacentSectors);
		newSector.AddComponent<SpriteRenderer> ();
		return newSector;
	}
		
	/**
	 * testAdjacentSectors:
	 * Tests if the actualSectors list matches the expectedSectors list.
	 */
	private void testAdjacentSectors(List<GameObject> expectedSectors, List<GameObject> actualSectors)
	{
		foreach (GameObject expectedSector in expectedSectors)
		{
			bool foundMatch = false;
			foreach (GameObject actualSector in actualSectors)
			{
				if (expectedSector.name == actualSector.name) {
					foundMatch = true;
				}
			}
			Assert.True (foundMatch);
		}
	}

	/**
	 * activateSectors:
	 * Activates all GameObjects in the sectors array.
	 */
	private void activateSectors(GameObject[] sectors)
	{
		foreach (GameObject s in sectors) 
		{
			s.SetActive (true);
		}
	}

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
	 * findASector:
	 * Returns: a sector nested within GameObject 'map', which is either owned by the current player or not (depending on 'ownedByCurrentPlayer').
	 * Throws: Exception if no sectors can be found.
	 */
	private GameObject findASector(GameObject map,bool ownedByCurrentPlayer)
	{
		foreach (Transform child in map.transform) 
		{
			if (child.name.Substring (0, 8) == "Sector #")
			{
				if ((child.gameObject.GetComponent<Sector> ().Owner == GameClass.CurrentPlayer) == ownedByCurrentPlayer) 
				{
					return child.gameObject;
				}
			}
		}
		throw new Exception ("Unable to find any sectors to test");
	}

	/**
	 * findAnAdjacentSector:
	 * Returns: a sector adjacent to 'selectedSector', either owned by the current player or not (depending on ownedByCurrentPlayer)
	 * Throws: Exception if there is only 1 player.
	 */
	private GameObject findAdjacentSector(GameObject selectedSector,bool ownedByCurrentPlayer)
	{
		foreach (GameObject adjSect in selectedSector.GetComponent<Sector>().AdjacentSectors) 
		{
			if ((adjSect.GetComponent<Sector> ().Owner == GameClass.CurrentPlayer) == ownedByCurrentPlayer) 
			{
				return adjSect;
			}
		}
		foreach (PlayerClass player in GameClass.Players) 
		{
			if ((player == GameClass.CurrentPlayer) == ownedByCurrentPlayer) 
			{
				GameObject newSector = selectedSector.GetComponent<Sector> ().AdjacentSectors [0];
				newSector.GetComponent<Sector>().Owner = player;
				newSector.GetComponent<SpriteRenderer> ().color = player.Colour;
				return newSector;
			}
		}
		throw new Exception ("Unable to find any sectors to test");
	}

	/**
	 * setupHighlightTest():
	 * Finds a sector owned by the currentplayer within map. Sets the GameState based on the attackState parameter.
	 * sets the currentPlayer's allocated attribute to true.
	 * Returns the sector found.
	 */
	private Sector setupHighlightTest(GameObject map,bool attackState)
	{
		GameObject aSector = this.findASector (map,true);
		Sector aSectorClass = aSector.GetComponent<Sector> ();
		if (attackState) 
		{
			GameClass.GameState = GameClass.ATTACK;
		} 
		else 
		{
			GameClass.GameState = GameClass.MOVEMENT;
		}
		aSectorClass.Owner.Allocated = true;
		return aSectorClass;
	}

	/**
	 * testSectorHighlighting:
	 * Tests if the sectors that are owned by the current player or not ('highlightedOwnedByPlayer') adjacent to 'clickedSector' are highlighted
	 * in colour 'expectedHighlightColor'. Also tests if clickedSector is selected if 1 or more adjacent sectors are highlighted.
	 * It then resets the game to it's starting state.
	 */
	private void testSectorHighlighting(GameObject map, Sector clickedSector,bool hightlightedOwnedByPlayer, Color expectedHighlightColour)
	{
		try
		{
			int numberOfValidSectors = 0;
			foreach (GameObject adjSect in clickedSector.AdjacentSectors) 
			{
				if ((adjSect.GetComponent<Sector> ().Owner == clickedSector.Owner) == hightlightedOwnedByPlayer) 
				{
					Assert.AreEqual (expectedHighlightColour, adjSect.GetComponent<SpriteRenderer> ().color);
					numberOfValidSectors++;
				}
				else
				{
					Assert.AreEqual (adjSect.GetComponent<Sector> ().Owner.Colour, adjSect.GetComponent<SpriteRenderer> ().color);
				}
			}
			if (numberOfValidSectors > 0) 
			{
				Assert.IsTrue (clickedSector.Selected);
			}
		}
		catch(Exception e)
		{
			Debug.LogException (e);
		}
		finally
		{
			map.GetComponent<MapClass> ().deselectAll ();
			clickedSector.Owner.Allocated = false;
			GameClass.GameState = GameClass.ATTACK;
		}
	}
		
	//*****************************************************************************************\\
	[UnityTest]
	/**
	 * sector_creation_2_sectors():
	 * Tests that the sectors are initialised with the correct adjacent sectors for a map with 2 sectors.
	 * Sector #1 should be adjacent to Sector #2, and Sector #2 should be adjacent to Sector #1.
	 */
	public IEnumerator sector_creation_2_sectors() 
	{
		Scene newScene = this.createScene ();
		GameObject map = new GameObject ("Map");

		//Sector #1
		int[] adjSectors = new int[]{2};
		GameObject sector1 = this.createSector ("Sector #1", adjSectors, map);

		//Sector #2
		adjSectors = new int[]{1};
		GameObject sector2 = this.createSector ("Sector #2", adjSectors, map);

		activateSectors(new GameObject[]{sector1,sector2});

		List<GameObject> sector1AdjSectors = new List<GameObject> (new GameObject[]{sector2});
		List<GameObject> sector2AdjSectors = new List<GameObject> (new GameObject[]{sector1});

		yield return null; //Waits for everything to load.

		testAdjacentSectors (sector1AdjSectors, sector1.GetComponent<Sector> ().AdjacentSectors);
		testAdjacentSectors (sector2AdjSectors, sector2.GetComponent<Sector> ().AdjacentSectors);
		SceneManager.UnloadSceneAsync (newScene.name);
	}

	[UnityTest]
	/**
	 * sector_creation_3_sectors_complete_graph():
	 * Tests if the sectors are initialised with the correct adjacent sectors for a 3 sector map where each sector is adjacent to all the rest.
	 * Sector #1 is adjacent to 2 & 3. Sector #2 is adjacent to 1 & 3. Sector #3 is adjacent to 1 & 2.
	 */ 
	public IEnumerator sector_creation_3_sectors_complete_graph()
	{
		Scene newScene = this.createScene ();
		GameObject map = new GameObject ("Map");

		//Sector1:
		int[] adjSectors = new int[]{2,3};
		GameObject sector1 = this.createSector ("Sector #1", adjSectors, map);
		//Sector2:
		adjSectors = new int[]{1,3};
		GameObject sector2 = this.createSector ("Sector #2", adjSectors, map);
		//Sector3:
		adjSectors = new int[]{1,2};
		GameObject sector3 = this.createSector ("Sector #3", adjSectors, map);

		activateSectors(new GameObject[]{sector1,sector2,sector3});

		List<GameObject> sector1AdjSectors = new List<GameObject> (new GameObject[]{sector2,sector3});
		List<GameObject> sector2AdjSectors = new List<GameObject> (new GameObject[]{sector1,sector3});
		List<GameObject> sector3AdjSectors = new List<GameObject> (new GameObject[]{sector1,sector2});

		yield return null;

		testAdjacentSectors (sector1AdjSectors, sector1.GetComponent<Sector> ().AdjacentSectors);
		testAdjacentSectors (sector2AdjSectors, sector2.GetComponent<Sector> ().AdjacentSectors);
		testAdjacentSectors (sector3AdjSectors, sector3.GetComponent<Sector> ().AdjacentSectors);

		SceneManager.UnloadSceneAsync (newScene.name);
	}

	[UnityTest]
	/**
	 * sector_creation_3_sectors_uncomplete_graph():
	 * Tests if the sectors are initialised with the correct adjacent sectors for an uncomplete 3 sector map.
	 * Some sectors are adjacent to more sectors than others.
	 * Sector #1 is adjacent to 2, Sector #2 is adjacent to 1 & 3, and Sector #3 is adjacent to 2.
	 */
	public IEnumerator sector_creation_3_sectors_uncomplete_graph()
	{
		Scene newScene = this.createScene ();
		GameObject map = new GameObject ("Map");

		//Sector1:
		int[] adjSectors = new int[]{2};
		GameObject sector1 = this.createSector ("Sector #1", adjSectors, map);
		//Sector2:
		adjSectors = new int[]{1,3};
		GameObject sector2 = this.createSector ("Sector #2", adjSectors, map);
		//Sector3:
		adjSectors = new int[]{2};
		GameObject sector3 = this.createSector ("Sector #3", adjSectors, map);

		activateSectors(new GameObject[]{sector1,sector2,sector3});

		List<GameObject> sector1AdjSectors = new List<GameObject> (new GameObject[]{sector2});
		List<GameObject> sector2AdjSectors = new List<GameObject> (new GameObject[]{sector1,sector3});
		List<GameObject> sector3AdjSectors = new List<GameObject> (new GameObject[]{sector2});

		yield return null;

		testAdjacentSectors (sector1AdjSectors, sector1.GetComponent<Sector> ().AdjacentSectors);
		testAdjacentSectors (sector2AdjSectors, sector2.GetComponent<Sector> ().AdjacentSectors);
		testAdjacentSectors (sector3AdjSectors, sector3.GetComponent<Sector> ().AdjacentSectors);

		SceneManager.UnloadSceneAsync (newScene.name);
	}

	[UnityTest]
	/**
	 * clickSector_highlight_adjacent_sectors_attack_phase:
	 * Tests that all adjacent sectors not owned by the player are highlighted when a sector is clicked during
	 * the ATTACK phase.
	 */
	public IEnumerator clickSector_highlight_adjacent_sectors_attack_phase()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		Sector aSectorClass = setupHighlightTest (map,true);
		aSectorClass.clickSector ();

		testSectorHighlighting (map, aSectorClass, false, new Color (1, 0, 0));
	}


	[UnityTest]
	/**
	 * clickSector_highlight_adjacent_sectors_movement_phase:
	 * Tests that all adjacent sectors owned by the current player are highlighted when a sector is clicked during
	 * the MOVEMENT phase.
	 */
	public IEnumerator clickSector_highlight_adjacent_sectors_movement_phase()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		Sector aSectorClass = this.setupHighlightTest (map, false);
		aSectorClass.clickSector ();

		testSectorHighlighting (map, aSectorClass, true, new Color (1, 1, 1));
	}

	[UnityTest]
	/**
	 * clickSector_unowned_sector:
	 * Tests that if a sector not owned by the current player is clicked, it should not become selected or highlighted.
	 */
	public IEnumerator clickSector_unowned_sector()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		GameObject aSector = this.findASector (map, false);
		Sector aSectorClass = aSector.GetComponent<Sector> ();
		aSectorClass.clickSector ();

		try
		{
			Assert.IsFalse (aSectorClass.Selected);
			Assert.AreEqual (aSectorClass.Owner.Colour, aSector.GetComponent<SpriteRenderer> ().color);
		}
		catch(Exception e)
		{
			Debug.LogException (e);
		}
		finally
		{
			map.GetComponent<MapClass> ().deselectAll ();
		}
	}

	[UnityTest]
	/**
	 * clickSector_invalid_second_sector_clicked_attack_phase:
	 * Tests that nothing changes when a player clicks one of their own adjacent sectors after selecting a sector during an attack phase.
	 * Tests that the first selected sector remains selected and the highlighted adjacent sectors stay highlighted.
	 */
	public IEnumerator clickSector_invalid_second_sector_clicked_attack_phase()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		Sector firstSector = this.setupHighlightTest (map, true);
		firstSector.clickSector ();
		GameObject secondSector = findAdjacentSector (firstSector.gameObject, true);
		secondSector.GetComponent<Sector>().clickSector ();
		try
		{
			
			Assert.IsTrue(firstSector.Selected);
			Assert.IsFalse(secondSector.GetComponent<Sector>().Selected);
			testSectorHighlighting(map,firstSector,false,new Color(1,0,0));
		}
		catch(Exception e) 
		{
			Debug.LogException (e);
		}
		finally 
		{
			map.GetComponent<MapClass> ().deselectAll ();
			GameClass.CurrentPlayer.Allocated = false;
		}
	}

	[UnityTest]
	/**
	 * clickSector_invalid_second_sector_clicked_movement_phase:
	 * Tests that nothing changes when a player clicks an enemy adjacent sector after selecting a sector during a movement phase.
	 * Tests that the first selected sector remains selected and the highlighted adjacent sectors stay highlighted.
	 */
	public IEnumerator clickSector_invalid_second_sector_clicked_movement_phase()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		Sector firstSector = this.setupHighlightTest (map, false);
		firstSector.clickSector ();

		GameObject secondSector = findAdjacentSector (firstSector.gameObject, false);
		secondSector.GetComponent<Sector>().clickSector ();
		try
		{
			Assert.IsTrue(firstSector.Selected);
			Assert.IsFalse(secondSector.GetComponent<Sector>().Selected);
			testSectorHighlighting(map,firstSector,true,new Color(1,1,1));
		}
		catch(Exception e) 
		{
			Debug.LogException (e);
		}
		finally 
		{
			map.GetComponent<MapClass> ().deselectAll ();
			GameClass.CurrentPlayer.Allocated = false;
		}
	}

	[UnityTest]
	/**
	 * clickSector_valid_attack_deselects_sectors:
	 * Tests if clicking an adjacent enemy sector after clicking an owned sector deselects the first sector and removes the highlighting.
	 */
	public IEnumerator clickSector_valid_attack_deselects_sectors()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		Sector firstSector = this.setupHighlightTest (map, true);
		firstSector.clickSector ();

		GameObject secondSector = findAdjacentSector (firstSector.gameObject, false);
		secondSector.GetComponent<Sector>().clickSector ();

		try
		{
			Assert.IsNull(map.GetComponent<MapClass>().getSelectedSector());
			foreach(GameObject adjSector in firstSector.AdjacentSectors)
			{
				Assert.AreEqual(adjSector.GetComponent<Sector>().Owner.Colour, adjSector.GetComponent<SpriteRenderer>().color);
			}
		}
		catch(Exception e) 
		{
			Debug.LogException (e);
		}
		finally 
		{
			map.GetComponent<MapClass> ().deselectAll ();
			GameClass.CurrentPlayer.Allocated = false;
		}
	}

	[UnityTest]
	/**
	 * clickSector_valid_attack_deselects_sectors:
	 * Tests if clicking an adjacent owned sector after clicking an owned sector deselects the first sector and removes the highlighing.
	 */
	public IEnumerator clickSector_valid_movement_deselects_sectors()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		Sector firstSector = this.setupHighlightTest (map, false);
		firstSector.clickSector ();

		GameObject secondSector = findAdjacentSector (firstSector.gameObject, true);
		secondSector.GetComponent<Sector>().clickSector ();
		try
		{
			Assert.IsNull(map.GetComponent<MapClass>().getSelectedSector());
			foreach(GameObject adjSector in firstSector.AdjacentSectors)
			{
				Assert.AreEqual(adjSector.GetComponent<Sector>().Owner.Colour, adjSector.GetComponent<SpriteRenderer>().color);
			}
		}
		catch(Exception e) 
		{
			Debug.LogException (e);
		}
		finally 
		{
			map.GetComponent<MapClass> ().deselectAll ();
			GameClass.CurrentPlayer.Allocated = false;
		}
	}

	[UnityTest]
	/**
	 * clickSector_no_valid_moves_for_attack:
	 * Tests that nothing is selected or highlighted when a sector is clicked that has no valid attack moves from it.
	 */
	public IEnumerator clickSector_no_valid_moves_for_attack()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		GameObject aSector = this.findASector (map, true);
		Sector aSectorClass = aSector.GetComponent<Sector> ();

		List<PlayerClass> sectorOwners = new List<PlayerClass> ();
		foreach (GameObject adjSect in aSectorClass.AdjacentSectors) 
		{
			sectorOwners.Add (adjSect.GetComponent<Sector> ().Owner);
			adjSect.GetComponent<Sector> ().Owner = GameClass.CurrentPlayer;
			adjSect.GetComponent<SpriteRenderer> ().color = GameClass.CurrentPlayer.Colour;
		}
		GameClass.CurrentPlayer.Allocated = true;
		GameClass.GameState = GameClass.ATTACK;
		aSectorClass.clickSector ();

		try
		{
			Assert.IsNull(map.GetComponent<MapClass>().getSelectedSector());
			foreach(GameObject adjSector in aSectorClass.AdjacentSectors)
			{
				Assert.AreEqual(adjSector.GetComponent<Sector>().Owner.Colour, adjSector.GetComponent<SpriteRenderer>().color);
			}
		}
		catch(Exception e)
		{
			Debug.LogException (e);
		}
		finally
		{
			for (int i = 0; i < aSectorClass.AdjacentSectors.Count ; i++) 
			{
				GameObject adjSect = aSectorClass.AdjacentSectors [i];
				PlayerClass owner = sectorOwners [i];
				adjSect.GetComponent<Sector> ().Owner = owner;
			}
			map.GetComponent<MapClass> ().deselectAll ();
			GameClass.CurrentPlayer.Allocated = false;
		}
	}

	[UnityTest]
	/**
	 * clickSector_no_valid_moves_for_attack:
	 * Tests that nothing is selected or highlighted when a sector is clicked that has no valid movement moves from it.
	 * (requires at least 2 players to pass).
	*/
	public IEnumerator clickSector_no_valid_moves_for_movement()
	{
		this.load_game ();
		yield return null;
		GameObject map = GameObject.Find ("Map");
		PlayerClass otherPlayer = GameClass.CurrentPlayer;
		GameClass.changeTurn ();
		GameObject aSector = this.findASector (map, true);
		Sector aSectorClass = aSector.GetComponent<Sector> ();

		List<PlayerClass> sectorOwners = new List<PlayerClass> ();
		foreach (GameObject adjSect in aSectorClass.AdjacentSectors) 
		{
			sectorOwners.Add (adjSect.GetComponent<Sector> ().Owner);
			adjSect.GetComponent<Sector> ().Owner = otherPlayer;
			adjSect.GetComponent<SpriteRenderer> ().color = otherPlayer.Colour;
		}
		GameClass.CurrentPlayer.Allocated = true;
		GameClass.GameState = GameClass.MOVEMENT;

		aSectorClass.clickSector ();
		try
		{
			Assert.IsNull(map.GetComponent<MapClass>().getSelectedSector());
			foreach(GameObject adjSector in aSectorClass.AdjacentSectors)
			{
				Assert.AreEqual(adjSector.GetComponent<Sector>().Owner.Colour, adjSector.GetComponent<SpriteRenderer>().color);
			}
		}
		catch(Exception e)
		{
			Debug.LogException (e);
		}
		finally
		{
			//Reset ownership of sectors.
			for (int i = 0; i < aSectorClass.AdjacentSectors.Count ; i++) 
			{
				GameObject adjSect = aSectorClass.AdjacentSectors [i];
				PlayerClass owner = sectorOwners [i];
				adjSect.GetComponent<Sector> ().Owner = owner;
			}
			//Reset currentplayer back to first player.
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
			//Reset selection, colouring, gamestate & player allocation.
			map.GetComponent<MapClass> ().deselectAll ();
			GameClass.CurrentPlayer.Allocated = false;
			GameClass.GameState = GameClass.ATTACK;
		}
	}
}