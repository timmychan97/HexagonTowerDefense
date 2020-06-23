using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour {
	public static Tile active;
	public Transform tileContentContainer;
	public Transform tileMeshContainer;
	public Vector3Int coords;
	public enum ID {Empty, Grass, Road, Tower, City, CityBuilding, Forest, Water}
	public bool isWalkable = false;
	// Use this for initialization
	public ID id = ID.Empty;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}

	public void SetTileMesh(GameObject tileMeshPf)
    {
		foreach (Transform c in tileMeshContainer)
			Destroy(c.gameObject);
		var tileMesh = Instantiate(tileMeshPf, tileMeshContainer);
		this.name = "Tile - " + tileMesh.name;
	}

	public bool CanPlaceTower() 
	{
		if (id == ID.Empty || id == ID.Grass)
		{
			return true;
		}
		return false;
	}

	public void SetTileContent(GameObject obj)
	{
		foreach (Transform c in tileContentContainer)
			Destroy(c.gameObject);
		Instantiate (obj, tileContentContainer);
	}

	public void OnClick()
    {
		TileManager.INSTANCE.OnClick(this);
    }


	#region highlighter
	public void Activate()
	{
		SetHighlight (transform, true);
	}
	public void Deactivate()
	{
		SetHighlight (transform, false);
	}

	void SetHighlight(Transform t, bool isEnabled)
	{
		// for each children, if it can be outlined, outline it.
		foreach (Transform c in t) {
			SetHighlight (c, isEnabled);
			cakeslice.Outline a = c.gameObject.GetComponent<cakeslice.Outline> ();
			if (a) {
				a.eraseRenderer = !isEnabled;
			}
		}
		return;
	}
	#endregion

	public void printCoords()
	{
		Debug.Log(coords);
	}
}
