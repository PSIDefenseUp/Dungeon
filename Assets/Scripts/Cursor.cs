using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    // Current selections
    private Transform selectedTile;
    private Transform selectedUnit;

    // Animation variables
    private float spinSpeed = 360; // Degrees per second to spin around
    private float spinWait = .5f; // Seconds to wait between spins
    private float spinWaitProgress = 0; // How far we are in our current wait
    private bool spinning = false; // Are we spinning or waiting?

    // Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
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

    public void selectTile(Transform t)
    {
        this.selectedTile = t;

        // Float our cursor above the selected tile
        transform.position = t.transform.position + new Vector3(0, 1, 0); 
    }

    public void selectTile(Point loc)
    {
        // TODO: IMPLEMENT
    }

    public void selectUnit(Transform t)
    {
        this.selectedUnit = t;
    }

    public void selectUnit(Point loc)
    {
        // TODO: IMPLEMENT
    }
}
