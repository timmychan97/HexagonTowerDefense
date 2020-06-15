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
		if(Input.GetKeyDown(primaryMouseButton) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
			//click on tile and hightlight it.
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit,600)) {
				Transform objectHit = hit.transform;
				Tile c = objectHit.gameObject.GetComponentInParent<Tile>();
				if (c) {
					c.OnClick();
				}
            }
        }
	}
}
