using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour
{
    private int speed = 10; // Move speed of the camera using keys or edge pan
    private int edgePanDistance = 5; // Max number of pixels from the edge of the screen to check for edge pan
    private int minZoom = 6;
    private int maxZoom = 11;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Camera drag on mouse wheel down
        if(Input.GetMouseButton(2))
        {
            // TODO: Improve
            transform.Translate(new Vector3(-1 * Input.GetAxis("Mouse X") * 50 * Time.deltaTime, 0, -1 * Input.GetAxis("Mouse Y") * 50 * Time.deltaTime), Space.World);            
        }
        else
        {
            // Zoom with mouse wheel, make sure we can't zoom too far in/out
            Vector3 oldpos = transform.position;

            transform.Translate(0, 0, Input.mouseScrollDelta.y * 100 * Time.deltaTime);

            if (transform.position.y < minZoom)
                transform.position = oldpos;

            if (transform.position.y > maxZoom)
                transform.position = oldpos;
            

            // Edge pan
            if (Input.mousePresent)
            {
                // Check west
                if(Input.mousePosition.x <= edgePanDistance)
                {
                    transform.Translate(new Vector3(-1 * speed * Time.deltaTime, 0, 0), Space.World);
                }

                // Check east
                if (Screen.width - Input.mousePosition.x <= edgePanDistance)
                {
                    transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);
                }

                // Check north
                if (Screen.height - Input.mousePosition.y <= edgePanDistance)
                {
                    transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.World);
                }

                // Check south
                if (Input.mousePosition.y <= edgePanDistance)
                {
                    transform.Translate(new Vector3(0, 0, -1 * speed * Time.deltaTime), Space.World);
                }                
            } 

            // Keyboard camera movement (wasd or arrow keys)
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                transform.Translate(new Vector3(-1 * speed * Time.deltaTime, 0, 0), Space.World);
            }

            // Check east
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                transform.Translate(new Vector3(speed * Time.deltaTime, 0, 0), Space.World);
            }

            // Check north
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 0, speed * Time.deltaTime), Space.World);
            }

            // Check south
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, 0, -1 * speed * Time.deltaTime), Space.World);
            }
        }
	}
}
