using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Created by Donny Chan

Responsible for create a "Dummy Game Unit" that is a Game Unit that does
nothing but display itself as a tooltip for the player. But a UnitRangeMarker
should be displayed along with it.
*/

public class DummyUnitManager : MonoBehaviour
{
    public static DummyUnitManager INSTANCE;

    GameUnit dummyUnit;
    Vector3 dummyUnitCoords = Vector3.one * int.MaxValue;

    void Awake()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        HandleDummyGameUnit();
    }

    void HandleDummyGameUnit()
    {
        if (dummyUnit == null) return;

        // get tile
        Tile tile = TileUtils.GetTileUnderMouse();
        if (tile == null) return;
        Vector3 pos = TileUtils.RGBCoordsToWorld(tile.coords) + Vector3.up * tile.GetY();
        
        if (tile.coords != dummyUnitCoords) 
        { 
            // if mouse pointed to a different tile
            dummyUnit.transform.position = pos;
            dummyUnitCoords = tile.coords;
            UnitRangeMarker.MoveToUnit(dummyUnit);
        }
    }

    public void OnToolSelected(UI_Tool tool)
    {
        // Create a Dummy Unit upon selecting a tool,
        // and diplay info of the corresponding Unit
        // using Panel Unit Info.

        // Parameter:
        //      The selected tool

        if (dummyUnit != null) Destroy(dummyUnit.gameObject);

        // check if selected tool is a UI_Tool_GameUnit
        // TODO: add support for different types when more types of UI_Tool are added
        
        UI_Tool_GameUnit gameUnitTool = tool as UI_Tool_GameUnit;
        if (gameUnitTool != null) 
        {
            dummyUnit = CreateDummyGameUnit(gameUnitTool.gameUnit);
            UI_PanelUnitInfoManager.INSTANCE.OnClick(dummyUnit.gameObject);
            UnitRangeMarker.Show();
        }
    }

    public void OnToolDeselected(UI_Tool tool)
    {
        if (dummyUnit != null) Destroy(dummyUnit.gameObject);
        UI_PanelUnitInfoManager.INSTANCE.CloseInfo();
        UnitRangeMarker.Hide();
    }

    GameUnit CreateDummyGameUnit(GameUnit gameUnit)
    {
        GameUnit t = Instantiate(gameUnit);
        t.SetIsDummy(true);
        Renderer rend = t.GetComponent<Renderer>();
        if (rend != null) {
            foreach (Material mat in rend.materials) {
                mat.SetColor("_Color", new Color(0.3f, 0.3f, 0.3f));
            }
        }
        return t;
    }
}
