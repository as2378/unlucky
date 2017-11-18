using UnityEngine;
using System.Collections.Generic;

/*
 * The class is used as a component script for sectors
 * 
 * When attached to a sector, properties have to be set
 * in the inspector in Unity (unit spawn position, the names of
 * adjacent sectors, whether the sector is a college) 
 * */

public class Sector : MonoBehaviour
{
    private int attack_value = 0;
    private int defence_value = 0;
    private int units = 0;

    private Position spawn_position;
    private List<GameObject> adjacent_sectors = new List<GameObject>();

    // Following fields are set via the inspector in Unity
    public double spawn_position_x;
    public double spawn_position_y;
    public double spawn_position_z;

    public List<int> adjacent_sector_ids = new List<int>();

    public bool is_college;

    public struct Position
    {
        public double x, y, z;
    }

    void Start()
    {
        this.spawn_position.x = spawn_position_x;
        this.spawn_position.y = spawn_position_y;
        this.spawn_position.z = spawn_position_z;

        if (adjacent_sector_ids.Count > 0)
        {
            foreach (int sector_id in adjacent_sector_ids)
            {
                GameObject sector = GameObject.Find("/Map/Sector #" + sector_id);

                if (sector != null)
                {
                    if (sector.name != name)
                    {
                        if (!adjacent_sectors.Contains(sector))
                        {
                            adjacent_sectors.Add(sector);
                        }
                        else
                        {
                            throw new System.Exception("In '/Map/" + name + "': \"adjacent_sectors\" list cannot contain duplicates.");
                        }
                    }
                    else
                    {
                        throw new System.Exception("In '/Map/" + name + "': A sector cannot be adjacent to itself.");
                    }
                }
                else
                {
                    throw new System.Exception("In '/Map/" + name + "': /Map/Sector #" + sector_id + " cannot be found.");
                }
            }
        }
        else
        {
            throw new System.Exception("In '/Map/" + name + "': A sector must have at least one adjacent sector.");
        }
    }

    public int Attack
    {
        get { return attack_value; }
        set { attack_value = value; }
    }

    public int Defence
    {
        get { return defence_value; }
        set { defence_value = value; }
    }

    public int Units
    {
        get { return units; }
        set { units = value; }
    }

    public Position SpawnPosition
    {
        get { return spawn_position; }
    }

    public List<GameObject> AdjacentSectors
    {
		get { return adjacent_sectors; }
    }
}
