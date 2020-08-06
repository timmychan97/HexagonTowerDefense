using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tool_GameUnit : UI_Tool
{
    public GameUnit gameUnit;
    public void SetGameUnit(GameUnit gameUnit)
    {
        this.gameUnit = gameUnit;
        base.SetAction(() => {
            if (GameController.INSTANCE.IsGamePaused()) return;
            if (Tile.active.CanPlaceUnit() && GameController.INSTANCE.CanBuyGameUnit(gameUnit))
            {
                GameUnit a = Instantiate(gameUnit);
                TileManager.INSTANCE.SetTileContent(a.gameObject);
                GameController.INSTANCE.OnBuyGameUnit(a);
            }
        });
    }
}
