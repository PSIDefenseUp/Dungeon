using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class StateMachine : MonoBehaviour {

  private Game  game;
  private Map map;
  public Shader hLight;

  public bool  moveOnce;          // flag so that computer only moves once
  public List<Unit> EnemyList;    // List of Enemies used for deciding who to attack
  public List<Unit> mapUnitList;  // List of all units on the board used to populate enemylist
  public List<Tile> path;         // path to travel decided by A* search
  public Unit Target;             // target computer is currently chaseing
  public Unit me;                 // refernce to Unit script
  public int stopPush;            // after list is populated for path to enemy use this to stop computer at move speed
  



	// Use this for initialization
	void Start () {
    game = GameObject.Find("GameManager").GetComponent<Game>();
    map = GameObject.Find("Map").GetComponent<Map>();
    me   = GameObject.Find(this.name).GetComponent<Unit>();

    moveOnce = true;
  }
	
	// Update is called once per frame
	void Update ()
  {

    if (game.currentPlayer.team == me.team && EnemyList != null && me.canMove && moveOnce)
    {
      runMachine();
    }
	
	}

  void runMachine()
  {

    // get target to persue
    getTarget();

    moveOnce = false;

    
    // get path to target using A* search
    path = search();
    Debug.Log(path);
    // if a path exist start populating unit me list 
    if (path != null && path.Count > 0)
    {
      Debug.Log(path.Count);
      if (me.moveSpeed < path.Count)
        stopPush = me.moveSpeed;
      else
        stopPush = path.Count;

      //physically move unit on board
      game.map.moveUnitAI(me.position, path[stopPush - 1].getPosition());

      // if unit is currently moving  add path to new list 
      if (me.path.Count > 0)
      {
        Tile[] curPath = me.path.ToArray();
        List<Tile> curPathList = curPath.ToList<Tile>();

        for (int i = curPathList.Count - 1; i >= 0; i--)
          path.Add(curPathList[i]);

        me.path = new Stack<Tile>(); 
      }

      //populate list based on stopPush
      for (int i = stopPush - 1; i >= 0; i--)
      {
        me.path.Push(path[i]);
      }
    }

    // is in range of enemy attack
    attack();
  }

  void attack()
  {
    if (!me.canAct)
      return;

    foreach(var x in EnemyList)
    {
      int dist = distance(me, x);
      if(dist <= me.maxRange && dist >=  me.minRange)
      {
        me.attack(x);
        return;
      } 
    }
  }

  void getTarget()
  {
    if (!me.canMove || EnemyList.Count == 0)
      return;

    List<int> enemyDisanceFromMe = new List<int>();

    foreach (var x in EnemyList)
    {
      enemyDisanceFromMe.Add(distance(me, x));
    }

    int minValue = Mathf.Min(enemyDisanceFromMe.ToArray());
    int MinIndex = enemyDisanceFromMe.IndexOf(minValue);

    Target = EnemyList[MinIndex];
  }

  List<Tile> search()
  {
    clearMap();

    List<Tile> openList = new List<Tile>();
    List<Tile> closedList = new List<Tile>();

    if (!Target)
      return null;

    Tile start = game.map.getTile(me.position);
    Tile end = game.map.getTile(Target.position);

    openList.Add(start);

    while(openList.Count > 0)
    {
      // Grab the lowest f(x) to process next
      var lowIndex = 0;

      for(int i = 0; i < openList.Count;i++ )
      {
        if (openList[i].f < openList[lowIndex].f)
          lowIndex = i;
      }

      var CurrentTile = openList[lowIndex];

      // End case -- result has been found, return the traced path
      if ( CurrentTile.getPosition().x == end.getPosition().x && CurrentTile.getPosition().y == end.getPosition().y)
      {
        var curr = CurrentTile.parent;
        var ret = new List<Tile>();

        while(curr.parent)
        {
          ret.Add(curr);
          curr = curr.parent;
        }
        ret.Reverse();
        return ret;
      }

      // Normal case -- move currentNode from open to closed, process each of its neighbors

      openList.Remove(CurrentTile);
      closedList.Add(CurrentTile);

      var neighbors = getNeighbor(game.map.getBoard(), CurrentTile);

      for (var i = 0; i < neighbors.Count; i++)
      {
        var neighbor = neighbors[i];

        if (closedList.Contains(neighbor) || neighbor.solid || (neighbor.occupy && neighbor != end))
        {
          // not a valid node to process, skip to next neighbor
          continue;
        }

        // g score is the shortest distance from start to current node, we need to check if
        //	 the path we have arrived at this neighbor is the shortest one we have seen yet
        var gScore = CurrentTile.g + 1; // 1 is the distance from a node to it's neighbor
        var gScoreIsBest = false;


        if (!openList.Contains(neighbor))
        {
          // This the the first time we have arrived at this node, it must be the best
          // Also, we need to take the h (heuristic) score since we haven't done so yet

          gScoreIsBest = true;
          neighbor.h = distance(neighbor.getPosition(), end.getPosition());
          openList.Add(neighbor);
        }
        else if (gScore < neighbor.g)
        {
          // We have already seen the node, but last time it had a worse g (distance from start)
          gScoreIsBest = true;
        }

        if (gScoreIsBest)
        {
          // Found an optimal (so far) path to this node.	 Store info on how we got here and
          //	just how good it really is...
          neighbor.parent = CurrentTile;
          neighbor.g = gScore;
          neighbor.f = neighbor.g + neighbor.h;
        }
       }//end for Loop
      }//end while Loop
    return null;
  }

  void clearMap()
  {
    Tile[,] grid = GameObject.Find("Map").GetComponent<Map>().tiles;
    int xmin = Mathf.RoundToInt(game.map.getBounds().xMin);
    int xmax = Mathf.RoundToInt(game.map.getBounds().xMax);
    int ymin = Mathf.RoundToInt(game.map.getBounds().yMin);
    int ymax = Mathf.RoundToInt(game.map.getBounds().yMax);

    int xVal = Mathf.Abs(xmin) + Mathf.Abs(xmax);
    int yVal = Mathf.Abs(ymin) + Mathf.Abs(ymax);

    for (int x = 0;x < xVal ; x++)
    {
      for (int y = 0; y < yVal; y++)
      {
        if (!grid[x, y])
          continue;

        grid[x, y].f = 0;
        grid[x, y].g = 0;
        grid[x, y].h = 0;
        grid[x, y].parent = null;
      }
    }
  }

  private int distance(Unit x, Unit y)
  {
    Point xp = x.position;
    Point yp = y.position;
    float dist;
 
    dist = (Mathf.Abs(xp.x - yp.x) + Mathf.Abs(xp.y - yp.y));

    return Mathf.RoundToInt(dist);
  }
  private int distance(Point x, Point y)
  {
    Point xp = x;
    Point yp = y;
    float dist;

    dist = (Mathf.Abs(xp.x - yp.x) + Mathf.Abs(xp.y - yp.y));

    return Mathf.RoundToInt(dist);
  }

  private List<Tile> getNeighbor(Tile[,] grid, Tile pos)
  {
    List<Tile> ret = new List<Tile>();

    int x = pos.getPosition().x;
    int y = pos.getPosition().y;

    if (grid[x - 1, y])
    {
      ret.Add(grid[x - 1, y]);
    }
    if (grid[x + 1, y])
    {
      ret.Add(grid[x + 1, y]);
    }
    if (grid[x, y - 1])
    {
      ret.Add(grid[x,  y - 1]);
    }
    if (grid[x, y + 1])
    {
      ret.Add(grid[x, y + 1]);
    }
    return ret;
  }

  public void updateEnemyList()
  {
    EnemyList = new List<Unit>();

    foreach (var x in mapUnitList)
    {
      if (x.team == 0)
      {
        EnemyList.Add(x);
      }
    }
  }

  IEnumerator wait(float x)
  {
    yield return new WaitForSeconds(x);
  }

}
