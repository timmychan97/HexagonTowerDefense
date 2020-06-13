using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {
	// Use this for initialization
	void Start (){
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void UpdateSelectedTile(GameObject obj){
		if (Tile.active != null) {
			Tile.active.ChangeTile (obj);
		}
	}
}
