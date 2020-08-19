using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* This class listens to the selected UI_Tool and changes the highlight method accordingly
 * 
 * Example:
 *      A tower that can only be placed on non-Path tiles is the selected tool.
 *      The class will make sure the tile is highlighted red when hovering over a path tile.
 *      And the tile is highlighted green when hovering over other tiles.
 */
public class TileHighlightManager : MonoBehaviour, ISelectionObserver
{
    void Start() => SelectionManager.INSTANCE.AddListener(SelectionManager.ObserverType.Tile, this);

    /* TODO: This function adds the support for towers that covers more than one tile.
     * It should get properties from the selected UI_Tool and get a size or formation defintion of the tool.
     * Return a reasonable object with corresponding data.
     */
    void GetUIToolPrefabTileForm()
    {
        return;
    }

    private static bool IsValidTileForSelectedTool(Tile tile, UI_Tool selectedTool)
    {
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

    void ISelectionObserver.OnSelect(Object obj) { }
    void ISelectionObserver.OnDeselect(Object obj) { }
    void ISelectionObserver.OnMouseDown(Object obj) { }
    void ISelectionObserver.OnMouseUp(Object obj) { }

    void ISelectionObserver.OnMouseEnter(Object obj)
    {
        Tile tile = (Tile)obj;
        if (tile == null) return;

        if (IsValidTileForSelectedTool(tile, UI_SelectionManager.INSTANCE.selectedTool))
            tile.Highlight(Color.green);
        else
            tile.Highlight(Color.red);
    }

    void ISelectionObserver.OnMouseExit(Object obj)
    {
        Tile tile = (Tile)obj;
        if (tile == null) return;

        tile.DeHighlight();
    }
}
