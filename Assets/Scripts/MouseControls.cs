using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Handles user mouse input on tiles
 */ 
public class MouseControls : MonoBehaviour {
    int MAP_LAYER_MASK = 1 << 8;
	private Camera cam = Camera.main;
    KeyCode primaryMouseButton = KeyCode.Mouse0; // left mouseButton
	
	void Update () {
		if(Input.GetKey(primaryMouseButton)) {
			// Do nothing if clicked on the UI elements in front
			bool hasCanvasUI = UnityEngine.EventSystems.EventSystem.current != null;
			if (hasCanvasUI && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
				return;
            }

            // Raycast a tile and invoke click event
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 600, MAP_LAYER_MASK))
            {
                Transform objectHit = hit.transform;
                Tile tile = objectHit.gameObject.GetComponentInParent<Tile>();
                if (tile)
                {
                    tile.OnClick();
                }
            }
        }
    }
}
