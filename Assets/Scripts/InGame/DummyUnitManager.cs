using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyUnitManager : MonoBehaviour
{
    public static DummyUnitManager INSTANCE;

    GameUnit dummyUnit;
    Vector3 dummyUnitCoords = Vector3.one * int.MaxValue;

    // When a dummy unit is being destroyed (ready to be destroyed at the end of frame)
    // The boolean will be set to true.
    // We need this to prevent this script adding new dummy units or show a panel after 
    // running destroy.
    bool isBeingDestroyed = false;

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
        // if(isBeingDestroyed) return;

        // get tile
        Tile tile = TileUtils.GetTileUnderMouse();
        if (tile == null) return;
        Vector3 pos = TileUtils.RGBCoordsToWorld(tile.coords) + Vector3.up * tile.GetY();
        
        if (tile.coords != dummyUnitCoords) 
        { 
            // if mouse pointed to a different tile
            dummyUnit.transform.position = pos;
            // UI_PanelUnitInfoManager.INSTANCE.OnClick(dummyUnit.gameObject);
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
        isBeingDestroyed = false;

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
        isBeingDestroyed = true;
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
