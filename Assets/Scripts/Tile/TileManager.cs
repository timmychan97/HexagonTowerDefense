using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class TileManager : MonoBehaviour {
	public static TileManager INSTANCE;

	void Start () => INSTANCE = this;

	// returns the instantiated Game Object,
	// null if no Game objects are instantiated
	public void SetTileContent(GameObject obj)
	{
		if (GameController.INSTANCE)
		{
			if (GameController.INSTANCE.gameState == GameController.GameState.Paused) return;
		}
		if (Tile.active != null)
		{
			Tile.active.SetTileContent(obj);
		}
	}

	public void SetTileMesh(GameObject tileMeshPf)
	{
		if (GameController.INSTANCE)
        {
			if (GameController.INSTANCE.gameState == GameController.GameState.Paused) return;
		}
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
}
