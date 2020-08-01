using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyUnitManager : MonoBehaviour
{
    public static DummyUnitManager INSTANCE;

    Unit dummyUnit;
    Vector3 dummyUnitCoords = Vector3.one * int.MaxValue;

    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        HandleDummyUnit();
    }

    void HandleDummyUnit()
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
            UI_PanelUnitInfoManager.INSTANCE.OnClick(dummyUnit.gameObject);
            dummyUnitCoords = tile.coords;
        }
    }

    public void OnToolSelected(UI_Tool tool)
    {
        if (dummyUnit != null)
        {
            Destroy(dummyUnit.gameObject);
        }
        // check if selected tool is a UI_Tool_Tower
        // Note: add support for different types when more types of UI_Tool are added
        UI_Tool_Unit towerTool = tool as UI_Tool_Unit;
        if (towerTool == null) return;
        dummyUnit = CreateDummyUnit(towerTool.unit, dummyUnitCoords);
    }

    public void OnToolDeselected(UI_Tool tool)
    {
        if (dummyUnit != null) Destroy(dummyUnit.gameObject);
        UI_PanelUnitInfoManager.INSTANCE.CloseInfo();
    }

    Unit CreateDummyUnit(Unit unit, Vector3 pos)
    {
        Unit t = Instantiate(unit);
        t.SetIsDummy(true);
        t.transform.position = pos;
        Renderer rend = t.GetComponent<Renderer>();
        if (rend != null) {
            foreach (Material mat in rend.materials) {
                mat.SetColor("_Color", new Color(0.3f, 0.3f, 0.3f));
            }
        }
        return t;
    }
}
