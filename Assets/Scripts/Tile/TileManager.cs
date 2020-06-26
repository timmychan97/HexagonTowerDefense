﻿using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TileManager : MonoBehaviour {
	public static TileManager INSTANCE;

	// Use this for initialization
	void Start (){
		INSTANCE = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetTileContent(GameObject obj){
		if (Tile.active != null)
		{
			Tile.active.SetTileContent (obj);
		}
	}

	public void SetTileMesh(GameObject tileMeshPf)
	{
		if (Tile.active != null)
		{
			Tile.active.SetTileMesh(tileMeshPf);
		}
    }


	

	public void OnClick(Tile tile)
    {
		if (tile == Tile.active)
		{
			tile.Deactivate();
			Tile.active = null;
		}
		else
		{
			if (Tile.active)
				Tile.active.Deactivate();
			tile.Activate();
			Tile.active = tile;
			UI_SelectionManager.INSTANCE.UseSelectedTool();
		}
	}

	public bool IsAdjacent(Tile a, Tile b)
    {
		return ((b.coords - a.coords).magnitude == 1);
    }

	public bool IsBridged(Tile a, Tile b)
    {
		// TODO: When two tiles are not adjacent, but can be walked through, like a bridge, or jumping over a gap.
		return false;
    }

	public bool IsConnected(Tile a, Tile b)
    {
		return IsAdjacent(a, b) || IsBridged(a, b);
    }

    
	private PathTile[] GetAllPathTiles()
    {
		return GetComponentsInChildren<PathTile>();
    }


	public static void GeneratePathGraph(PathTile[] pathTiles)
    {

		foreach (var tile in pathTiles)
        {
			
        }
    }

}