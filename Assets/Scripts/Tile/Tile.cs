﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[SelectionBase]
public class Tile : MonoBehaviour {
	public static Tile active;
	public Transform tileContentContainer;
	public Transform tileMeshContainer;
	public Vector3Int coords;
	public enum TileType {Empty, Grass, Stone, Road, Forest, Mountain, Water}
	public bool isWalkable = false;
	// Use this for initialization
	public TileType tileType = TileType.Empty;

	void Start () {
		tileType = TileType.Empty;
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
		if (hasTower()) return false;
		if (tileType == TileType.Empty || tileType == TileType.Grass || tileType == TileType.Stone)
		{
			return true;
		}
		return false;
	}

	public bool hasTower() 
	{
		TileContent t = tileContentContainer.GetComponentInChildren<Tower>();
		if (t == null) return false;
		return true;
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