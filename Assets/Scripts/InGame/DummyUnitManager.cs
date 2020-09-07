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

    void Awake() => INSTANCE = this;

    // Update is called once per frame
    void Update()
    {
        HandleDummyGameUnit();
    }

    void HandleDummyGameUnit()
    {
        if (dummyUnit == null || dummyUnit.gameObject == null) return;

        Tile tile = TileUtils.GetTileUnderMouse();
        if (tile == null) return;

        Vector3 pos = TileUtils.RGBCoordsToWorld(tile.coords) + Vector3.up * tile.GetY();
        
        if (tile.coords != dummyUnitCoords) 
        {
            dummyUnit.gameObject.SetActive(true);
            // If mouse pointed to a different tile
            dummyUnit.transform.position = pos;
            dummyUnitCoords = tile.coords;

            // Destroy(dummyUnit.gameObject) will run at the end of frame. We use .enabled to detect it.
            // If the dummy unit is to be destroyed at the end of this frame, we don't make range marker follow it.
            if (dummyUnit.enabled)
                AttackRangeMarker.FollowUnit(dummyUnit);
        }
    }

    /// <summary>
    /// Create a Dummy Unit upon selecting a tool,
    /// and display info of the corresponding Unit
    /// using Panel Unit Info.
    /// </summary>
    /// <param name="tool">The selected tool</param>
    public void OnToolSelected(UI_Tool tool)
    {
        if (dummyUnit != null) Destroy(dummyUnit.gameObject);

        // Check if selected tool is a UI_Tool_GameUnit
        // TODO: Add support for different types when more types of UI_Tool are added
        
        UI_Tool_GameUnit gameUnitTool = tool as UI_Tool_GameUnit;
        if (gameUnitTool != null) 
        {
            dummyUnit = CreateDummyGameUnit(gameUnitTool.gameUnit);
            dummyUnit.gameObject.SetActive(false); // Don't show it until the mouse is on a tile
            UI_PanelUnitInfoManager.INSTANCE.OnClick(dummyUnit.gameObject);
        }
    }

    public void OnToolDeselected(UI_Tool tool)
    {
        if (dummyUnit != null) Destroy(dummyUnit.gameObject);
        UI_PanelUnitInfoManager.INSTANCE.CloseInfo();
        AttackRangeMarker.Hide();
    }

    GameUnit CreateDummyGameUnit(GameUnit gameUnit)
    {
        GameUnit t = Instantiate(gameUnit);
        t.SetIsDummy(true);

        //var rends = t.GetComponentsInChildren<Renderer>();
        //foreach (var rend in rends)
        //{
        //    foreach (Material mat in rend.materials)
        //    {
        //        var col = mat.GetColor("_BaseColor");
        //        col.a = 0.5f;
        //        //mat.SetColor("_BaseColor", col);
        //    }
        //}
        return t;
    }
}
