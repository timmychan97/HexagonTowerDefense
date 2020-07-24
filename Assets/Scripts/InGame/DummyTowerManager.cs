using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyTowerManager : MonoBehaviour
{
    public static DummyTowerManager INSTANCE;

    Tower dummyTower;
    Vector3 dummyTowerCoords = Vector3.one * int.MaxValue;

    void Start()
    {
        INSTANCE = this;
    }

    // Update is called once per frame
    void Update()
    {
        HandleDummyTower();
    }

    void HandleDummyTower()
    {
        if (dummyTower == null) return;

        // get tile
        Tile tile = TileUtils.GetTileUnderMouse();
        if (tile == null) return;
        Vector3 pos = TileUtils.RGBCoordsToWorld(tile.coords) + Vector3.up * tile.GetY();
        
        if (tile.coords != dummyTowerCoords) 
        { 
            // if mouse pointed to a different tile
            // if (dummyTower != null) Destroy(dummyTower.gameObject);
            dummyTower.transform.position = pos;
            UI_PanelUnitInfoManager.INSTANCE.OnClick(dummyTower.gameObject);
            dummyTowerCoords = tile.coords;
        }
    }

    public void OnToolSelected(UI_Tool tool)
    {
        if (dummyTower != null)
        {
            Destroy(dummyTower.gameObject);
        }
        // check if selected tool is a UI_Tool_Tower
        // Note: add support for different types when more types of UI_Tool are added
        UI_Tool_Tower towerTool = tool as UI_Tool_Tower;
        if (towerTool == null) return;
        dummyTower = CreateDummyTower(towerTool.tower, dummyTowerCoords);
    }

    public void OnToolDeselected(UI_Tool tool)
    {
        if (dummyTower != null) Destroy(dummyTower.gameObject);
        UI_PanelUnitInfoManager.INSTANCE.CloseInfo();
    }

    Tower CreateDummyTower(Tower tower, Vector3 pos)
    {
        Tower t = Instantiate(tower);
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
