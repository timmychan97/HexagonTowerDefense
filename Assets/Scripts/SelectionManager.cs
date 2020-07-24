using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager INSTANCE;

    public enum ObserverType { Tile, Unit }
    private Dictionary<ObserverType, List<ISelectionObserver>> observers = new Dictionary<ObserverType, List<ISelectionObserver>>();



    // Configurations
    int unitsLayerMask = 1 << 8;
    int tilesLayerMask = 1 << 9;



    private struct PrevState
    {
        public Object mouseOver;
        public bool mouseDown;
        public Object selected;
    }

    PrevState tilePrevState = new PrevState();
    PrevState unitPrevState = new PrevState();


    // Alias
    KeyCode primaryMouseButton = KeyCode.Mouse0;


    void Awake()
    {
        INSTANCE = this;

        observers.Add(ObserverType.Tile, new List<ISelectionObserver>());
        observers.Add(ObserverType.Unit, new List<ISelectionObserver>());
    }

    void Update()
    {
        UpdateUnitSelection();

        UpdateTileSelection();
        // Update tile after Tile selection.
        // Tile selection could place a tower on the tile
        UpdateTileHover();
        tilePrevState.mouseDown = Input.GetKey(primaryMouseButton);
    }

    private void UpdateTileSelection()
    {
        if (tilePrevState.mouseDown != Input.GetKey(primaryMouseButton))
        {
            // If clicked on the UI elements in front
            if (isPointerOverUIElement()) return;

            // Raycast a unit and invoke click event
            RaycastHit hit;
            Tile tile = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 600, tilesLayerMask))
                tile = hit.transform.gameObject.GetComponentInParent<Tile>();

            if (tile)
            {
                tile.OnClick();
            }

        }
    }

    private void UpdateTileHover()
    {
        // If no selected tool, or the selected tool is deselected on the UI
        if (!UI_SelectionManager.INSTANCE.selectedTool)
        {
            // If there is a tile hovered already
            if (tilePrevState.mouseOver)
            {
                // Set prev to null, and then call MouseExit on the previously registered tile
                var tmp = tilePrevState.mouseOver;
                tilePrevState.mouseOver = null;
                observers[ObserverType.Tile].ForEach(e => e.OnMouseExit(tmp));
            }
            return;
        }

        // If tile at mouse pos is changed
        ISelectable selectableAtMousePos = GetSelectableAtMousePos();
        if ((Object)selectableAtMousePos != tilePrevState.mouseOver)
        {
            var prev = tilePrevState.mouseOver;
            tilePrevState.mouseOver = (Object)selectableAtMousePos;
            if (prev)
                observers[ObserverType.Tile].ForEach(e => e.OnMouseExit(prev));
            if ((Object)selectableAtMousePos)
                observers[ObserverType.Tile].ForEach(e => e.OnMouseEnter(tilePrevState.mouseOver));
        }

        // Check if the user placed a tower right now, which should OnMouseEnter once more.
        // FIXME: Fix properly
        if (tilePrevState.mouseOver)
        {
            if (tilePrevState.mouseDown != Input.GetKey(primaryMouseButton))
            {
                var prev = tilePrevState.mouseDown;
                tilePrevState.mouseDown = Input.GetKey(primaryMouseButton);
                observers[ObserverType.Tile].ForEach(e => e.OnMouseEnter(tilePrevState.mouseOver));
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>null when no selectable at mouse position</returns>
    ISelectable GetSelectableAtMousePos()
    {
        int cameraToSelectableDistance = 600;
        RaycastHit a;
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(r, out a, cameraToSelectableDistance, tilesLayerMask))
        {
            Transform objectHit = a.transform;
            ISelectable selectableObj = objectHit.gameObject.GetComponentInParent<ISelectable>();
            return selectableObj;
        }
        return null;
    }

    void UpdateUnitSelection()
    {
        // If a tool is selected
        if (UI_SelectionManager.INSTANCE.selectedTool)
        {
            // If there is a unit selected already
            if (unitPrevState.selected)
            {
                // Set prev to null, and then call SelectionDelect on the previously registered unit
                var tmp = unitPrevState.selected;
                unitPrevState.selected = null;
                observers[ObserverType.Tile].ForEach(e => e.OnDeselect(tmp));
            }
            return;
        }

        // If left mouse button not clicked
        if (!Input.GetKey(primaryMouseButton)) return;

        // If clicked on the UI elements in front
        if (isPointerOverUIElement()) return;


        // Raycast a unit and invoke click event
        RaycastHit hit;
        GameObject unitHit = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 600, unitsLayerMask)) 
            unitHit = hit.transform.gameObject;


        var prev = unitPrevState.selected;
        unitPrevState.selected = unitHit;
        if (prev)
            observers[ObserverType.Unit].ForEach(e => e.OnDeselect(prev));
        if (unitPrevState.selected)
            observers[ObserverType.Unit].ForEach(e => e.OnSelect(unitPrevState.selected));
    }

    private bool isPointerOverUIElement()
    {
        var currentEventSystem = UnityEngine.EventSystems.EventSystem.current;
        bool hasCanvasUI = currentEventSystem != null;
        bool isPointerOverUIElement = currentEventSystem.IsPointerOverGameObject();
        return hasCanvasUI && isPointerOverUIElement;
    }


    public void AddListener(ObserverType type, ISelectionObserver observer)
    {
        observers[type].Add(observer);
    }

    public void RemoveListener(ObserverType type, ISelectionObserver observer)
    {
        observers[type].Add(observer);
    }
}
