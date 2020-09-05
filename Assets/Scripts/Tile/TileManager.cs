using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TileManager : MonoBehaviour {
	public static TileManager INSTANCE;

	void Start () => INSTANCE = this;

	public void SetTileContent(GameObject obj)
	{
		if (GameController.INSTANCE.IsGamePaused()) return;
		if (Tile.active) Tile.active.SetTileContent(obj);
	}

	public void SetTileMesh(GameObject tileMeshPf)
	{
		if (GameController.INSTANCE.IsGamePaused()) return;
		if (Tile.active) Tile.active.SetTileMesh(tileMeshPf);
    }

	public void OnClick(Tile tile)
    {
		if (tile == Tile.active)
		{
			Tile.active = null;
		}
		else
		{
			Tile.active = tile;
			UI_SelectionManager.INSTANCE.UseSelectedTool();
		}
	}
}
