using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Cursor :NetworkBehaviour
{

    private Point position;    // The current position of the cursor
    private Game game;         // Reference to the current Game object
    private SelectionInventoryDisplay selectInv; 
    private DMBuildDisplay dmDisplay;
    private int selMyUnit = Animator.StringToHash("selectMyUnit");
    private int selEnUnit = Animator.StringToHash("selectEnemyUnit");
    private GameObject cursorObj;

    // 
    public networkPlayerScript netPlayer;

    // The spotlight for the selected unit
    public Light spotlight;
    
    // The Tile under our cursor
    private Transform currentTile;

    // The selected unit (if we have made a selection)
    public Unit selectedUnit;

   
    void Start()
    {
    // Grab Game object from scene
    if (netPlayer != null)
      game = netPlayer.game;

    if (netPlayer != null)
      spotlight = netPlayer.spotL;

    cursorObj = this.transform.GetChild(2).gameObject;

    if (netPlayer.isLocalPlayer)
      cursorObj.GetComponent<CursorSpin>().cursorColor(Color.white);
  }

  // Update is called once per frame
  void Update()
  {
    syncCursorObj();

    bool S = isServer;
    bool C = isClient;
    bool L = isLocalPlayer;

    if (!isLocalPlayer)
      return;

    if (game == null && netPlayer != null)
      game = netPlayer.game;

    if (spotlight == null && netPlayer != null)
      spotlight = netPlayer.spotL;

    // Don't allow for selections if we're placing a unit
    if (DMBuildDisplay.isPlacing())
    {
       selectUnit(null);
       return;
    }


    // On mouse left click, select the unit at the current location
    if (Input.GetMouseButtonDown(0))
         {          
             // Select Unit
             selectUnit(game.map.getUnit(position));

                 //play IsHighlighted in mecanin if unit selected is yours (and exists)
                 if (selectedUnit != null)
                 {
                         selectedUnit.playSelectedAudio(netPlayer.myPlayerInfo.team);

                     if (selectedUnit.animator)
                     {
                         if (netPlayer.myPlayerInfo.team == selectedUnit.team)
                             selectedUnit.animator.SetTrigger(selMyUnit);
                         else
                             selectedUnit.animator.SetTrigger(selEnUnit);
                     }                    
                 }
         }

         // On mouse right click, if we have a unit selected, move the unit to the cursor's location or attack the unit there if in range
         if (Input.GetMouseButtonDown(1) && selectedUnit != null && selectedUnit.owner == game.currentPlayerIndex)
         {
              var other = game.map.getUnit(position);
             // If we clicked on a unit, see if we can interact with it
             if (selectedUnit.canInteract(other))
             {
                 CmdInteract(selectedUnit.position, position);
             }
             else if (selectedUnit.canAttack(other))
             {
                 CmdAttack(selectedUnit.position, position);
             }

             

             // If we clicked on an empty space, try to move there
             if (selectedUnit.canReach(position) && selectedUnit.canMove)
             {
                  if(!isServer)
                  {
                    CmdMoveUnit(selectedUnit.position, position);
                    selectedUnit.moveTo(position);
                  }
                  else
                  {   
                     RpcMoveUnit(selectedUnit.position, position);
                     selectedUnit.moveTo(position); 
                  }
             }


             // Dim the light on a unit that can't do anything more this turn
             if(!(selectedUnit.canMove || selectedUnit.canAct))
                 selectedUnit.gameObject.GetComponentInChildren<Light>().intensity = 0;
         }         
  }

  public Point getPosition()
  {
        return position;
  }
  public Unit getSelection()
  {
        return this.selectedUnit;
  }

    public void setCurrentTile(Transform t)
    {
        this.currentTile = t;

        // Set our grid position
        this.position = t.GetComponent<Tile>().getPosition();

        // Float our cursor above the selected tile
        transform.position = t.transform.position + new Vector3(0, 1, 0);
    }

    public void selectUnit(Unit u)
    {
        // Remove highlights from tiles highlighted by previous selection
        if (selectedUnit != null)
            selectedUnit.removeHighlights();

        // Set new selection
        selectedUnit = u;         

        if (selectedUnit != null)
        {
            
            // If selected by its owner, make the unit say something
            if(selectedUnit.owner == netPlayer.myPlayerInfo.team)
            {
            //    DialogDisplay.speak(selectedUnit, selectedUnit.getSelectLine());
            }

            // Update reachable, highlight if the unit can move and is owned by the current player
            if (selectedUnit.canMove && selectedUnit.owner == netPlayer.myPlayerInfo.team)
            {
                selectedUnit.pathfinder.updateReachable(selectedUnit);
                selectedUnit.highlightReachable();
            }
            
            // Highlight interactable tiles if the unit can act and is owned by the current player
            if(selectedUnit.canAct && selectedUnit.owner == netPlayer.myPlayerInfo.team)
            {
                selectedUnit.highlightInteractable();
            }

            // Set spotlight on selected unit
            spotlight.gameObject.transform.parent = selectedUnit.transform;
            spotlight.gameObject.transform.position = selectedUnit.transform.position + new Vector3(0, 2, 0);
            spotlight.intensity = 1;
        }
        else
        {
            // Disable spotlight if our selection is null
            spotlight.intensity = 0;
        }
    }

    private void syncCursorObj()
    {
      cursorObj.transform.position = this.transform.position;
    }

  [ClientRpc]
  public void RpcMoveUnit(Point unit, Point p)
  {
    Unit x = game.map.getUnit(unit);

    if (x == null)
      return;

    x.pathfinder.updateReachable(x);
    x.moveTo(p);
  }

  [Command]
  public void CmdInteract(Point unit, Point P)
  {
    Unit me = game.map.getUnit(unit);

    if (me == null)
      return;

    Unit other = game.map.getUnit(P);

    if (other == null)
      return;
    me.interact(other);
  }
  [Command]
  public void CmdAttack(Point unit, Point P)
  {
    Unit Me = game.map.getUnit(unit);

    if (Me == null)
      return;

    Unit other = game.map.getUnit(P);

    if (other == null)
      return;

    Me.attack(other);
    RpcAttack(unit, P);
  }

  [ClientRpc]
  public void RpcAttack(Point unit, Point enemy)
  {
    Unit Me = game.map.getUnit(unit);

    if (Me == null)
      return;

    Unit other = game.map.getUnit(enemy);

    if (other == null)
      return;

    Me.AnimateAttack(other);

  }

  [Command]
  public void CmdMoveUnit(Point unit, Point p)
  {
    Unit x = game.map.getUnit(unit);

    if (x == null)
      return;

    x.pathfinder.updateReachable(x);
    x.moveTo(p);
  }
}
