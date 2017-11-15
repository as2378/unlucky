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
    private List<string> adj_sectors = new List<string>();

	// Following fields are set via the inspector in Unity
    public double spawn_position_x;
    public double spawn_position_y;
    public double spawn_position_z;

    private int num_of_adj_sectors = 5;
    public string adj_sector1 = "-1";
    public string adj_sector2 = "-1";
    public string adj_sector3 = "-1";
    public string adj_sector4 = "-1";
    public string adj_sector5 = "-1";

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

        for(int i = 1; i <= num_of_adj_sectors; i++)
        {
            string value = (string)this.GetType().GetField("adj_sector" + i).GetValue(this);

            if(value != "-1")
            {
                adj_sectors.Add(value);
            }
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

    public List<string> AdjSectors
    {
        get { return adj_sectors; }
    }
}
