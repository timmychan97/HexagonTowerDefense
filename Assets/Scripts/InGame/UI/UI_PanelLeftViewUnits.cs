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

    public string pathUnits = "UnitMeshes";

    void Start()
    {
        Unit[] towerMeshPf = UI_Utils.GetResourcePrefabsComponentsSorted<Unit>(pathUnits);
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
