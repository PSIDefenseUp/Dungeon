using UnityEngine;
using System.Collections;

public class Unit : MonoBehaviour
{    
    private bool canMove;           // Does this unit still have its move action?
    private bool canAct;            // Does this unit still have its turn action?
    private int[,] reachable;       // Array containing the cost of movement to each tile on the map

    public Pathfinder pathfinder;   // The pathfinder used to navigate this unit around the map
    public Point position;          // The current position of this unit on the map
    public int team;                // The 'team' this unit is on -- 0: heroes, 1: dungeon master
    public int maxHealth;           // The maximum health of this unit
    public int currentHealth;       // The current health of this unit
    public int moveSpeed;           // The maximum number of tiles this unit can move in one turn
    public int attackBase;          // The minimum damage this unit can inflict on attack -- Damage = (attackSpread * random) + attackBase - enemy.armor
    public int attackSpread;        // A random value between 0 and attackSpread is added to each attack
    public int minRange;            // Minimum number of tiles away that this unit's attacks can reach
    public int maxRange;            // Maximum number of tiles away that this unit's attacks can reach -- An enemy unit must be within [minRange, maxRange] to be a valid target
    public int armor;               // The amount by which we reduce the damage of incoming attacks
    public int cost;                // The cost of purchasing this unit -- only applies to the DM when building the map

    // Use this for initialization
    void Start ()
    {
        this.canMove = false;
        this.canAct = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    public bool canAttack(Unit other)
    {
        // Don't let us attack our own teammates
        if (other.team == this.team)
            return false;

        // Check whether or not the other unit is within our attack range
        int distance = position.distanceTo(other.position);
        return distance >= minRange && distance <= maxRange;
    }

    public void Attack(Unit other)
    {
        // Reduce HP of targetted unit
        other.currentHealth -= attackBase + (int)(attackSpread * Random.value) - other.armor;

        // Don't let HP go to crazy, negative values
        if (other.currentHealth < 0)
            other.currentHealth = 0;
    }

    public void setReachable(int[,] newReachable)
    {
        this.reachable = newReachable;
    }
}
