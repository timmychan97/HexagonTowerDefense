using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tool_Sell : UI_Tool
{
    // Start is called before the first frame update
    void Start()
    {
        SetAction(() => {
            // Debug.Log("Trigger Sell action");
            if (GameController.INSTANCE.gameState == GameController.GameState.Paused) return;
            GameUnit gameUnit = Tile.active.GetGameUnit();
            if (gameUnit != null)
            {
                GameController.INSTANCE.OnSellGameUnit(gameUnit);
                Destroy(gameUnit.gameObject);
            }
        });
    }
}
