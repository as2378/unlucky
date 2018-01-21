using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;

public class MapClassTests {
	/**
	 * getSectorFromMap:
	 * Returns: a sector whose parent is GameObject 'map'.
	 * Throws: System.Exception if no sectors are found.
	 */
	private Sector getSectorFromMap(GameObject map)
	{
		//Find a sector on the map.
		foreach(Transform child in map.transform)
		{
			if (child.name.Substring (0, 8) == "Sector #") 
			{
				Sector aSector = child.GetComponent<Sector> ();
				return aSector;
			}
		}
		throw new System.Exception ("Unable to find a sector in map");
	}

	/**
	 * setupSectorForTest:
	 * Finds a sector within the map, then sets it's Selected attribute to 'selected' and if 'highlighted' is true, the sector's SpriteRenderer is coloured black.
	 * Returns: the sector found within the map.
	 */
	private Sector setupSectorForTest(bool selected, bool hightlighted)
	{
		GameObject map = GameObject.Find ("Map");
		Sector aSector = this.getSectorFromMap (map); //Find a sector
		SpriteRenderer aSectorSprite = aSector.GetComponent<SpriteRenderer> ();
		aSector.Selected = selected;			    // Select/deselect the sector.
		if (hightlighted)
		{
			aSectorSprite.color = new Color (0, 0, 0); //  Set color to the highlight color.
		} 
		else
		{
			aSectorSprite.color = aSector.Owner.Colour; // Set color to the non-highlight color.
		}
		return aSector;
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
	//*****************************************************************************************\\


	[UnityTest]
	/**
	 * DeselectAll_deselects_sector:
	 * Tests if deselectAll deselects a sector and returns its colour back to the owner's colour.
	 */
	public IEnumerator deselectAll_deselects_sector()
	{
		this.load_game ();
		yield return null;

		Sector aSector = this.setupSectorForTest (true, true);
		SpriteRenderer aSectorSprite = aSector.GetComponent<SpriteRenderer> ();

		GameObject.Find ("Map").GetComponent<MapClass> ().deselectAll ();

		Assert.AreEqual (false, aSector.Selected);	//The sector should now be deselected.
		Assert.AreEqual (aSector.Owner.Colour, aSectorSprite.color); //The sector colour should've returned to its owner's colour.
	}
		
	[UnityTest]
	/**
	 * DeselectAll_does_not_change_deselected_sectors:
	 * Tests if deselectAll doesn't change non-selected sectors.
	 */
	public IEnumerator deselectAll_does_not_change_deselected_sectors()
	{
		this.load_game ();
		yield return null;

		Sector aSector = this.setupSectorForTest (false, false);
		SpriteRenderer aSectorSprite = aSector.GetComponent<SpriteRenderer> ();

		GameObject.Find ("Map").GetComponent<MapClass> ().deselectAll (); //Run the method

		Assert.AreEqual (false, aSector.Selected);	//The sector should now be deselected.
		Assert.AreEqual (aSector.Owner.Colour, aSectorSprite.color); //The sector colour should've returned to its owner's colour.
	}

	[UnityTest]
	/**
	 * colourSectors_colours_all_sectors_to_owner_colour:
	 * Tests if running colourSectors changes the colour of all the sector sprites to their
	 * owner's colour.
	 */
	public IEnumerator colourSectors_colours_all_sectors_to_owner_colour()
	{
		this.load_game ();
		yield return null;

		GameObject map = GameObject.Find ("Map");
		foreach (Transform child in map.transform) //Change colour of all sectors to black.
		{
			Sector aSector = child.GetComponent<Sector> ();
			if (aSector != null) 
			{
				aSector.gameObject.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0);
			}
		}

		map.GetComponent<MapClass> ().colourSectors (); //run colourSectors()

		foreach (Transform child in map.transform) //Does the colour of each sector match their owner's colour?
		{
			Sector aSector = child.GetComponent<Sector> ();
			if (aSector != null) 
			{
				SpriteRenderer aSectorSprite = aSector.gameObject.GetComponent<SpriteRenderer> ();
				Assert.AreEqual (aSector.Owner.Colour, aSectorSprite.color);
			}
		}
	}

	[UnityTest]
	/**
	 * getSelectedSector_returns_correct_sector:
	 * Tests if getSelectedSector returns a selected sector. 
	 */ 
	public IEnumerator getSelectedSector_returns_correct_sector()
	{
		this.load_game ();
		yield return null;

		GameObject map = GameObject.Find ("Map");
		Sector aSector = this.setupSectorForTest (true, true); //Find & select a sector.

		GameObject actualSector = map.GetComponent<MapClass> ().getSelectedSector (); //Run getSelectedSector()

		try
		{
			Assert.AreSame (aSector.gameObject, actualSector);	//Test if actualSector is the same as the sector manually selected.
		}
		catch(AssertionException e)
		{
			Debug.LogError (e.Message);
		}
		finally  // Returns the scene to its starting state.
		{
			aSector.Selected = false;
			aSector.GetComponent<SpriteRenderer> ().color = aSector.Owner.Colour;
		}
	}

	[UnityTest]
	/**
	 * getSelectedSector_no_selected_sectors:
	 * Tests if there are no selected sectors, getSelectedSector should returned null.
	 */ 
	public IEnumerator getSelectedSector_no_selected_sectors()
	{
		this.load_game ();
		yield return null;

		GameObject map = GameObject.Find ("Map");
		GameObject selectedSector = map.GetComponent<MapClass> ().getSelectedSector ();
		
		Assert.IsNull (selectedSector); //getSelectedSector should return null.
	}
}
