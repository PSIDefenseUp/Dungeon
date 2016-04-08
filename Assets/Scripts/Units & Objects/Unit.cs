using UnityEngine;
using System.Collections.Generic;
[System.Serializable]

public class Unit : MonoBehaviour
{
    protected Game game;                       // The game object        
    protected float animationMoveSpeed = 2;    // Speed at which we move from tile to tile (in units / second)

    public int[,] reachable;        // Array containing the cost of movement to each tile on the map 
    
    // Properties
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
    public GameObject skill;

    // Inventory
    public Inventory inventory;     // The inventory of items this unit has -- Only heroes should have one!

    // Animation vars
    private Transform model;        // The model
    public Animator animator;      // The animator attached to the model (for animations)
    private int isWalkingHash = Animator.StringToHash("isWalking");
    private int toAttackHash = Animator.StringToHash("toAttack");
    private int attackTransition = Animator.StringToHash("Afire");
    public Transform Target;        // The next tile we want to walk to

    // Audio
    public AudioSource source;
    public List <AudioClip> UserSelected = new List<AudioClip>();
    public AudioClip EnemySelected;
    public AudioClip Move;
    public AudioClip Walk;
    public AudioClip Hit;
    public AudioClip AttackSound;
    private float walktimer = 0f;
    private float walkCooldown = 0.28f;
    private float volLowRange = 0.5f;
    private float volHighRange = 1.0f;

    // Dialog strings
    List<string> selectLines; // Lines this unit can say when selected

    // Use this for initialization
    void Start()
    {
        // Grab current Game object
        game = GameObject.Find("GameManager").GetComponent<Game>();
        model = this.transform.FindChild("Model");
        
        if(model != null)
            animator = model.GetComponent<Animator>();      

        // Initialize path + movement
        path = new Stack<Tile>();
        this.canMove = true;
        this.canAct = true;

        // Add dialog (TODO: REMOVE, THIS NEEDS TO BE PER-UNIT)
        selectLines = new List<string>();
        selectLines.Add("Ugh... I cannot believe I signed up for this.");
        selectLines.Add("This is beginning to feel rather hopeless.");
        selectLines.Add("I feel... quite comfortable down here.");
        selectLines.Add("I thought I would be working alone...");
        selectLines.Add("If I had muscles, I would most certainly have a headache by now.");
        selectLines.Add("Do not fear, I'll keep you able bodied... Maybe.");

        // If a hero, add inventory
        if(this.team == 0)
        {
            this.inventory = new Inventory();
        }
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move towards destination if we have one
        if (path.Count > 0)
        {

            if (path.Peek() == null)
            {
                Debug.Log("WHAT");
                path.Pop();
                return;
            }

             // Set Target
             setTarget(path.Peek().gameObject.transform);

            // Manage walking animation
            if (animator != null)
            {
                // Turn Unit to face target
                turnUnit();

                // double check if im walking and if not start walking
                if (!animator.GetBool(isWalkingHash))
                {
                    animator.SetBool(isWalkingHash, true);
                }
            }

            // Update our position
            transform.position = Vector3.MoveTowards(transform.position, path.Peek().gameObject.transform.position + new Vector3(0, 1, 0), animationMoveSpeed * Time.deltaTime);
            walktimer -= Time.deltaTime;
            playwalkAudio();
            // If we're at the target position, start looking at the next position to move to (TODO: reimplement moving by animation if we don't want to use this method)
            if ((transform.position - (Target.position + new Vector3(0, 1, 0))).magnitude == 0)
            {
                path.Pop();
            }

        }        
        else if(animator != null && animator.GetBool(isWalkingHash)) // if i dont have a path to walk and im currently walking stop
        {
            animator.SetBool(isWalkingHash, false);
            // TODO:  turn towards user after move 
            //this.transform.eulerAngles = new Vector3(0, 180, 0);
        }  

        /*
        // Fire prjectile
        if(animator.GetAnimatorTransitionInfo(0).userNameHash == attackTransition && attackOnce == true )
         {
                Vector3 pos = this.transform.position;
                Vector3 dir = this.transform.forward;
                spawnSkill(pos, dir.normalized);
                attackOnce = false;
         }
         */
    }

    void OnMouseEnter()
    {
        
    }

    public bool canAttack(Unit other)
    {
        // We can't attack something that doesn't exist
        if (other == null)
            return false;

        // Don't let us attack interactable objects (not real 'units')
        if (other is Interactable)
            return false;

        // Don't let us attack if we can't act anymore
        if (canAct == false)
            return false;

        // Don't let us attack our own teammates
        if (other.team == this.team)
            return false;

        // Check whether or not the other unit is within our attack range
        int distance = position.distanceTo(other.position);
        return distance >= getMinRange() && distance <= getMaxRange();
    }

    public void attack(Unit other)
    {
        source.PlayOneShot(AttackSound,randomVolRange());

        // turn toward unit
        setTarget(other.transform);
        turnUnit();
       
        // run mecanim attack animation
        animator.SetTrigger(toAttackHash);

        // Reduce HP of targetted unit
        other.getAttacked(getAttackBase() + (int)(attackSpread * Random.value) - other.getArmor(), this);
        canAct = false;
        canMove = false;
        removeHighlights();
    }

    public void getAttacked(int damage, Unit other)
    {
        source.PlayOneShot(Hit, randomVolRange());
        if (damage <= 0)
            return;

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
            source.PlayOneShot(Move, randomVolRange());
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
        for (int y = Mathf.Max(position.y - getMoveSpeed(), 0); y <= Mathf.Min(position.y + getMoveSpeed(), mapBounds.height - 1); y++)
        {
            for (int x = Mathf.Max(position.x - getMoveSpeed(), 0); x <= Mathf.Min(position.x + getMoveSpeed(), mapBounds.width - 1); x++)
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
        for (int y = Mathf.Max(position.y - getMaxRange(), 0); y <= Mathf.Min(position.y + getMaxRange(), mapBounds.height - 1); y++)
        {
            for (int x = Mathf.Max(position.x - getMaxRange(), 0); x <= Mathf.Min(position.x + getMaxRange(), mapBounds.width - 1); x++)
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
        this.heal(getRegen());

        gameObject.GetComponentInChildren<Light>().intensity = 1;
    }

    // set target Transform

    public void setTarget(Transform x)
    {
        Target = x;
    }

    // turn unit towards current Target
    public void turnUnit()
    {
        Vector3 targetPosition = new Vector3(Target.position.x, this.transform.position.y, Target.position.z);
        this.transform.LookAt(targetPosition);
    }

    public void spawnSkill(Vector3 position, Vector3 direction)
    {
        //GameObject sk = (GameObject)GameObject.Instantiate(skill, position, skill.transform.rotation);
        //sk.transform.forward = direction;
    }

    public string getSelectLine()
    {
        // returns a random line from the pool of on-selection dialog for this unit
        string ret = "";

        if (this.team == 0)
            ret = selectLines[(int)(Random.value * (selectLines.Count - 1))];

        return ret;
    }

    public int getRegen()
    {
        if (inventory == null)
            return regen;
        else
        {
            return regen + inventory.getBonusRegen();
        }
    }

    public int getArmor()
    {
        if (inventory == null)
            return armor;
        else
        {
            return armor + inventory.getBonusArmor();
        }
    }

    public int getAttackBase()
    {
        if (inventory == null)
            return attackBase;
        else
        {
            return attackBase + inventory.getBonusAttack();
        }
    }

    public int getMinRange()
    {
        return minRange;
    }

    public int getMaxRange()
    {
        if (inventory == null)
            return maxRange;
        else
        {
            return maxRange + inventory.getBonusRange();
        }
    }

    public int getMoveSpeed()
    {
        if (inventory == null)
            return moveSpeed;
        else
        {
            return moveSpeed + inventory.getBonusSpeed();
        }
    }
  public void playSelectedAudio()
  {
    
    if (game.currentPlayer.team == team)
      source.PlayOneShot(UserSelected[Random.Range(0,UserSelected.Count)], randomVolRange());
    else

      source.PlayOneShot(EnemySelected, randomVolRange());
  }

  public void playwalkAudio()
  {
    if (walktimer <= 0)
    {
      source.PlayOneShot(Walk, randomVolRange());
      walktimer = walkCooldown;
    }
  }
  
  public float randomVolRange()
  {
    return Random.Range(volLowRange, volHighRange);
  }
}
