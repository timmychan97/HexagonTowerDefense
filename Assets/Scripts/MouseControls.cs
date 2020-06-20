using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour {
    //Control settings
	Camera cam;
    KeyCode primaryMouseButton = KeyCode.Mouse0; //left mouseButton
    cakeslice.Outline currentOutlinedComponent;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
        //cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(primaryMouseButton)) {
			// Do nothing if clicked on the UI elements in front
			bool hasCanvasUI = UnityEngine.EventSystems.EventSystem.current != null;
			if (hasCanvasUI && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
				return;
            }

            // Click on tile and hightlight it.
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 600))
            {
                Transform objectHit = hit.transform;
                Tile c = objectHit.gameObject.GetComponentInParent<Tile>();
                if (c)
                {
                    c.OnClick();
                }
            }

            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //// create a plane at 0,0,0 whose normal points to +Y:
            //Plane hPlane = new Plane(Vector3.up, Vector3.zero);
            //// Plane.Raycast stores the distance from ray.origin to the hit point in this variable:
            //float distance = 0;
            //// if the ray hits the plane...
            //if (hPlane.Raycast(ray, out distance))
            //{
            //	// get the hit point:
            //	var a = ray.GetPoint(distance);
            //	Debug.Log(a);
            //}
        }
	}
}
