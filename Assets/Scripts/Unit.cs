using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    private Game game;                       // The game object        
    private float animationMoveSpeed = 10;   // Speed at which we move from tile to tile (in units / second)

    public int[,] reachable;        // Array containing the cost of movement to each tile on the map 
    //public List<Unit> attack;   
    public Point position;          // The current position of this unit on the map
    public Queue<Tile> path;        // The path for our unit to travel
    public Pathfinder pathfinder;   // The pathfinder used to navigate this unit around the map   
    public bool canMove;            // Does this unit still have its move action?
    public bool canAct;             // Does this unit still have its turn action?
    public int owner;               // The player that owns this unit
    public int team;                // The 'team' this unit is on -- 0: heroes, 1: dungeon master
    public int maxHealth;           // The maximum health of this unit
    public int currentHealth;       // The current health of this unit
    public int moveSpeed;           // The maximum number of tiles this unit can move in one turn
    public int attackBase;          // The minimum damage this unit can inflict on attack -- Damage = (attackSpread * random) + attackBase - enemy.armor
    public int attackSpread;        // A random value between 0 and attackSpread (inclusive) is added to each attack
    public int minRange;            // Minimum number of tiles away that this unit's attacks can reach
    public int maxRange;            // Maximum number of tiles away that this unit's attacks can reach -- An enemy unit must be within [minRange, maxRange] to be a valid target
    public int armor;               // The amount by which we reduce the damage of incoming attacks
    public int cost;                // The cost of purchasing this unit -- only applies to the DM when building the map
    public int regen;               // The amount of health this unit regenerates each turn

    // Use this for initialization
    void Start()
    {
        // Grab current Game object
        game = GameObject.Find("GameManager").GetComponent<Game>();

        path = new Queue<Tile>();
        this.canMove = true;
        this.canAct = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Turn to face the camera! Billboards! DO NOT DO THIS FOR THE LOVE OF GOD
        //transform.LookAt(new Vector3(game.gameCamera.transform.position.x, transform.position.y, game.gameCamera.transform.position.z));

        // Move towards destination if we have one
        if(path.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, path.Peek().gameObject.transform.position + new Vector3(0, 1, 0), animationMoveSpeed * Time.deltaTime);

            if (transform.position.Equals(path.Peek().gameObject.transform.position))
                path.Dequeue();
        }
    }

    void OnMouseEnter()
    {
        
    }

    public bool canAttack(Unit other)
    {
        // We can't attack something that doesn't exist
        if (other == null)
            return false;

        // Don't let us attack if we can't act anymore
        if (canAct == false)
            return false;

        // Don't let us attack our own teammates
        if (other.team == this.team)
            return false;

        // Check whether or not the other unit is within our attack range
        int distance = position.distanceTo(other.position);
        return distance >= minRange && distance <= maxRange;
    }

    public void attack(Unit other)
    {
        // Reduce HP of targetted unit
        other.getAttacked(attackBase + (int)(attackSpread * Random.value) - other.armor, this);
        canAct = false;
    }

    public void getAttacked(int damage, Unit other)
    {
        this.currentHealth -= damage;

        if (currentHealth <= 0)
            game.map.removeUnit(this);
    }

    public void heal(int health)
    {
        this.currentHealth += health;

        if (this.currentHealth > this.maxHealth)
            this.currentHealth = this.maxHealth;
    }

    public void moveTo(Point p)
    {
        // TODO: Replace with setDestination so we don't just teleport everywhere
        if (game.map.getUnit(p) == null || game.map.getUnit(p) == this)
        {
            game.map.moveUnit(this.position, p);
        }
    }

    // Returns true if we the input location has no other unit, and is reachable by this unit
    private bool canAccess(Point p)
    {
        return true; // TODO: IMPLEMENT
    }

    // Highlight tiles that this unit can travel to
    public void highlightReachable()
    {
        Rect mapBounds = game.map.getBounds();

        // Loop only over tiles that could be in move range of this unit
        for (int y = Mathf.Max(position.y - moveSpeed, 0); y < Mathf.Max(position.y + moveSpeed, mapBounds.height); y++)
        {
            for (int x = Mathf.Max(position.x - moveSpeed, 0); x < Mathf.Max(position.x + moveSpeed, mapBounds.width); x++)
            {
                if (canReach(new Point(x, y)))
                    game.map.getTile(x, y).highlight(Color.cyan);
            }
        }
    }

    // Remove highlights from reachable tiles
    public void unHighlightReachable()
    {
        Rect mapBounds = game.map.getBounds();

        // Loop only over tiles that could be in move range of this unit
        for (int y = 0; y < mapBounds.height; y++)
        {
            for (int x = 0; x < mapBounds.width; x++)
            {
                if (game.map.getTile(x, y) != null)
                    game.map.getTile(x, y).highlight(Color.white);
            }
        }
    }

    public bool canReach(Point p)
    {
        return reachable[p.x, p.y] >= 0;
    }

    public Point getPosition()
    {
        return this.position;
    }

    public void setPosition(Point p)
    {
        this.position = p;
    }

    public void setPosition(int x, int y)
    {
        this.position = new Point(x, y);
    }

    public void setCurrentTile(Transform t)
    {
        // Set our grid position
        this.position = t.GetComponent<Tile>().getPosition();

        // Float our cursor above the selected tile
        transform.position = t.transform.position + new Vector3(0, 1, 0);
    }

    public void refresh()
    {
        // To be called on the start of its owner's turn
        // Allows the unit to act and move again, and brings back its light
        canMove = true;
        canAct = true;
        this.heal(this.regen);

        gameObject.GetComponentInChildren<Light>().intensity = 1;
    }
}
