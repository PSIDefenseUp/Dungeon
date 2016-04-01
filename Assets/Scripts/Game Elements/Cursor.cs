using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    private Point position;    // The current position of the cursor
    private Game game;         // Reference to the current Game object
    private Animator animator; // get animator for manipulation on animations
    private Unit anim;         // unit holder for animation manipulation        

    private int selMyUnit = Animator.StringToHash("selectMyUnit");
    private int selEnUnit = Animator.StringToHash("selectEnemyUnit");

    // The spotlight for the selected unit
    private Light spotlight;

    // The Tile under our cursor
    private Transform currentTile;

    // The selected unit (if we have made a selection)
    private Unit selectedUnit;

    // Animation variables
    private float spinSpeed = 360; // Degrees per second to spin around
    private float spinWait = .5f; // Seconds to wait between spins
    private float spinWaitProgress = 0; // How far we are in our current wait
    private bool spinning = false; // Are we spinning or waiting?

    // Use this for initialization
    void Start()
    {
        // Grab Game object from scene
        game = GameObject.Find("GameManager").GetComponent<Game>();
        spotlight = GameObject.Find("Spotlight").GetComponent<Light>();
  }

    // Update is called once per frame
    void Update()
    {
        // On mouse left click, select the unit at the current location
        if (Input.GetMouseButtonDown(0))
        {
          //get unit for manipulation
          anim = game.map.getUnit(position);

          //Select Unit
          selectUnit(anim);

      //play IsHighlighted in mecanin if unit selected is yours
      animator = anim.GetComponent<Animator>();

      if (game.currentPlayer.team == anim.team)
        animator.SetTrigger(selMyUnit);
      else
        animator.SetTrigger(selEnUnit);
     }

        // On mouse right click, if we have a unit selected, move the unit to the cursor's location or attack the unit there if in range
        // TODO: Make units move rather than teleport
        if (Input.GetMouseButtonDown(1) && selectedUnit != null && selectedUnit.owner == game.currentPlayerIndex)
        {
            // If we clicked on a unit, see if we can interact with it
            if (selectedUnit.canInteract(game.map.getUnit(position)))
            {
                selectedUnit.interact(game.map.getUnit(position));
            }
            else if (selectedUnit.canAttack(game.map.getUnit(position)))
            {
                selectedUnit.attack(game.map.getUnit(position));
            }

            // If we clicked on an empty space, try to move there
            if (selectedUnit.canReach(position) && selectedUnit.canMove)
                selectedUnit.moveTo(position);

            // Dim the light on a unit that can't do anything more this turn
            if(!(selectedUnit.canMove || selectedUnit.canAct))
                selectedUnit.gameObject.GetComponentInChildren<Light>().intensity = 0;
        }       

        animate();
    }

    private void animate()
    {
        if (spinning)
        {
            // When spinning, check to see when we've spun the whole way around (90 degrees)

            if (transform.rotation.eulerAngles.y + (spinSpeed * Time.deltaTime) >= 90)
            {
                // If we reach 90 degrees this frame, stay at 90 degrees and start waiting until next spin
                transform.rotation = Quaternion.AngleAxis(0, new Vector3(0, 1, 0));
                spinning = false;
            }
            else
            {
                // If we should keep spinning, SPIN
                transform.Rotate(new Vector3(0, 1, 0), spinSpeed * Time.deltaTime);
            }
        }
        else
        {
            // When not spinning, advance through the wait time
            spinWaitProgress += Time.deltaTime;

            // If our wait time is over, start spinning again
            if (spinWaitProgress >= spinWait)
            {
                spinWaitProgress = 0;
                spinning = true;
            }
        }
    }

    public void selectUnit(Unit u)
    {
        if (selectedUnit != null)
            selectedUnit.removeHighlights();

        selectedUnit = u;        

        if (selectedUnit != null)
        {
            // Update reachable, highlight if the unit can move and is owned by the current player
            if (selectedUnit.canMove && selectedUnit.owner == game.currentPlayerIndex)
            {
                selectedUnit.pathfinder.updateReachable(selectedUnit);
                selectedUnit.highlightReachable();
            }
            
            // Highlight interactable tiles if the unit can act and is owned by the current player
            if(selectedUnit.canAct && selectedUnit.owner == game.currentPlayerIndex)
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

    public void setCurrentTile(Transform t)
    {
        this.currentTile = t;

        // Set our grid position
        this.position = t.GetComponent<Tile>().getPosition();

        // Float our cursor above the selected tile
        transform.position = t.transform.position + new Vector3(0, 1, 0);
    }

    public void setCurrentUnit(Point loc)
    {
        // TODO: IMPLEMENT
    }

    public Point getPosition()
    {
        return position;
    }
}
