using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The class is used as a collection of sectors.
 * Contains various methods which manipulate the sectors as a whole.
 * It is attached to the 'Map' GameObject
 */
public class MapClass : MonoBehaviour
{
	//Key = Sector, Data = list of sectors adjacent to key.
	private Dictionary<GameObject,List<GameObject>> sector_graph = new Dictionary<GameObject,List<GameObject>>();

	/**
	 * Start():
	 * called when the scene is loaded.
	 * Puts all sectors into the graph sector_graph (stored as a dictionary);
	 */
	void Start()
    {
		GameClass.generatePlayers ();

		foreach (Transform child in transform) 
		{
			if (child.name.Substring (0, 8) == "Sector #") 
			{
				Sector sector = child.GetComponent<Sector>();
				List<GameObject> adjacent_sectors = sector.AdjacentSectors;

				this.sector_graph.Add(child.gameObject,adjacent_sectors);
			}
		}	
		this.assignSectorsToPlayers ();
		this.colourSectors ();
		GameClass.allocateGangMembersToPlayers ();
		GameClass.changeTurn ();
    }

	/**
	 * assignSectorsToPlayers():
	 * used for testing sectors and sector highlighing.
	 * Assigns sectors with a random player from GameClass' player list.
	 */
	private void assignSectorsToPlayers()
	{
		List<PlayerClass> players = GameClass.Players;

		foreach (GameObject sector in sector_graph.Keys)
		{
			int index = Random.Range (0, players.Count);
			Sector sectorClass = sector.GetComponent<Sector> ();
			sectorClass.Owner = players [index];
			//print ("Sector: " + sectorClass.name + " Owner: " + sectorClass.Owner.Name + " Colour: " + sectorClass.Owner.Colour);
		}
	}

	/**
	 * deselectAll():
	 * used to deselect all sectors and return them to their owner's colour.
	 */ 
	public void deselectAll(){
		foreach (GameObject sector in sector_graph.Keys) 
		{
			Sector sectorClass = sector.GetComponent<Sector> ();
			sector.GetComponent<SpriteRenderer> ().color = sectorClass.Owner.Colour;

			if (sectorClass.Selected == true) 
			{
				sectorClass.Selected = false;
			}
		}

        //Hide the allocation UI in case it's gang member allocation phase
        GameObject.Find("UICanvas").GetComponent<GameUI>().showAllocationUIForm(false);
	}

	/**
	 * Update():
	 * called every frame. Checks if the right mouse button is clicked.
	 * If so, deselectAll is called.
	 */
	void Update()
	{
		if (Input.GetMouseButtonDown (1)) 
		{
			this.deselectAll ();
		}
	}

	/**
	 * colourSectors():
	 * Changes the colour of all sectors within map to their owner's colour.
	 */
	public void colourSectors()
	{
		foreach (GameObject sector in sector_graph.Keys)
		{
			Sector sectorClass = sector.GetComponent<Sector> ();
			sector.GetComponent<SpriteRenderer> ().color = sectorClass.Owner.Colour;
		}
	}

	/**
	 * getSelectedSector():
	 * Return:  The sector where it's Selected attribute is true;
	 * 			otherwise: null. 
	 * Searches through all sectors in the game until a Selected one is found.
	 */
	public GameObject getSelectedSector()
	{
		foreach (GameObject sector in sector_graph.Keys) 
		{
			if (sector.GetComponent<Sector>().Selected == true) 
			{
				return sector;
			}
		}
		return null;
	}

    public void Combat(GameObject Attacker, GameObject Defender)
    {  
        MarkerMovement sliderGame = GameObject.Find("Marker").GetComponent<MarkerMovement>();
		sliderGame.StartSlider (Attacker, Defender);
    }

	public void calculateCombatOutcome(GameObject attacker, GameObject defender, float attackMultiplier, float defenceMultiplier)
	{
		Sector attackSector = attacker.GetComponent<Sector>();
		Sector defenderSector = defender.GetComponent<Sector>();
		print(defenderSector.Defence + "=Defence  " + attackSector.Attack + "=Attack");
		//this is to test it with expected values
		int attackerA = attackSector.Attack;
		int defenderD = defenderSector.Defence;

		float attack = Random.Range(attackerA/2, attackerA);
		attack += attack * attackMultiplier;
		float defence = Random.Range(defenderD/2, defenderD);
		defence += defence * defenceMultiplier;

		print ("attackMultiplier x" + attackMultiplier + "  defenceMultiplier x" + defenceMultiplier);

		int DamageDone = Mathf.Abs(Mathf.RoundToInt(defence - attack));
		print(attack + "=attack   " + defence + "=defence   " + DamageDone + "=damageDone");
		if(defence > attack)
		{      
			if (DamageDone > attackerA)
			{
				attackSector.Owner = defenderSector.Owner;
				attacker.GetComponent<SpriteRenderer> ().color = defenderSector.Owner.Colour;
				attackSector.Units = 1;
				//defenderSector.Attack = defenderSector.Attack - 1;
				//defenderSector.Defence = defenderSector.Defence - 1;
				defenderSector.addUnits(-1);
			}
			else if (DamageDone < attackerA)
			{
				//attackSector.Attack = attackerA - DamageDone;
				attackSector.addUnits (-DamageDone);
				defenderSector.addUnits (-DamageDone);
			}
		}
		else if(attack > defence)
		{      
			if (DamageDone > defenderD)
			{
				defenderSector.Owner = attackSector.Owner;
				defender.GetComponent<SpriteRenderer> ().color = attackSector.Owner.Colour;
				defenderSector.Units = 1;
				//attackSector.Attack = attackSector.Attack - 1;
				//attackSector.Defence = attackSector.Defence - 1;
				attackSector.addUnits(-1);
			}
			else if (DamageDone < defenderD)
			{
				//defenderSector.Defence = defenderD + DamageDone;
				defenderSector.addUnits(-DamageDone);
				attackSector.addUnits (-DamageDone);
			}
		}
		print(defenderSector.Defence + "=Defence" + attackSector.Attack + "=Attack ");
	}
}
