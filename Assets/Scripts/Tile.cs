using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {
	public static Tile active;
	//public enum ID {Empty, Grass, Road, Tower, City, CityBuilding, Forest, Water}
	// Use this for initialization
	//public ID id = ID.Empty;
	public GameObject goods;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	public void ChangeTile(GameObject obj){
		foreach (Transform c in transform) {
			if(c.name != "geo")
				Destroy (c.gameObject);
		}
		Instantiate (obj, transform);
	}


	#region highlighter
	public void Activate(){
		if (Tile.active)
			Tile.active.Deactivate ();
		loopChild (transform, true);
		Tile.active = this;
	}
	public void Deactivate(){
		loopChild (transform, false);
		if (Tile.active == this)
			Tile.active = null;
	}

	void loopChild(Transform t, bool isEnabled){
		//for each element, if it can be outlined, outline it.
		foreach (Transform c in t) {
			loopChild (c,isEnabled);
			cakeslice.Outline a = c.gameObject.GetComponent<cakeslice.Outline> ();
			if (a) {
				a.eraseRenderer = !isEnabled;
			}
		}
		return;
	}
	#endregion
}
