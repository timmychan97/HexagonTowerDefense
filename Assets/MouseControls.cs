using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControls : MonoBehaviour {
    //Control settings
    Camera cam;
    KeyCode primaryMouseButton = KeyCode.Mouse0;
    cakeslice.Outline currentOutlinedComponent;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(primaryMouseButton)) {
            print("pressed");
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit)) {
                Transform objectHit = hit.transform;
                cakeslice.Outline c = objectHit.GetComponent<cakeslice.Outline>();
                if(c) {
                    c.eraseRenderer = false;
                    if(currentOutlinedComponent) {
                        currentOutlinedComponent.eraseRenderer = true;
                    }
                    currentOutlinedComponent = c;
                }
            }
        }
	}
}
