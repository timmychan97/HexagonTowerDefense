using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyUnitManager : MonoBehaviour
{
    public static DummyUnitManager INSTANCE;

    GameUnit dummyUnit;
    Vector3 dummyUnitCoords = Vector3.one * int.MaxValue;

    void Start()
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
        // TODO: add support for different types when more types of UI_Tool are added
        UI_Tool_GameUnit gameUnitTool = tool as UI_Tool_GameUnit;
        if (gameUnitTool == null) return;
        dummyUnit = CreateDummyGameUnit(gameUnitTool.gameUnit, dummyUnitCoords);
    }

    public void OnToolDeselected(UI_Tool tool)
    {
        if (dummyUnit != null) Destroy(dummyUnit.gameObject);
        UI_PanelUnitInfoManager.INSTANCE.CloseInfo();
    }

    GameUnit CreateDummyGameUnit(GameUnit unit, Vector3 pos)
    {
        GameUnit t = Instantiate(unit);
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
