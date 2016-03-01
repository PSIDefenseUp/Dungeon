using UnityEngine;
using System.Collections;

public class Camera : MonoBehaviour
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
            transform.Translate(new Vector3(-1 * Input.GetAxis("Mouse X"), -1 * Input.GetAxis("Mouse Y"), 0));
        }
	}
}
