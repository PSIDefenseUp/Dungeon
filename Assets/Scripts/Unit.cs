using UnityEngine;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    protected Game game;                       // The game object        
    protected float animationMoveSpeed = 10;   // Speed at which we move from tile to tile (in units / second)

    public int[,] reachable;        // Array containing the cost of movement to each tile on the map 
    //public List<Unit> attack;   
    public Point position;          // The current position of this unit on the map
    public Stack<Tile> path;        // The path for our unit to travel
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

        path = new Stack<Tile>();
        this.canMove = true;
        this.canAct = true;
    }

    // Update is called once per frame
    void Update()
    {
        // Turn to face the camera! Billboards! DO NOT DO THIS FOR THE LOVE OF GOD
        //transform.LookAt(new Vector3(game.gameCamera.transform.position.x, transform.position.y, game.gameCamera.transform.position.z));
        //transform.LookAt(Camera.main.transform.position, -Vector3.up);

        // Move towards destination if we have one
        if (path.Count > 0)
        {
            if (path.Peek() == null)
                Debug.Log("WHAT");

            transform.position = Vector3.MoveTowards(transform.position, path.Peek().gameObject.transform.position + new Vector3(0, 1, 0), animationMoveSpeed * Time.deltaTime);

            if (transform.position.Equals(path.Peek().gameObject.transform.position + new Vector3(0, 1, 0)))
            {
                path.Pop();
            }
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
        canMove = false;
        removeHighlights();
    }

    public void getAttacked(int damage, Unit other)
    {
        this.currentHealth -= damage;

        if (currentHealth <= 0)
            game.map.removeUnit(this);
    }

    public bool canInteract(Unit other)
    {
        // Don't let the DM interact with anything
        if (game.playerList[this.owner].team == 1)
            return false;

        // We can't interact with something that doesn't exist
        if (other == null)
            return false;

        // Don't let us act if we can't act anymore
        if (canAct == false)
            return false;

        // Make sure the object is interactable
        if (!(other is Interactable))
            return false;

        // Check whether or not we are next to the unit
        int distance = position.distanceTo(other.position);
        return distance == 1;
    }

    public void interact(Unit other)
    {
        // Reduce HP of targetted unit
        (other as Interactable).getInteracted(this);
        canAct = false;
        canMove = false;
        removeHighlights();
    }    

    public void heal(int health)
    {
        this.currentHealth += health;

        if (this.currentHealth > this.maxHealth)
            this.currentHealth = this.maxHealth;
    }

    public void moveTo(Point p)
    {
        if (game.map.getUnit(p) == null || game.map.getUnit(p) == this)
        {
            game.map.moveUnit(this.position, p);
        }

        // If we can act, start highlighting tiles we can interact with
        if (this.canAct)
            this.highlightInteractable();
    }

    // Returns true if we the input location has no other unit, and is reachable by this unit
    private bool canAccess(Point p)
    {
        return true; // TODO: IMPLEMENT
    }

    // Highlight tiles that this unit interact with (attack or otherwise act on)
    public void highlightReachable()
    {
        Rect mapBounds = game.map.getBounds();

        // Loop only over tiles that could be in attack range of this unit
        for (int y = Mathf.Max(position.y - moveSpeed, 0); y <= Mathf.Min(position.y + moveSpeed, mapBounds.height - 1); y++)
        {
            for (int x = Mathf.Max(position.x - moveSpeed, 0); x <= Mathf.Min(position.x + moveSpeed, mapBounds.width - 1); x++)
            {
                if (canReach(new Point(x, y)))
                    game.map.getTile(x, y).highlight(Color.cyan);
            }
        }
    }

    // Highlight tiles that this unit can travel to
    public void highlightInteractable()
    {
        Rect mapBounds = game.map.getBounds();

        // Loop only over tiles that could be in move range of this unit
        for (int y = Mathf.Max(position.y - maxRange, 0); y <= Mathf.Min(position.y + maxRange, mapBounds.height - 1); y++)
        {
            for (int x = Mathf.Max(position.x - maxRange, 0); x <= Mathf.Min(position.x + maxRange, mapBounds.width - 1); x++)
            {
                // Highlight tiles with units we can attack in red
                if (canAttack(game.map.getUnit(new Point(x, y))))
                    game.map.getTile(x, y).highlight(Color.red);

                if (canInteract(game.map.getUnit(new Point(x, y))))
                    game.map.getTile(x, y).highlight(Color.yellow);
            }
        }
    }

    // Remove highlights from reachable tiles
    public void removeHighlights()
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
        //Debug.Log("canreach (" + p.x + ", " + p.y + ") - " + (reachable[p.x, p.y] >= 0 ? "yes" : "no"));
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

    public virtual void refresh()
    {
        // To be called on the start of its owner's turn
        // Allows the unit to act and move again, and brings back its light
        canMove = true;
        canAct = true;
        this.heal(this.regen);

        gameObject.GetComponentInChildren<Light>().intensity = 1;
    }
}
