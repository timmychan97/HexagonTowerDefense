using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEditor;

public class UI_InGame : MonoBehaviour
{
    public UI_Tool_Unit toolPf;

    private static string PATH_TOWER_MESH = "UnitMeshes";

    void Start()
    {
        Unit[] towerMeshPf = UI_Utils.GetResourcePrefabsComponentsSorted<Unit>(PATH_TOWER_MESH);
        Array.ForEach(towerMeshPf, x => CreateBtnUnit(x));
    }

    void CreateBtnUnit(Unit unit)
    {
        UI_Tool_Unit toolUnit = Instantiate(toolPf, transform);
        if (unit.iconSmall != null)
        {
            toolUnit.SetButtonSprite(unit.iconSmall);
            toolUnit.SetButtonText("");
        }
        else
        {
            toolUnit.SetButtonText(unit.name);
        }
        toolUnit.unit = unit;
        toolUnit.SetAction(() =>
        {
            if (GameController.INSTANCE.gameState == GameController.GameState.Paused) return;
            if (Tile.active.CanPlaceUnit() && GameController.INSTANCE.CanBuyUnit(unit))
            {
                Unit inst = Instantiate(unit);
                TileManager.INSTANCE.SetTileContent(inst.gameObject);
                GameController.INSTANCE.OnBuyUnit(inst);
            }
        });
    }
}
