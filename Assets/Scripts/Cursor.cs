using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    private Point position; // The current position of the cursor
    private Game game;      // Reference to the current Game object

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
	void Start ()
    {
        // Grab Game object from scene
        game = GameObject.Find("GameManager").GetComponent<Game>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        // On mouse left click, select the unit at the current location
        if(Input.GetMouseButtonDown(0))
        {
            selectUnit(position);
        }

        // On mouse right click, if we have a unit selected, move the unit to the cursor's location
        // TODO: Make units move rather than teleport
        if(Input.GetMouseButtonDown(1) && selectedUnit != null)
        {
            if(selectedUnit.canReach(position) && selectedUnit.canMove)
                selectedUnit.moveTo(position);
        }

        // When space is pressed, go to the next turn (for testing purposes) -- TODO: DELETE THIS
        if(Input.GetKeyDown(KeyCode.Space))
        {
            game.advanceTurn();
        }

        animate();
	}

    private void animate()
    {
        if(spinning)
        {            
            // When spinning, check to see when we've spun the whole way around (90 degrees)

            if(transform.rotation.eulerAngles.y + (spinSpeed * Time.deltaTime) >= 90)
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
            if(spinWaitProgress >= spinWait)
            {
                spinWaitProgress = 0;
                spinning = true;
            }
        }
    }
    
    public void selectUnit(Point p)
    {
        if (selectedUnit != null)
            selectedUnit.unHighlightReachable();

        selectedUnit = game.map.getUnit(position);

        // PRINT DEBUG INFO ON SELECTION -- TODO: DELETE THIS
        if (selectedUnit != null)
        {
            Debug.Log("Selected " + selectedUnit.name);
            selectedUnit.pathfinder.updateReachable(selectedUnit);

            if(selectedUnit.canMove)
                selectedUnit.highlightReachable();
        }
        else
        {
            Debug.Log("Selected Null");
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

    public void setSelectedUnit(Unit u)
    {
        this.selectedUnit = u;
    }
}
