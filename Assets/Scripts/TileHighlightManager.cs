using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class listens to the selected UI_Tool and changes the highlight method accordingly
 * 
 * FIXME: Can be optimized better, but no need to, yet.
 * 
 * Example:
 *      A tower that can only be placed on non-Path tiles is the selected tool.
 *      The class will make sure the tile is highlighted red when hovering over a path tile.
 *      And the tile is highlighted green when hovering over other tiles.
 */
public class TileHighlightManager : MonoBehaviour
{
    int MAP_LAYER_MASK = 1 << 9;

    // Use a list to support multiple highlights
    List<ISelectable> highlightedSelectables = new List<ISelectable>();
    List<ISelectable> toBeHighlighted = new List<ISelectable>();


    // Update is called once per frame
    void Update()
    {
        if (UI_SelectionManager.INSTANCE.selectedTool) {
            DeHighlightAll(highlightedSelectables);
            highlightedSelectables.Clear();

            ISelectable selectableAtMousePos = GetSelectableAtMousePos();
            if (selectableAtMousePos != null)
                toBeHighlighted.Add(selectableAtMousePos);
            // Add other tiles to be highlighted corresponding to the selected UI_Tool

            if (toBeHighlighted.Count > 0)
            {
                if (IsValidTileForSelectedTool(selectableAtMousePos, UI_SelectionManager.INSTANCE.selectedTool))
                    HighlightAll(toBeHighlighted, Color.green);
                else
                    HighlightAll(toBeHighlighted, Color.red);
                highlightedSelectables.Add(selectableAtMousePos);

                toBeHighlighted.Clear();
            }
        }
    }

    void DeHighlightAll(List<ISelectable> selectables) => selectables.ForEach(s => s.DeHighlight());

    void HighlightAll(List<ISelectable> selectables, Color? color) => selectables.ForEach(s => s.Highlight(color));

    ISelectable GetSelectableAtMousePos()
    {
        int cameraToSelectableDistance = 600;
        RaycastHit a;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out a, cameraToSelectableDistance, MAP_LAYER_MASK))
        {
            Transform objectHit = a.transform;
            ISelectable selectableObj = objectHit.gameObject.GetComponentInParent<ISelectable>();
            if (selectableObj != null)
            {
                return selectableObj;
            }
        }
        return null;
    }

    /* TODO: This function adds the support for towers that covers more than one tile.
     * It should get properties from the selected UI_Tool and get a size or formation defintion of the tool.
     * Return a reasonable object with corresponding data.
     */
    void GetUIToolPrefabTileForm()
    {
        return;
    }

    private static bool IsValidTileForSelectedTool(ISelectable selectable, UI_Tool selectedTool)
    {
        Tile tile = (Tile)selectable;
        if (tile)
        {
            if (tile.HasUnit())
                return false;
            var invalidTileTypes = selectedTool.GetInvalidTileTypes();
            if (invalidTileTypes.IndexOf(tile.tileType) == -1)
                return true;
        }
        return false;
    }


    private static void HighlightSelectable(ISelectable selectable, Color? color) => selectable.Highlight(color);

}
