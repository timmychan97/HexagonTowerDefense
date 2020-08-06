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
            Unit unit = Tile.active.GetUnit();
            Debug.Log($"Tile.active: {Tile.active}");
            Debug.Log($"tower: {unit}");
            if (unit != null)
            {
                GameController.INSTANCE.OnSellUnit(unit);
                Destroy(unit.gameObject);
            }
        });
    }
}
