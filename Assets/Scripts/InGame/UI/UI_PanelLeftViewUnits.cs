using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using UnityEditor;

public class UI_PanelLeftViewUnits : MonoBehaviour
{
    public UI_Tool_GameUnit toolPf;

    private static string PATH_TOWER_MESH = "UnitMeshes";

    void Start()
    {
        Unit[] towerMeshPf = UI_Utils.GetResourcePrefabsComponentsSorted<Unit>(PATH_TOWER_MESH);
        Array.ForEach(towerMeshPf, x => CreateBtnUnit(x));
    }

    void CreateBtnUnit(Unit unit)
    {
        UI_Tool_GameUnit toolUnit = Instantiate(toolPf, transform);
        if (unit.iconSmall != null)
        {
            toolUnit.SetButtonSprite(unit.iconSmall);
            toolUnit.SetButtonText("");
        }
        else
        {
            toolUnit.SetButtonText(unit.name);
        }
        toolUnit.SetGameUnit(unit);
    }
}
