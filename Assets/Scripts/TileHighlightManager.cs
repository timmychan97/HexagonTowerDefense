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
public class TileHighlightManager : MonoBehaviour, ISelectionObserver
{
    void Start()
    {
        SelectionManager.INSTANCE.AddListener(SelectionManager.ObserverType.Tile, this);
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
            if (tile.HasTower())
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
        ISelectable selectable = (ISelectable)obj;
        if (selectable == null) return;

        if (IsValidTileForSelectedTool(selectable, UI_SelectionManager.INSTANCE.selectedTool))
            selectable.Highlight(Color.green);
        else
            selectable.Highlight(Color.red);
    }

    void ISelectionObserver.OnMouseExit(Object obj)
    {
        ISelectable selectable = (ISelectable)obj;
        if (selectable == null) return;

        selectable.DeHighlight();
    }
}
