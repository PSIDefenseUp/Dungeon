using UnityEngine;
using System.Collections;

public class TopDownCamera : MonoBehaviour
{
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
            transform.Translate(0, 0, Input.mouseScrollDelta.y * 100 * Time.deltaTime);
        }
	}
}
