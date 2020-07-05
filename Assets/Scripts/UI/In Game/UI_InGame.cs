using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEditor;

public class UI_InGame : MonoBehaviour
{
    public UI_Tool_Tower toolPf;

    private static string PATH_TOWER_MESH = "TowerMeshes";

    void Start()
    {
        Tower[] towerMeshPf = UI_Utils.GetResourcePrefabsSorted<Tower>(PATH_TOWER_MESH);
        Array.ForEach(towerMeshPf, x => CreateBtnTower(x));
    }

    void CreateBtnTower(Tower tower)
    {
        UI_Tool_Tower toolTower = Instantiate(toolPf, transform);
        toolTower.SetButtonText(tower.name);
        toolTower.tower = tower;
        toolTower.SetAction(() =>
        {
            if (GameController.INSTANCE.gameState == GameController.GameState.Paused) return;
            if (Tile.active.CanPlaceTower() && GameController.INSTANCE.BuyTower(tower))
            {
                TileManager.INSTANCE.SetTileContent(tower.gameObject);
            }
        });
    }
}
