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
        GameUnit[] gameUnits = UI_Utils.GetResourcePrefabsComponentsSorted<GameUnit>(pathUnits);
        // Debug.Log($"Getting from {pathUnits}");
        // Debug.Log(gameUnits.Count());
        Array.ForEach(gameUnits, x => CreateBtnUnit(x));
    }

    void CreateBtnUnit(GameUnit gameUnit)
    {
        UI_Tool_GameUnit toolUnit = Instantiate(toolPf, transform);
        if (gameUnit.iconSmall != null)
        {
            toolUnit.SetButtonSprite(gameUnit.iconSmall);
            toolUnit.SetButtonText("");
        }
        else
        {
            toolUnit.SetButtonText(gameUnit.name);
        }
        toolUnit.SetGameUnit(gameUnit);
    }
}
