using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Tool_Sell : UI_Tool
{
    // Start is called before the first frame update
    void Start()
    {
        SetAction(() => {
            Debug.Log("Trigger Sell action");
            if (GameController.INSTANCE.gameState == GameController.GameState.Paused) return;
            Unit tower = Tile.active.GetUnit();
            Debug.Log($"Tile.active: {Tile.active}");
            Debug.Log($"tower: {tower}");
            if (tower != null)
            {
                GameController.INSTANCE.OnSellUnit(tower);
                Destroy(tower.gameObject);
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
